using System;
using System.Collections.Generic;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueComponentScript : TypeScriptStatement
            {
                public List<VueImport> Imports { get; set; } = new();
                public List<TypeScriptClass> VueClass { get; set; } = new();

                public override string Render()
                {
                    var imports = string.Join(Environment.NewLine, Imports.Select(a => a.Render()));
                    var classes = string.Join(Environment.NewLine, VueClass.Select(a => a.Render()));
                    return $@"<script lang=""ts"">
                            {imports}
                            {classes}
                            </script>";
                }
            }
        }
    }
}