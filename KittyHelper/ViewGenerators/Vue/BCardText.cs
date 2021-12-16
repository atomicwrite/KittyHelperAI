namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class BCardText : VueElement
            {
                public BCardText(params VueAttribute[] attributes) : base(new VueTag("b-card-text", attributes))
                {

                }
            }
        }
    }
}