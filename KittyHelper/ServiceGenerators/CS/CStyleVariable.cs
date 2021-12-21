namespace KittyHelper.ServiceGenerators.CS
{
    public class CStyleVariable : CsPrintable
    {
        private readonly string init;
        private readonly string name;
        private readonly CStyleType type;

        public CStyleVariable(string init, string name, CStyleType type)
        {
            this.init = init;
            this.name = name;
            this.type = type;
        }
        public override string Render()
        {
            string typeStr = type.Render();
                    
            return $"{init} {name} {typeStr}";
        }
    }
}