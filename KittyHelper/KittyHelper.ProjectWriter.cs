using System;
using System.IO;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
        public class ProjectWriter
        {
            private readonly string _projectServiceInterfaceRoot;
            private readonly string _projectServiceModelRoot;


            public ProjectWriter(string projectRoot, string projectServiceModelRoot, string projectServiceInterfaceRoot,
                string projectVueViewRoot, string ServiceBaseNameSpace, string ModelBaseNamespace)
            {
                var tmp = projectRoot;
                if (!tmp.EndsWith(Path.DirectorySeparatorChar))
                    tmp += Path.DirectorySeparatorChar;
                ProjectRoot = tmp;
                ProjectVueViewRoot = projectVueViewRoot;
                this.ServiceBaseNameSpace = ServiceBaseNameSpace;
                this.ModelBaseNamespace = ModelBaseNamespace;
                _projectServiceModelRoot = projectServiceModelRoot;
                _projectServiceInterfaceRoot = projectServiceInterfaceRoot;
            }

            public string ProjectRoot { get; }
            public string ProjectVueViewRoot { get; }
            public string ServiceBaseNameSpace { get; }
            public string ModelBaseNamespace { get; }

            public void WriteCsServiceFile(string typeName,
                string contents, string folder, bool OverWrite)
            {
                var FileExportConfig =
                    new KittyFileHelper.FileExportConfig(_projectServiceInterfaceRoot, folder,
                        $"{typeName}.cs", contents, OverWrite);

                KittyFileHelper.WriteFileFromNamespace(FileExportConfig);
            }

            public void WriteCsModelFile(string typeName,
                string contents, string folder, bool OverWrite)
            {
                var FileExportConfig =
                    new KittyFileHelper.FileExportConfig(_projectServiceModelRoot, folder,
                        $"{typeName}.cs", contents, OverWrite);

                KittyFileHelper.WriteFileFromNamespace(FileExportConfig);
            }

            public void WriteCsMigrationFile(string migrationClassContents, string fileName, bool OverWrite)
            {
                var Migrations = ProjectRoot + "migrations" + Path.DirectorySeparatorChar;
                if (!Directory.Exists(Migrations))
                    Directory.CreateDirectory(Migrations);
                var filepath = Migrations + fileName;
                if (!OverWrite && File.Exists(filepath))
                    return;
                File.WriteAllText(filepath, migrationClassContents);
            }

            public void WriteVueFile(Type type, string vueFileName, string vueFileContents, bool overWrite)
            {
                var FolderPath = $"{ProjectVueViewRoot}{type.Name}{Path.DirectorySeparatorChar}";
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);
                var filepath = $"{FolderPath}{vueFileName}";
                if (!overWrite && File.Exists(filepath))
                    return;
                File.WriteAllText(filepath, vueFileContents);
            }
        }
    }
}