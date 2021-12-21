using System;

namespace KittyHelper.Options
{
    public class CreateListEndPointOptions<A> : CreateOptions<A>
    {
        public CreateListEndPointOptions(string baseNameSpace,  string baseType = null,
            CreateOptionsAuthenticationOptions authenticate = null,
            string[] requiredRoles = null) : base( baseType ?? "List" + (typeof(A)).Name,baseNameSpace, authenticate)
        {
            var t = typeof(A);
            ResponseObjectFieldName = $"{t.Name}s";
        }

        public string RequestObjectAfterField { get; set; } = "After";
        public string DbModelIdfield { get; set; } = "Id";
        public string RecordReturnCountLimit { get; set; } = "50";
        public string ResponseObjectFieldName { get; set; }
        public string ComponentAfterFieldName { get; set; } = "After";
    }
}