namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptDecorator
            {
                private readonly string name;
                private readonly TypeScriptStatement settings;

                public TypeScriptDecorator(string name, TypeScriptStatement settings)
                {
                    this.name = name;
                    this.settings = settings;
                }

                public string Render()
                {
                    return $"@{name}({settings})";
                }
            }
        }
    }
}