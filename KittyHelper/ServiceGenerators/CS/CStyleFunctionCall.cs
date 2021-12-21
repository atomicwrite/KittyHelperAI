using ServiceStack;
using System;
using System.Linq;
using KittyHelper.ServiceGenerators.CS;

namespace KittyHelper
{
   
            public class CStyleFunctionCall : CStyleStatement
            {
                private readonly string functionname;
                private readonly CStyleFunctionArguments[] calls;
                private readonly bool _await;

                public CStyleFunctionCall(string functionname, CStyleFunctionArguments[] calls=null, bool @await = false)
                {
                    this.functionname = functionname;
                    this.calls = calls ?? Array.Empty<CStyleFunctionArguments>();
                    _await = @await;
                }
                public override string Render()
                {
                    var argumentsToFunction = calls.Select(a => a.Render()).Join();
                    var @await = _await ? "await" : "";
                    return $"{@await } {functionname}({argumentsToFunction})";
                }
            }
        }
  