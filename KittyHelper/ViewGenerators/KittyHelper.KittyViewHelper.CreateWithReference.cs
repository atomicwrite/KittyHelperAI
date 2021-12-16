using System;
using System.Linq;
using System.Text;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
        public static partial class KittyViewHelper
        {
            public static string GenerateCreateWithReferencePage(Type T, CreateWithReferenceViewOptions options)
            {
                StringBuilder StringBuilder = new();
                StringBuilder.AppendLine("<template>");
                StringBuilder.AppendLine("<section><div class='container'> ");
                StringBuilder.AppendLine($"<h4>Create  {T.Name}</h4>");

                StringBuilder.AppendLine("    <b-alert show v-if=\"Message.length >0\">{{  Message }} </b-alert>");

                var FieldInfos = T.GetProperties();
                foreach (var field in FieldInfos)
                {
                    var CustomAttributesData = field.GetCustomAttributesData();
                    if (CustomAttributesData.Any(a => a.AttributeType.Name == "AutoIncrementAttribute")) continue;

                    switch (field.PropertyType.Name)
                    {
                        case "String":
                            StringBuilder.AppendLine(GenerateVueTextInput(field));
                            break;
                        case "Int32":
                        case "Int64":
                        case "Double":
                        case "Float":

                            StringBuilder.AppendLine(GenerateVueNumberInput(field));
                            break;
                        case "DateTime":
                            StringBuilder.AppendLine(GenerateVueDateTimeInput(field));
                            break;
                    }
                }

                StringBuilder.AppendLine(GenerateVueButton("Create", $"CreateWithReference{T.Name}"));
                StringBuilder.AppendLine(GenerateVueButton("Back", "Back"));
                StringBuilder.AppendLine("</div></section>");
                StringBuilder.AppendLine("</template>");

                StringBuilder.AppendLine(@"<script lang=""ts"">");
                StringBuilder.AppendLine($@"
import {{Component, Vue}} from 'vue-property-decorator';
import {{client}} from '@/shared';
import {{ {options.RequestObjectName}, {T.Name} }} from '@/shared/dtos';");

                StringBuilder.AppendLine($@"@Component({{
    components: {{}},
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

                StringBuilder.AppendLine("Back(){ this.$router.go(-1)}");
                StringBuilder.AppendLine($"async CreateWithReference{T.Name}() {{");
                StringBuilder.AppendLine("try{");
                StringBuilder.AppendLine("this.Error = false;");
                StringBuilder.AppendLine("this.Message = '';");
                StringBuilder.AppendLine($"this.DataModel.{options.ReferenceField}  =  +this.$route.params['id']");
                StringBuilder.AppendLine(
                    $" const Response = await client.{options.HttpVerb.ToLower()}(new {options.RequestObjectName}({{ {options.RequestObjectField} : this.DataModel }} ));");
                StringBuilder.AppendLine(
                    $"if(Response.Id >0) this.Message = 'CreateWithReferenced ' + Response.Id + ' {T.Name}'");
                StringBuilder.AppendLine("else this.Message = 'Did Not CreateWithReference {T.Name} -- odd' ");
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

            public class CreateWithReferenceViewOptions
            {
                public string ComponentName { get; set; }
                public string RequestObjectField { get; set; }
                public string HttpVerb { get; set; }
                public string RequestObjectName { get; set; }
                public string ReferenceField { get; set; }
            }
        }
    }
}