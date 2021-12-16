namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VueImport
            {
                private readonly string path;
                private readonly string defaultExport;
                private readonly string[] objects;
                private bool decompose;

                public VueImport(string path, params string[] decomposition)
                {
                    decompose = true;
                    this.path = path;
                    this.objects = decomposition;
                }
                public VueImport(string path, string defaultExport)
                {
                    decompose = false;
                    this.path = path;
                    this.defaultExport = defaultExport;

                }

                public string Render()
                {
                    if (decompose)
                    {
                        string objs = string.Join(",", objects);
                        return $"import {{ {objs} }} from '{path}'";
                    }
                    else
                    {
                        return $"import { defaultExport}  from '{path}'";
                    }
                }
            }
        }
    }
}