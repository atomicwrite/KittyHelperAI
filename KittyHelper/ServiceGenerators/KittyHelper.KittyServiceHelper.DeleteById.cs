using System;
using System.Text;
using KittyHelper.Options;

namespace KittyHelper.ServiceGenerators
{
    public static partial class KittyServiceHelper
    {
        public static string CreateDeleteByIdEndpointServiceClass(Type t,
            CreateDeleteByIdEndPointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateDeleteByIdEndPointOptions(t);

            str.AppendLine($"public class {options.ServiceType} : ServiceStack.Service {{");
            var functionContents =
                $@"public {options.ReturnType} {options.HttpVerb}({options.RequestType} {options.RequestObjectName}){{
                    
                    {options.GenerateUserLookUp()}          
                   var Count= Db.Delete<{t.Name}>( a=> {options.GenerateAuthIfNeeded()}   {options.RequestObjectName}.{options.RequestObjectField}.Contains(a.{options.DatabaseObjectIdField} ));
                    return new {options.ReturnType}(){{
                         
                        Count  = Count
                    }} ;          
                }}";
            str.AppendLine(options.Annotations);
            str.AppendLine(functionContents);
            str.AppendLine("}");


            return str.ToString();
        }

        public static string CreateDeleteByIdEndpointRequestClass(Type t,
            CreateDeleteByIdEndPointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateDeleteByIdEndPointOptions(t);
            var classContents = $@"public class {options.RequestType} :IReturn<{options.ReturnType}> {{ 
              public List<int> {options.RequestObjectField} {{get;set;}}

 }}";
            str.AppendLine(classContents);
            return str.ToString();
        }

        public static string CreateDeleteByIdEndpointResponseClass(Type t,
            CreateDeleteByIdEndPointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateDeleteByIdEndPointOptions(t);
            var classContents = $@"public class {options.ReturnType} {{ 
                public long Count {{get;set;}}

 }}";
            str.AppendLine(classContents);
            return str.ToString();
        }

        public class CreateDeleteByIdEndPointOptions : CreateOptions
        {
            public CreateDeleteByIdEndPointOptions(Type t, string baseType = null,
                CreateOptionsAuthenticationOptions authenticate = null,
                string[] requiredRoles = null) : base(t, baseType ?? "DeleteById" + t.Name, authenticate)
            {
                DatabaseObjectIdField = "Id";

                RequestObjectField = "Ids";
            }

            public string RequestObjectField { get; set; }


            public string DatabaseObjectIdField { get; set; }
        }
    }
}