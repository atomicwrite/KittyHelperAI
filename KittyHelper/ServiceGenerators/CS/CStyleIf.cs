using ServiceStack;
using System;
using System.Linq;
using KittyHelper.ServiceGenerators.CS;

namespace KittyHelper
{
   
            
            public class CStyleIf : CStyleStatement
            {
                private readonly CStyleConditionStatement condition;
                private readonly CStyleStatement[] @true;
                private readonly CStyleStatement[] @false;

                public CStyleIf(CStyleConditionStatement condition, CStyleStatement[] _true, CStyleStatement[] _false)
                {
                    this.condition = condition;
                    @true = _true;
                    @false = _false;
                }
                public override string Render()
                {
                    string falseStr = @false.Length > 0 ? $"else {{ {@false.Select(a=>a.Render()) .Join(Environment.NewLine) } }}" : "";
                    string trueStr = $"{{ {@true.Select(a => a.Render()).Join(Environment.NewLine)} }} ";
                    string conditionStr = condition.Render();

                    return $@"if({conditionStr})  {trueStr} {falseStr} ";
                }
            }
        }
  