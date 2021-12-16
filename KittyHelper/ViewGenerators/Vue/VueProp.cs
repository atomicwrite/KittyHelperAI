using System;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueProp : TypeScriptClassField
            {

                public VueProp(string settings, string name, TypescriptTypeDeclaration type, string defaultValue) : base(name, type, defaultValue, new[] { new TypeScriptDecorator("Prop", settings) })
                {

                }

                internal string Render()
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}