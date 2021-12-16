namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypescriptTypeDeclaration
            {
                public static TypescriptTypeDeclaration NoReturnType = new TypescriptTypeDeclaration(new TypescriptType(null));
                private readonly TypescriptType type;
                public TypescriptTypeDeclaration(string tpe)
                {
                    type = new TypescriptType(tpe);
                }
                public TypescriptTypeDeclaration(TypescriptType type)
                {
                    this.type = type;
                }
                public string Render()
                {
                    return type.Render();
                    
                }
            }
        }
    }
}