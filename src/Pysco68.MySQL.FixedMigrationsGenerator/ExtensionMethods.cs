using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace Pysco68.MySQL.FixedMigrationsGenerator
{
    public static class FixedMySQLGeneratorExtensionMethods
    {
        /// <summary>
        /// Register the fixed MySQL Migration Generator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        public static void SetFixedMySQLMigrationsGenerator<T>(this DbMigrationsConfiguration<T> configuration) where T : DbContext
        {
            configuration.SetSqlGenerator("MySql.Data.MySqlClient", new FixedMySqlMigrationSqlGenerator());
        }
    }
}
