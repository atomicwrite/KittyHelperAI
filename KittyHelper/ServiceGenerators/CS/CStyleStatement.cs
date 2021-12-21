namespace KittyHelper.ServiceGenerators.CS
{
     
    public abstract class CsPrintable 
    {
        public abstract string Render();
    }

    public class CStyleStatement : CsPrintable
    {
        public static implicit operator CStyleStatement(string a)
        {
            return new CStyleStatement(a);
        }

        private readonly string text;

        public CStyleStatement()
        {
        }

        public CStyleStatement(string text)
        {
            this.text = text;
        }

        public override string Render()
        {
            return text;
        }
    }
    
}