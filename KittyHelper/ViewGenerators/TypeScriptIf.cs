using ServiceStack;
using System;
using System.Linq;

namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypeScriptIf : TypeScriptStatement
            {
                private readonly TypescriptConditionStatement condition;
                private readonly TypeScriptStatement[] @true;
                private readonly TypeScriptStatement[] @false;

                public TypeScriptIf(TypescriptConditionStatement condition, TypeScriptStatement[] _true, TypeScriptStatement[] _false)
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
    }
}