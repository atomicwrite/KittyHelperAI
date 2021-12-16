using System;
using System.Linq;
using System.Text;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            //    private static string GenerateApiMixins(Type T)
            public static string GenerateUpdatePage(Type T, UpdateViewOptions options)
            {
                VueComponent baseComponent = new();


                string maskTypeName = T.Name + "CreateMask";

                CreateUpdateComponentTemplate(T, baseComponent);
                VueComponentScript script = baseComponent.Script;

                CreateUpdateScriptImports(T, options, script);

                TypeScriptClass apiMixin = CreateUpdateMixin(T, options, maskTypeName);
                TypeScriptClass maskMixin = CreateUpdateMaskForType(T, maskTypeName);

                TypeScriptClass componentClass = CreateUpdateComponentClass(T, options, maskTypeName, apiMixin);



                script.VueClass.Add(maskMixin);
                script.VueClass.Add(apiMixin);


                script.VueClass.Add(componentClass);

                return baseComponent.Render();

            }

            private static void CreateUpdateScriptImports(Type T, UpdateViewOptions options, VueComponentScript script)
            {
                script.Imports.Add(new VueImport("vue-property-decorator", "Component", "Vue"));
                script.Imports.Add(new VueImport("vue-property-decorator", "{Mixins}"));
                script.Imports.Add(new VueImport("@/shared", "{client}"));
                script.Imports.Add(new VueImport("@/shared/dtos", options.RequestObjectName, T.Name, options.ResponseObjectName));
            }

            private static void CreateUpdateComponentTemplate(Type T, VueComponent baseComponent)
            {
                var root = baseComponent.RootElement;
                VueElement sectionElement = new VueSection();
                VueElement containerDiv = new VueDiv();

                root.AddChild(sectionElement);
                sectionElement.AddChild(containerDiv);

                containerDiv.AddChild(new VueH4($"Create {T.Name}Mask"));

                containerDiv.AddChild(new VueBAlert("{{  DataModel.Message }}", new VueAttribute(":show", "true"), new VIf("DataModel.Message.length >0")));

                CreateUpdateFormFields(T, containerDiv);
            }

            private static TypeScriptClass CreateUpdateMaskForType(Type T, string maskTypeName)
            {
                var superCall = new TypescriptFunctionCall("super");
                TypeScriptStatement[] block = new TypeScriptStatement[]
                {
                    superCall,
                     new TypescriptFunctionCall("Object.assign",new TypeScriptFunctionArguments[]{
                         new TypeScriptFunctionArguments("this"),
                         new TypeScriptFunctionArguments("originalObject")
                     })
                };

                TypeScriptClassField[] fields = new TypeScriptClassField[] {
                new TypeScriptClassField("Message", new TypescriptTypeDeclaration("string"),"\"\""),
                new TypeScriptClassField("Success", new TypescriptTypeDeclaration("boolean"),"true"),
                new TypeScriptClassField("Completed", new TypescriptTypeDeclaration("boolean"),"true"),
                new TypeScriptClassField("Error", new TypescriptTypeDeclaration("string"),"\"\""),
                };
                TypeScriptParameter[] parameters = new TypeScriptParameter[]
                {
                    new TypeScriptParameter("originalObject",new TypescriptTypeDeclaration(T.Name))
                };
                TypeScriptFunction[] functions = new TypeScriptFunction[]
                {
                    new TypeScriptFunction("constructor",TypescriptTypeDeclaration.NoReturnType,false,block:block,vueParameters:parameters)
                };
                TypeScriptClass maskMixin = new(maskTypeName, fields: fields, functions: functions, extends: new TypeScriptClass(T.Name));
                maskMixin.ExportNonDefault();
                return maskMixin;
            }



            private static void CreateUpdateFormFields(Type T, VueElement containerDiv)
            {
                var FieldInfos = T.GetProperties();
                foreach (var field in FieldInfos)
                {
                    var CustomAttributesData = field.GetCustomAttributesData();
                    if (CustomAttributesData.Any(a => a.AttributeType.Name == "AutoIncrementAttribute")) continue;
                    var typeStr = TypeToInput(field.PropertyType.Name);
                    containerDiv.AddChild(GenerateVueInputElement(field, typeStr));
                }
                containerDiv.AddChild(new BButton("Create", new VueClickAttribute($"Create{T.Name}")));
            }

            private static TypeScriptClass CreateUpdateComponentClass(Type T, UpdateViewOptions options, string maskTypeName, TypeScriptClass apiMixin)
            {
                var classFields = new TypeScriptClassField[] { new TypeScriptClassField("DataModel", new TypescriptTypeDeclaration(maskTypeName), $"new {maskTypeName}(new {T.Name}())") };
                VueClassProp ComponentProp = new VueClassProp("Component", "{ components: {}}");
                var componentClass = new TypeScriptClass(options.ComponentName, new[] { ComponentProp }, null, null, new[] { apiMixin }, null, null, classFields);
                return componentClass;
            }

            private static TypeScriptClass CreateUpdateMixin(Type T, UpdateViewOptions options, string MaskType)
            {
                TypeScriptFunctionArguments[] functionArguments = new TypeScriptFunctionArguments[] { new TypeScriptFunctionArguments($"new {options.RequestObjectName}  ( {{ {options.RequestObjectField} : DataModel }} ) ") };
                var apiCallStatement = new TypescriptFunctionCall($"client.{options.HttpVerb.ToLower()}", functionArguments, true);

                var block = new TypeScriptStatement[]
                {
                    new TypeScriptTryCatchFinally(new TypeScriptStatement[]
                    {
                       new TypescriptAssignment(new TypescriptVariable("const","Response",new TypescriptType(options.ResponseObjectName)),apiCallStatement),
                       " DataModel.Success = Response.Success;",
                        new TypeScriptIf(new TypescriptConditionStatement("Response.Id",">","0"),
                       new TypeScriptStatement[] {" DataModel.Message = 'Created'", },
                        new TypeScriptStatement [] {" DataModel.Message = Response.Message;" })


                    },
                    new TypeScriptStatement [] {" DataModel.Message = e.message;","console.log(e)" })
                };


                var createFunction = new TypeScriptFunction("Create" + T.Name, new TypescriptTypeDeclaration(new TypescriptType(null)),
                    true, new[] { new TypeScriptParameter("DataModel", new TypescriptTypeDeclaration(MaskType)) }, block);

                var apiMixin = new TypeScriptClass(T.Name + "ApiMixin", null, new TypeScriptFunction[] { createFunction }, TypeScriptClass.Vue);
                apiMixin.ExportNonDefault();
                return apiMixin;
            }

            
           
        }
    }
}