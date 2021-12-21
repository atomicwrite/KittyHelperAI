// using System;
// using System.Text;
// using KittyHelper.Options;
// using static KittyHelper.KittyHelper.KittyViewHelper;
//
// namespace KittyHelper.ServiceGenerators
// {
//     public static partial class KittyServiceHelper
//     {
//         public static string CreateUpdateEndpointServiceClass<A>( CreateUpdateEndPointOptions<A> options = null)
//         {
//             var t = typeof(A);
//             StringBuilder str = new();
//             options ??= new CreateUpdateEndPointOptions<A>();
//
//             str.AppendLine($"public class {options.ServiceType} : ServiceStack.Service {{");
//             var functionContents =
//                 $@"public {options.ResponseObjectType} {options.HttpVerb}({options.RequestObjectType} {options.RequestObjectName}){{
//                     {options.GenerateUserLookUp()}         
//                     {options.GenerateAssignToUser()}          
//                    var Count= Db.Update( {options.RequestObjectName}.{options.RequestObjectUpdateObjectField} );
//                     return new {options.ResponseObjectType}(){{
//                          
//                         Count  = Count
//                     }} ;          
//                 }}";
//             str.AppendLine(options.Annotations);
//             str.AppendLine(functionContents);
//             str.AppendLine("}");
//
//
//             return str.ToString();
//         }
//
//         public static string CreateUpdateEndpointRequestClass<A>(CreateUpdateEndPointOptions<A> options = null)
//         {
//             var t = typeof(A);
//             StringBuilder str = new();
//             options ??= new CreateUpdateEndPointOptions<A>();
//             var classContents = $@"public class {options.RequestObjectType} :IReturn<{options.ResponseObjectType}> {{ 
//               public {t.Name} {options.RequestObjectUpdateObjectField} {{get;set;}}
//
//  }}";
//             str.AppendLine(classContents);
//             return str.ToString();
//         }
//
//         public static string CreateUpdateEndpointResponseClass<A>(CreateUpdateEndPointOptions<A> options = null)
//         {
//             var t = typeof(A);
//             StringBuilder str = new();
//             options ??= new CreateUpdateEndPointOptions<A>();
//             var classContents = $@"public class {options.ResponseObjectType} {{ 
//                 public long Count {{get;set;}}
//
//  }}";
//             str.AppendLine(classContents);
//             return str.ToString();
//         }
//


//     }
// }