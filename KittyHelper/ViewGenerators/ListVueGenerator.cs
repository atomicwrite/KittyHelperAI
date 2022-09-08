using System;
using KittyHelper.Options;
using static KittyHelper.KittyHelper.KittyViewHelper;

namespace KittyHelper.ViewGenerators
{
    public class ListVueGenerator<A> : ICreateAVueComponent
    {
        private readonly Type T;
        private readonly CreateListEndPointOptions<A> _options;
        private readonly VueElement _root;
        private readonly string _maskTypeName;

        public ListVueGenerator(CreateListEndPointOptions<A> options)
        {
            T = typeof(A);
            _root = new VueElement(new VueTag("template"));
            _options = options;
            _maskTypeName = T.Name + "ListMask";
        }

        private VueElement CreateListVForDisplayAsTable()
        {
            VueTableSimple table = new VueTableSimple();
            var head = new VueBHead();
            var tr = new VueBTr();
            head.AddChild(tr);
            var vForObjectName = "a";
            var vForTr = new VueBTr(new VFor(vForObjectName, "DataModel"));

            foreach (var a in T.GetProperties())
            {
                var vueBTh = new VueBTh(a.Name);
                tr.AddChild(CreateListButtonGroup(vForObjectName));
                tr.AddChild(vueBTh);
                vForTr.AddChild(new VueBTh($"{{{{ {a.Name} }}}}"));
            }

            table.AddChild(head);
            var vueBtBody = new VueBTBody();
            vueBtBody.AddChild(vForTr);
            table.AddChild(vueBtBody);
            return table;
        }

        private void CreateListAllDisplay(VueElement containerDiv)
        {
            var bAlert = new VueBAlert("{{  ApiCallMessage.Message }}", new VueAttribute(":show", "true"),
                new VIf("ApiCallMessage.Message.length >0"));

            var @group = CreatePagingGroup(T);


            containerDiv.AddChild(new VueH4($"List {T.Name}"));

            containerDiv.AddChild(bAlert);


            containerDiv.AddChild(group);


            containerDiv.AddChild(CreateListVForDisplayAsTable());
        }

        protected virtual void CreateListAllScriptImports(VueComponentScript script)
        {
            script.Imports.Add(new VueImport("vue-property-decorator", "Component", "Vue"));
            script.Imports.Add(new VueImport("vue-property-decorator", "{Mixins}"));
            script.Imports.Add(new VueImport("@/shared", "{client}"));
            script.Imports.Add(new VueImport("@/shared/dtos", _options.RequestObjectName, T.Name,
                _options.ResponseObjectType));
        }

        public VueComponent Create()
        {
            VueElement sectionElement = new VueSection();
            VueElement containerDiv = new VueDiv();
            _root.AddChild(sectionElement);
            sectionElement.AddChild(containerDiv);

            VueComponent baseComponent = new();
            CreateListAllDisplay(containerDiv);

            baseComponent.RootElement.AddChild(sectionElement);

            VueComponentScript script = baseComponent.Script;

            CreateListAllScriptImports(script);

            TypeScriptClass apiMixin = CreateListAllMixin();
            TypeScriptClass maskMixin = CreateListAllMaskForType();

            TypeScriptClass componentClass = CreateListAllComponentClass(apiMixin);


            script.VueClass.Add(maskMixin);
            script.VueClass.Add(apiMixin);


            script.VueClass.Add(componentClass);

            return baseComponent;
        }

        protected virtual TypeScriptClass CreateListAllComponentClass(TypeScriptClass apiMixin)
        {
            var classFields = new TypeScriptClassField[]
            {
                new TypeScriptClassField("DataModel", new TypescriptTypeDeclaration(_maskTypeName + "[]"),
                    "[]")
            };
            VueClassProp ComponentProp = new VueClassProp("Component", "{ components: {}}");
            var componentClass = new TypeScriptClass(_options.ComponentName, new[] { ComponentProp }, null, null,
                new[] { apiMixin }, null, null, classFields);
            return componentClass;
        }

        protected virtual TypeScriptClass CreateListAllMaskForType()
        {
            var superCall = new TypescriptFunctionCall("super");
            TypeScriptStatement[] block = new TypeScriptStatement[]
            {
                superCall,
                new TypescriptFunctionCall("Object.assign", new TypeScriptFunctionArguments[]
                {
                    new TypeScriptFunctionArguments("this"),
                    new TypeScriptFunctionArguments("originalObject")
                })
            };

            var fields = new TypeScriptClassField[]
            {
                new TypeScriptClassField("Message", new TypescriptTypeDeclaration("string"), "\"\""),
                new TypeScriptClassField("Success", new TypescriptTypeDeclaration("boolean"), "true"),
                new TypeScriptClassField("Completed", new TypescriptTypeDeclaration("boolean"), "true"),
                new TypeScriptClassField("Error", new TypescriptTypeDeclaration("string"), "\"\""),
            };
            var parameters = new TypeScriptParameter[]
            {
                new TypeScriptParameter("originalObject", new TypescriptTypeDeclaration(T.Name))
            };
            var functions = new TypeScriptFunction[]
            {
                new TypeScriptFunction("constructor", TypescriptTypeDeclaration.NoReturnType, false, block: block,
                    vueParameters: parameters)
            };
            TypeScriptClass maskMixin =
                new(_maskTypeName, fields: fields, functions: functions, extends: new TypeScriptClass(T.Name));
            maskMixin.ExportNonDefault();
            return maskMixin;
        }

        protected virtual TypeScriptClass CreateListAllMixin()
        {
            TypeScriptFunctionArguments[] functionArguments = new TypeScriptFunctionArguments[]
            {
                new TypeScriptFunctionArguments(
                    $"new {_options.RequestObjectName}  ( {{ {_options.RequestObjectAfterField} : {_options.ComponentAfterFieldName} }} ) ")
            };
            var apiCallStatement =
                new TypescriptFunctionCall($"client.{_options.HttpVerb.ToLower()}", functionArguments, true);

            var block = new TypeScriptStatement[]
            {
                new TypeScriptTryCatchFinally(new TypeScriptStatement[]
                    {
                        new TypescriptAssignment(
                            new TypescriptVariable("const", "Response",
                                new TypescriptType(_options.ResponseObjectName)), apiCallStatement),
                        " this.ApiCallSuccess = Response.Success;",
                        " this.ApiCallMessage = Response.Message;",
                        $" if ( Response.Success) {{",
                        "DataModel.clear()",
                        " Response.{_options.ResponseObjectFieldName}.forEach(DataModel.add)",
                        "}"
                    },
                    new TypeScriptStatement[] {" DataModel.Message = e.message;", "console.log(e)"})
            };
            var apiSuccessField =
                new TypeScriptClassField("ApiCallSuccess", new TypescriptTypeDeclaration("boolean"), "true");
            var apiMessageField =
                new TypeScriptClassField("ApiCallMessage", new TypescriptTypeDeclaration("string"), "\"\"");

            var createFunction = new TypeScriptFunction("List" + T.Name,
                new TypescriptTypeDeclaration(new TypescriptType(null)),
                true, new[] { new TypeScriptParameter("DataModel", new TypescriptTypeDeclaration(_maskTypeName + "[]")) },
                block);

            var apiMixin = new TypeScriptClass(T.Name + "ApiMixin",
                functions: new TypeScriptFunction[] { createFunction }, extends: TypeScriptClass.Vue,
                fields: new TypeScriptClassField[]
                {
                    apiSuccessField,
                    apiMessageField
                });
            apiMixin.ExportNonDefault();
            return apiMixin;
        }
    }
}