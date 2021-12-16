namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueVModel
            {
                private readonly string setting;
                private readonly string name;
                private readonly TypescriptTypeDeclaration type;

                public VueVModel(string setting, string name, TypescriptTypeDeclaration type)
                {
                    this.setting = setting;
                    this.name = name;
                    this.type = type;
                }

                public string Render()
                {
                    return $"@VModel({setting}) name {type.Render()} ";
                }
            }
        }
    }
}