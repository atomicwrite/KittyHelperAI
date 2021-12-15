using System;
using System.Text;
using KittyHelper.Options;

namespace KittyHelper.ServiceGenerators
{
    public static partial class KittyServiceHelper
    {
        public static string CreateGetByIdEndpointServiceClass(Type t, CreateGetByIdEndpointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateGetByIdEndpointOptions(t);

            str.AppendLine($"public class {options.ServiceType} : ServiceStack.Service {{");
            var functionContents =
                $@"public {options.ReturnType} {options.HttpVerb}({options.RequestType} {options.RequestObjectName}){{
                    {options.GenerateUserLookUp()}
                    var data = Db.Single<{t.Name}>(a=>{options.GenerateUserLookUp()}  a.{options.IdField} == {options.RequestObjectName}.{options.RequestIdField});
                    return new {options.ReturnType}(){{
                        {t.Name} = data
                    }} ;          
                }}";
            str.AppendLine(options.Annotations);
            str.AppendLine(functionContents);
            str.AppendLine("}");


            return str.ToString();
        }

        public static string CreateGetByIdEndpointRequestClass(Type t, CreateGetByIdEndpointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateGetByIdEndpointOptions(t);
            var classContents = $@"public class {options.RequestType} :IReturn<{options.ReturnType}> {{ 
              public int {options.RequestIdField} {{get;set;}}

 }}";
            str.AppendLine(classContents);
            return str.ToString();
        }

        public static string CreateGetByIdEndpointResponseClass(Type t, CreateGetByIdEndpointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateGetByIdEndpointOptions(t);
            var classContents = $@"public class {options.ReturnType} {{ 
                public bool Success {{get;set;}}
               public {t.Name} {t.Name} {{get;set;}}

 }}";
            str.AppendLine(classContents);
            return str.ToString();
        }
    }
}