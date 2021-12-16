namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueH3 : VueElement
            {
                public VueH3(string textContent = "", params VueAttribute[] attributes) : base(new VueTag("h3", attributes), textContent)
                {

                }

            }
        }
    }
}