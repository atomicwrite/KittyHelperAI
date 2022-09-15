using ServiceStack;
using System;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptObject : TypeScriptStatement
            {
                private readonly string name;
                private readonly TypeScriptObjectInitalizer[] initalizers;
                private readonly TypeScriptFunctionArguments[] parameters;

                public TypeScriptObject(TypeScriptObjectInitalizer[] initalizers = null)
                {
                    this.initalizers = initalizers ?? Array.Empty<TypeScriptObjectInitalizer>();

                }
                public TypeScriptObject(string name, TypeScriptFunctionArguments[] parameters=null)
                {
                    this.name = name;
                    this.initalizers =  Array.Empty<TypeScriptObjectInitalizer>();
                    this.parameters = parameters;
                }
            
                public override string Render()
                {
                    if(name != null)
                    {
                        var param = "";
                        if(parameters != null)
                        {
                            param = string.Join(",", parameters.Select(a => a.Render()));
                        }
                        return $"new {name}({param})";
                    }
                    return "{" + initalizers.Select(a => a.Render()).Join("," + Environment.NewLine) + "}";
                }

            }
        }
    }
}