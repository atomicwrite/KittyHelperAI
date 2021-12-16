namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VIf : VueAttribute
            {
                private readonly string condition;

                public VIf(string condition) : base("v-if", condition)
                {
                    this.condition = condition;
                }
            }
        }
    }
}