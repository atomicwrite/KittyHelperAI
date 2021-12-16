using System;

namespace KittyHelper.Options
{
    public class CreateGetByIdEndpointOptions : CreateOptions
    {
        public CreateGetByIdEndpointOptions(Type t, string baseType = null,
            CreateOptionsAuthenticationOptions authenticate = null,
            string[] requiredRoles = null) : base(t, baseType ?? $"Get{t.Name}ById", authenticate)
        {
        }

        public string IdField { get; set; } = "Id";
        public string RequestIdField { get; set; } = "Id";
    }
}