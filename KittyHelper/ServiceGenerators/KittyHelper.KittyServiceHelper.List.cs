using System;
using System.Linq;
using System.Text;
using KittyHelper.Options;
using ServiceStack;

namespace KittyHelper.ServiceGenerators
{
    public static partial class KittyServiceHelper
    {
        public static string CreateListEndpointServiceClass(Type t, CreateListEndPointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateListEndPointOptions(t);
            var AllRefFields = t.Properties().Where(a =>
                a.CustomAttributes.Any(b =>
                    b.AttributeType.Name == "ReferencesAttribute")).ToArray();
 
            var DisplayField = t.GetProperties().FirstOrDefault(a =>
                a.GetCustomAttributesData().Any(b => b.AttributeType.Name == "DisplayTitleAttribute"));
            str.AppendLine($"public class {options.ServiceType} : ServiceStack.Service {{");
            var WhereFilter =  new StringBuilder();

            if (DisplayField != null)
                WhereFilter.AppendLine(
                    $@"if(!string.IsNullOrEmpty({options.RequestObjectName}.SearchText)) sql=sql.Where(a=>a.{DisplayField.Name}.Contains({options.RequestObjectName}.SearchText));");

            foreach (var ReferenceField in AllRefFields)
            {     //ReferenceId
                WhereFilter.AppendLine($@"if({options.RequestObjectName}.{ReferenceField.Name}ReferenceId > 0) ");
                WhereFilter.AppendLine($@"if(!{options.RequestObjectName}.{ReferenceField.Name}AntiReference) ");
                WhereFilter.AppendLine($"sql=sql.Where(a=>a.{ReferenceField.Name} ==  {options.RequestObjectName}.{ReferenceField.Name}ReferenceId);");
                WhereFilter.AppendLine("else");
                WhereFilter.AppendLine($"sql=sql.Where(a=>a.{ReferenceField.Name} !=  {options.RequestObjectName}.{ReferenceField.Name}ReferenceId);");
            }
            var functionContents =
                $@"public {options.ReturnType} {options.HttpVerb}({options.RequestType} {options.RequestObjectName}){{
                    {options.GenerateUserLookUp()}                   
                    var sql = Db.From<{t.Name}>();
                      {WhereFilter}  
                    var Count = Db.Count(sql);
                    sql = sql.Where(a=>{options.GenerateUserLookUp()}  a.{options.DbModelIdfield} > {options.RequestObjectName}.{options.RequestObjectAfterField}).OrderBy(a=>a.{options.DbModelIdfield}).Limit({options.RecordReturnCountLimit});
                    var data = Db.Select(sql);
                    return new {options.ReturnType}(){{
                        {t.Name}s = data,
                        Count  = Count
                    }} ;          
                }}";
            str.AppendLine(options.Annotations);
            str.AppendLine(functionContents);
            str.AppendLine("}");


            return str.ToString();
        }

        public static string CreateListEndpointRequestClass(Type t, CreateListEndPointOptions options = null)
        {
            var ReferenceFields = t.Properties().Where(a =>
             a.CustomAttributes.Any(b =>
                 b.AttributeType.Name == "ReferencesAttribute" )).ToArray();

           

            StringBuilder str = new();
            options ??= new CreateListEndPointOptions(t);

            str.AppendLine( $@"public class {options.RequestType} :IReturn<{options.ReturnType}> {{ 
              public int {options.RequestObjectAfterField} {{get;set;}}
                public string  SearchText {{get;set;}} ");
            foreach (var ReferenceField in ReferenceFields)
            {
                str.AppendLine($@"    
                public int {ReferenceField.Name}ReferenceId {{get;set;}}
                public bool {ReferenceField.Name}AntiReference {{get;set;}}");
            }
                str.AppendLine("}");
            
            return str.ToString();
        }

        public static string CreateListEndpointResponseClass(Type t, CreateListEndPointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateListEndPointOptions(t);
            var classContents = $@"public class {options.ReturnType} {{ 
                public bool Success {{get;set;}} 
               public List<{t.Name}> {options.ResponseObjectFieldName} {{get;set;}}
                public long Count {{get;set;}}

 }}";
            str.AppendLine(classContents);
            return str.ToString();
        }
    }
}