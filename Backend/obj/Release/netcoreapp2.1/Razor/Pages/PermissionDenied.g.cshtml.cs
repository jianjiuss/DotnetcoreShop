#pragma checksum "C:\Users\hbox029\Source\Repos\MyShop\Backend\Pages\PermissionDenied.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1f1a0bac88a26bdd77ab1ac3807fe2526b03db49"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Backend.Pages.Pages_PermissionDenied), @"mvc.1.0.razor-page", @"/Pages/PermissionDenied.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/PermissionDenied.cshtml", typeof(Backend.Pages.Pages_PermissionDenied), null)]
namespace Backend.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\hbox029\Source\Repos\MyShop\Backend\Pages\_ViewImports.cshtml"
using Backend;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1f1a0bac88a26bdd77ab1ac3807fe2526b03db49", @"/Pages/PermissionDenied.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f1a940f7584a4c70b6e2429675f9a6e64bfb2a2d", @"/Pages/_ViewImports.cshtml")]
    public class Pages_PermissionDenied : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(51, 28, true);
            WriteLiteral("\r\n\r\n<h2>你没有权限访问该页面！</h2>\r\n\r\n");
            EndContext();
#line 7 "C:\Users\hbox029\Source\Repos\MyShop\Backend\Pages\PermissionDenied.cshtml"
   
    Layout = null;

#line default
#line hidden
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Backend.Pages.PermissionDeniedModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Backend.Pages.PermissionDeniedModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Backend.Pages.PermissionDeniedModel>)PageContext?.ViewData;
        public Backend.Pages.PermissionDeniedModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
