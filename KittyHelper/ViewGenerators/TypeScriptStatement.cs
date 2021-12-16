using ServiceStack;
using System;

namespace KittyHelper
{
    public static class HelperClass
    {
        
    }
    public static partial class KittyHelper
    {
      
        public static partial class KittyViewHelper
        {
            public abstract class TypeScriptPrintable
            {
                 public abstract string Render();
            }
            public class TypeScriptStatement : TypeScriptPrintable
            {
               
                public static implicit operator TypeScriptStatement(string a)
                {
                    return new TypeScriptStatement(a);
                }
                private readonly string text;

                public TypeScriptStatement()
                {

                }
                public TypeScriptStatement(string text)
                {
                    this.text = text;
                }

                public override string Render()
                {
                    return text;
                }
            }
        }
    }
}