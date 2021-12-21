using ServiceStack;
using System;
using System.Linq;
using KittyHelper.ServiceGenerators.CS;

namespace KittyHelper
{
    
            public class CStyleObject : CStyleStatement
            {
                private readonly string name;
                private readonly CStyleObjectInitalizer[] initalizers;
                public CStyleObject(string name, CStyleObjectInitalizer[] initalizers)
                {
                    this.name = name;

                    this.initalizers = initalizers ?? Array.Empty<CStyleObjectInitalizer>();
                }
              
                public override string Render()
                {
                    var inits = initalizers.Select(a => a.Render()).Join(Environment.NewLine + ",");
                    return $"new {name}() {{  {inits} }}";
                }

            }
        }
 