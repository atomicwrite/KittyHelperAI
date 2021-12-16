using ServiceStack;
using System.Collections.Generic;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueTag
            {
                private readonly string tagName;
                private List<VueAttribute> attributes;

                public VueTag(string tagName, params VueAttribute[] vueAttributes)
                {
                    this.tagName = tagName;

                    attributes = new(vueAttributes);

                }
                public string OpenTag()
                {
                    string attributesStr = attributes.Select(a => a.Render()).Join(" "  );
                    return $"<{tagName} {attributesStr}>";
                }
                public string CloseTag()
                {
                    return $"</{tagName}>";
                }
                public void AddAttribute(VueAttribute a)
                {
                    attributes.Add(a);
                }
            }
        }
    }
}