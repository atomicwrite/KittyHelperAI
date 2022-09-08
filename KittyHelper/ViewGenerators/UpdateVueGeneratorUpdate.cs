using System;
using System.Linq;
using KittyHelper.Options;
using static KittyHelper.KittyHelper.KittyViewHelper;


namespace KittyHelper
{
  
    public class UpdateVueGenerator<A> : ICreateAVueComponent
    {
        private readonly Type T;
        private readonly CreateUpdateEndPointOptions<A> _options;
 
        private readonly string _maskTypeName;

        public UpdateVueGenerator(CreateUpdateEndPointOptions<A> options)
        {
            T = typeof(A);
           
            _options = options;
            _maskTypeName = T.Name + "UpdateMask";
        }


   


        public VueComponent Create()
        {
            VueComponent baseComponent = new();


            string maskTypeName = T.Name + "UpdateMask";

            CreateUpdateComponentTemplate(baseComponent.RootElement);
            VueComponentScript script = baseComponent.Script;

            CreateUpdateScriptImports(script);

            TypeScriptClass apiMixin = CreateUpdateMixin();
            TypeScriptClass maskMixin = CreateUpdateMaskForType();

            TypeScriptClass componentClass = CreateUpdateComponentClass(apiMixin);


            script.VueClass.Add(maskMixin);
            script.VueClass.Add(apiMixin);


            script.VueClass.Add(componentClass);

            return baseComponent;
        }

        private void CreateUpdateScriptImports(VueComponentScript script)
        {
            
            script.Imports.Add(new VueImport("vue-property-decorator", "Component", "Vue"));
            script.Imports.Add(new VueImport("vue-property-decorator", "{Mixins}"));
            script.Imports.Add(new VueImport("@/shared", "{client}"));
            script.Imports.Add(new VueImport("@/shared/dtos", _options.RequestObjectType, T.Name,
                _options.ResponseObjectType));
        }

        private void CreateUpdateComponentTemplate(VueElement _root)
        {
            VueElement sectionElement = new VueSection();
            VueElement containerDiv = new VueDiv();
                
            _root.AddChild(sectionElement);
            sectionElement.AddChild(containerDiv);

            containerDiv.AddChild(new VueH4($"Update {T.Name}Mask"));

            containerDiv.AddChild(new VueBAlert("{{  DataModel.Message }}", new VueAttribute(":show", "true"),
                new VIf("DataModel.Message.length >0")));

            CreateUpdateFormFields(containerDiv);
        }

        private TypeScriptClass CreateUpdateMaskForType()
        {     //init?: Partial<ResponseError>
            //(Object as any).assign(this, init);
            var superCall = new TypescriptFunctionCall("super");
            TypeScriptStatement[] block = new TypeScriptStatement[]
            {
                superCall,
                new TypescriptFunctionCall("(Object as any).assign", new TypeScriptFunctionArguments[]
                {
                    new TypeScriptFunctionArguments("this"),
                    new TypeScriptFunctionArguments("originalObject")
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
                new TypeScriptParameter("init?", new TypescriptTypeDeclaration("Partial<" + _maskTypeName + ">"))
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


        private void CreateUpdateFormFields(VueElement containerDiv)
        {
            var FieldInfos = T.GetProperties();
            foreach (var field in FieldInfos)
            {
                var CustomAttributesData = field.GetCustomAttributesData();
                if (CustomAttributesData.Any(a => a.AttributeType.Name == "AutoIncrementAttribute")) continue;
                var typeStr = TypeToInput(field.PropertyType.Name);
                containerDiv.AddChild(GenerateVueInputElement(field, typeStr));
            }

            containerDiv.AddChild(new BButton("Update", new VueClickAttribute($"Update{T.Name}")));
        }

        private TypeScriptClass CreateUpdateComponentClass(TypeScriptClass apiMixin)
        {
            var classFields = new TypeScriptClassField[]
            {
                new TypeScriptClassField("DataModel", new TypescriptTypeDeclaration(_maskTypeName),
                    $"new {_maskTypeName}(new {T.Name}())")
            };
            VueClassProp ComponentProp = new VueClassProp("Component", "{ components: {}}");
            var componentClass = new TypeScriptClass(_options.ComponentName, new[] {ComponentProp}, null, null,
                new[] {apiMixin}, null, null, classFields);
            return componentClass;
        }

        private TypeScriptClass CreateUpdateMixin()
        {
            TypeScriptFunctionArguments[] functionArguments = new TypeScriptFunctionArguments[]
            {
                new TypeScriptFunctionArguments(
                    $"new {_options.RequestObjectType}  ( {{ {_options.RequestObjectUpdateObjectField} : DataModel }} ) ")
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
                    new TypeScriptStatement[] {" DataModel.Message = e.message;", "console.log(e)"})
            };


            var createFunction = new TypeScriptFunction("Update" + T.Name,
                new TypescriptTypeDeclaration(new TypescriptType(null)),
                true, new[] {new TypeScriptParameter("DataModel", new TypescriptTypeDeclaration(_maskTypeName))},
                block);

            var apiMixin = new TypeScriptClass(T.Name + "ApiMixin", null, new TypeScriptFunction[] {createFunction},
                TypeScriptClass.Vue);
            apiMixin.ExportNonDefault();
            return apiMixin;
        }
    }
}