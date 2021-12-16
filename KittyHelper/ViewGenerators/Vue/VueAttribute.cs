using System;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueAttribute
            {
                private readonly string name;
                private readonly string value;
                
                public VueAttribute(string name, string value)
                {
                    this.name = name;
                    this.value = value;
                }

                public string Render()
                {
                    return $"{name}=\"{value}\"";
                }
            }
        }
    }
}