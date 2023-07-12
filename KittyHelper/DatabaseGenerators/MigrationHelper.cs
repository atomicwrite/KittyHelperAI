using System;
using System.Data;
using System.Linq;

namespace KittyHelper.DatabaseGenerators
{
    public static class KittyHelper
    {
        public static class MigrationHelper
        {
            public static void AutoWireUp(IDbConnection dbConnection)
            {
                var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
                var autos = types
                    .Where(x => typeof(IHasTable4U).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(a => (IHasTable4U) Activator.CreateInstance(a)).OrderBy(a => a?.Priority);
                foreach (var auto in autos) auto?.TableUp(dbConnection);
            }

            public static string GenerateCreateIfNotExists(Type t, int createPriority = 4)
            {
                return $@" 

using System.Data;
using KittyHelper; 
using {t.Namespace};
using ServiceStack.OrmLite;

namespace MuhBot.Migrations
{{

public class {t.Name}Migration : IHasTable4U {{
public int Priority {{get;set;}} = {createPriority};
public void TableUp(IDbConnection Db){{
 if (!Db.TableExists<{t.Name}>())
                {{
                    Db.CreateTable<{t.Name}>();
                }}
}}
}} }}
                ";
            }
        }
    }
}