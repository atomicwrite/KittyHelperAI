namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptParameter
            {
                private readonly string name;
                private readonly TypescriptTypeDeclaration type;

                public TypeScriptParameter(string name, TypescriptTypeDeclaration type)
                {
                    this.name = name;
                    this.type = type;
                }

                public string Render()
                {
                    return $"{name} {type.Render()}";
                }
            }
        }
    }
}