using KittyHelper.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using static KittyHelper.KittyHelper.KittyViewHelper;

namespace KittyHelper
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
                if (a.GetCustomAttributesData().Any(b => b.AttributeType.Name == "PrimaryKeyAttribute"))
                    vForTr.AddChild(CreateListButtonGroup(vForObjectName));
                tr.AddChild(vueBTh);
                VueTd td = new VueTd($"{{{{ a.{a.Name} }}}}");

                vForTr.AddChild(td);
            }

            table.AddChild(head);
            var vueBtBody = new VueBTBody();
            vueBtBody.AddChild(vForTr);
            table.AddChild(vueBtBody);
            return table;
        }

        private void CreateListAllDisplay(VueElement containerDiv)
        {
            var bAlert = new VueBAlert("{{  ApiCallMessage }}", new VueAttribute(":show", "true"),
                new VIf("ApiCallMessage && ApiCallMessage.length >0"));

            string[] searchFields = _options.SearchFields.Select(a => a.Name).ToArray();
            var @group = CreatePagingGroup(T, searchFields);
            var searchGroup = CreateSearchFilter(searchFields);
            containerDiv.AddChild(new VueH4($"List {T.Name}"));

            containerDiv.AddChild(bAlert);

            containerDiv.AddChild(group);
            containerDiv.AddChild(searchGroup);
            containerDiv.AddChild(CreateListVForDisplayAsTable());
        }

        protected virtual void CreateListAllScriptImports(VueComponentScript script)
        {
            script.Imports.Add(new VueImport("vue-property-decorator", "Component", "Vue"));
            script.Imports.Add(new VueImport("vue-property-decorator", "{Mixins}"));
            script.Imports.Add(new VueImport("@/shared", "{client}"));
            script.Imports.Add(new VueImport("@/shared/dtos", _options.RequestObjectType, T.Name,
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

            TypeScriptClass dataFieldsClass = CreateListDataFieldsMixin(apiMixin);
            TypeScriptClass componentClass = CreateListAllComponentClass(dataFieldsClass);

            script.VueClass.Add(maskMixin);
            script.VueClass.Add(apiMixin);
            script.VueClass.Add(dataFieldsClass);
            script.VueClass.Add(componentClass);

            return baseComponent;
        }

        protected virtual TypeScriptClass CreateListAllComponentClass(TypeScriptClass dataFieldsClass)
        {
            var createblock = new TypeScriptStatement[]
       {           new TypescriptFunctionCall("this.Load" + T.Name )       };
            var deleteblock = new TypeScriptStatement[] { new TypescriptFunctionCall("this.$router.push", new TypeScriptFunctionArguments[] { new TypeScriptFunctionArguments($"'Delete{T.Name}'") }) }; var editblock = new TypeScriptStatement[]
{
           new TypescriptFunctionCall("this.$router.push", new TypeScriptFunctionArguments[] { new TypeScriptFunctionArguments($"'Edit{T.Name}'") })
};

            var previousFunction = new TypeScriptFunction("Previous", new TypescriptTypeDeclaration(new TypescriptType(null)), async: true, vueParameters: Array.Empty<TypeScriptParameter>(), block: createblock);
            var nextFunction = new TypeScriptFunction("Next", new TypescriptTypeDeclaration(new TypescriptType(null)), async: true, vueParameters: Array.Empty<TypeScriptParameter>(), createblock);

            var editFunction = new TypeScriptFunction("Edit", new TypescriptTypeDeclaration(new TypescriptType(null)), async: true, vueParameters: Array.Empty<TypeScriptParameter>(), block: editblock);
            var deleteFunction = new TypeScriptFunction("Delete", new TypescriptTypeDeclaration(new TypescriptType(null)), async: true, vueParameters: Array.Empty<TypeScriptParameter>(), deleteblock);
            var createEventFunction = new TypeScriptFunction("created", new TypescriptTypeDeclaration(new TypescriptType(null)), async: true, vueParameters: Array.Empty<TypeScriptParameter>(), createblock);
            var classFields = Array.Empty<TypeScriptClassField>();
            TypeScriptFunction[] mixinFunctions = new TypeScriptFunction[] { previousFunction, nextFunction, createEventFunction, deleteFunction, editFunction };
            VueClassProp ComponentProp = new("Component", "{ components: {}}");
            VueClassProp[] classProps = new[] { ComponentProp };
            TypeScriptClass[] mixins = new[] { dataFieldsClass };
            var componentClass = new TypeScriptClass(_options.ComponentName, classProps: classProps, functions: mixinFunctions, mixins: mixins, fields: classFields);

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

        protected virtual TypeScriptClass CreateListDataFieldsMixin(TypeScriptClass apiMixin)
        {
            TypeScriptFunctionArguments[] functionArguments = new TypeScriptFunctionArguments[]
            {
            };

            var functionCallParams = new List<TypeScriptFunctionArguments>() { new TypeScriptFunctionArguments("this.DataModel"), new TypeScriptFunctionArguments("this.After") };
            foreach (var field in _options.SearchFields)
            {
                functionCallParams.Add(new TypeScriptFunctionArguments("this." + field.Name));
            }
            var block = new TypeScriptStatement[]        {
            new TypescriptAssignment(new TypescriptVariable("","this.DataModel",null)
            ,new TypescriptFunctionCall($"this.List{T.Name}",functionCallParams .ToArray(),true)                    )
            };
            var classFields = new List<TypeScriptClassField>()
        {
                new TypeScriptClassField("DataModel", new TypescriptTypeDeclaration(_maskTypeName + "[]"),
                    "[]"),
                  new TypeScriptClassField("After", new TypescriptTypeDeclaration("number"),"0"),
        };
            List<TypeScriptParameter> listParamaters = new List<TypeScriptParameter>();

            foreach (var field in _options.SearchFields)
            {
                classFields.Add(new TypeScriptClassField(field.Name, new TypescriptTypeDeclaration(TypeToparam(field.TypeName())), TypeToparamdefaultValue(field.TypeName())));
            }
            var createFunction = new TypeScriptFunction("Load" + T.Name,
                new TypescriptTypeDeclaration(new TypescriptType(null)),
                true, listParamaters.ToArray(),
                block);
            VueClassProp ComponentProp = new VueClassProp("Component", "{ components: {}}");
            var dataMixin = new TypeScriptClass(T.Name + "DataFields", classProps: new[] { ComponentProp },

                functions: new TypeScriptFunction[] { createFunction }, mixins: new TypeScriptClass[] { apiMixin },
                fields: classFields.ToArray());
            dataMixin.ExportNonDefault();
            return dataMixin;
        }

        protected virtual TypeScriptClass CreateListAllMixin()
        {
            // $"new {_options.RequestObjectType}  ( {{ {_options.RequestObjectAfterField} : {_options.ComponentAfterFieldName} }} ) "
            var initalizers = new List<TypeScriptObjectInitalizer>() { new TypeScriptObjectInitalizer(_options.RequestObjectAfterField, _options.ComponentAfterFieldName) };
            
            foreach (var field in _options.SearchFields)
            {
                initalizers.Add(new TypeScriptObjectInitalizer(field.Name, field.Name));
            }
            var requestObject = new TypeScriptObject(_options.RequestObjectType,new TypeScriptFunctionArguments[] { new TypeScriptFunctionArguments(new TypeScriptObject(initalizers.ToArray())) });
            TypeScriptFunctionArguments apicallArguments = new TypeScriptFunctionArguments(requestObject);

            TypeScriptFunctionArguments[] functionArguments = new TypeScriptFunctionArguments[]
            {
                apicallArguments
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
                        " this.ApiCallSuccess = Response.Success;",
                        " this.ApiCallMessage = Response.Message;",
                        $" if ( Response.Success) {{",

                        $"return  Response.{_options.ResponseObjectFieldName}.map(a => new {_maskTypeName}(a))",
                        "}",
                        " return []"
                    },
                    new TypeScriptStatement[] { " this.ApiCallMessage = e.message;", "console.log(e); return []"})
            };
            var apiSuccessField =
                new TypeScriptClassField("ApiCallSuccess", new TypescriptTypeDeclaration("boolean"), "true");
            var apiMessageField =
                new TypeScriptClassField("ApiCallMessage", new TypescriptTypeDeclaration("string"), "\"\"");
            var classFields = new List<TypeScriptClassField>(new TypeScriptClassField[]
                {
                    apiSuccessField,
                    apiMessageField
                });
            List<TypeScriptParameter> listParamaters = new List<TypeScriptParameter>(new[] { new TypeScriptParameter("DataModel", new TypescriptTypeDeclaration(_maskTypeName + "[]")),
                 new TypeScriptParameter("After", new TypescriptTypeDeclaration("number")),
            });

            foreach (var field in _options.SearchFields)
            {
                listParamaters.Add(new TypeScriptParameter(field.Name, new TypescriptTypeDeclaration(TypeToparam(field.TypeName()))));
                //    classFields.Add(new TypeScriptClassField(field.Name, new TypescriptTypeDeclaration(TypeToparam(field.TypeName())), TypeToparamdefaultValue(field.TypeName())));
            }
            var createFunction = new TypeScriptFunction("List" + T.Name,
                new TypescriptTypeDeclaration(new TypescriptType(null)),
                true, listParamaters.ToArray(),
                block);
            VueClassProp ComponentProp = new VueClassProp("Component", "{ components: {}}");
            var apiMixin = new TypeScriptClass(T.Name + "ApiMixin", classProps: new[] { ComponentProp },
                functions: new TypeScriptFunction[] { createFunction }, extends: TypeScriptClass.Vue,
                fields: classFields.ToArray());
            apiMixin.ExportNonDefault();
            return apiMixin;
        }
    }
}