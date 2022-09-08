using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static KittyHelper.DatabaseGenerators.KittyHelper.MigrationHelper;

namespace KittyHelper.Options
{
    public abstract class ViewOptions
    {
    }

    public abstract class CreateOptions<T>
    { 
        //todo: move to extension method

        private readonly string baseNameSpace;
        public readonly CreateOptionsAuthenticationOptions Authenticate;

        protected CreateOptions( string baseType, string baseNameSpace, CreateOptionsAuthenticationOptions authenticate = null)
        {
            var t = typeof(T);
            this.baseNameSpace = baseNameSpace;
            Authenticate = authenticate;
            BaseType = baseType;
            ResponseObjectType = $"{BaseType}Response";
            ServiceObjectType = $"{BaseType}Service";
            RequestObjectType = $"{BaseType}Request";
            ServiceType = $"{BaseType}Service";
            var an = new StringBuilder();
            if (Authenticate is {Authenticate: true}) an.AppendLine("[Authenticate]");

            if (Authenticate is {AllowedRoles: { }})
                an.AppendLine($"[RequiredRoles({FormatRoles(Authenticate.AllowedRoles)}]");

            
            ComponentName = BaseType;
               
            ResponseObjectNamespace =   $"{baseNameSpace}.{BaseType}Models";
            RequestObjectNamespace =  $"{baseNameSpace}.{BaseType}Models";
            ServiceObjectNamespace =  $"{baseNameSpace}.{BaseType}Service";

            Annotations = an.ToString();
        }
        public string ResponseObjectNamespace { get; set; }
        public string RequestObjectNamespace { get; set; }
        
        public string ServiceObjectNamespace { get; set; }
        public   string VueRouterDirectory { get; set; }
        public string UserIdField { get; set; } = "UserId";

        public string HttpVerb { get; set; } = "Post";
        public string ResponseObjectType { get; set; }
        public string ServiceObjectType { get; set; }
        public string RequestObjectType { get; set; }

 
        public string RequestObjectName { get; set; } = "request";

        public string Annotations { get; set; } = "";
        public string ServiceType { get; set; }
        protected string BaseType { get; set; }
        public string ResponseObjectName { get; set; } = "response";
        public string ComponentName { get; set; }  
      
    



        protected static string FormatRoles(string[] requiredRoles)
        {
            return string.Join(",", requiredRoles.Select(a => '"' + a + '"'));
        }
    }
}