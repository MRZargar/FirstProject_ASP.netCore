#pragma checksum "D:\__________M.R.Zargar\Mehr - Razieh\Mehr\Mehr\Views\Home\Colleagues.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6226c8e1380a5ef4cdcdb20e6693400577a0d8af"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Colleagues), @"mvc.1.0.view", @"/Views/Home/Colleagues.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\__________M.R.Zargar\Mehr - Razieh\Mehr\Mehr\Views\_ViewImports.cshtml"
using Mehr;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\__________M.R.Zargar\Mehr - Razieh\Mehr\Mehr\Views\_ViewImports.cshtml"
using Mehr.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6226c8e1380a5ef4cdcdb20e6693400577a0d8af", @"/Views/Home/Colleagues.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1337110ded55148aacbf7f8085ffd2d44a678d41", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Colleagues : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\__________M.R.Zargar\Mehr - Razieh\Mehr\Mehr\Views\Home\Colleagues.cshtml"
  
    ViewData["Title"] = "Colleagues";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<section id=\"content-colleagues\">\n\t<div class=\"row match-height\">\n\t\t");
#nullable restore
#line 7 "D:\__________M.R.Zargar\Mehr - Razieh\Mehr\Mehr\Views\Home\Colleagues.cshtml"
   Write(await Component.InvokeAsync("ListColleague"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n\t\t");
#nullable restore
#line 8 "D:\__________M.R.Zargar\Mehr - Razieh\Mehr\Mehr\Views\Home\Colleagues.cshtml"
   Write(await Component.InvokeAsync("NewColleague"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n\t</div>\n</section>\n\n\n");
            WriteLiteral("\n");
            DefineSection("Script", async() => {
                WriteLiteral("\n");
                WriteLiteral(@"	<script>
		function deleteColleague(id){
			$.get(""/Colleague/Delete/""+id, function(result){
				$("".modal"").modal();
				$("".modal-title"").html(""Delete Colleague"");
				$("".modal-body"").html(result); 
			});
		};
	</script>

	<script>
		$(document).ready(function(){
		$(""#input-search"").on(""keyup"", function() {
			var value = $(this).val().toLowerCase();
			$(""#recent-buyers *"").filter(function() {
				$(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
			});
		});
		});
	</script>
");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591