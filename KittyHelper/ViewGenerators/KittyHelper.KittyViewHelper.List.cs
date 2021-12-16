using System;
using System.Linq;
using System.Text;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
        public static partial class KittyViewHelper
        {
            public static string GenerateListPage(Type T, ListViewOptions options)
            {
                StringBuilder StringBuilder = new();
                StringBuilder.AppendLine("<template>");
                StringBuilder.AppendLine("<section><div class='container'> ");
                StringBuilder.AppendLine($"<h4>List {T.Name}</h4>");
                StringBuilder.AppendLine(GenerateVueButton("Previous", "Previous"));
                StringBuilder.AppendLine(GenerateVueButton("{{After}}", ""));
                StringBuilder.AppendLine(GenerateVueButton("Next", "Next"));
                StringBuilder.AppendLine(GenerateVueTextInput("Search", "Search", "", "SearchText"));
                StringBuilder.AppendLine("    <b-alert show v-if=\"Message.length >0\">{{  Message }} </b-alert>");

                var FieldInfos = T.GetProperties();
                var DisplayField = FieldInfos.FirstOrDefault(a =>
                    a.GetCustomAttributesData().Any(b => b.AttributeType.Name == "DisplayTitleAttribute"));

                var DisplayText = FieldInfos.FirstOrDefault(a =>
                    a.GetCustomAttributesData().Any(b => b.AttributeType.Name == "DisplayTextAttribute"));

                var PrimaryKey = FieldInfos.FirstOrDefault(a =>
                    a.GetCustomAttributesData().Any(b => b.AttributeType.Name == "PrimaryKeyAttribute"));

                var KeyField = PrimaryKey != null ? $"a.{PrimaryKey.Name}" : "";
                var DisplayFieldName = DisplayField != null ? $"a.{DisplayField.Name}" : "``";
                var DisplayTextName = DisplayText != null ? $"a.{DisplayText.Name}" : "``";
                var vFor = "v-for=\"a of DataModel\"";
                StringBuilder.AppendLine(GenerateVueCard(DisplayFieldName, "``", DisplayTextName, GenerateVueButton(
                    "Edit",
                    $"Edit({KeyField})"), vFor));


                StringBuilder.AppendLine("</div></section>");
                StringBuilder.AppendLine("</template>");

                StringBuilder.AppendLine(@"<script lang=""ts"">");
                StringBuilder.AppendLine($@"
import {{Component, Vue,Watch}} from 'vue-property-decorator';
import {{client}} from '@/shared';
import {{ {options.RequestObjectName}, {T.Name} }} from '@/shared/dtos';");

                StringBuilder.AppendLine($@"@Component({{
    components: {{}},
}})
export default class {options.ComponentName} extends Vue {{");
                StringBuilder.AppendLine("SearchText:string=''");

                StringBuilder.AppendLine(
                    $"@Watch('SearchText') changed(old:string,newx:string) {{  this.List{T.Name}();  }}");
                StringBuilder.AppendLine("Message : string = \"\"");
                StringBuilder.AppendLine("Loading : boolean = false");
                StringBuilder.AppendLine("Error : boolean = false");
                StringBuilder.AppendLine($"DataModel : {T.Name}[] = []");
                StringBuilder.AppendLine("After : number= 0");
                StringBuilder.AppendLine($"async List{T.Name}() {{");
                StringBuilder.AppendLine("try{");
                StringBuilder.AppendLine("this.Error = false;");
                StringBuilder.AppendLine("this.Message = '';");
                StringBuilder.AppendLine(
                    $" const Response = await client.{options.HttpVerb.ToLower()}(new {options.RequestObjectName}({{ {options.RequestObjectField} : this.After, SearchText:this.SearchText }} ));");
                StringBuilder.AppendLine($"this.DataModel = Response.{options.ResponseObjectField}");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("catch(e) {");
                StringBuilder.AppendLine("console.log(e)");
                StringBuilder.AppendLine(
                    "this.Message = e.message");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine(
                    $" Previous(){{  this.After =  this.After > 50? this.After - 50:0;   this.List{T.Name}();   ; }} ");
                StringBuilder.AppendLine($" created(){{  this.List{T.Name}(); }} ");
                StringBuilder.AppendLine(
                    $" Next(){{  this.After  = this.DataModel[this.DataModel.length -1].{options.DataBaseObjectIdField} +1  ;  this.List{T.Name}(); }} ");
                StringBuilder.AppendLine($" Edit(id:number){{  this.$router.push('{options.EditObjectRoute}'+id );}} ");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("</script>");
                StringBuilder.AppendLine("<style></style>");
                return StringBuilder.ToString();
            }

            public class ListViewOptions
            {
                public string ComponentName { get; set; }
                public string RequestObjectField { get; set; }
                public string HttpVerb { get; set; }
                public string RequestObjectName { get; set; }
                public string ResponseObjectField { get; set; }
                public string EditObjectRoute { get; set; }
                public string DataBaseObjectIdField { get; set; }
            }
        }
    }
}