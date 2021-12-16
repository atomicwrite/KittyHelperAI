using System;
using System.Collections.Generic;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueElement
            {
                private readonly List<VueElement> children;
                private readonly VueTag tag;
                protected string textContent;

                public VueElement(VueTag tag, string textContent = "", VueElement[] children = null)
                {
                    this.children = children is not null ? (new(children)) : (new());
                    this.tag = tag;
                    this.textContent = textContent;
                }


                public void AddChild(VueElement vueElement)
                {
                    children.Add(vueElement);
                }
                public string Render()
                {
                    string content = string.Join(Environment.NewLine, children.Select(a => a.Render()));
                    return tag.OpenTag() + content + tag.CloseTag();
                }
            }
        }
    }
}