using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ServiceStack;
using KittyHelper.Options;

namespace KittyHelper.ServiceGenerators
{
    
        public static partial class KittyServiceHelper
        {
            public static string CreateListEndpointServiceClass(Type t, CreateListEndPointOptions options = null)
            {
                StringBuilder str = new();
                options ??= new CreateListEndPointOptions(t);
                var ReferenceField = t.Properties().FirstOrDefault(a =>
                    a.CustomAttributes.Any(b =>
                        b.AttributeType.Name == "ReferencesAttribute" &&
                        Nullable.GetUnderlyingType(a.PropertyType) == null));
                var DisplayField = t.GetProperties().FirstOrDefault(a =>
                    a.GetCustomAttributesData().Any(b => b.AttributeType.Name == "DisplayTitleAttribute"));
                str.AppendLine($"public class {options.ServiceType} : ServiceStack.Service {{");
                string WhereFilter = "";
                string ReferenceFilter = "";
                if (DisplayField != null)
                {
                    WhereFilter =
                        $@"if(!string.IsNullOrEmpty({options.RequestObjectName}.SearchText)) sql=sql.Where(a=>a.{DisplayField.Name}.Contains({options.RequestObjectName}.SearchText));" +
                        Environment.NewLine;
                }

                if (ReferenceField != null)
                {
                    //ReferenceId
                    WhereFilter +=
                        $@"if({options.RequestObjectName}.ReferenceId > 0) sql=sql.Where(a=>a.{ReferenceField.Name} ==  {options.RequestObjectName}.ReferenceId);" +
                        Environment.NewLine;
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
                StringBuilder str = new();
                options ??= new CreateListEndPointOptions(t);
                var classContents = $@"public class {options.RequestType} :IReturn<{options.ReturnType}> {{ 
              public int {options.RequestObjectAfterField} {{get;set;}}
                public string  SearchText {{get;set;}} 
                public int ReferenceId {{get;set;}}

 }}";
                str.AppendLine(classContents);
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
 