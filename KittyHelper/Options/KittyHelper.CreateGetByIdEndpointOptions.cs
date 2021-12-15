using System;

namespace KittyHelper.Options
{
     
        public class CreateGetByIdEndpointOptions : CreateOptions
        {
            public string IdField { get; set; } = "Id";
            public string RequestIdField { get; set; } = "Id";

            public CreateGetByIdEndpointOptions(Type t, string baseType = null,
                CreateOptionsAuthenticationOptions authenticate = null,
                string[] requiredRoles = null) : base(t, baseType ?? $"Get{t.Name}ById", authenticate)
            {
            }
        }
     
}