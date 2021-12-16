namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueWatcher
            {
                private readonly TypeScriptStatement setting;
                private readonly TypeScriptFunction function;

                public VueWatcher(TypeScriptStatement setting, TypeScriptFunction function)
                {
                    this.setting = setting;
                    this.function = function;
                }

                internal string Render()
                {
                    return $"@Watch({setting.Render()}) {function.Render()}";
                }
            }
        }
    }
}