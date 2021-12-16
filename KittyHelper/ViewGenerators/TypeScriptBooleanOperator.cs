using System;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptBooleanOperator
            {
                public static implicit operator TypeScriptBooleanOperator(string a)
                {
                    return new TypeScriptBooleanOperator(a);
                }
                private readonly string op;

                public TypeScriptBooleanOperator(string op)
                {
                    this.op = op;
                }

                public string Render()
                {
                    return op;
                }
            }
        }
    }
}