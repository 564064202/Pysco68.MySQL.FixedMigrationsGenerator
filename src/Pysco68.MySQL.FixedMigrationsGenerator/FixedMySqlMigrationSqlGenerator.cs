namespace Pysco68.MySQL.FixedMigrationsGenerator
{
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Sql;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Extended MySqlMigrationSqlGenerator to fix GUID as index name issue
    /// </summary>
    public class FixedMySqlMigrationSqlGenerator : MySql.Data.Entity.MySqlMigrationSqlGenerator
    {
        private static MD5 md5 = MD5.Create();

        /// <summary>
        /// Make sure keys don't get too long...
        /// </summary>
        /// <param name="fkName"></param>
        /// <returns></returns>
        private string EscapeForeignKeyName(string fkName)
        {
            if (fkName.Length > 64)
            {
                StringBuilder sb = new StringBuilder();

                var part1 = fkName.Substring(0, 51);
                sb.Append(part1);
                sb.Append("_");

                var part2 = fkName.Substring(51);
                byte[] buf = System.Text.Encoding.UTF8.GetBytes(part2);
                byte[] hash = md5.ComputeHash(buf, 0, buf.Length);

                for (int i = 0; i < 6; i++)
                    sb.Append(hash[i].ToString("x2"));

                return sb.ToString();
            }

            return fkName;
        }

        private string TrimSchemaPrefix(string name)
        {
            // I have to do that on my own...
            if (name.StartsWith("dbo."))
                return name.Substring(4);

            return name;
        }

        protected override MigrationStatement Generate(DropForeignKeyOperation op)
        {
            op.Name = TrimSchemaPrefix(op.Name);
            op.Name = EscapeForeignKeyName(op.Name);
            op.PrincipalTable = TrimSchemaPrefix(op.PrincipalTable)
            op.DependentTable = TrimSchemaPrefix(op.DependentTable);
            return base.Generate(op);
        }

        protected override MigrationStatement Generate(AddForeignKeyOperation op)
        {
            op.Name = TrimSchemaPrefix(op.Name);
            op.Name = EscapeForeignKeyName(op.Name);
            return base.Generate(op);
        }
    }
}
