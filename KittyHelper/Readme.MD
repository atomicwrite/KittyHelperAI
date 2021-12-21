# Kitty Helper

- V.04
- Builds C# Webservices for a type
- Builds Vue crud files
- Wharfs reference structures and build appropriate trees into the forms


# Usage
 ```c#
 
         var t = typeof(T);
    var projectWriter = GetProjectWriter();
    var createUpdateEndPointOptions = new CreateCreateEndPointOptions<T>(projectWriter.ModelBaseNamespace);
    var generateUpdateEndPoint = new CreateCreateEndPoint<T>(createUpdateEndPointOptions);
    var updateVueGenerator = new CreateVueGenerator<T>(createUpdateEndPointOptions);
    
    projectWriter.GenerateServiceAndVue<T>(updateVueGenerator, generateUpdateEndPoint, createUpdateEndPointOptions);

```
# Basic Ideology
0. TypescriptStatement is a basic printable statement, everything inherits from it.
0. You can cast a string implicityl to `TypeScriptStatement` to make writing it easier
0. CStyleStatement is a basic printable statement, everything inherits from it (on the c side)
0. You can cast a string implicitly to `CStyleStatement` to make writing it easier
0. Both have classes that inherit to make them easier like `CStyleIf` or `TypeScriptClassFunction` to keep your {  } and in check.
0. Vue templates can be generated using a `VueElement`. VueElements take a `VueAttribute` and again there are some premade like `VueClick` and also some premade vue elements like `BButton`


# Endpoint Classes
0. CreateCreateEndPoint
0. CreateListEndPoint
0. CreateUpdateEndPoint

* Can be inherited 

```c#
        protected virtual CStyleStatement GenerateDatabaseMethod()
        {
            return
                $" var count= Db.Update( {options.RequestObjectName}.{options.RequestObjectUpdateObjectField});";
        }

```

for example can be overrode to 

```c#
        protected override CStyleStatement GenerateDatabaseMethod()
        {
            return "{options.RequestObjectName}.Apple = 2" + 
                $" var count= Db.Update( {options.RequestObjectName}.{options.RequestObjectUpdateObjectField} );";
        }

```

or what ever your heart desires. All the classes are like this to make generating out a create/list/update with custom stampable functionality very fast but also easy to maintain.


One example would be to create an "updateonly on a list of fields by reading the properties. I'm sure I'll add those later but the sky is the limit"