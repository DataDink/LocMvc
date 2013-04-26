using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LocMvc;

namespace System.Xml.Linq
{
    internal static class XmlExtensions
    {
        public static void Localize(this XElement root, ILocalizationService service, Dictionary<string, string> context = null)
        {
            context = context ?? new Dictionary<string, string>();

            var nodes = root.DescendantNodesAndSelf().OfType<XText>();
            foreach (var node in nodes) {
                var itemContext = new Dictionary<string, string>(context);
                var namedNode = node.Ancestors().FirstOrDefault(n => n.Attributes().Any(a => a.Name == "name"));
                if (namedNode != null) {
                    itemContext.Add("contexttype", namedNode.Name.LocalName.ToUpperInvariant());
                    itemContext.Add("contextname", namedNode.Attribute("name").Value);
                    if (namedNode.Attributes().Any(a => a.Name == "type")) {
                        itemContext["contexttype"] = namedNode.Attribute("type").Value;
                    }
                }
                var key = service.GenerateKey(itemContext, node.Value.Trim());
                node.Value = service.GetLocalizedString(key, node.Value);
            }

        }
    }
}
