namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueClickAttribute : VueAttribute
            {
                public VueClickAttribute(string value) : base("@click", value)
                {

                }
            }
        }
    }
}