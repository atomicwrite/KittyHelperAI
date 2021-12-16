using ServiceStack;
using System;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptObject : TypeScriptStatement
            {
                private readonly TypeScriptObjectInitalizer[] initalizers;
                public TypeScriptObject(TypeScriptObjectInitalizer[] initalizers)
                {
                    this.initalizers = initalizers ?? Array.Empty<TypeScriptObjectInitalizer>();
                }
              
                public override string Render()
                {
                    return "{" + initalizers.Select(a => a.Render()).Join(Environment.NewLine) + "}";
                }

            }
        }
    }
}