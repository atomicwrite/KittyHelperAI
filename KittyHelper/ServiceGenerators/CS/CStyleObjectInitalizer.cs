using KittyHelper.ServiceGenerators.CS;
namespace KittyHelper
{
   
            public class CStyleObjectInitalizer : CStyleStatement
            {
                private readonly string name;
                private readonly string value;

                public CStyleObjectInitalizer(string name, string value)
                {
                    this.name = name;
                    this.value = value;
                }
                public override string Render()
                {
                    return $"{name} = {value}";
                }
            }
        }
 