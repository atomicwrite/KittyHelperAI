using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
        public static partial class KittyViewHelper
        {
            public static string GenerateUpdatePage(Type T, UpdateViewOptions options)
            {
                StringBuilder StringBuilder = new();
                StringBuilder.AppendLine("<template>");
                StringBuilder.AppendLine("<section><div class='container'> ");
                StringBuilder.AppendLine($"<h4>Update {T.Name}</h4>");
                if (options.ReferencedByTypes != null)
                    foreach (var ByType in options.ReferencedByTypes)
                        StringBuilder.AppendLine(
                            GenerateVueButton("New " + ByType.Name, "New" + ByType.Name));

                StringBuilder.AppendLine("    <b-alert show v-if=\"Message.length >0\">{{  Message }} </b-alert>");

                var FieldInfos = T.GetProperties();
                var PrimaryKeyField = T.GetProperties().Where(a =>
                    a.GetCustomAttributesData().Any(b => b.AttributeType.Name == "PrimaryKeyAttribute"));
                foreach (var field in FieldInfos)
                {
                    var CustomAttributesData = field.GetCustomAttributesData();
                    if (CustomAttributesData.Any(a => a.AttributeType.Name == "AutoIncrementAttribute"))
                    {
                        StringBuilder.AppendLine(GenerateVueNumberInput(field));
                        continue;
                    }

                    switch (field.PropertyType.Name)
                    {
                        case "String":
                            StringBuilder.AppendLine(GenerateVueTextInput(field, options.DisableUpdate));
                            break;
                        case "Int32":
                        case "Int64":
                        case "Double":
                        case "Float":

                            StringBuilder.AppendLine(GenerateVueNumberInput(field, options.DisableUpdate));
                            break;
                        case "DateTime":
                            StringBuilder.AppendLine(GenerateVueDateTimeInput(field, options.DisableUpdate));
                            break;
                    }
                }

                StringBuilder.AppendLine(GenerateVueButton("Update", "Update"));
                List<string> AdditionalImports = new();
                List<string> AdditionalComponents = new();
                if (options.ReferencedByTypes != null)
                    foreach (var ByType in options.ReferencedByTypes)
                    {
                        var ComponentName = $"ListWithReference{ByType.Name}";
                        var snake = ComponentName.ToUnderscoreCase();
                        StringBuilder.AppendLine($"<{snake} :select_mode=\"true\" :{T.Name.ToLower()}reference-id=\"DataModel.Id\" :{T.Name.ToLower()}-anti=\"false\"  ></{snake}>");
                        StringBuilder.AppendLine($"<{snake} :select_mode=\"true\" :{T.Name.ToLower()}reference-id=\"DataModel.Id\" :{T.Name.ToLower()}-anti=\"true\" ></{snake}>");
                        AdditionalImports.Add(
                            $"import {ComponentName} from '@/Views/{ByType.Name}/{ComponentName}.vue' ");
                        AdditionalComponents.Add(ComponentName);
                    }


                StringBuilder.AppendLine("</div></section>");
                StringBuilder.AppendLine("</template>");

                StringBuilder.AppendLine(@"<script lang=""ts"">");
                StringBuilder.AppendLine($@"
import {{Component, Vue}} from 'vue-property-decorator';
import {{client}} from '@/shared';
import {{ {options.RequestObjectName}, {T.Name}, {options.ListOneRequestObjectName} }} from '@/shared/dtos';");
                foreach (var AdditionalImport in AdditionalImports) StringBuilder.AppendLine(AdditionalImport);

                StringBuilder.AppendLine($@"@Component({{
    components: {{{string.Join(",", AdditionalComponents)}}},
}})
export default class {options.ComponentName} extends Vue {{");
                StringBuilder.AppendLine("Message : string = \"\"");
                StringBuilder.AppendLine("Loading : boolean = false");
                StringBuilder.AppendLine("Error : boolean = false");
                StringBuilder.AppendLine($"DataModel : {T.Name} = new {T.Name}()");
                foreach (var field in T.GetProperties())
                {
                    var CustomAttributesData = field.GetCustomAttributesData();
                    if (CustomAttributesData.Any(a => a.AttributeType.Name == "AutoIncrementAttribute")) continue;

                    StringBuilder.AppendLine($"{field.Name}state : boolean  = true; ");
                    StringBuilder.AppendLine($"{field.Name}invalidFeedback : string  = \"\"; ");
                }

                if (options.ReferencedByTypes != null)
                    foreach (var ByType in options.ReferencedByTypes)
                    {
                        StringBuilder.AppendLine($" New{ByType.Name} (){{");
                        StringBuilder.AppendLine($"this.$router.push('/Create{ByType.Name}/' + this.DataModel.Id)");
                        StringBuilder.AppendLine("}");
                    }


                StringBuilder.AppendLine($"async ListOne{T.Name}(){{");
                StringBuilder.AppendLine("try{");
                StringBuilder.AppendLine("this.Error = false;");
                StringBuilder.AppendLine("this.Message = '';");
                StringBuilder.AppendLine(
                    $" const Response = await client.{options.ListOneHttpVerb.ToLower()}(new {options.ListOneRequestObjectName}({{ {options.ListOneRequestObjectField} : +this.$route.params['id'] }} ));");
                StringBuilder.AppendLine(
                    $"this.DataModel  = Response.{T.Name} ");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("catch(e) {");
                StringBuilder.AppendLine("console.log(e)");
                StringBuilder.AppendLine(
                    "this.Message = e.message");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("}");

                StringBuilder.AppendLine($" created(){{  this.ListOne{T.Name}(); }} ");

                StringBuilder.AppendLine("async Update() {");
                StringBuilder.AppendLine("try{");
                StringBuilder.AppendLine("this.Error = false;");
                StringBuilder.AppendLine("this.Message = '';");
                StringBuilder.AppendLine(
                    $" const Response = await client.{options.HttpVerb.ToLower()}(new {options.RequestObjectName}({{ {options.RequestObjectField} : this.DataModel }} ));");
                StringBuilder.AppendLine(
                    $"if(Response.Count >0) this.Message = 'Updated ' + Response.Count + ' {T.Name}'");
                StringBuilder.AppendLine("else this.Message = 'Did Not Update {T.Name} -- odd' ");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("catch(e) {");
                StringBuilder.AppendLine("console.log(e)");
                StringBuilder.AppendLine(
                    "this.Message = e.message");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("</script>");
                StringBuilder.AppendLine("<style></style>");
                return StringBuilder.ToString();
            }

            public class UpdateViewOptions
            {
                public string ComponentName { get; set; }
                public string RequestObjectField { get; set; }
                public string HttpVerb { get; set; }
                public string RequestObjectName { get; set; }
                public string ListOneHttpVerb { get; set; }
                public string ListOneRequestObjectName { get; set; }
                public string ListOneRequestObjectField { get; set; }
                public Type[] ReferencedByTypes { get; set; }
                public bool DisableUpdate { get; set; }
            }
        }
    }

    public static class ExtensionMethods
    {
        public static string ToUnderscoreCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString()))
                .ToLower();
        }
    }
}