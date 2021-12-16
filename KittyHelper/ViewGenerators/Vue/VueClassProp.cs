namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueClassProp : TypeScriptStatement
            {
                private readonly string name;
                private readonly TypeScriptStatement options;

                public VueClassProp(string name, TypeScriptStatement options)
                {
                    this.name = name;

                    this.options = options;
                }
                public override string ToString()
                {
                    return Render();
                }

                public override string Render()
                {

                    return $"@{name}({options.Render()})   ";
                }
            }
        }
    }
}