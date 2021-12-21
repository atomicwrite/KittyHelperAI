using System;
using System.Linq;
using KittyHelper.ServiceGenerators.CS;
using ServiceStack;

namespace KittyHelper
{
    public class CStyleLambda : CStyleStatement
    {
        private readonly CStyleParameter[] parameters;
        private readonly CStyleStatement[] body;
        public static CStyleLambda Get => new CStyleLambda(new CStyleParameter[] {new CStyleParameter("get",new CStyleTypeDeclaration(""))});
        public static CStyleLambda Set => new CStyleLambda(new CStyleParameter[] {new CStyleParameter("set",new CStyleTypeDeclaration(""))});
        public CStyleLambda(CStyleParameter[] parameters, CStyleStatement[] body = null)
        {
            this.parameters = parameters;
            this.body = body;
        }

        public override string Render()
        {
            var parametersStr = parameters.Select(a => a.Render()).Join(",");
            if (body == null)
            {
                return parameters.Length switch
                {
                    0 => "()=>{}",
                    1 => $"{parametersStr};",
                    _ => $"({parametersStr})"
                };
            }

            var bodyStr = body.Select(a => a.Render()).Join(Environment.NewLine);
            return parameters.Length switch
            {
                0 => $"()=>{{{bodyStr}}}",
                1 => $"{parametersStr} => {{{bodyStr}}}",
                _ => $"({parametersStr})=>{{{bodyStr}}}"
            };
        }
    }
}