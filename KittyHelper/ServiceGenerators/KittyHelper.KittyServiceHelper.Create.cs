using System;
using System.Text;
using KittyHelper.Options;

namespace KittyHelper.ServiceGenerators
{
    
        public static partial class KittyServiceHelper
        {
            /// <summary>
            /// I keep the option types by the functions that use them. It really helps clarity. 
            /// 
            /// </summary>
            public class CreateCreateEndPointOptions : CreateOptions
            {
                public CreateCreateEndPointOptions(Type t, string baseType = null,
                    CreateOptionsAuthenticationOptions authenticate = null,
                    string[] requiredRoles = null) : base(t, baseType ?? "Create" + t.Name, authenticate)
                {
                    RequestObjectNewObjectField = t.Name;
                }


                public string RequestObjectNewObjectField { get; set; }
            }

            public static string CreateCreateEndpointServiceClass(Type t, CreateCreateEndPointOptions options = null)
            {
                StringBuilder str = new();
                options ??= new CreateCreateEndPointOptions(t);

                str.AppendLine($"public class {options.ServiceType} : ServiceStack.Service {{");
                var functionContents =
                    $@"public {options.ReturnType} {options.HttpVerb}({options.RequestType} {options.RequestObjectName}){{
                    {options.GenerateUserLookUp()}         
                    {options.GenerateAssignToUser()}          
                   var Id= Db.Insert( {options.RequestObjectName}.{options.RequestObjectNewObjectField},true);
                    return new {options.ReturnType}(){{
                         
                        Id  = Id
                    }} ;          
                }}";
                str.AppendLine(options.Annotations);
                str.AppendLine(functionContents);
                str.AppendLine("}");


                return str.ToString();
            }

            public static string CreateCreateEndpointRequestClass(Type t, CreateCreateEndPointOptions options = null)
            {
                StringBuilder str = new();
                options ??= new CreateCreateEndPointOptions(t);
                var classContents = $@"public class {options.RequestType} :IReturn<{options.ReturnType}> {{ 
              public {t.Name} {options.RequestObjectNewObjectField} {{get;set;}}

 }}";
                str.AppendLine(classContents);
                return str.ToString();
            }

            public static string CreateCreateEndpointResponseClass(Type t, CreateCreateEndPointOptions options = null)
            {
                StringBuilder str = new();
                options ??= new CreateCreateEndPointOptions(t);
                var classContents = $@"public class {options.ReturnType} {{ 
                public long Id {{get;set;}}

 }}";
                str.AppendLine(classContents);
                return str.ToString();
            }
        }
    }
 