#pragma checksum "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e83b69e81e85e025e0c760469479bb7737b6195f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Audit_Info), @"mvc.1.0.view", @"/Views/Audit/Info.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e83b69e81e85e025e0c760469479bb7737b6195f", @"/Views/Audit/Info.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3e0d0a6675f93f6100e25c899177331cbcfafabb", @"/Views/_ViewImports.cshtml")]
    public class Views_Audit_Info : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AuditEntry>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Account", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Info", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 3 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml"
  
    ViewBag.Title = "Запись №" + Model.Id;
    Layout = "_Layout";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"container\">\r\n    <div class=\"row mb-3\">\r\n        <h1 class=\"display-3\">");
#nullable restore
#line 10 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml"
                         Write(ViewBag.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n    </div>\r\n    <div class=\"row\">\r\n        <h2>");
#nullable restore
#line 13 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml"
       Write(Model.Time);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n    </div>\r\n    <div class=\"row\">\r\n        <div class=\"col-auto\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e83b69e81e85e025e0c760469479bb7737b6195f5100", async() => {
                WriteLiteral("Пользователь: ");
#nullable restore
#line 17 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml"
                                                                                                 Write(Model.User.Name);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 17 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml"
                                                            WriteLiteral(Model.User.Id);

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
            WriteLiteral("\r\n        </div>\r\n        <div class=\"col\">\r\n            ");
#nullable restore
#line 20 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml"
       Write(Model.IpAddress);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n    <div class=\"row\">\r\n        <div class=\"col-auto\">\r\n            ");
#nullable restore
#line 25 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml"
       Write(Model.Method);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n        <div class=\"col-auto\">\r\n            ");
#nullable restore
#line 28 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml"
       Write(Model.Item);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n        <div class=\"col-auto\">\r\n            ");
#nullable restore
#line 31 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml"
       Write(Model.Result);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n    <div class=\"row\">\r\n        ");
#nullable restore
#line 35 "E:\Disk D\ProjectsCode\C#\RealProjects\Inter\Inter\Views\Audit\Info.cshtml"
   Write(Model.Info);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AuditEntry> Html { get; private set; }
    }
}
#pragma warning restore 1591
