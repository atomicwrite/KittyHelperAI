using System;

namespace KittyHelper.Options
{
    public class CreateListEndPointOptions : CreateOptions
    {
        public CreateListEndPointOptions(Type t, string baseType = null,
            CreateOptionsAuthenticationOptions authenticate = null,
            string[] requiredRoles = null) : base(t, baseType ?? "List" + t.Name, authenticate)
        {
            ResponseObjectFieldName = $"{t.Name}s";
        }

        public string RequestObjectAfterField { get; set; } = "After";
        public string DbModelIdfield { get; set; } = "Id";
        public string RecordReturnCountLimit { get; set; } = "50";
        public string ResponseObjectFieldName { get; set; }
    }
}