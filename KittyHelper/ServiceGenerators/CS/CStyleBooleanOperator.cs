using System;

namespace KittyHelper
{
    public class CStyleBooleanOperator
    {
        public static implicit operator CStyleBooleanOperator(string a)
        {
            return new CStyleBooleanOperator(a);
        }

        private readonly string op;

        public CStyleBooleanOperator(string op)
        {
            this.op = op;
        }

        public string Render()
        {
            return op;
        }
    }
}