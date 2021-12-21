using KittyHelper.ServiceGenerators.CS;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
    
        public static partial class KittyViewHelper
        {
            public class TypescriptVariable  : TypeScriptStatement
            {
                private readonly string init;
                private readonly string name;
                private readonly TypescriptType type;

                public TypescriptVariable(string init, string name, TypescriptType type)
                {
                    this.init = init;
                    this.name = name;
                    this.type = type;
                }
                public override string Render()
                {
                    string typeStr = type.Render();
                    
                    return $"{init} {name} {typeStr}";
                }
            }
        }
    }
}