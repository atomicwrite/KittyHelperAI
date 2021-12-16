using System.Data;

namespace KittyHelper
{
    public interface IHasTable4U
    {
        public int Priority { get; set; }
        void TableUp(IDbConnection Db);
    }
}