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
            public static string GenerateListAllPage(Type T, ListAllViewOptions options)
            {
                VueComponent baseComponent = new();


                string maskTypeName = T.Name + "CreateMask";

                CreateListAllComponentTemplate(T, baseComponent);
                VueComponentScript script = baseComponent.Script;

                CreateListAllScriptImports(T, options, script);

                TypeScriptClass apiMixin = CreateListAllMixin(T, options, maskTypeName);
                TypeScriptClass maskMixin = CreateListAllMaskForType(T, maskTypeName);

                TypeScriptClass componentClass = CreateListAllComponentClass(T, options, maskTypeName, apiMixin);


                script.VueClass.Add(maskMixin);
                script.VueClass.Add(apiMixin);


                script.VueClass.Add(componentClass);

                return baseComponent.Render();
            }

            private static void CreateListAllScriptImports(Type T, ListAllViewOptions options,
                VueComponentScript script)
            {
                script.Imports.Add(new VueImport("vue-property-decorator", "Component", "Vue"));
                script.Imports.Add(new VueImport("vue-property-decorator", "{Mixins}"));
                script.Imports.Add(new VueImport("@/shared", "{client}"));
                script.Imports.Add(new VueImport("@/shared/dtos", options.RequestObjectName, T.Name,
                    options.ResponseObjectName));
            }

            private static void CreateListAllComponentTemplate(Type T, VueComponent baseComponent)
            {
                var root = baseComponent.RootElement;
                VueElement sectionElement = new VueSection();
                VueElement containerDiv = new VueDiv();

                root.AddChild(sectionElement);
                sectionElement.AddChild(containerDiv);


                CreateListAllDisplay(T, containerDiv);
            }

            private static TypeScriptClass CreateListAllMaskForType(Type T, string maskTypeName)
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

                TypeScriptClassField[] fields = new TypeScriptClassField[]
                {
                    new TypeScriptClassField("Message", new TypescriptTypeDeclaration("string"), "\"\""),
                    new TypeScriptClassField("Success", new TypescriptTypeDeclaration("boolean"), "true"),
                    new TypeScriptClassField("Completed", new TypescriptTypeDeclaration("boolean"), "true"),
                    new TypeScriptClassField("Error", new TypescriptTypeDeclaration("string"), "\"\""),
                };
                TypeScriptParameter[] parameters = new TypeScriptParameter[]
                {
                    new TypeScriptParameter("originalObject", new TypescriptTypeDeclaration(T.Name))
                };
                TypeScriptFunction[] functions = new TypeScriptFunction[]
                {
                    new TypeScriptFunction("constructor", TypescriptTypeDeclaration.NoReturnType, false, block: block,
                        vueParameters: parameters)
                };
                TypeScriptClass maskMixin =
                    new(maskTypeName, fields: fields, functions: functions, extends: new TypeScriptClass(T.Name));
                maskMixin.ExportNonDefault();
                return maskMixin;
            }


            private static void CreateListAllDisplay(Type T, VueElement containerDiv)
            {
                var bAlert = new VueBAlert("{{  DataModel.Message }}", new VueAttribute(":show", "true"),
                    new VIf("DataModel.Message.length >0"));

                var @group = CreatePagingGroup(T);


                containerDiv.AddChild(new VueH4($"List {T.Name}"));

                containerDiv.AddChild(bAlert);


                containerDiv.AddChild(group);


                containerDiv.AddChild(CreateListVForDisplayAsTable(T));
            }

            private static VueElement CreateListVForDisplayAsTable(Type type)
            {
                VueTableSimple table = new VueTableSimple();
                var head = new VueBHead();
                var tr = new VueBTr();
                head.AddChild(tr);
                var vForObjectName = "a";
                var vForTr = new VueBTr(new VFor(vForObjectName, "DataModel"));

                foreach (var t in type.GetProperties())
                {
                    var vueBTh = new VueBTh(t.Name);
                    tr.AddChild(CreateListButtonGroup(vForObjectName));
                    tr.AddChild(vueBTh);
                    vForTr.AddChild(new VueBTh($"{{{{ {t.Name} }}}}"));
                }

                table.AddChild(head);
                var vueBtBody = new VueBTBody();
                vueBtBody.AddChild(vForTr);
                table.AddChild(vueBtBody);
                return table;
            }

            public static BButtonGroup CreateListButtonGroup(string VForObjectName)
            {
                BButtonGroup group = new BButtonGroup();
                @group.AddChild(new BButton("Edit", new VueClickAttribute($"Edit({VForObjectName})")));
                @group.AddChild(new BButton("Delete", new VueClickAttribute($"Delete({VForObjectName})")));

                return @group;
            }
            public static BForm CreateSearchFilter( string[] SearchFields = null)
            {
                BForm group = new();
                foreach(var item in SearchFields)
                {
                    var formLabel = new VueElement(new VueTag("label",new VueAttribute("class","sr-only"),new VueAttribute("for",item+"Id")),item);
                    var formInput= new BFormInput(new VModelAttribute(item),new VueAttribute("id", item + "Id"));
                    group.AddChild(formLabel);
                    group.AddChild(formInput);

                }
 
                return @group;
            }
            public static BButtonGroup CreatePagingGroup(Type T, string[] SearchFields = null)
            {
                BButtonGroup group = new BButtonGroup();
                @group.AddChild(new BButton("Previous", new VueClickAttribute("Previous")));
                if(SearchFields == null || SearchFields.Length ==0)
                    @group.AddChild(new BButton("{{After}}", new VueClickAttribute($"List{T.Name}(DataModel,After)")));
                else
                {
                    
                    @group.AddChild(new BButton("{{After}}", new VueClickAttribute($"List{T.Name} (DataModel,After,{string.Join(",",SearchFields)})")));
                }
                @group.AddChild(new BButton("Next", new VueClickAttribute("Next")));
                return @group;
            }

            private static TypeScriptClass CreateListAllComponentClass(Type T, ListAllViewOptions options,
                string maskTypeName, TypeScriptClass apiMixin)
            {
                var classFields = new TypeScriptClassField[]
                {
                    new TypeScriptClassField("DataModel", new TypescriptTypeDeclaration(maskTypeName),
                        $"new {maskTypeName}(new {T.Name}())")
                };
                VueClassProp ComponentProp = new VueClassProp("Component", "{ components: {}}");
                var componentClass = new TypeScriptClass(options.ComponentName, new[] {ComponentProp}, null, null,
                    new[] {apiMixin}, null, null, classFields);
                return componentClass;
            }

            private static TypeScriptClass CreateListAllMixin(Type T, ListAllViewOptions options, string MaskType)
            {
                TypeScriptFunctionArguments[] functionArguments = new TypeScriptFunctionArguments[]
                {
                    new TypeScriptFunctionArguments(
                        $"new {options.RequestObjectName}  ( {{ {options.RequestObjectField} : DataModel }} ) ")
                };
                var apiCallStatement =
                    new TypescriptFunctionCall($"client.{options.HttpVerb.ToLower()}", functionArguments, true);

                var block = new TypeScriptStatement[]
                {
                    new TypeScriptTryCatchFinally(new TypeScriptStatement[]
                        {
                            new TypescriptAssignment(
                                new TypescriptVariable("const", "Response",
                                    new TypescriptType(options.ResponseObjectName)), apiCallStatement),
                            " DataModel.Success = Response.Success;",
                            new TypeScriptIf(new TypescriptConditionStatement("Response.Id", ">", "0"),
                                new TypeScriptStatement[] {" DataModel.Message = 'Created'",},
                                new TypeScriptStatement[] {" DataModel.Message = Response.Message;"})
                        },
                        new TypeScriptStatement[] {" DataModel.Message = e.message;", "console.log(e)"})
                };


                var createFunction = new TypeScriptFunction("Create" + T.Name,
                    new TypescriptTypeDeclaration(new TypescriptType(null)),
                    true, new[] {new TypeScriptParameter("DataModel", new TypescriptTypeDeclaration(MaskType))}, block);

                var apiMixin = new TypeScriptClass(T.Name + "ApiMixin", null, new TypeScriptFunction[] {createFunction},
                    TypeScriptClass.Vue);
                apiMixin.ExportNonDefault();
                return apiMixin;
            }

            public class ListAllViewOptions
            {
                public string ComponentName { get; set; }
                public string RequestObjectField { get; set; }
                public string HttpVerb { get; set; }
                public string RequestObjectName { get; set; }
                public string ListOneHttpVerb { get; set; }
                public string ListOneRequestObjectName { get; set; }
                public string ListOneRequestObjectField { get; set; }
                public Type[] ReferencedByTypes { get; set; }
                public bool DisableListAll { get; set; }
                public string ResponseObjectName { get; internal set; }
                public string ResponseObjectField { get; set; }
            }
        }
    }
}