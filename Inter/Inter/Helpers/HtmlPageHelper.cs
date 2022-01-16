using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inter.Helpers
{
    public static class HtmlPageHelper
    {
        private const string DivTagName = "div";
        private const string InputTagName = "input";
        
        public static HtmlString GetDragAndDropField(bool multiple, bool onlyPics)
        {
            const string picName = "drag_and_drop_file_upload_arrow.png";
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
            var row = new TagBuilder(DivTagName);
            
            if (multiple)
            {
                chooseString = "Выберите файлы";
                dragString = " или перетащите их сюда";
                input.MergeAttribute("multiple", "");
            }

            if (onlyPics)
                input.MergeAttribute("accept", "images/*");
            

            mainDiv.AddCssClass( "drag-and-drop-field shadow");
            mainDiv.MergeAttributes(new Dictionary<string, string>
            {
                { "id", "drop-area" }, 
                { "ondragenter", "changeClass(this, '', 'drag-and-drop-field-highlight')" },
                { "ondragover", "changeClass(this, '', 'drag-and-drop-field-highlight')" }
            });
            innerDiv.AddCssClass("container drag-and-drop-inner-field");
            row.AddCssClass("row justify-content-center");
            img.AddCssClass("drag-and-drop-pic");
            img.MergeAttributes(new Dictionary<string, string>
            {
                { "id", "uploadFile" },
                {"src", "../files/_system/images/" + picName }
            });
            input.MergeAttributes(new Dictionary<string, string>
            {
                { "id", "drag-and-drop-input" },
                { "type", "file" },
                { "style", "height: 0; width: 0; display: none;" }
            });
            label.AddCssClass("font-weight-bold");
            label.MergeAttribute("for", "drag-and-drop-input");
            script.MergeAttributes(new Dictionary<string, string>
            {
                { "lang", "js" },
                { "src", "../js/dragAndDrop.js" }
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
            row.InnerHtml.AppendHtml(img.RenderSelfClosingTag());
            innerDiv.InnerHtml.AppendHtml(row);
            mainDiv.InnerHtml.AppendHtml(hiddenInput);
            mainDiv.InnerHtml.AppendHtml(innerDiv);

            using var writer = new StringWriter();
            mainDiv.WriteTo(writer, HtmlEncoder.Default);
            script.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }
    }
}