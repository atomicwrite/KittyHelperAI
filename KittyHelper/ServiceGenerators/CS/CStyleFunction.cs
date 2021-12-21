using System;
using System.Linq;
using KittyHelper.ServiceGenerators.CS;

namespace KittyHelper
{
 
            public class CStyleFunction : CStyleStatement
            {
                private readonly string name;
                private readonly CStyleTypeDeclaration returnType;
                private readonly bool async;
                private readonly string accessor;
                private readonly CStyleParameter[] vueParameters;
                private readonly CStyleDecorator[] decorators;
                private readonly CStyleStatement[] block;
                

                public CStyleFunction(string name, CStyleTypeDeclaration returnType, bool async, CStyleParameter[] cStyleParameters = null, CStyleStatement[] block = null ,CStyleDecorator[] decorators = null, string accessor = "public")
                {
                    this.name = name;
                    this.returnType = returnType;
                    this.async = async;
                    this.accessor = accessor;
                    this.vueParameters = cStyleParameters ?? Array.Empty<CStyleParameter>();
                    this.decorators = decorators ?? Array.Empty<CStyleDecorator>();
                    this.block = block ?? Array.Empty<CStyleStatement>();
                }

                public override string Render()
                {
                    var decoratorString = string.Join(Environment.NewLine, decorators.Select(a => a.Render()));
                    var parameterSTring = string.Join(Environment.NewLine, vueParameters.Select(a => a.Render()));
                    var blocks = string.Join(Environment.NewLine, block.Select(a => a.Render()));
                    var retType = returnType.Render();
                    var asy = async ? "async" : "";
                    if (async)
                    {
                        retType = $"Task<{retType}> ";
                    }
                    return $@"
                          {decoratorString}
                      {accessor}  {asy} {retType} {name} ({parameterSTring}) {{
                                    
                                    {blocks}
                                        
                                    }}
";
                }
            }
        }
 