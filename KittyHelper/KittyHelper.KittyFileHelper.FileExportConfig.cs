namespace KittyHelper
{
    public static partial class KittyHelper
    {
        public static partial class KittyFileHelper
        {
            public class FileExportConfig
            {
                public FileExportConfig(string projectRoot, string subfolder, string fileName, string contents,
                    bool overWrite)
                {
                    ProjectRoot = projectRoot;
                    Subfolder = subfolder;
                    FileName = fileName;
                    Contents = contents;
                    OverWrite = overWrite;
                }

                public string ProjectRoot { get; }
                public string Subfolder { get; }
                public string FileName { get; }
                public string Contents { get; }
                public bool OverWrite { get; }
            }
        }
    }
}