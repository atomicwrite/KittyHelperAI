namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueComponent
            {
                public VueComponentScript Script { get; set; } = new();

                public VueElement RootElement = new(new VueTag("template"));

                public override string ToString()
                {
                    return base.ToString();
                }

                internal string Render()
                {
                    return RootElement.Render() + Script.Render();
                }
            }
        }
    }
}