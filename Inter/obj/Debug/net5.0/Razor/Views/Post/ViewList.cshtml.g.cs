#pragma checksum "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4e20e24feafb079507a8f43ff5db0ddb2abf4d49"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Post_ViewList), @"mvc.1.0.view", @"/Views/Post/ViewList.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\_ViewImports.cshtml"
using Inter;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\_ViewImports.cshtml"
using Inter.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\_ViewImports.cshtml"
using System.Collections.Generic;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\_ViewImports.cshtml"
using Inter.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Mvc.Rendering;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4e20e24feafb079507a8f43ff5db0ddb2abf4d49", @"/Views/Post/ViewList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3e0d0a6675f93f6100e25c899177331cbcfafabb", @"/Views/_ViewImports.cshtml")]
    public class Views_Post_ViewList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<PostView>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Info", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Account", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("link-inter"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
  
    ViewBag.Title = ViewBag.ThreadName;
    Layout = "_Layout";
    
    var pathHelper = new PathHelper(Url);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1 class=\"display-3 text-md-end text-center\">");
#nullable restore
#line 10 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                                         Write(ViewBag.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n");
#nullable restore
#line 11 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
 foreach(var post in Model)
{
    var isFirstPost = string.CompareOrdinal(post.Id, "0") == 0;
    var className = isFirstPost ? "first-post" : "post shadow";
    var column = isFirstPost ? "12" : "auto";


#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"row\">\r\n        <div");
            BeginWriteAttribute("class", " class=\"", 469, "\"", 495, 3);
            WriteAttributeValue("", 477, "col", 477, 3, true);
            WriteAttributeValue(" ", 480, "col-lg-", 481, 8, true);
#nullable restore
#line 18 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
WriteAttributeValue("", 488, column, 488, 7, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n            <div");
            BeginWriteAttribute("class", " class=\"", 515, "\"", 569, 6);
            WriteAttributeValue("", 523, "container-lg", 523, 12, true);
#nullable restore
#line 19 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
WriteAttributeValue(" ", 535, className, 536, 10, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 546, "rounded", 547, 8, true);
            WriteAttributeValue(" ", 554, "pt-2", 555, 5, true);
            WriteAttributeValue(" ", 559, "pb-2", 560, 5, true);
            WriteAttributeValue(" ", 564, "mb-2", 565, 5, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n                <div class=\"row align-items-center mb-2\">\r\n                    <div class=\"col-auto\">\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4e20e24feafb079507a8f43ff5db0ddb2abf4d496920", async() => {
                WriteLiteral("\r\n                            <img");
                BeginWriteAttribute("src", " src=\"", 811, "\"", 863, 1);
#nullable restore
#line 23 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
WriteAttributeValue("", 817, pathHelper.GetCompressedFilePath(post.Poster), 817, 46, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" alt=\"Аватар\" \r\n                                 class=\"d-inline-block rounded-circle\" />\r\n                        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 22 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                                                                        WriteLiteral(post.Poster.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    </div>\r\n                    <div class=\"col-auto\">\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4e20e24feafb079507a8f43ff5db0ddb2abf4d499969", async() => {
                WriteLiteral("\r\n                            ");
#nullable restore
#line 29 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                       Write(post.Poster.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 28 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                                                                                           WriteLiteral(post.Poster.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    </div>\r\n                    <div class=\"col-auto\">\r\n                        ");
#nullable restore
#line 33 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                   Write(post.CreationTime.ToString(ConstHelper.DateFormat));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                </div>\r\n");
#nullable restore
#line 36 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                 switch (post.FileNames.Count)
                {
                    case > 1:
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"row mb-2\">\r\n");
#nullable restore
#line 41 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                             for (var i = 0; i < post.FileNames.Count; ++i)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <div class=\"col-md-3\" name=\"media\">\r\n                                    <img");
            BeginWriteAttribute("src", " src=\"", 1843, "\"", 1873, 1);
#nullable restore
#line 44 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
WriteAttributeValue("", 1849, post.CompressedPaths[i], 1849, 24, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("alt", " alt=\"", 1874, "\"", 1898, 1);
#nullable restore
#line 44 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
WriteAttributeValue("", 1880, post.FileNames[i], 1880, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"img-fluid\"/>\r\n                                    <input type=\"hidden\"");
            BeginWriteAttribute("value", " value=\"", 1977, "\"", 1999, 1);
#nullable restore
#line 45 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
WriteAttributeValue("", 1985, post.Paths[i], 1985, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                                </div>\r\n");
#nullable restore
#line 47 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </div>\r\n                        <div class=\"row\">\r\n                            <div name=\"post-text\" class=\"col-12 text-break\">\r\n                                ");
#nullable restore
#line 51 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                           Write(post.Text);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </div>\r\n                        </div>\r\n");
#nullable restore
#line 54 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"

                        break;
                    }
                    case 1:
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"row\">\r\n                            <div class=\"col-md-3\" name=\"media\">\r\n                                <img");
            BeginWriteAttribute("src", " src=\"", 2594, "\"", 2624, 1);
#nullable restore
#line 61 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
WriteAttributeValue("", 2600, post.CompressedPaths[0], 2600, 24, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("alt", " alt=\"", 2625, "\"", 2649, 1);
#nullable restore
#line 61 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
WriteAttributeValue("", 2631, post.FileNames[0], 2631, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"img-fluid\"/>\r\n                                <input type=\"hidden\"");
            BeginWriteAttribute("value", " value=\"", 2724, "\"", 2746, 1);
#nullable restore
#line 62 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
WriteAttributeValue("", 2732, post.Paths[0], 2732, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                            </div>\r\n                            <div name=\"post-text\" class=\"col text-break\">\r\n                                ");
#nullable restore
#line 65 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                           Write(post.Text);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </div>\r\n                        </div>\r\n");
#nullable restore
#line 68 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"

                        break;
                    }
                    default:
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"row\">\r\n                            <div name=\"post-text\" class=\"col-12 text-break\">\r\n                                ");
#nullable restore
#line 75 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
                           Write(post.Text);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </div>\r\n                        </div>\r\n");
#nullable restore
#line 78 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"

                        break;
                    }
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\r\n        </div>\r\n    </div>\r\n");
#nullable restore
#line 85 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Post\ViewList.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<PostView>> Html { get; private set; }
    }
}
#pragma warning restore 1591
