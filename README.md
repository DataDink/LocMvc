LocMvc (0.0.3) (and LocASP)
=====================================

by [Mark Nelson](http://www.markonthenet.com/)

This is a .NET MVC (and WebForms) framework package that helps organize and orchestrate localization.

What it is:
---------------
* Simple localization for RAZOR and ASP pages
* Service oriented - so can be used in other project types
* Customizable

What it does:
-------------
The purpose behind LocMvc is to have a nuget delivered package that will set your application up with working localization right out of the box.
LocMvc wires up a localization service which hosts a localization strategy (resource-based strategy included). 
The included HtmlHelper methods allow a very fluid integration with RAZOR-based pages. LocASP also includes a "Localize" webform container control in place of the HtmlHelper extension methods in MVC.

Out of the box:
---------------
* Localizing your existing pages will be relatively painless

*Localizing blocks of content:*
The following example will localize an entire table header. LocMvc will identify each header text as a string needing to be localized and will generate a contextual key based on the current controller/action.
```html
<table>
	@Html.Localize(@<thead>
						<tr>
							<th>Header A</th>
							<th>Header B</th>
							<th>Header C</th>
						</tr>
					</thead>)
	<tbody>
		<tr>
			<td>Unlocalized user content</td>
			<td>Unlocalized user content</td>
			<td>Unlocalized user content</td>
		</tr>
	</tbody>
</table>
```
(ASP version):
```html
<table>
	<loc:Localize runat="server">
		<thead>
			<tr>
				<th>Header A</th>
				<th>Header B</th>
				<th>Header C</th>
			</tr>
		</thead>
	</loc:Localize>
	<tbody>
		<tr>
			<td>Unlocalized user content</td>
			<td>Unlocalized user content</td>
			<td>Unlocalized user content</td>
		</tr>
	</tbody>
</table>
```
Unlocalized strings will display their generated keys:
"(UNLOCALIZED)Home_Index_Header_A"

These keys can then be added to the appropriate resource files

For example: LocalizedString.en.resx, LocalizedString.fr.resx, LocalizedString.cn.resx



###See the [wiki](https://github.com/DataDink/LocMvc/wiki) for more info


Change Log
----------
*4/26/13:*
* Moved LocalizationExtensions.cs to an appropriate folder
* Split out all extensions to the LocMvc.Extensions namespace
* Removed configuration dependency from LocMvc.Extensions extensions
* Reconfigured extensions in the System and System.Web.Mvc namespaces to call the LocMvc.Extension ones
* Added a Localization container control for use with ASPX and ASCX pages. (for you non-mvc users)
* Fixed an issue where context values were not getting sanitized with the rest of the contextual key.
* Adding LocASP package for nuget (contains appropriate configurations).