using KittyHelper.ServiceGenerators.CS;

namespace KittyHelper
{
 
            public class CStyleConditionStatement : CStyleStatement
            {
                private readonly CStyleStatement leftside;
                private readonly  CStyleBooleanOperator op;
                private readonly CStyleStatement rightside;

                public CStyleConditionStatement(CStyleStatement leftside,  CStyleBooleanOperator op, CStyleStatement rightside)
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
 