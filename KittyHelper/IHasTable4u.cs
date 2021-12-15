using System.Data;

namespace KittyHelper
{
    public interface IHasTable4U
    {
        void TableUp(IDbConnection Db);
        public int Priority { get; set; }
    }
}