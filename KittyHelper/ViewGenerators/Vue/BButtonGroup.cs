namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class BButtonGroup : VueElement
            {
                public BButtonGroup( params VueAttribute[] attributes) : base(new VueTag("b-button-group", attributes))
                {
                    
                }
            }
        }
    }
}