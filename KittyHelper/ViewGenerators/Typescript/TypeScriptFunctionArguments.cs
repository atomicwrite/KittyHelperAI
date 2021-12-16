using System;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptFunctionArguments : TypeScriptStatement
            {
             
                private TypeScriptStatement requestArgument;

                public TypeScriptFunctionArguments(TypeScriptStatement requestArgument)
                {
                    this.requestArgument = requestArgument;
                }

                public TypeScriptFunctionArguments(TypeScriptObject requestArgument)
                {
                    this.requestArgument = requestArgument;
                }

                public override string Render()
                {
                    return requestArgument.Render();
                }
            }
        }
    }
}