namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueAttribute
            {
                private readonly string name;
                private readonly string value;
                public override string ToString()
                {
                    return $"{name}=\"{value}\"";
                }
                public VueAttribute(string name, string value)
                {
                    this.name = name;
                    this.value = value;
                }
            }
        }
    }
}