namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptObjectInitalizer : TypeScriptStatement
            {
                private readonly string name;
                private readonly string value;

                public TypeScriptObjectInitalizer(string name, string value)
                {
                    this.name = name;
                    this.value = value;
                }
                public override string Render()
                {
                    return $"{name} : {value}";
                }
            }
        }
    }
}