using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Xml.Linq;
using LocMvc.Configuration;

namespace LocMvc.Controls
{
    public class Localize : Control
    {
        private readonly ILocalizationService _service;

        public Localize() : this(LocalizationService.Configured) {}

        public Localize(ILocalizationService service)
        {
            _service = service;
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            var page = Page.AppRelativeVirtualPath;
            page = page.Substring(Page.AppRelativeTemplateSourceDirectory.Length);
            page = Regex.Replace(page, "\\.[^\\.]+$", "");

            var parts = page.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var action = parts.LastOrDefault() ?? "";
            var path = parts.Length > 0 ? string.Join("/", parts.Take(parts.Length - 1)) : "";
            var context = new Dictionary<string, string> {
                {"page", page},
                {"action", action},
                {"path", path}
            };
            
            using (var content = new StringWriter())
            using (var contentWriter = new HtmlTextWriter(content)) {
                base.RenderControl(contentWriter);
                var raw = string.Format("<root>{0}</root>", content);
                var xml = XElement.Parse(raw);
                xml.Localize(_service, context);
                foreach (var node in xml.Nodes()) {
                    writer.Write(node.ToString());
                }
            }
        }
    }
}
