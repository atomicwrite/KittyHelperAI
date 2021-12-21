using System;
using System.IO;
using KittyHelper.Options;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
        /*
        public static void WriteService(Type t, ProjectWriter projectWriter, string requestClassDefinition,
            string responseClassDefinition, string serviceClassDefinition, CreateOptions options, bool OverWrite)
        {
            var RequestWrapperOptions = new WrapWithClassAndNameSpaceOptions(t);
            var ResponseWrapperOptions = new WrapWithClassAndNameSpaceOptions(t);
            var ServiceWrapperOptions = new WrapWithClassAndNameSpaceOptions(t);

            ServiceWrapperOptions.AddUsing($"using {projectWriter.ServiceBaseNameSpace};");
            ServiceWrapperOptions.AddUsing("using Microsoft.AspNetCore.Hosting;");
            ServiceWrapperOptions.AddUsing("using ServiceStack;");
            ServiceWrapperOptions.AddUsing("using ServiceStack.Auth;");
            ServiceWrapperOptions.AddUsing("using ServiceStack.Configuration;");
            ServiceWrapperOptions.AddUsing("using ServiceStack.Logging; ");
            ServiceWrapperOptions.AddUsing("using System.Linq;");
            ServiceWrapperOptions.AddUsing("using ServiceStack.OrmLite;");
            ServiceWrapperOptions.AddUsing("using ServiceStack.FluentValidation;");
            ServiceWrapperOptions.AddUsing("using System;");
            ServiceWrapperOptions.AddUsing("using System.Threading.Tasks;");

            ServiceWrapperOptions.SetNameSpace($"{projectWriter.ServiceBaseNameSpace}.{t.Name}Service");
            ResponseWrapperOptions.SetNameSpace($"{projectWriter.ModelBaseNamespace}.{t.Name}Models");
            RequestWrapperOptions.SetNameSpace($"{projectWriter.ModelBaseNamespace}.{t.Name}Models");

            ResponseWrapperOptions.AddUsing($"using {t.Namespace};");
            ResponseWrapperOptions.AddUsing("using System.Collections.Generic;");
            RequestWrapperOptions.AddUsing("using ServiceStack;");
            RequestWrapperOptions.AddUsing("using System.Collections.Generic;");
            RequestWrapperOptions.AddUsing($"using {projectWriter.ModelBaseNamespace}.{t.Name}Models;");
            ServiceWrapperOptions.AddUsing($"using {projectWriter.ModelBaseNamespace}.{t.Name}Models;");
            ServiceWrapperOptions.AddUsing($"using {t.Namespace};");

            var RequestFolder = $"{t.Name}Models{Path.DirectorySeparatorChar}";
            var ResponseFolder = $"{t.Name}Models{Path.DirectorySeparatorChar}";
            var ServiceFolder = $"{t.Name}Service{Path.DirectorySeparatorChar}";

            var requestClass =
                KittyServiceHelper.WrapWithUsingsAndNameSpace(t, requestClassDefinition, RequestWrapperOptions);

            var responseClass =
                KittyServiceHelper.WrapWithUsingsAndNameSpace(t, responseClassDefinition, ResponseWrapperOptions);

            var servceClass =
                KittyServiceHelper.WrapWithUsingsAndNameSpace(t, serviceClassDefinition, ServiceWrapperOptions);


            projectWriter.WriteCsModelFile(options.RequestObjectType, requestClass, RequestFolder, OverWrite);

            projectWriter.WriteCsModelFile(options.ResponseObjectType, responseClass, ResponseFolder, OverWrite);

            projectWriter.WriteCsServiceFile(options.ServiceType, servceClass, ServiceFolder, OverWrite);
        }
*/
        public static partial class KittyFileHelper
        {
            public static void WriteFileFromNamespace(FileExportConfig config)
            {
                var ProjectRoot = config.ProjectRoot;
                var ConfigSubfolder = config.Subfolder;

                if (!Directory.Exists(ProjectRoot))
                    throw new ArgumentException("Directory " + ProjectRoot + " does not exist");
                if (!ProjectRoot.EndsWith(Path.DirectorySeparatorChar))
                    ProjectRoot += Path.PathSeparator;

                if (!ConfigSubfolder.EndsWith(Path.DirectorySeparatorChar))
                    ConfigSubfolder += Path.DirectorySeparatorChar;


                var exportPath = ProjectRoot + ConfigSubfolder;
                if (!Directory.Exists(exportPath))
                    Directory.CreateDirectory(exportPath);

                var exportFullPath = exportPath + config.FileName;
                if (File.Exists(exportPath) && !config.OverWrite) return;

                File.WriteAllText(exportFullPath, config.Contents);
            }
        }
    }
}