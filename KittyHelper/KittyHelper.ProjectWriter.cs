using System;
using System.IO;
using KittyHelper.Options;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
        public class ProjectWriter
        {
            private readonly string _projectServiceInterfaceRoot;
            private readonly string _projectServiceModelRoot;

            public void GenerateServiceAndVue<T>(ICreateAVueComponent vueRenderer, ICreateAnEndPoint serviceGenerator,
                CreateOptions<T> options)
            {
                var t = typeof(T);

                var vueComponent = vueRenderer.Create();
                var vueFileContents = vueComponent.Render();
                var service = serviceGenerator.Create();
                var requestObject = serviceGenerator.CreateRequestClass();
                var responseObject = serviceGenerator.CreateResponseType();

                var requestFolder = $"{t.Name}Models{Path.DirectorySeparatorChar}";
                var responseFolder = $"{t.Name}Models{Path.DirectorySeparatorChar}";
                var serviceFolder = $"{t.Name}Service{Path.DirectorySeparatorChar}";

                WriteVueFile(t, options.ComponentName + ".vue", vueFileContents, true);
                WriteCsServiceFile(options.ServiceObjectType, service.Render(), serviceFolder, true);
                WriteCsModelFile(options.ResponseObjectType, responseObject.Render(), responseFolder, true);
                WriteCsModelFile(options.RequestObjectType, requestObject.Render(), requestFolder, true);
            //    WriteAutoRoute(options, t);
            }

            private void WriteAutoRoute<T>(CreateOptions<T> options, Type t)
            {
                WriteVueFile(t, "router.ts", KittyViewHelper.GenerateVueAutoRoute(new KittyViewHelper.ComponentPath[]
                {
                    new KittyViewHelper.ComponentPath()
                    {
                        Component = options.ComponentName ,
                        Path = "/" + options.ComponentName
                    }
                }), true);
            }

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