using System;
using System.Collections.Generic;
using System.Linq;

namespace KittyHelper.Options
{
    public class CreateListEndPointOptions<A> : CreateOptions<A>
    {
        public CreateListEndPointOptions(string baseNameSpace,  string baseType = null,
            CreateOptionsAuthenticationOptions authenticate = null,
            string[] requiredRoles = null,SearchField[] searchFields = null) : base( baseType ?? "List" + (typeof(A)).Name,baseNameSpace, authenticate)
        {
            var t = typeof(A);
            ResponseObjectFieldName = $"{t.Name}s";
            VueRouterDirectory = t.Name;
            if (searchFields == null)
            {
                var tmp = new List<SearchField>();
                var _SearchFields = t.GetProperties().ToArray();
                foreach(var field in _SearchFields)
                {
                    var attr = field.GetCustomAttributes(true);
                    if (attr.Any(a => a.GetType().Name == "SearchFieldAttribute"))
                        tmp.Add(new SearchField(field.Name, field.PropertyType));
                }

                SearchFields = tmp.ToArray();
                tmp.Clear();
            }
            else
            {
                SearchFields = searchFields;
            }
        }
        public SearchField[] SearchFields { get; set;  }

        public string RequestObjectAfterField { get; set; } = "After";
        public string DbModelIdfield { get; set; } = "Id";
        public string RecordReturnCountLimit { get; set; } = "50";
        public string ResponseObjectFieldName { get; set; }
        public string ComponentAfterFieldName { get; set; } = "After";
    }
}