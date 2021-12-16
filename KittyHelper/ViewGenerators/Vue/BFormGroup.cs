namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class BFormGroup : VueElement
            {
                public BFormGroup(params VueAttribute[] attributes) : base(new VueTag("b-form-group", attributes))
                {

                }
            }
        }
    }
}