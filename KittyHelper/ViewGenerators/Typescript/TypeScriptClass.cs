using System;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        { 
            public class TypeScriptClass  : TypeScriptStatement
            {
                private readonly string name;
                private readonly VueClassProp[] classProps;
                private readonly TypeScriptClass extends;
                private readonly TypeScriptClass[] mixins;
                private readonly VueVModel[] vmodels;
                private readonly VueProp[] props;
                private readonly TypeScriptClassField[] fields;
                private readonly VueWatcher[] watchers;
                private readonly TypeScriptFunction[] functions;
                public static TypeScriptClass Vue = new("Vue");
                private string exportString;

                public string ExternalRequirePath { get;   set; }
                public string ExportName { get;   set; }

                public TypeScriptClass(string name, VueClassProp[] classProps = null, TypeScriptFunction[] functions = null, TypeScriptClass extends = null, TypeScriptClass[] mixins = null, VueVModel[] Vmodels = null, VueProp[] props = null, TypeScriptClassField[] fields = null, VueWatcher[] watchers = null)
                {
                    this.name = name;
                    exportString = "export default ";
                    this.classProps = classProps ?? Array.Empty<VueClassProp>();
                    this.extends = extends;
                    this.mixins = mixins ?? Array.Empty<TypeScriptClass>();
                    vmodels = Vmodels ?? Array.Empty<VueVModel>();
                    this.props = props ?? Array.Empty<VueProp>();
                    this.fields = fields ?? Array.Empty<TypeScriptClassField>();
                    this.watchers = watchers ?? Array.Empty<VueWatcher>();
                    this.functions = functions ?? Array.Empty<TypeScriptFunction>();
                }

                public override string Render()
                {
                    string extendsStr = "";

                    string classPropStr = string.Join(Environment.NewLine, classProps.Select(a => a.Render()));
                    var vvmodelsStr = string.Join(Environment.NewLine, vmodels.Select(a => a.Render()));
                    var propsStr = string.Join(Environment.NewLine, props.Select(a => a.Render()));
                    var fieldsStr = string.Join(Environment.NewLine, fields.Select(a => a.Render()));
                    var watchersStr = string.Join(Environment.NewLine, watchers.Select(a => a.Render()));
                    var functionsStr = string.Join(Environment.NewLine, functions.Select(a => a.Render()));


                    if (mixins.Length != 0)
                    {
                        extendsStr = "extends Mixins(" + string.Join(",", mixins.Select(a => a.name)) + ")";
                    }
                    else if (extends != null)
                    {
                        extendsStr = "extends " + extends.name;
                    }

                    return @$"
                                 {classPropStr}
                                 {exportString} class {name} {extendsStr} {{
                                    {propsStr}
                                    {vvmodelsStr}
                                    {fieldsStr}
                                    {watchersStr}
                                    {functionsStr}
                    }}

";

                }

                public void ExportNonDefault()
                {
                    exportString = "export";
                }
            }
        }
    }
}