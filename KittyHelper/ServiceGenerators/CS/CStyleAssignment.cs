using KittyHelper.ServiceGenerators.CS;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
       
        public static partial class KittyViewHelper
        {
            public class CStyleAssignment : CStyleStatement
            {
                private readonly CStyleVariable variable;
                private readonly CStyleStatement statement;

                public CStyleAssignment(CStyleVariable variable, CStyleStatement statement)
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