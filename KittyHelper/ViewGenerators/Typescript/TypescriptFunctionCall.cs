using ServiceStack;
using System;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypescriptFunctionCall : TypeScriptStatement
            {
                private readonly string functionname;
                private readonly TypeScriptFunctionArguments[] calls;
                private readonly bool await;

                public TypescriptFunctionCall(string functionname, TypeScriptFunctionArguments[] calls=null, bool await = false)
                {
                    this.functionname = functionname;
                    this.calls = calls ?? Array.Empty<TypeScriptFunctionArguments>();
                    this.await = await;
                }
                public override string Render()
                {
                    var argumentsToFunction = calls.Select(a => a.Render()).Join();
                    var await_ = await ? "await" : "";
                    return $"{await_ } {functionname}({argumentsToFunction})";
                }
            }
        }
    }
}