namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueBAlert : VueElement
            {
                public VueBAlert(string textContent = "", params VueAttribute[] attributes) : base(new VueTag("b-alert", attributes), textContent)
                {

                }
            }
        }
    }
}