using System;
using System.Linq;
using System.Reflection;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
        public static partial class KittyViewHelper
        {
            public static string GenerateVueCard(string title, string subtitle, string cardtext, string buttonVue,
                string vFor = "")
            {
                return $@" <b-card {vFor} :title=""{title}"" :sub-title=""{subtitle}"">
                    <b-card-text>
                    {cardtext}
                    </b-card-text>
                    {buttonVue}
                    </b-card>";
            }

            public class ComponentPath
            {
                public string Component { get; set; }
                public string Path { get; set; }
                
            }
            public static string GenerateVueAutoRoute(ComponentPath[] ComponentNames)
            {
                return string.Join(Environment.NewLine, ComponentNames.Select(a => $"import {a.Component} from './{a.Component}.vue'")) +
                       Environment.NewLine + $@"
                    import {{requiresAuth, router}} from ""@/shared/router"";" + "router.addRoutes([" + String.Join(
                           ",",
                           ComponentNames.Select(a => $"{{path: '{a.Path}', component: {a.Component}, beforeEnter: requiresAuth}}")) +
                       "])";
            }

            public static string GenerateVueButton(string buttonText, string buttonClick)
            {
                string click = "";
                if (!string.IsNullOrEmpty(buttonClick))
                {
                    click = $"@click=\"{buttonClick}\"";
                    ;
                }

                return $"<b-button {click}>{buttonText}</b-button>";
            }

            private static string GenerateVueDateTimeInput(PropertyInfo fieldInfo, bool optionsDisableUpdate=false)
            {
                var attributes = fieldInfo.GetCustomAttributesData();
                var LabelAttr = attributes.FirstOrDefault(a => a.AttributeType.Name == "LabelAttribute");
                var DescAttr = attributes.FirstOrDefault(a => a.AttributeType.Name == "DescriptionAttribute");
                string Label = fieldInfo.Name;
                string Desc = "";
                if (DescAttr != null && DescAttr.NamedArguments != null && DescAttr.NamedArguments.Count > 0)
                {
                    Desc = DescAttr.NamedArguments.First().ToString();
                }

                if (LabelAttr != null && LabelAttr.NamedArguments != null && LabelAttr.NamedArguments.Count > 0)
                {
                    Label = LabelAttr.NamedArguments.First().ToString();
                }

                return $@" <b-form-group
                    id=""fieldset-{fieldInfo.Name}""
                description=""{Desc}"" 
                label=""{Label}""
                label-for=""input-{fieldInfo.Name}""
                valid-feedback=""""
                    :invalid-feedback=""{fieldInfo.Name}invalidFeedback""
                    :state=""{fieldInfo.Name}state""
                           >
                    <b-form-input type=""datetime-local"" :disabled=""{optionsDisableUpdate.ToString().ToLower()}"" id=""input-{fieldInfo.Name}"" v-model=""DataModel.{fieldInfo.Name}"" :state=""{fieldInfo.Name}state"" trim></b-form-input>
                    </b-form-group>";
            }

            private static string GenerateVueNumberInput(PropertyInfo fieldInfo, bool disabled = true)
            {
                var attributes = fieldInfo.GetCustomAttributesData();
                var LabelAttr = attributes.FirstOrDefault(a => a.AttributeType.Name == "LabelAttribute");
                var DescAttr = attributes.FirstOrDefault(a => a.AttributeType.Name == "DescriptionAttribute");
                string Label = fieldInfo.Name;
                string Desc = "";
                if (DescAttr != null && DescAttr.NamedArguments != null && DescAttr.NamedArguments.Count > 0)
                {
                    Desc = DescAttr.NamedArguments.First().ToString();
                }

                if (LabelAttr != null && LabelAttr.NamedArguments != null && LabelAttr.NamedArguments.Count > 0)
                {
                    Label = LabelAttr.NamedArguments.First().ToString();
                }

                string Extra = "";
                string ExtraInput = "";
                if (!disabled)
                {
                    Extra = $@"valid-feedback=""""
                    :invalid-feedback=""{fieldInfo.Name}invalidFeedback""
                    :state=""{fieldInfo.Name}state""
";
                    ExtraInput=$":state=\"{fieldInfo.Name}state\"";
                }
                
                return $@" <b-form-group
                    id=""fieldset-{fieldInfo.Name}""
                description=""{Desc}"" 
                label=""{Label}""
                label-for=""input-{fieldInfo.Name}""
                {Extra}
                           >
                    <b-form-input {ExtraInput} :disabled=""{disabled.ToString().ToLower()}"" type=""number"" id=""input-{fieldInfo.Name}"" v-model=""DataModel.{fieldInfo.Name}""  trim></b-form-input>
                    </b-form-group>";
            }
            public static string GenerateVueTextInput(string Name, string Label,string Desc,string vModel)
            {
                
                return $@" <b-form-group
                    id=""fieldset-{Name}""
                description=""{Desc}"" 
                label=""{Label}""
                label-for=""input-{Name}""
                valid-feedback=""""
             
                           >
                    <b-form-input id=""input-{Name}"" v-model=""{vModel}""  trim></b-form-input>
                    </b-form-group>";
            }
            public static string GenerateVueTextInput(PropertyInfo fieldInfo, bool optionsDisableUpdate=false)
            {
                var attributes = fieldInfo.GetCustomAttributesData();
                var LabelAttr = attributes.FirstOrDefault(a => a.AttributeType.Name == "LabelAttribute");
                var DescAttr = attributes.FirstOrDefault(a => a.AttributeType.Name == "DescriptionAttribute");
                string Label = fieldInfo.Name;
                string Desc = "";
                if (DescAttr != null && DescAttr.NamedArguments != null && DescAttr.NamedArguments.Count > 0)
                {
                    Desc = DescAttr.NamedArguments.First().ToString();
                }

                if (LabelAttr != null && LabelAttr.NamedArguments != null && LabelAttr.NamedArguments.Count > 0)
                {
                    Label = LabelAttr.NamedArguments.First().ToString();
                }

                return $@" <b-form-group
                    id=""fieldset-{fieldInfo.Name}""
                description=""{Desc}"" 
                label=""{Label}""
                label-for=""input-{fieldInfo.Name}""
                valid-feedback=""""
                    :invalid-feedback=""{fieldInfo.Name}invalidFeedback""
                    :state=""{fieldInfo.Name}state""
                           >
                    <b-form-input id=""input-{fieldInfo.Name}"" :disabled=""{optionsDisableUpdate.ToString().ToLower()}"" v-model=""DataModel.{fieldInfo.Name}"" :state=""{fieldInfo.Name}state"" trim></b-form-input>
                    </b-form-group>";
            }
        }
    }
}