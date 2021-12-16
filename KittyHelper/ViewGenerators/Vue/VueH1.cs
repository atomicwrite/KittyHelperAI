namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueH1 : VueElement
            {
                public VueH1(string textContent = "", params VueAttribute[] attributes) : base(new VueTag("h1", attributes), textContent)
                {

                }
            }
        }
    }
}