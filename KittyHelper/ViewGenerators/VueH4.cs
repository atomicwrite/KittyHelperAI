namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueH4 : VueElement
            {
                public VueH4(string textContent = "", params VueAttribute[] attributes) : base(new VueTag("h4", attributes), textContent)
                {

                }
            }
        }
    }
}