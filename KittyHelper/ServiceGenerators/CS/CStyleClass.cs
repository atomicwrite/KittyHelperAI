using System;
using System.Linq;
using KittyHelper.ServiceGenerators.CS;
using ServiceStack;

namespace KittyHelper
{
    public class CStyleClass  : CStyleStatement
            {
                private readonly string name;
                private readonly CStyleDecorator[] classDecorators;
                private readonly CStyleClass extends;

                private readonly string classModifiers;
                
                private readonly CStyleClassField[] fields;
                
                private readonly CStyleFunction[] functions;

                private readonly string nameSpace;

                private readonly string[] usings;
                
                

                public CStyleClass(string name, string nameSpace, string[] usings=null, string classModifiers = "public", CStyleDecorator[] classProps = null, CStyleFunction[] functions = null, CStyleClass extends = null,  CStyleClassField[] fields = null)
                {
                    this.name = name;
                    this.nameSpace = nameSpace;
                    this.usings = usings??Array.Empty<string>();
                    this.classModifiers = classModifiers;
                    this.classDecorators = classProps ?? Array.Empty<CStyleDecorator>();
                    this.extends = extends;
                
                    this.fields = fields ?? Array.Empty<CStyleClassField>();
                
                    this.functions = functions ?? Array.Empty<CStyleFunction>();
                }

                public override string Render()
                {
                    

                    string classPropStr = string.Join(Environment.NewLine, classDecorators.Select(a => a.Render()));

                    var usingsStr = usings.Select(a=> $"using {a};").Join(Environment.NewLine);
                    var fieldsStr = string.Join(Environment.NewLine, fields.Select(a => a.Render()));
                    
                    var functionsStr = string.Join(Environment.NewLine, functions.Select(a => a.Render()));


                    var extendsStr = "";
                    if (extends != null)
                    {
                        extendsStr = ": " + extends.name;
                    }

                    return @$"{usingsStr}

                    namespace {nameSpace} {{

                                 {classPropStr}
                                 {classModifiers} class {name} {extendsStr} {{
                                    {fieldsStr}
                                    {functionsStr}
                    }}
}}
";

                }

             
            }
        }
     
 