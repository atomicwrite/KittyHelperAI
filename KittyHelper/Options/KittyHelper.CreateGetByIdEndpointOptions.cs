using System;

namespace KittyHelper.Options
{
    public class CreateGetByIdEndpointOptions<A> : CreateOptions<A>
    {
        public CreateGetByIdEndpointOptions(string baseNameSpace, string baseType = null,
            CreateOptionsAuthenticationOptions authenticate = null,
            string[] requiredRoles = null) : base( baseType ?? $"Get{typeof(A).Name}ById", baseNameSpace,authenticate)
        {
        }

        public string IdField { get; set; } = "Id";
        public string RequestIdField { get; set; } = "Id";
    }
}