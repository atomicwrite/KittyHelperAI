namespace KittyHelper
{
  
            public class CStyleParameter
            {
                private readonly string name;
                private readonly CStyleTypeDeclaration type;

                public CStyleParameter(string name, CStyleTypeDeclaration type)
                {
                    this.name = name;
                    this.type = type;
                }

                public string Render()
                {
                    return $"{type.Render()} {name}";
                }
            }
        }
 