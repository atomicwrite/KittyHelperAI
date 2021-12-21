using KittyHelper.ServiceGenerators.CS;
namespace KittyHelper
{
  
            public class CStyleTypeDeclaration
            {
                public static CStyleTypeDeclaration NoReturnType = new CStyleTypeDeclaration(new CStyleType(null));
                private readonly CStyleType type;
                public CStyleTypeDeclaration(string tpe)
                {
                    type = new CStyleType(tpe);
                }
                public CStyleTypeDeclaration(CStyleType type)
                {
                    this.type = type;
                }
                public string Render()
                {
                    return type.Render();
                    
                }
            }
        }
 