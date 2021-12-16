namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VModelAttribute : VueAttribute
            {
                public VModelAttribute(string value) : base("v-model", value)
                {

                }
            }
        }
    }
}