namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class BFormInput : VueElement
            {
                public BFormInput(params VueAttribute[] attributes) : base(new VueTag("b-form-input", attributes))
                {

                }
            }
        }
    }
}