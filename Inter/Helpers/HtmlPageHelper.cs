using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inter.Helpers
{
    public static class HtmlPageHelper
    {
        private const string DivTagName = "div";
        private const string InputTagName = "input";
        
        private static readonly string[,] EditorTags =
        {
            { "[b]", "[/b]", "fw-bold", "bold" }, 
            { "[i]", "[/i]", "fst-italic", "italic" },
            { "[u]", "[/u]", "underline-inter", "underline" },
            { "[t]", "[/t]", "through-line-inter", "through_line" },
            { "[s]", "[/s]", "spoiler-inter", "spoiler" }
        };
        
        public static HtmlString GetDragAndDropField(IUrlHelper url, bool multiple, bool onlyPics)
        {
            const string picName = "drag_and_drop.svg";
            var chooseString = "Выберите файл";
            var dragString = " или перетащите его сюда";
            var mainDiv = new TagBuilder(DivTagName);
            var innerDiv = new TagBuilder(DivTagName);
            var inputInnerDiv = new TagBuilder(DivTagName);
            var img = new TagBuilder("img");
            var input = new TagBuilder(InputTagName);
            var label = new TagBuilder("label");
            var span = new TagBuilder("span");
            var script = new TagBuilder("script");
            var hiddenInput = new TagBuilder(InputTagName);
            var col = new TagBuilder(DivTagName);
            
            if (multiple)
            {
                chooseString = "Выберите файлы";
                dragString = " или перетащите их сюда";
                input.MergeAttribute("multiple", "");
            }

            if (onlyPics)
                input.MergeAttribute("accept", "images/*");
            

            mainDiv.AddCssClass("container drag-and-drop-field inter-dark rounded shadow");
            mainDiv.MergeAttributes(new Dictionary<string, string>
            {
                { "id", "drop-area" }, 
                { "ondragenter", "changeClass(this, '', 'drag-and-drop-highlight')" },
                { "ondragover", "changeClass(this, '', 'drag-and-drop-highlight')" }
            });
            innerDiv.AddCssClass("row justify-content-center drag-and-drop-inner-field");
            inputInnerDiv.MergeAttribute("style", "font-size: 1.08em");
            inputInnerDiv.AddCssClass("row");
            col.AddCssClass("col-auto");
            img.AddCssClass("drag-and-drop-pic");
            img.MergeAttributes(new Dictionary<string, string>
            {
                { "id", "uploadFile" },
                {"src", url.Content($"~/files/_system/images/{picName}") }
            });
            input.MergeAttributes(new Dictionary<string, string>
            {
                { "id", "drag-and-drop-input" },
                { "type", "file" },
                { "style", "height: 0; width: 0; display: none;" }
            });
            label.AddCssClass("fw-bold");
            label.MergeAttributes(new Dictionary<string, string>
            {
                { "for", "drag-and-drop-input" },
                { "style", "cursor: pointer" }
            });
            script.MergeAttributes(new Dictionary<string, string>
            {
                { "lang", "js" },
                { "src", url.Content("~/js/dragAndDrop.js") }
            });
            hiddenInput.MergeAttributes(new Dictionary<string, string>
            {
                { "name", "filePathInput" },
                { "type", "hidden" }
            });

            label.InnerHtml.Append(chooseString);
            span.InnerHtml.Append(dragString);
            inputInnerDiv.InnerHtml.AppendHtml(input.RenderSelfClosingTag());
            inputInnerDiv.InnerHtml.AppendHtml(label);
            inputInnerDiv.InnerHtml.AppendHtml(span);
            mainDiv.InnerHtml.AppendHtml(inputInnerDiv);
            col.InnerHtml.AppendHtml(img.RenderSelfClosingTag());
            innerDiv.InnerHtml.AppendHtml(col);
            mainDiv.InnerHtml.AppendHtml(hiddenInput);
            mainDiv.InnerHtml.AppendHtml(innerDiv);

            using var writer = new StringWriter();
            mainDiv.WriteTo(writer, HtmlEncoder.Default);
            script.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }

        public static HtmlString GetEditorPanel(IUrlHelper url)
        {
            using var writer = new StringWriter();
            
            for (var i = 0; i < EditorTags.GetLength(0); ++i)
            {
                var rounded = i == 0 ? " rounded-start" : string.Empty;
                rounded = i == EditorTags.GetLength(0) - 1 ? " rounded-end" : rounded;
                
                var btn = new TagBuilder("button");
                btn.AddCssClass("btn-svg");
                btn.MergeAttributes(new Dictionary<string, string>
                {
                    { "type", "button" },
                    { "value", EditorTags[i, 0] + " " + EditorTags[i, 1] }
                });
                
                var img = new TagBuilder("img");
                img.MergeAttributes(new Dictionary<string, string>
                {
                    { "src", url.Content($"~/files/_system/images/text/{EditorTags[i, 3]}.svg") },
                    { "draggable", "false" }
                });
                img.MergeAttribute("src", url.Content($"~/files/_system/images/text/{EditorTags[i, 3]}.svg"));
                img.AddCssClass("img-fluid p-1" + rounded);

                btn.InnerHtml.AppendHtml(img.RenderSelfClosingTag());
                btn.WriteTo(writer, HtmlEncoder.Default);
            }
            
            return new HtmlString(writer.ToString());
        }

        public static HtmlString GetHtmlText(string text)
        {
            text = GetNewLinesInText(text);
            
            var patterns = GetTags();
            var className = GetTags(true);

            for (var i = 0; i < patterns.Length; ++i)
            {
                var lengths = new[]
                {
                    patterns[i].IndexOf('.') - 2, 
                    patterns[i].Length - patterns[i].IndexOf('*') - 2
                };
                var regex = new Regex(patterns[i], RegexOptions.Singleline);

                for (var match = regex.Match(text); match.Success; match = regex.Match(text, match.Index))
                    if (match.Length != 0)
                        text = text[..match.Index] + "<span class=\"" + className[i] + "\">" + // left
                               text[(match.Index + lengths[0])..(match.Index + match.Length - lengths[1] + 1)] + // central
                               "</span>" + text[(match.Index + match.Length)..]; // right
                
            }

            return new HtmlString(text);
        }

        private static string GetNewLinesInText(string text)
        {
            var regex = new Regex("\\r\\n", RegexOptions.Singleline);

            for (var match = regex.Match(text); match.Success; match = regex.Match(text, match.Index))
                text = text[..match.Index] + "<br>" + text[(match.Index + match.Length)..];

            return text;
        }

        private static string[] GetTags(bool isHtmlClass = false)
        {
            string[] tags;
            
            if (isHtmlClass)
            {
                tags = new string[EditorTags.GetLength(0)];

                for (var i = 0; i < tags.Length; ++i)
                    tags[i] = EditorTags[i, 2];
            }
            else
            {
                tags = new string[EditorTags.GetLength(0)];

                for (var i = 0; i < tags.Length; ++i)
                    tags[i] = EditorTags[i, 0].Replace("[", "\\[").Replace("]", "\\]") 
                              + ".*" + EditorTags[i, 1].Replace("[", "\\[").Replace("]", "\\]");
            }
            
            return tags;
        }
    }
}