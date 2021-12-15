using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KittyHelper.Options;
using ServiceStack;


namespace KittyHelper
{
    public static partial class CrudHelper
    {
        private static void GenerateMigration(Type type, KittyHelper.ProjectWriter projectWriter, bool ow)
        {
            string fileName = type.Name + ".cs";
            projectWriter.WriteCsMigrationFile(
                DatabaseGenerators.KittyHelper.MigrationHelper.GenerateCreateIfNotExists(type), fileName,
                ow);
        }

        private static void GenerateCreateDeleteByIdEndpointService(Type t, KittyHelper.ProjectWriter projectWriter,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace))
            {
                throw new ArgumentException("Type must have a name space");
            }

            var ServiceClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndPointOptions(t);

            var RequestClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndPointOptions(t);

            var ResponseClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndPointOptions(t);

            string serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            string responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateDeleteByIdEndpointResponseClass(t,
                    ResponseClassEndpointOptions);


            string requestClassDefinition =
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
            if (string.IsNullOrEmpty(t.Namespace))
            {
                throw new ArgumentException("Type must have a name space");
            }

            var ServiceClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateUpdateEndPointOptions(t);


            string serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateUpdateEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            string responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateUpdateEndpointResponseClass(t,
                    ServiceClassEndpointOptions);


            string requestClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateUpdateEndpointRequestClass(t, ServiceClassEndpointOptions);

            var RequestType = ServiceClassEndpointOptions.RequestType;
            var ReturnType = ServiceClassEndpointOptions.ReturnType;
            var ServiceType = ServiceClassEndpointOptions.ServiceType;
            if (create)
            {
                KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition,
                    serviceClassDefinition,
                    ServiceClassEndpointOptions, OverWrite);
            }

            var ComponentName = "Update" + t.Name;
            string VueFileName = $"{ComponentName}.vue";
            string Updateview = KittyHelper.KittyViewHelper.GenerateUpdatePage(t,
                new KittyHelper.KittyViewHelper.UpdateViewOptions()
                {
                    ComponentName = ComponentName,
                    HttpVerb = ServiceClassEndpointOptions.HttpVerb,
                    RequestObjectName = ServiceClassEndpointOptions.RequestType,
                    RequestObjectField = ServiceClassEndpointOptions.RequestObjectUpdateObjectField,
                    ListOneHttpVerb = "get",
                    ListOneRequestObjectField = "Id",
                    ListOneRequestObjectName = "Get" + t.Name + "ByIdRequest",
                    ReferencedByTypes = types,
                    DisableUpdate = !create
                });
            projectWriter.WriteVueFile(t, VueFileName, Updateview, OverWrite);
            return ServiceClassEndpointOptions;
        }

        private static void GenerateCreateCreateWithReferenceEndpointService(Type t,
            KittyHelper.ProjectWriter projectWriter,
            PropertyInfo referenceField,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace))
            {
                throw new ArgumentException("Type must have a name space");
            }


            var ServiceClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateCreateEndPointOptions(t);


            string serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateCreateEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            string responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateCreateEndpointResponseClass(t,
                    ServiceClassEndpointOptions);


            string requestClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateCreateEndpointRequestClass(t, ServiceClassEndpointOptions);
            var ComponentName = "Create" + t.Name;

            string CreateVueFile = KittyHelper.KittyViewHelper.GenerateCreateWithReferencePage(t,
                new KittyHelper.KittyViewHelper.CreateWithReferenceViewOptions()
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
            string VueFileName = $"{ComponentName}.vue";

            KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition, serviceClassDefinition,
                ServiceClassEndpointOptions, OverWrite);

            projectWriter.WriteVueFile(t, VueFileName, CreateVueFile, OverWrite);
        }

        private static void GenerateCreateCreateEndpointService(Type t, KittyHelper.ProjectWriter projectWriter,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace))
            {
                throw new ArgumentException("Type must have a name space");
            }


            var ServiceClassEndpointOptions =
                new ServiceGenerators.KittyServiceHelper.CreateCreateEndPointOptions(t);


            string serviceClassDefinition =
                ServiceGenerators. KittyServiceHelper.CreateCreateEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            string responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateCreateEndpointResponseClass(t,
                    ServiceClassEndpointOptions);


            string requestClassDefinition =
                ServiceGenerators. KittyServiceHelper.CreateCreateEndpointRequestClass(t, ServiceClassEndpointOptions);
            var ComponentName = "Create" + t.Name;

            string CreateVueFile = KittyHelper.KittyViewHelper.GenerateCreatePage(t,
                new KittyHelper.KittyViewHelper.CreateViewOptions()
                {
                    ComponentName = ComponentName,
                    HttpVerb = ServiceClassEndpointOptions.HttpVerb,
                    RequestObjectName = ServiceClassEndpointOptions.RequestType,
                    RequestObjectField = ServiceClassEndpointOptions.RequestObjectNewObjectField
                });

            var RequestType = ServiceClassEndpointOptions.RequestType;
            var ReturnType = ServiceClassEndpointOptions.ReturnType;
            var ServiceType = ServiceClassEndpointOptions.ServiceType;
            string VueFileName = $"{ComponentName}.vue";

            KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition, serviceClassDefinition,
                ServiceClassEndpointOptions, OverWrite);

            projectWriter.WriteVueFile(t, VueFileName, CreateVueFile, OverWrite);
        }

        private static CreateListEndPointOptions GenerateCreateListWithReferenceEndpointService(Type t,
            KittyHelper.ProjectWriter projectWriter,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace))
            {
                throw new ArgumentException("Type must have a name space");
            }

            var ComponentName = "ListWithReference" + t.Name;
            var ServiceClassEndpointOptions =
                new CreateListEndPointOptions(t);


            string VueFileName = $"{ComponentName}.vue";
            string ListVueContents = KittyHelper.KittyViewHelper.GenerateListFromReferencePage(t,
                new KittyHelper.KittyViewHelper.ListFromReferenceViewOptions()
                {
                    ComponentName = ComponentName,
                    HttpVerb = ServiceClassEndpointOptions.HttpVerb,
                    RequestObjectName = ServiceClassEndpointOptions.RequestType,
                    RequestObjectField = ServiceClassEndpointOptions.RequestObjectAfterField,
                    ResponseObjectField = ServiceClassEndpointOptions.ResponseObjectFieldName,
                    EditObjectRoute = $"/Update{t.Name}/",
                    DataBaseObjectIdField = "Id",
                });

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
            if (string.IsNullOrEmpty(t.Namespace))
            {
                throw new ArgumentException("Type must have a name space");
            }

            var ComponentName = "List" + t.Name;
            var ServiceClassEndpointOptions =
                new CreateListEndPointOptions(t);


            string serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateListEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            string responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateListEndpointResponseClass(t,
                    ServiceClassEndpointOptions);


            string requestClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateListEndpointRequestClass(t,
                    ServiceClassEndpointOptions);
            string VueFileName = $"{ComponentName}.vue";
            string ListVueContents = KittyHelper.KittyViewHelper.GenerateListPage(t,
                new KittyHelper.KittyViewHelper.ListViewOptions()
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

           KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition, serviceClassDefinition,
                ServiceClassEndpointOptions, OverWrite);

            projectWriter.WriteVueFile(t, VueFileName, ListVueContents, OverWrite);
            return ServiceClassEndpointOptions;
        }

        private static void GenerateCreateGetByIdEndpointService(Type t, KittyHelper.ProjectWriter projectWriter,
            bool OverWrite)
        {
            if (string.IsNullOrEmpty(t.Namespace))
            {
                throw new ArgumentException("Type must have a name space");
            }

            var ServiceClassEndpointOptions =
                new CreateGetByIdEndpointOptions(t)
                {
                    HttpVerb = "Get"
                };

            var RequestClassEndpointOptions =
                new CreateGetByIdEndpointOptions(t);

            var ResponseClassEndpointOptions =
                new CreateGetByIdEndpointOptions(t);

            string serviceClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateGetByIdEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            string responseClassDefinition =
                ServiceGenerators.KittyServiceHelper.CreateGetByIdEndpointResponseClass(t,
                    ResponseClassEndpointOptions);


            string requestClassDefinition =
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
            KittyHelper.KittyViewHelper.ComponentPath createPath = new()
                {Component = "Create" + t.Name, Path = "/Create" + t.Name};
            var ComponentNames = new List<KittyHelper.KittyViewHelper.ComponentPath>()
            {
                new() {Component = "List" + t.Name, Path = "/List" + t.Name},
            };

            bool Create = !(t.GetCustomAttributesData().Any(a => a.AttributeType.Name == "NoCreateViewsAttribute"));
            bool Update = !(t.GetCustomAttributesData().Any(a => a.AttributeType.Name == "NoUpdateViewsAttribute"));
            if (Update)
            {
                GenerateCreateUpdateEndpointService(t, projectWriter, OverWrite, types, Create);
                ComponentNames.Add(new() {Component = "Update" + t.Name, Path = "/Update" + t.Name + "/:id"});
            }

            if (ReferenceField == null)
            {
                if (Create)
                {
                    GenerateCreateCreateEndpointService(t, projectWriter, OverWrite);
                    ComponentNames.Add(new() {Component = "Create" + t.Name, Path = "/Create" + t.Name});
                }
            }
            else
            {
                if (Create)
                {
                    GenerateCreateCreateWithReferenceEndpointService(t, projectWriter, ReferenceField, OverWrite);
                    ComponentNames.Add(new() {Component = "Create" + t.Name, Path = "/Create" + t.Name + "/:id"});
                }

                GenerateCreateListWithReferenceEndpointService(t, projectWriter, OverWrite);
            }


            string VueRouter = KittyHelper.KittyViewHelper.GenerateVueAutoRoute(ComponentNames.ToArray());
            GenerateMigration(t, projectWriter, OverWrite);
            var ServiceClassEndpointOptions = GenerateCreateListEndpointService(t, projectWriter, OverWrite);
            GenerateCreateGetByIdEndpointService(t, projectWriter, OverWrite);
            var ComponentName = "Select" + t.Name;
            string VueFileName = $"{ComponentName}.vue";
            string SelectView = KittyHelper.KittyViewHelper.GenerateSelectPage(t,
                new KittyHelper.KittyViewHelper.SelectViewOptions()
                {
                    ComponentName = ComponentName,
                    HttpVerb = ServiceClassEndpointOptions.HttpVerb,
                    RequestObjectName = ServiceClassEndpointOptions.RequestType,
                    RequestObjectField = ServiceClassEndpointOptions.RequestObjectAfterField,
                    ResponseObjectField = ServiceClassEndpointOptions.ResponseObjectFieldName,

                    DataBaseObjectIdField = "Id"
                });
            projectWriter.WriteVueFile(t, VueFileName, SelectView, OverWrite);


            projectWriter.WriteVueFile(t, "router.ts", VueRouter, OverWrite);
        }
    }

    public static partial class KittyServiceHelper
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
            usings = new();
            _nameSpace = type.Namespace ?? "System." + type.Name;
        }

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

        private List<string> usings { get; set; }

        public string Usings
        {
            get => string.Join(Environment.NewLine, usings);
        }

        public string NameSpace
        {
            get => $" namespace {_nameSpace} {{";
        }

        public string NameSpaceClose { get; set; } = "}";
    }
}