using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KittyHelper.Options;
using ServiceStack;
using static KittyHelper.KittyHelper.KittyViewHelper;

namespace KittyHelper
{
    public static class CrudHelper
    {
        private static void GenerateMigration(Type type, KittyHelper.ProjectWriter projectWriter, bool ow)
        {
            var fileName = type.Name + ".cs";
            projectWriter.WriteCsMigrationFile(
                DatabaseGenerators.KittyHelper.MigrationHelper.GenerateCreateIfNotExists(type), fileName,
                ow);
        }

        private static void GenerateCreateDeleteByIdEndpointService(Type t, KittyHelper.ProjectWriter projectWriter,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace)) throw new ArgumentException("Type must have a name space");

            var ServiceClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndPointOptions(t);

            var RequestClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndPointOptions(t);

            var ResponseClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndPointOptions(t);

            var serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            var responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndpointResponseClass(t,
                    ResponseClassEndpointOptions);


            var requestClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndpointRequestClass(t,
                    RequestClassEndpointOptions);

            var RequestType = ServiceClassEndpointOptions.RequestType;
            var ReturnType = ServiceClassEndpointOptions.ReturnType;
            var ServiceType = ServiceClassEndpointOptions.ServiceType;

            KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition,
                serviceClassDefinition,
                ServiceClassEndpointOptions, OverWrite);
        }

        private static ServiceGenerators.KittyServiceHelper.CreateUpdateEndPointOptions
            GenerateCreateUpdateEndpointService(Type t,
                KittyHelper.ProjectWriter projectWriter, bool OverWrite, Type[] types, bool create)
        {
            if (string.IsNullOrEmpty(t.Namespace)) throw new ArgumentException("Type must have a name space");

            var ServiceClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateUpdateEndPointOptions(t);


            var serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateUpdateEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            var responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateUpdateEndpointResponseClass(t,
                    ServiceClassEndpointOptions);


            var requestClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateUpdateEndpointRequestClass(t, ServiceClassEndpointOptions);

            var RequestType = ServiceClassEndpointOptions.RequestType;
            var ReturnType = ServiceClassEndpointOptions.ReturnType;
            var ServiceType = ServiceClassEndpointOptions.ServiceType;
            if (create)
                KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition,
                    serviceClassDefinition,
                    ServiceClassEndpointOptions, OverWrite);

            var ComponentName = "Update" + t.Name;
            var VueFileName = $"{ComponentName}.vue";
            var Updateview = KittyHelper.KittyViewHelper.GenerateUpdatePage(t,
                new KittyHelper.KittyViewHelper.UpdateViewOptions
                {
                    ComponentName = ComponentName,
                    HttpVerb = ServiceClassEndpointOptions.HttpVerb,
                    RequestObjectName = ServiceClassEndpointOptions.RequestType,
                    RequestObjectField = ServiceClassEndpointOptions.RequestObjectUpdateObjectField,
                    ListOneHttpVerb = "get",
                    ListOneRequestObjectField = "Id",
                    ListOneRequestObjectName = "Get" + t.Name + "ByIdRequest",
                    ReferencedByTypes = types,
                    DisableUpdate = !create,
                    
                });
            projectWriter.WriteVueFile(t, VueFileName, Updateview, OverWrite);
            return ServiceClassEndpointOptions;
        }

        private static void GenerateCreateCreateWithReferenceEndpointService(Type t,
            KittyHelper.ProjectWriter projectWriter,
            PropertyInfo referenceField,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace)) throw new ArgumentException("Type must have a name space");


            var ServiceClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateCreateEndPointOptions(t);


            var serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateCreateEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            var responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateCreateEndpointResponseClass(t,
                    ServiceClassEndpointOptions);


            var requestClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateCreateEndpointRequestClass(t, ServiceClassEndpointOptions);
            var ComponentName = "Create" + t.Name;

            var CreateVueFile = KittyHelper.KittyViewHelper.GenerateCreateWithReferencePage(t,
                new KittyHelper.KittyViewHelper.CreateWithReferenceViewOptions
                {
                    ComponentName = ComponentName,
                    HttpVerb = ServiceClassEndpointOptions.HttpVerb,
                    RequestObjectName = ServiceClassEndpointOptions.RequestType,
                    RequestObjectField = ServiceClassEndpointOptions.RequestObjectNewObjectField,
                    ReferenceField = referenceField.Name
                });

            var RequestType = ServiceClassEndpointOptions.RequestType;
            var ReturnType = ServiceClassEndpointOptions.ReturnType;
            var ServiceType = ServiceClassEndpointOptions.ServiceType;
            var VueFileName = $"{ComponentName}.vue";

            KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition,
                serviceClassDefinition,
                ServiceClassEndpointOptions, OverWrite);

            projectWriter.WriteVueFile(t, VueFileName, CreateVueFile, OverWrite);
        }

        private static void GenerateCreateCreateEndpointService(Type t, KittyHelper.ProjectWriter projectWriter,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace)) throw new ArgumentException("Type must have a name space");


            var ServiceClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateCreateEndPointOptions(t);


            var serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateCreateEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            var responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateCreateEndpointResponseClass(t,
                    ServiceClassEndpointOptions);


            var requestClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateCreateEndpointRequestClass(t, ServiceClassEndpointOptions);
            var ComponentName = "Create" + t.Name;

            var CreateVueFile = KittyHelper.KittyViewHelper.GenerateCreatePage(t,
                new KittyHelper.KittyViewHelper.CreateViewOptions
                {
                    ComponentName = ComponentName,
                    HttpVerb = ServiceClassEndpointOptions.HttpVerb,
                    RequestObjectName = ServiceClassEndpointOptions.RequestType,
                    RequestObjectField = ServiceClassEndpointOptions.RequestObjectNewObjectField,
                    ResponseObjectName = ServiceClassEndpointOptions.ReturnType
                });

            var RequestType = ServiceClassEndpointOptions.RequestType;
            var ReturnType = ServiceClassEndpointOptions.ReturnType;
            var ServiceType = ServiceClassEndpointOptions.ServiceType;
            var VueFileName = $"{ComponentName}.vue";

            KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition,
                serviceClassDefinition,
                ServiceClassEndpointOptions, OverWrite);

            projectWriter.WriteVueFile(t, VueFileName, CreateVueFile, OverWrite);
        }

        private static CreateListEndPointOptions GenerateCreateListWithReferenceEndpointService(Type t,
            KittyHelper.ProjectWriter projectWriter,TypeScriptClass updateMixin,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace)) throw new ArgumentException("Type must have a name space");

            var ComponentName = "ListWithReference" + t.Name;
            var ServiceClassEndpointOptions =
                new CreateListEndPointOptions(t);


            var VueFileName = $"{ComponentName}.vue";
            var ListVueContents = GenerateListFromReferencePage(t,
                new KittyHelper.KittyViewHelper.ListFromReferenceViewOptions
                {
                    ComponentName = ComponentName,
                    HttpVerb = ServiceClassEndpointOptions.HttpVerb,
                    RequestObjectName = ServiceClassEndpointOptions.RequestType,
                    RequestObjectField = ServiceClassEndpointOptions.RequestObjectAfterField,
                    ResponseObjectField = ServiceClassEndpointOptions.ResponseObjectFieldName,
                    EditObjectRoute = $"/Update{t.Name}/",
                    DataBaseObjectIdField = "Id",
                    UpdateHttpVerb="Post",
                    ReferenceFieldToUpdate= t.Name,
                    UpdateObjectName = "Update" + t.Name + "Request",
                    UpdateObjectNameField= t.Name,
                    ResponseObjectName=ServiceClassEndpointOptions.ReturnType
                }
                , updateMixin
                );

            var RequestType = ServiceClassEndpointOptions.RequestType;
            var ReturnType = ServiceClassEndpointOptions.ReturnType;
            var ServiceType = ServiceClassEndpointOptions.ServiceType;


            projectWriter.WriteVueFile(t, VueFileName, ListVueContents, OverWrite);
            return ServiceClassEndpointOptions;
        }

        private static CreateListEndPointOptions GenerateCreateListEndpointService(Type t,
            KittyHelper.ProjectWriter projectWriter,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace)) throw new ArgumentException("Type must have a name space");

            var ComponentName = "List" + t.Name;
            var ServiceClassEndpointOptions =
                new CreateListEndPointOptions(t);


            var serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateListEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            var responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateListEndpointResponseClass(t,
                    ServiceClassEndpointOptions);


            var requestClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateListEndpointRequestClass(t,
                    ServiceClassEndpointOptions);
            var VueFileName = $"{ComponentName}.vue";
            var ListVueContents = KittyHelper.KittyViewHelper.GenerateListPage(t,
                new KittyHelper.KittyViewHelper.ListViewOptions
                {
                    ComponentName = ComponentName,
                    HttpVerb = ServiceClassEndpointOptions.HttpVerb,
                    RequestObjectName = ServiceClassEndpointOptions.RequestType,
                    RequestObjectField = ServiceClassEndpointOptions.RequestObjectAfterField,
                    ResponseObjectField = ServiceClassEndpointOptions.ResponseObjectFieldName,
                    EditObjectRoute = $"/Update{t.Name}/",
                    DataBaseObjectIdField = "Id"
                });

            var RequestType = ServiceClassEndpointOptions.RequestType;
            var ReturnType = ServiceClassEndpointOptions.ReturnType;
            var ServiceType = ServiceClassEndpointOptions.ServiceType;

            KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition,
                serviceClassDefinition,
                ServiceClassEndpointOptions, OverWrite);

            projectWriter.WriteVueFile(t, VueFileName, ListVueContents, OverWrite);
            return ServiceClassEndpointOptions;
        }

        private static void GenerateCreateGetByIdEndpointService(Type t, KittyHelper.ProjectWriter projectWriter,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace)) throw new ArgumentException("Type must have a name space");

            var ServiceClassEndpointOptions =
                new CreateGetByIdEndpointOptions(t)
                {
                    HttpVerb = "Get"
                };

            var RequestClassEndpointOptions =
                new CreateGetByIdEndpointOptions(t);

            var ResponseClassEndpointOptions =
                new CreateGetByIdEndpointOptions(t);

            var serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateGetByIdEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            var responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateGetByIdEndpointResponseClass(t,
                    ResponseClassEndpointOptions);


            var requestClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateGetByIdEndpointRequestClass(t,
                    RequestClassEndpointOptions);


            var RequestType = ServiceClassEndpointOptions.RequestType;
            var ReturnType = ServiceClassEndpointOptions.ReturnType;
            var ServiceType = ServiceClassEndpointOptions.ServiceType;


            KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition,
                serviceClassDefinition,
                ServiceClassEndpointOptions, OverWrite);
        }

        public static void GenerateData(Type t, KittyHelper.ProjectWriter projectWriter, Type[] types)
        {
            var OverWrite = true;

            GenerateCreateDeleteByIdEndpointService(t, projectWriter, OverWrite);


            var attrs = t.Properties().SelectMany(a => a.GetCustomAttributes());
            var ReferenceField = t.Properties().FirstOrDefault(a =>
                a.CustomAttributes.Any(b =>
                    b.AttributeType.Name == "ReferencesAttribute" &&
                    Nullable.GetUnderlyingType(a.PropertyType) == null));

            
            var ChooseableReferenceFields = t.Properties().Where(a =>
                a.CustomAttributes.Any(b =>
                    b.AttributeType.Name == "ReferencesAttribute" &&
                    Nullable.GetUnderlyingType(a.PropertyType) != null)).ToArray();

            KittyHelper.KittyViewHelper.ComponentPath createPath = new()
                {Component = "Create" + t.Name, Path = "/Create" + t.Name};
            var ComponentNames = new List<KittyHelper.KittyViewHelper.ComponentPath>
            {
                new() {Component = "List" + t.Name, Path = "/List" + t.Name}
            };

            var Create = !t.GetCustomAttributesData().Any(a => a.AttributeType.Name == "NoCreateViewsAttribute");
            var Update = !t.GetCustomAttributesData().Any(a => a.AttributeType.Name == "NoUpdateViewsAttribute");
            if (Update)
            {
              var options=  GenerateCreateUpdateEndpointService(t, projectWriter, OverWrite, types, Create);
                ComponentNames.Add(new KittyHelper.KittyViewHelper.ComponentPath()
                    {Component = "Update" + t.Name, Path = "/Update" + t.Name + "/:id"});
                GenerateCreateListWithReferenceEndpointService(t, projectWriter, options.UpdateMixin, OverWrite);
            }
            else
            {
                GenerateCreateListWithReferenceEndpointService(t, projectWriter, null,OverWrite);
            }

            if (ReferenceField == null)
            {
                if (Create)
                {
                    GenerateCreateCreateEndpointService(t, projectWriter, OverWrite);
                    ComponentNames.Add(new KittyHelper.KittyViewHelper.ComponentPath()
                        {Component = "Create" + t.Name, Path = "/Create" + t.Name});
                }
            }
            else
            {
                if (Create)
                {
                    GenerateCreateCreateWithReferenceEndpointService(t, projectWriter, ReferenceField, OverWrite);
                    ComponentNames.Add(new KittyHelper.KittyViewHelper.ComponentPath()
                        {Component = "Create" + t.Name, Path = "/Create" + t.Name + "/:id"});
                }

         
            }
 

            var VueRouter = KittyHelper.KittyViewHelper.GenerateVueAutoRoute(ComponentNames.ToArray());
            GenerateMigration(t, projectWriter, OverWrite);
            var ServiceClassEndpointOptions = GenerateCreateListEndpointService(t, projectWriter, OverWrite);
            GenerateCreateGetByIdEndpointService(t, projectWriter, OverWrite);
            var ComponentName = "Select" + t.Name;
            var VueFileName = $"{ComponentName}.vue";
            var SelectView = KittyHelper.KittyViewHelper.GenerateSelectPage(t,
                new KittyHelper.KittyViewHelper.SelectViewOptions
                {
                    ComponentName = ComponentName,
                    HttpVerb = ServiceClassEndpointOptions.HttpVerb,
                    RequestObjectName = ServiceClassEndpointOptions.RequestType,
                    RequestObjectField = ServiceClassEndpointOptions.RequestObjectAfterField,
                    ResponseObjectField = ServiceClassEndpointOptions.ResponseObjectFieldName,

                    DataBaseObjectIdField = "Id"
                });
            projectWriter.WriteVueFile(t, VueFileName, SelectView, OverWrite);
            foreach(var referenceField in ChooseableReferenceFields)
            {
                var GenerateSelectAgainstAssociation = KittyHelper.KittyViewHelper.GenerateSelectAgainstAssociationPage(t,
           new KittyHelper.KittyViewHelper.SelectAgainstAssociationViewOptions
           {
               ComponentName = "GenerateSelectAgainstAssociation" + t.Name,
               HttpVerb = ServiceClassEndpointOptions.HttpVerb,
               RequestObjectName = ServiceClassEndpointOptions.RequestType,
               RequestObjectField = ServiceClassEndpointOptions.RequestObjectAfterField,
               ResponseObjectField = ServiceClassEndpointOptions.ResponseObjectFieldName,
               DataBaseObjectIdField = "Id"
           });
                projectWriter.WriteVueFile(t, VueFileName, SelectView, OverWrite);
            }

            projectWriter.WriteVueFile(t, "router.ts", VueRouter, OverWrite);
        }
    }

    public static class KittyServiceHelper
    {
        public static string WrapWithUsingsAndNameSpace(Type t, string classContent,
            WrapWithClassAndNameSpaceOptions options = null)
        {
            StringBuilder str = new();
            options ??= new WrapWithClassAndNameSpaceOptions(t);
            str.AppendLine(options.Usings);
            str.AppendLine(options.NameSpace);
            str.AppendLine(classContent);
            str.AppendLine(options.NameSpaceClose);
            return str.ToString();
        }
    }

    public class WrapWithClassAndNameSpaceOptions
    {
        private string _nameSpace;

        public WrapWithClassAndNameSpaceOptions(Type type)
        {
            usings = new List<string>();
            _nameSpace = type.Namespace ?? "System." + type.Name;
        }

        private List<string> usings { get; }

        public string Usings => string.Join(Environment.NewLine, usings);

        public string NameSpace => $" namespace {_nameSpace} {{";

        public string NameSpaceClose { get; set; } = "}";

        public void AddUsings(IEnumerable<string> usings)
        {
            this.usings.AddRange(usings);
        }

        public void AddUsing(string @using)
        {
            usings.Add(@using);
        }

        public void SetNameSpace(string ns)
        {
            _nameSpace = ns;
        }
    }
}