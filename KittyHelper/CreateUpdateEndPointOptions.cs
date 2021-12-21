using KittyHelper.Options;

namespace KittyHelper
{
    public class CreateUpdateEndPointOptions<A> : CreateOptions<A>
    {
        public CreateUpdateEndPointOptions(string baseNameSpace, string baseType = null,
            CreateOptionsAuthenticationOptions authenticate = null,
            string[] requiredRoles = null) : base(baseType ?? "Update" + typeof(A).Name, baseNameSpace,authenticate)
        {
            var t = typeof(A);
            RequestObjectUpdateObjectField = t.Name;
        }


        public string RequestObjectUpdateObjectField { get; set; }
    }
}