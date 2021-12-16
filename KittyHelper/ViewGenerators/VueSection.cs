namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueSection : VueElement
            {
                public VueSection(string textContent = "", params VueAttribute[] attributes) : base(new VueTag("section", attributes), textContent)
                {

                }
            }
        }
    }
}