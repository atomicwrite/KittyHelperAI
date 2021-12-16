// See https://aka.ms/new-console-template for more information
using KittyHelper;
using MuhBot.ServiceModel;
using static KittyHelper.KittyHelper;

Console.WriteLine("Hello, World!");


void GenerateSiteConfigStructures()
{
    var projectWriter = GetProjectWriter();
    var t = typeof(SiteConfig);
    CrudHelper.GenerateData(t, projectWriter, new Type[] { typeof(SiteConfigSection), typeof(GitRepo) });
    t = typeof(SiteConfigSection);
    CrudHelper.GenerateData(t, projectWriter, new Type[] { typeof(SiteConfigValue) });
    t = typeof(SiteConfigValue);
    CrudHelper.GenerateData(t, projectWriter, null);
    t = typeof(AmazonAccount);
    CrudHelper.GenerateData(t, projectWriter, null);
    t = typeof(GitRepo);
    CrudHelper.GenerateData(t, projectWriter, null);
    t = typeof(SafePageTemplate);
    CrudHelper.GenerateData(t, projectWriter, null);
    t = typeof(GitAccount);
    CrudHelper.GenerateData(t, projectWriter, new Type[] { typeof(GitRepo) });
}

 ProjectWriter GetProjectWriter()
{
    var projectRoot = "C:\\ethan\\bloodorange\\MuhBot\\";
    var projectModelsRoot = "C:\\ethan\\bloodorange\\MuhBot.ServiceModel\\";
    var projectServiceRoot = "C:\\ethan\\bloodorange\\MuhBot.ServiceInterface\\";
    var ProjectViewFolder = "C:\\ethan\\bloodorange\\MuhBot\\src\\Views\\";
    var ModelBaseNamespace = "MuhBot.ServiceModel";
    var ServiceBaseNameSpace = "MuhBot.ServiceInterface";

    ProjectWriter projectWriter =
        new(projectRoot, projectModelsRoot, projectServiceRoot, ProjectViewFolder, ServiceBaseNameSpace,
            ModelBaseNamespace);
    return projectWriter;
}

GenerateSiteConfigStructures();


