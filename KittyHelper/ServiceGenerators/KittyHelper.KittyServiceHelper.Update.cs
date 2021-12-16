using System;
using System.Text;
using KittyHelper.Options;
using static KittyHelper.KittyHelper.KittyViewHelper;

namespace KittyHelper.ServiceGenerators
{
    public static partial class KittyServiceHelper
    {
        public static string CreateUpdateEndpointServiceClass(Type t, CreateUpdateEndPointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateUpdateEndPointOptions(t);

            str.AppendLine($"public class {options.ServiceType} : ServiceStack.Service {{");
            var functionContents =
                $@"public {options.ReturnType} {options.HttpVerb}({options.RequestType} {options.RequestObjectName}){{
                    {options.GenerateUserLookUp()}         
                    {options.GenerateAssignToUser()}          
                   var Count= Db.Update( {options.RequestObjectName}.{options.RequestObjectUpdateObjectField} );
                    return new {options.ReturnType}(){{
                         
                        Count  = Count
                    }} ;          
                }}";
            str.AppendLine(options.Annotations);
            str.AppendLine(functionContents);
            str.AppendLine("}");


            return str.ToString();
        }

        public static string CreateUpdateEndpointRequestClass(Type t, CreateUpdateEndPointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateUpdateEndPointOptions(t);
            var classContents = $@"public class {options.RequestType} :IReturn<{options.ReturnType}> {{ 
              public {t.Name} {options.RequestObjectUpdateObjectField} {{get;set;}}

 }}";
            str.AppendLine(classContents);
            return str.ToString();
        }

        public static string CreateUpdateEndpointResponseClass(Type t, CreateUpdateEndPointOptions options = null)
        {
            StringBuilder str = new();
            options ??= new CreateUpdateEndPointOptions(t);
            var classContents = $@"public class {options.ReturnType} {{ 
                public long Count {{get;set;}}

 }}";
            str.AppendLine(classContents);
            return str.ToString();
        }

        public class CreateUpdateEndPointOptions : CreateOptions
        {
            public CreateUpdateEndPointOptions(Type t, string baseType = null,
                CreateOptionsAuthenticationOptions authenticate = null,
                string[] requiredRoles = null) : base(t, baseType ?? "Update" + t.Name, authenticate)
            {
                RequestObjectUpdateObjectField = t.Name;
            }

             public TypeScriptClass  UpdateMixin { get; set; }
            public string RequestObjectUpdateObjectField { get; set; }
        }
    }
}