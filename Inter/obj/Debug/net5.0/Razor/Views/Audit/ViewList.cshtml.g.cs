#pragma checksum "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a0dfcd9c9ae859ffcd6d301ca5869b83221da7d5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Audit_ViewList), @"mvc.1.0.view", @"/Views/Audit/ViewList.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a0dfcd9c9ae859ffcd6d301ca5869b83221da7d5", @"/Views/Audit/ViewList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3e0d0a6675f93f6100e25c899177331cbcfafabb", @"/Views/_ViewImports.cshtml")]
    public class Views_Audit_ViewList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<AuditEntry>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("container-sm ps-0"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ViewList", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn shadow"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ClearAudit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("page-link"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Info", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
  
    ViewBag.Title = "Аудит";
    ViewBag.Page ??= 0;
    Layout = "_Layout";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1 class=\"display-3 mb-2 text-md-start text-center\">");
#nullable restore
#line 9 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                                                Write(ViewBag.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n<div class=\"d-sm-flex flex-sm-row align-items-center mb-2\">\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a0dfcd9c9ae859ffcd6d301ca5869b83221da7d57129", async() => {
                WriteLiteral(@"
        <div class=""row align-items-center mb-2"">
            <div class=""col-9"">
                <div class=""form-floating"">
                    <input class=""form-control"" id=""searchPattern"" name=""searchPattern"" placeholder=""Поиск"" autocomplete=""off"" />
                    <label for=""searchPattern"">Поиск</label>
                </div>
            </div>
            <div class=""col-3"">
                <input class=""btn shadow"" type=""submit"" value=""Поиск""/>
            </div>
        </div>
    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    <div class=\"col-auto ms-sm-auto\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a0dfcd9c9ae859ffcd6d301ca5869b83221da7d59514", async() => {
                WriteLiteral("Очистить аудит");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<div class=\"d-flex flex-row justify-content-end mb-2\">\r\n    <ul class=\"pagination\">\r\n");
#nullable restore
#line 30 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
         if (ViewBag.Page == 0)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"page-item disabled shadow\">\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a0dfcd9c9ae859ffcd6d301ca5869b83221da7d511184", async() => {
                WriteLiteral("&lt;");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n            </li>\r\n");
#nullable restore
#line 35 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
        }
        else
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"page-item shadow\">\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a0dfcd9c9ae859ffcd6d301ca5869b83221da7d512732", async() => {
                WriteLiteral("&lt;");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-page", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 39 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                                                                WriteLiteral((int)ViewBag.Page - 1);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["page"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-page", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["page"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n            </li>\r\n");
#nullable restore
#line 41 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("        <li class=\"page-item disabled shadow\">\r\n            <a class=\"page-link\">");
#nullable restore
#line 43 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                             Write(ViewBag.Page + 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n        </li>\r\n");
#nullable restore
#line 45 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
         if (((int)ViewBag.Page + 1) * ConstHelper.CountAuditDocumentsPerPage - Model.Count >= ConstHelper.CountAuditDocumentsPerPage 
             || ViewBag.Page == 0 && Model.Count < ConstHelper.CountAuditDocumentsPerPage)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"page-item disabled shadow\">\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a0dfcd9c9ae859ffcd6d301ca5869b83221da7d516092", async() => {
                WriteLiteral("&gt;");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n            </li>\r\n");
#nullable restore
#line 51 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
        }
        else
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"page-item shadow\">\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a0dfcd9c9ae859ffcd6d301ca5869b83221da7d517640", async() => {
                WriteLiteral("&gt;");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-page", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 55 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                                                                WriteLiteral((int)ViewBag.Page + 1);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["page"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-page", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["page"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n            </li>\r\n");
#nullable restore
#line 57 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    </ul>
</div>
<div class=""table-responsive-md"">
    <table class=""table table-md table-striped table-hover align-middle text-center"">
        <thead>
        <tr>
            <th scope=""col"">№</th>
            <th scope=""col"">Время</th>
            <th scope=""col"">Пользователь</th>
            <th scope=""col"">Действие</th>
            <th scope=""col"">Объект</th>
            <th scope=""col"">Результат</th>
            <th scope=""col"">Дополнительно</th>
        </tr>
        </thead>
        <tbody>
");
#nullable restore
#line 74 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
          
            var count = (int)ViewBag.Page * ConstHelper.CountAuditDocumentsPerPage;
            var length = ConstHelper.CountAuditDocumentsPerPage > Model.Count - count 
                ? Model.Count - count 
                : ConstHelper.CountAuditDocumentsPerPage;

            for (var i = count; i < count + length; ++i)
            {
                var index = i;

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <th scope=\"row\">");
#nullable restore
#line 84 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                                Write(i + 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                    <td>");
#nullable restore
#line 85 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                   Write(Model[i].Time.ToString(ConstHelper.DateFormatSecs));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 86 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                    Write(Model[i].User is null ? RoleName.Anon : Model[i].User.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 87 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                   Write(Model[i].Method);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 88 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                   Write(Model[i].Item);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 89 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                   Write(Model[i].Result);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a0dfcd9c9ae859ffcd6d301ca5869b83221da7d523040", async() => {
                WriteLiteral("\r\n                            Подробнее\r\n                        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 91 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                                                                  WriteLiteral(Model[index].Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 91 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
                                                                                                    WriteLiteral(ViewBag.Page);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["page"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-page", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["page"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 96 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\ViewList.cshtml"
            }
        

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<AuditEntry>> Html { get; private set; }
    }
}
#pragma warning restore 1591
