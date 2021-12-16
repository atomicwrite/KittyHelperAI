namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class BCardTitle : VueElement
            {
                public BCardTitle(string content,params VueAttribute[] attributes) : base(new VueTag("b-card-title", attributes),content)
                {

                }
            }
            public class BCardHeader : VueElement
            {
                public BCardHeader(params VueAttribute[] attributes) : base(new VueTag("b-card-header", attributes))
                {

                }
            }
        }
    }
}