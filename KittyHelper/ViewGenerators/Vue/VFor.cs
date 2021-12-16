namespace KittyHelper
{
    public static partial class KittyHelper
    {

        public static partial class KittyViewHelper
        {
            public class VFor : VueAttribute 
            {
                private readonly string loopVariable;
                private readonly string loopOverVariable;
                private readonly string loopVariableInit;

                public VFor(string loopVariable, string loopOverVariable, string loopVariableInit = "const") : base("v-for", $"({loopVariableInit} {loopVariable}  of {loopOverVariable})")
                {
                    this.loopVariable = loopVariable;
                    this.loopOverVariable = loopOverVariable;
                    this.loopVariableInit = loopVariableInit;
                }
            }
        }
    }
}