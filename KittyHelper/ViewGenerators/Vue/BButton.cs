namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class BButton : VueElement
            {
                public BButton(string text, params VueAttribute[] attributes) : base(new VueTag("b-button", attributes))
                {
                    textContent = text;
                }
            }
        }
    }
}