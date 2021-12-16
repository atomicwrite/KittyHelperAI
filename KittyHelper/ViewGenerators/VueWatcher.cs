namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueWatcher
            {
                private readonly string setting;
                private readonly TypeScriptFunction function;

                public VueWatcher(string setting, TypeScriptFunction function)
                {
                    this.setting = setting;
                    this.function = function;
                }

                internal string Render()
                {
                    return $"@Watch({setting}) {function.Render()}";
                }
            }
        }
    }
}