using ServiceStack;
using System;
using System.Linq;
using System.Text;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
        public static partial class KittyViewHelper
        {
            public static string GenerateListFromReferencePage(Type T, ListFromReferenceViewOptions options)
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
                string buttonVue = GenerateVueButton(
                                    "Edit",
                                    $"Edit({KeyField})") + GenerateVueButton("Select",
                                    $"Select(a)","v-if=\"select_mode\"") + GenerateVueButton("UnSelect",
                                    $"UnSelect(a)", "v-if=\"!select_mode\"");
                StringBuilder.AppendLine(GenerateVueCard(DisplayFieldName, "``", DisplayTextName, buttonVue, vFor));


                StringBuilder.AppendLine("</div></section>");
                StringBuilder.AppendLine("</template>");

                StringBuilder.AppendLine(@"<script lang=""ts"">");
                StringBuilder.AppendLine($@"
import {{Component, Vue,Watch,Prop}} from 'vue-property-decorator';
import {{client}} from '@/shared';
import {{ {options.RequestObjectName}, {T.Name} }} from '@/shared/dtos';");

                StringBuilder.AppendLine($@"@Component({{
    components: {{}},
}})
export default class {options.ComponentName} extends Vue {{");
                StringBuilder.AppendLine("SearchText:string=''");

                StringBuilder.AppendLine(
                    $"@Watch('SearchText') changed(old:string,newx:string) {{  this.ListFromReference{T.Name}();  }}");
                StringBuilder.AppendLine("Message : string = \"\"");

                var AllRefFields = T.Properties().Where(a =>
      a.CustomAttributes.Any(b =>
          b.AttributeType.Name == "ReferencesAttribute")).ToArray();
                StringBuilder.AppendLine($" @Prop(Number) readonly  select_mode : boolean ");
                foreach (var field in AllRefFields)
                {
                    StringBuilder.AppendLine($" @Prop(Number) readonly  {field.Name.ToLower()}_reference_id : number ");
                    StringBuilder.AppendLine($" @Prop(Number) readonly  {field.Name.ToLower()}_anti : boolean ");
                }
                
                StringBuilder.AppendLine("Loading : boolean = false");
                StringBuilder.AppendLine("Error : boolean = false");
                StringBuilder.AppendLine($"DataModel : {T.Name}[] = []");
                StringBuilder.AppendLine("After : number= 0");
                StringBuilder.AppendLine($"async ListFromReference{T.Name}() {{");
                StringBuilder.AppendLine("try{");
                StringBuilder.AppendLine("this.Error = false;");
                StringBuilder.AppendLine("this.Message = '';");
                StringBuilder.AppendLine(
                    $" const Response = await client.{options.HttpVerb.ToLower()}(new {options.RequestObjectName}({{ ");
                StringBuilder.AppendLine(" {options.RequestObjectField} : this.After, SearchText:this.SearchText");
                foreach (var field in AllRefFields)
                {
                    StringBuilder.AppendLine(
                        $" {field.Name}ReferenceId:this. {field.Name.ToLower()}_reference_id,   }} ));");
                    StringBuilder.AppendLine(
                       $" {field.Name}AntiReference:this. {field.Name.ToLower()}_anti,   }} ));");
                }
                StringBuilder.AppendLine($"this.DataModel = Response.{options.ResponseObjectField}");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("catch(e) {");
                StringBuilder.AppendLine("console.log(e)");
                StringBuilder.AppendLine(
                    "this.Message = e.message");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine(
                    $" Previous(){{  this.After =  this.After > 50? this.After - 50:0;   this.ListFromReference{T.Name}();   ; }} ");
                StringBuilder.AppendLine($" created(){{  this.ListFromReference{T.Name}(); }} ");
                StringBuilder.AppendLine(
                    $" Next(){{  this.After  = this.DataModel[this.DataModel.length -1].{options.DataBaseObjectIdField} +1  ;  this.ListFromReference{T.Name}(); }} ");
                StringBuilder.AppendLine($" Edit(id:number){{  this.$router.push('{options.EditObjectRoute}'+id );}} ");
                StringBuilder.AppendLine($" async Update{T.Name}(item:{T.Name}){{  ");
                StringBuilder.AppendLine("try{");
                StringBuilder.AppendLine(
                $" const Response = await client.{options.UpdateHttpVerb.ToLower()}(new {options.UpdateObjectName}({{ {options.UpdateObjectNameField} : a }} ));");
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
                //this.$emit('select', item)}}; this.ListFromReference{T.Name}();  ");
                StringBuilder.AppendLine($" async Select(item:{T.Name}){{ item.{options.ReferenceFieldToUpdate} = this.{T.Name.ToLower()}_reference_id;  this.$emit('select', item); await this.Update{T.Name}(item); await  this.ListFromReference{T.Name}(); }} ");
                StringBuilder.AppendLine($" async UnSelect(item:{T.Name}){{ item.{options.ReferenceFieldToUpdate} = 0;  this.$emit('unselect', item); await this.Update{T.Name}(item); await  this.ListFromReference{T.Name}(); }} ");
                StringBuilder.AppendLine("}");
                StringBuilder.AppendLine("</script>");
                StringBuilder.AppendLine("<style></style>");
                return StringBuilder.ToString();
            }

            public class ListFromReferenceViewOptions
            {
                public string ComponentName { get; set; }
                public string RequestObjectField { get; set; }
                public string HttpVerb { get; set; }
                public string RequestObjectName { get; set; }
                public string ResponseObjectField { get; set; }
                public string EditObjectRoute { get; set; }
                public string DataBaseObjectIdField { get; set; }
                public string UpdateObjectName { get; internal set; }
                public string UpdateHttpVerb { get; internal set; }
                public string UpdateObjectNameField { get; internal set; }
                public string ReferenceFieldToUpdate { get; internal set; }
            }
        }
    }
}