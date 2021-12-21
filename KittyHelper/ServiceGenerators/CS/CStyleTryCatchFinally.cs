using ServiceStack;
using System;
using System.Linq;
using KittyHelper.ServiceGenerators.CS;
namespace KittyHelper
{
    
            public class CStyleTryCatchFinally : CStyleStatement
            {
                private readonly CStyleStatement[] @try;
                private readonly CStyleStatement[] @catch;
                private readonly CStyleStatement[] @finally;
                private readonly string exceptionName;

                public CStyleTryCatchFinally(CStyleStatement[] _try, CStyleStatement[] _catch = null, CStyleStatement[] _finally = null, string exceptionName = "e")
                {
                    @try = _try??Array.Empty<CStyleStatement>();
                    @catch = _catch ?? Array.Empty<CStyleStatement>();
                    @finally = _finally ?? Array.Empty<CStyleStatement>();
                    this.exceptionName = exceptionName;
                }
                public override string Render()
                {
                    var tryBlock = @try.Select(a=>a.Render()).Join(System.Environment.NewLine);
                    var catchBlock = @catch.Select(a => a.Render()).Join(System.Environment.NewLine);
                    var finallyBlock = @finally.Select(a => a.Render()).Join(System.Environment.NewLine);
                    return @$"try {{
                            {tryBlock}
                            }}catch(Exception {exceptionName}){{ 
                        {catchBlock}
                }}finally{{  
                        {finallyBlock}
}}


";
                }
            }
        }
 