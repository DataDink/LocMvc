using System.Collections.Generic;
using System.Linq;
using LocMvc;
using System.Web.WebPages;
using System.Xml.Linq;

namespace System.Web.Mvc
{
    public static class LocalizationExtensions
    {
        private static readonly ILocalizationService Service = LocalizationService.Configured; 

        public static MvcHtmlString Localize(this HtmlHelper context, Func<object, HelperResult> content)
        {
            var text = content(null).ToHtmlString();
            var contentContext = new Dictionary<string, string> {
                {"controller", context.ViewContext.RequestContext.RouteData.Values["controller"] as string ?? ""}, 
                {"action", context.ViewContext.RequestContext.RouteData.Values["action"] as string ?? ""}
            };

            try // Looking for a better way to detect <text>
            {
                var xml = XElement.Parse(text);
                var nodes = xml.DescendantNodesAndSelf().OfType<XText>();
                foreach (var node in nodes) {
                    var itemContext = new Dictionary<string, string>(contentContext);
                    var namedNode = node.Ancestors().FirstOrDefault(n => n.Attributes().Any(a => a.Name == "name"));
                    if (namedNode != null)
                    {
                        itemContext.Add("contexttype", namedNode.Name.LocalName.ToUpperInvariant());
                        itemContext.Add("contextname", namedNode.Attribute("name").Value);
                        if (namedNode.Attributes().Any(a => a.Name == "type")) {
                            itemContext["contexttype"] = namedNode.Attribute("type").Value;
                        }
                    }
                    var key = Service.GenerateKey(itemContext, node.Value);
                    node.Value = Service.GetLocalizedString(key, node.Value);
                }
                return new MvcHtmlString(xml.ToString());
            }
            catch
            {
                var key = Service.GenerateKey(contentContext, text);
                return new MvcHtmlString(Service.GetLocalizedString(key, text));
            }
        }

        public static MvcHtmlString Localize(this HtmlHelper context, string key)
        {
            return new MvcHtmlString(Service.GetLocalizedString(key, key));
        }

        public static MvcHtmlString Localize(this HtmlHelper context, Enum value)
        {
            var contentContext = new Dictionary<string, string> {
                {"controller", context.ViewContext.RequestContext.RouteData.Values["controller"] as string ?? ""}, 
                {"action", context.ViewContext.RequestContext.RouteData.Values["action"] as string ?? ""},
                { "contexttype", "ENUM" },
                { "contextname", value.GetType().Name }
            };
            var key = Service.GenerateKey(contentContext, value.ToString());
            return new MvcHtmlString(Service.GetLocalizedString(key, value.ToString()));
        }
    }
}
