using System;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptFunction : TypeScriptStatement
            {
                private readonly string name;
                private readonly TypescriptTypeDeclaration returnType;
                private readonly bool async;
                private readonly TypeScriptParameter[] vueParameters;
                private readonly TypeScriptDecorator[] decorators;
                private readonly TypeScriptStatement[] block;

                public TypeScriptFunction(string name, TypescriptTypeDeclaration returnType, bool async, TypeScriptParameter[] vueParameters = null, TypeScriptStatement[] block = null ,TypeScriptDecorator[] decorators = null)
                {
                    this.name = name;
                    this.returnType = returnType;
                    this.async = async;
                    this.vueParameters = vueParameters ?? Array.Empty<TypeScriptParameter>();
                    this.decorators = decorators ?? Array.Empty<TypeScriptDecorator>();
                    this.block = block ?? Array.Empty<TypeScriptStatement>();
                }

                public override string Render()
                {
                    var decoratorString = string.Join(Environment.NewLine, decorators.Select(a => a.Render()));
                    var parameterSTring = string.Join(Environment.NewLine, vueParameters.Select(a => a.Render()));
                    var blocks = string.Join(Environment.NewLine, block.Select(a => a.Render()));
                    var asy = async ? "async" : "";
                    return $@"
                          {decoratorString}
                        {asy} {name} ({parameterSTring}) {{
                                    
                                    {blocks}
                                        
                                    }}
";
                }
            }
        }
    }
}