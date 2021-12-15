# Kitty Helper

- V.01
- Builds C# Webservices for a type
- Builds Vue crud files
- Wharfs reference structures and build appropriate trees into the forms


# Usage
 ```c#
 
    Type t = typeof(rabblebabble)
    var ServiceClassEndpointOptions =
                new KittyServiceHelper.CreateCreateEndPointOptions(t);

                   string requestClassDefinition =
                KittyServiceHelper.CreateCreateEndpointRequestClass(t, ServiceClassEndpointOptions);
                
                     string serviceClassDefinition =
                KittyServiceHelper.CreateCreateEndpointServiceClass(t,
                    ServiceClassEndpointOptions);

            string responseClassDefinition =
               KittyServiceHelper.CreateCreateEndpointResponseClass(t,
                    ServiceClassEndpointOptions);
             
             var RequestType = ServiceClassEndpointOptions.RequestType;
            var ReturnType = ServiceClassEndpointOptions.ReturnType;
            var ServiceType = ServiceClassEndpointOptions.ServiceType;
                    
                    KittyHelper.WriteService(t, projectWriter, requestClassDefinition, responseClassDefinition, serviceClassDefinition,
                    RequestType, ReturnType, ServiceType, OverWrite);
                    
                    


```


------------------------

# or 

- You can use the crudgenerator AI

```c#

var projectWriter = GetProjectWriter();
            var t = typeof(WiggleWaggleThatBazzle);
            CrudHelper. GenerateData(t, projectWriter, new Type[] {typeof(SomeOtherTypeConnectedByTree)});



```

#To do
`Completely automate the tree walking.`

`Namespaces and coding conventions.` 
 
`Add more attributes for control of how things are displayed. Right now we have DisplayTitleAttribute which sets what field shows up. If it doesn't find one it should just guess based on common names like "Name" or "title"`

`Wharfen zie`