namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueDiv : VueElement
            {
                public VueDiv(string textContent = "", params VueAttribute[] attributes) : base(new VueTag("div", attributes), textContent)
                {

                }
            }
        }
    }
}