namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class TypescriptAssignment : TypeScriptStatement
            {
                private readonly TypescriptVariable variable;
                private readonly TypeScriptStatement statement;

                public TypescriptAssignment(TypescriptVariable variable, TypeScriptStatement statement)
                {
                    this.variable = variable;
                    this.statement = statement;
                }
                public override string Render()
                {
                    return $"{variable.Render()} = {statement.Render()}";
                }
            }

        }
    }
}