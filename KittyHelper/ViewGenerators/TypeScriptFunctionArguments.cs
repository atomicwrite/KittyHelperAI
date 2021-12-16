using System;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptFunctionArguments : TypeScriptStatement
            {
                private readonly string value;

                public TypeScriptFunctionArguments(string value)
                {
                    this.value = value;
                }

                public override string Render()
                {
                    return value;
                }
            }
        }
    }
}