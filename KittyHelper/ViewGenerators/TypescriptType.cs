namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypescriptType
            {
                private readonly string name;

                public TypescriptType(string name)
                {
                    this.name = name;
                }
                
                public string Render()
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        return "";
                    }
                    return ": " + name;
                }
            }
        }
    }
}