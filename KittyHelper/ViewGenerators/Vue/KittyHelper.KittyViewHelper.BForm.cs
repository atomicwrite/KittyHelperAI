namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class BForm : VueElement
            {
                public BForm(params VueAttribute[] attributes) : base(new VueTag("b-form", attributes))
                {

                }
            }
        }
    }
}