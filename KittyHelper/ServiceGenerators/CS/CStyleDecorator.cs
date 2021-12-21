using System;
using System.Linq;
using KittyHelper.ServiceGenerators.CS;
using ServiceStack;

namespace KittyHelper
{
    public class CStyleDecorator : CStyleStatement
    {
        private readonly string name;
        private readonly CStyleStatement[] options;

        public CStyleDecorator(string name, CStyleStatement[] options)
        {
            this.name = name;

            this.options = options ?? Array.Empty<CStyleStatement>();
        }
        public override string ToString()
        {
            return Render();
        }

        public override string Render()
        {
            var parametersStr = options.Select(a=>a.Render()).Join(",");
            return $"[{name}({parametersStr})]";
        }
    }
}