// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using KittyHelper;
using KittyHelper.Options;
using MuhBot.ServiceModel;
using static KittyHelper.KittyHelper;
using static KittyHelper.ServiceGenerators.KittyServiceHelper;

Console.WriteLine("Hello, World!");

void GenerateServiceUpdateTest<T>()
{
    var t = typeof(T);
    var projectWriter = GetProjectWriter();
    var createUpdateEndPointOptions = new CreateUpdateEndPointOptions<T>(projectWriter.ModelBaseNamespace)
    {
        HttpVerb = "Put"
    };
    var generateUpdateEndPoint = new CreateUpdateEndPoint<T>(createUpdateEndPointOptions);
    var updateVueGenerator = new UpdateVueGenerator<T>(createUpdateEndPointOptions);

    projectWriter.GenerateServiceAndVue<T>(updateVueGenerator, generateUpdateEndPoint, createUpdateEndPointOptions);
}
void GenerateServiceListTest<T>()
{
    var t = typeof(T);
    var projectWriter = GetProjectWriter();
    var createUpdateEndPointOptions = new CreateListEndPointOptions<T>(projectWriter.ModelBaseNamespace)
    {
        HttpVerb = "Get"
    };
    var generateUpdateEndPoint = new CreateListEndPoint<T>(createUpdateEndPointOptions);
    var updateVueGenerator = new ListVueGenerator<T>(createUpdateEndPointOptions);

    projectWriter.GenerateServiceAndVue<T>(updateVueGenerator, generateUpdateEndPoint, createUpdateEndPointOptions);
}
void GenerateServiceCreateTest<T>()
{
    var t = typeof(T);
    var projectWriter = GetProjectWriter();
    var createUpdateEndPointOptions = new CreateCreateEndPointOptions<T>(projectWriter.ModelBaseNamespace);
    var generateUpdateEndPoint = new CreateCreateEndPoint<T>(createUpdateEndPointOptions);
    var updateVueGenerator = new CreateVueGenerator<T>(createUpdateEndPointOptions);
    
    projectWriter.GenerateServiceAndVue<T>(updateVueGenerator, generateUpdateEndPoint, createUpdateEndPointOptions);
    
}

GenerateServiceCreateTest<LanderJob>();
GenerateServiceListTest<ShellScript>();
GenerateServiceListTest<LanderJob>();
GenerateServiceListTest<FileStorage>();

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
/*
void GenerateSiteConfigStructures()
{
  //  var projectWriter = GetProjectWriter();
   // var t = typeof(SiteConfig);
 /*   CrudHelper.GenerateData(t, projectWriter, new Type[] { typeof(SiteConfigSection), typeof(GitRepo) });
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


GenerateSiteConfigStructures();


*/