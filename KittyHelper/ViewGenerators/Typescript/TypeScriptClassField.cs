using System;
using System.Collections.Generic;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptClassField
            {

                protected readonly string name;
                protected readonly TypescriptTypeDeclaration type;
                private readonly string defaultValue;
                protected readonly List<TypeScriptDecorator> decorators;
                public TypeScriptClassField(string name, TypescriptTypeDeclaration type, string defaultValue = "", params TypeScriptDecorator[] decorators)
                {
                    this.decorators = new(decorators);
                    this.name = name;
                    this.type = type;
                    this.defaultValue = defaultValue;
                }

                public string Render()
                {
                    string dValue = "";
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        dValue = " = " + defaultValue;
                    }
                    string decoratorString = string.Join(Environment.NewLine, decorators.Select(a => a.Render()));
                    return @$"
                                {decoratorString}
                                {name} {type.Render()}  {dValue}
";
                }
            }
        }
    }
}