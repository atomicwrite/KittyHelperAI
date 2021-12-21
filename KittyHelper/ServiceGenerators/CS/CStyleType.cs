namespace KittyHelper.ServiceGenerators.CS
{
    public class CStyleType
    {
        private readonly string name;

        public CStyleType(string name)
        {
            this.name = name;
        }
                
        public string Render()
        {
            return string.IsNullOrEmpty(name) ? "" : name;
        }
    }
}