using System;
using System.Linq;
using KittyHelper.Options;
using static KittyHelper.KittyHelper.KittyViewHelper;
using static KittyHelper.ServiceGenerators.KittyServiceHelper;

namespace KittyHelper
{
    public class CreateVueGenerator<A> : ICreateAVueComponent
    {
        private readonly Type T;
        private readonly CreateCreateEndPointOptions<A> _options;
     
        private readonly string _maskTypeName;

        public CreateVueGenerator(CreateCreateEndPointOptions<A> options)
        {
            T = typeof(A);
        
            _options = options;
            _maskTypeName = T.Name + "CreateMask";
        }

 

        public VueComponent Create()
        {
            VueComponent baseComponent = new();


            string maskTypeName = T.Name + "CreateMask";

            CreateCreateComponentTemplate(baseComponent);
            VueComponentScript script = baseComponent.Script;

            CreateCreateScriptImports(script);

            TypeScriptClass apiMixin = CreateMixin();
            TypeScriptClass maskMixin = CreateMaskForType();

            TypeScriptClass componentClass = CreateComponentClass(apiMixin);


            script.VueClass.Add(maskMixin);
            script.VueClass.Add(apiMixin);


            script.VueClass.Add(componentClass);

            return baseComponent;
        }

        private void CreateCreateScriptImports(VueComponentScript script)
        {
            script.Imports.Add(new VueImport("vue-property-decorator", "Component", "Vue"));
            script.Imports.Add(new VueImport("vue-property-decorator", "{Mixins}"));
            script.Imports.Add(new VueImport("@/shared", "{client}"));
            script.Imports.Add(new VueImport("@/shared/dtos", _options.RequestObjectType, T.Name,
                _options.ResponseObjectType));
        }

        private void CreateCreateComponentTemplate(VueComponent baseComponent)
        {
            var root = baseComponent.RootElement;
            VueElement sectionElement = new VueSection();
            VueElement containerDiv = new VueDiv();

            root.AddChild(sectionElement);
            sectionElement.AddChild(containerDiv);

            containerDiv.AddChild(new VueH4($"Create {T.Name}Mask"));

            containerDiv.AddChild(new VueBAlert("{{  DataModel.Message }}", new VueAttribute(":show", "true"),
                new VIf("DataModel.Message.length >0")));

            CreateCreateFormFields(containerDiv);
        }

        private TypeScriptClass CreateMaskForType()
        {
            //init?: Partial<ResponseError>
            //(Object as any).assign(this, init);
            var superCall = new TypescriptFunctionCall("super");
            TypeScriptStatement[] block = new TypeScriptStatement[]
            {
                superCall,
                ";",
                new TypescriptFunctionCall("(Object as any).assign", new TypeScriptFunctionArguments[]
                {
                    new TypeScriptFunctionArguments("this"),
                    new TypeScriptFunctionArguments("init")
                })
            };

            TypeScriptClassField[] fields = new TypeScriptClassField[]
            {
                new TypeScriptClassField("Message", new TypescriptTypeDeclaration("string"), "\"\""),
                new TypeScriptClassField("Success", new TypescriptTypeDeclaration("boolean"), "true"),
                new TypeScriptClassField("Completed", new TypescriptTypeDeclaration("boolean"), "true"),
                new TypeScriptClassField("Error", new TypescriptTypeDeclaration("string"), "\"\""),
            };
            TypeScriptParameter[] parameters = new TypeScriptParameter[]
            {
                new TypeScriptParameter("init?", new TypescriptTypeDeclaration("Partial<" +_maskTypeName + ">"))
            };
            TypeScriptFunction[] functions = new TypeScriptFunction[]
            {
                new TypeScriptFunction("constructor", TypescriptTypeDeclaration.NoReturnType, false, block: block,
                    vueParameters: parameters)
            };
            TypeScriptClass maskMixin =
                new(_maskTypeName, fields: fields, functions: functions, extends: new TypeScriptClass(T.Name));
            
            maskMixin.ExportNonDefault();
            return maskMixin;
        }


        private void CreateCreateFormFields(VueElement containerDiv)
        {
            var FieldInfos = T.GetProperties();
            foreach (var field in FieldInfos)
            {
                var CustomAttributesData = field.GetCustomAttributesData();
                if (CustomAttributesData.Any(a => a.AttributeType.Name == "AutoIncrementAttribute")) continue;
                var typeStr = TypeToInput(field.PropertyType.Name);
                containerDiv.AddChild(GenerateVueInputElement(field, typeStr));
            }

            containerDiv.AddChild(new BButton("Create", new VueClickAttribute($"Create{T.Name}(DataModel)")));
        }

        private TypeScriptClass CreateComponentClass(TypeScriptClass apiMixin)
        {
            var classFields = new TypeScriptClassField[]
            {
                new TypeScriptClassField("DataModel", new TypescriptTypeDeclaration(_maskTypeName),
                    $"new {_maskTypeName}(new {T.Name}({{}}))")
            };
            VueClassProp ComponentProp = new VueClassProp("Component", "{ components: {}}");
            var componentClass = new TypeScriptClass(_options.ComponentName, new[] {ComponentProp}, null, null,
                new[] {apiMixin}, null, null, classFields);
            return componentClass;
        }

        private TypeScriptClass CreateMixin()
        {
            TypeScriptFunctionArguments[] functionArguments = new TypeScriptFunctionArguments[]
            {
                new TypeScriptFunctionArguments(
                    $"new {_options.RequestObjectType}  ( {{ {_options.RequestObjectNewObjectField} : DataModel }} ) ")
            };
            var apiCallStatement =
                new TypescriptFunctionCall($"client.{_options.HttpVerb.ToLower()}", functionArguments, true);

            var block = new TypeScriptStatement[]
            {
                new TypeScriptTryCatchFinally(new TypeScriptStatement[]
                    {
                        new TypescriptAssignment(
                            new TypescriptVariable("const", "Response",
                                new TypescriptType(_options.ResponseObjectType)), apiCallStatement),
                        " DataModel.Success = Response.Success;",
                        new TypeScriptIf(new TypescriptConditionStatement("Response.Id", ">", "0"),
                            new TypeScriptStatement[] {" DataModel.Message = 'Created'",},
                            new TypeScriptStatement[] {" DataModel.Message = Response.Message;"})
                    },
                    new TypeScriptStatement[] {" DataModel.Message = e.message;", "console.log(e)",
                    "const fieldErrors = e.GetFieldErrors()",
                    "if (fieldErrors){",
                        //
                    "}"
                    },
                    excType:"WebException")
            };


            var createFunction = new TypeScriptFunction("Create" + T.Name,
                new TypescriptTypeDeclaration(new TypescriptType(null)),
                true, new[] {new TypeScriptParameter("DataModel", new TypescriptTypeDeclaration(_maskTypeName))},
                block);
            VueClassProp ComponentProp = new VueClassProp("Component", "{ components: {}}");

            var apiMixin = new TypeScriptClass(T.Name + "ApiMixin", new []{ComponentProp}, new TypeScriptFunction[] {createFunction},
                TypeScriptClass.Vue);
            apiMixin.ExportNonDefault();
            return apiMixin;
        }
    }
}