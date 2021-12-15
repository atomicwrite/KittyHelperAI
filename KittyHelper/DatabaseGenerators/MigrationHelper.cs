using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using KittyHelper.Options;


namespace KittyHelper.DatabaseGenerators
{
    public static partial class KittyHelper
    {

        public static class MigrationHelper
        {
            public static void AutoWireUp(IDbConnection dbConnection)
            {
                var Types =  AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
                var Autos = Types
                    .Where(x => typeof(IHasTable4U).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(a=> (IHasTable4U) Activator.CreateInstance(a)).OrderBy(a=>a.Priority);
                foreach (var Auto in Autos)
                {
                    Auto.TableUp(dbConnection);
                    
                }
            }
            public static string GenerateCreateIfNotExists(Type t, int CreatePriority =4)
            {
                return $@" 

using System.Data;
using KittyHelper; 
using {t.Namespace};
using ServiceStack.OrmLite;

namespace MuhBot.Migrations
{{

public class {t.Name}Migration : IHasTable4U {{
public int Priority {{get;set;}} = {CreatePriority};
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