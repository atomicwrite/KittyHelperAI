namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypescriptConditionStatement : TypeScriptStatement
            {
                private readonly TypeScriptStatement leftside;
                private readonly TypeScriptBooleanOperator op;
                private readonly TypeScriptStatement rightside;

                public TypescriptConditionStatement(TypeScriptStatement leftside, TypeScriptBooleanOperator op, TypeScriptStatement rightside)
                {
                    this.leftside = leftside;
                    this.op = op;
                    this.rightside = rightside;
                }
                public override string Render()
                {
                    return $"{leftside.Render()} {op.Render()} {rightside.Render()}";
                }



            }
        }
    }
}