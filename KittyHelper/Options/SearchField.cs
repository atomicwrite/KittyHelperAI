using System;

namespace KittyHelper.Options
{
    public class SearchField
    {
        private Type type;

        public SearchField(string name, Type type)
        {
            Name = name;
            this.type = type;
        }

        public string Name { get; set; }

 

        public string TypeName()
        {
            return type.Name;
        }

        
    }
}