using ServiceStack;
using System;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptTryCatchFinally : TypeScriptStatement
            {
                private readonly TypeScriptStatement[] @try;
                private readonly TypeScriptStatement[] @catch;
                private readonly TypeScriptStatement[] @finally;
                private readonly string exceptionName;

                public TypeScriptTryCatchFinally(TypeScriptStatement[] _try, TypeScriptStatement[] _catch = null, TypeScriptStatement[] _finally = null, string exceptionName = "e")
                {
                    @try = _try??Array.Empty<TypeScriptStatement>();
                    @catch = _catch ?? Array.Empty<TypeScriptStatement>();
                    @finally = _finally ?? Array.Empty<TypeScriptStatement>();
                    this.exceptionName = exceptionName;
                }
                public override string Render()
                {
                    var tryBlock = @try.Select(a=>a.Render()).Join(System.Environment.NewLine);
                    var catchBlock = @catch.Select(a => a.Render()).Join(System.Environment.NewLine);
                    var finallyBlock = @finally.Select(a => a.Render()).Join(System.Environment.NewLine);
                    return @$"try {{
                            {tryBlock}
                            }}catch({exceptionName} : any){{ 
                        {catchBlock}
                }}finally{{  
                        {finallyBlock}
}}


";
                }
            }
        }
    }
}