# Pysco68.MySQL.FixedMigrationsGenerator

Anyone who ever had to build something bigger using EntityFramework **with migrations** with MySQL or MariaDB as DBMS has probably experienced some of the flaws that the MySQL Connector/NET has.
The `FixedMigrationsGenerator` is a *duct-tape fixed* version of `MySqlMigrationSqlGenerator` found in the Connector/NET which makes it mostly *usable* in my context, in the hope it might help some of you too!

Currently it contains only one fix: the Connect/NET is using GUIDs as index names if the convention name is longer than 64 chars. This means every time the migration is actually executed, the generated GUID is different, which makes it pretty unlikely the DBMS will be able to - say - drop the index if a foreign key you want to suppress... (the whole story's over there: https://bugs.mysql.com/bug.php?id=74726)

## Installation

You can either clone this repository and include the project in your sources or install the nuget package using:

```
Install-Package Pysco68.MySQL.FixedMigrationGenerator 
```

## Usage

After installing the package as a dependency in your project you can replace the SQL code generator in the migrations configuration:

```C#
using Pysco68.MySQL.FixedMigrationsGenerator;

internal sealed class Configuration : DbMigrationsConfiguration<MyContext>
{
    public Configuration()
    {
        AutomaticMigrationsEnabled = false;
        SetSqlGenerator("MySql.Data.MySqlClient", new FixedMySqlMigrationSqlGenerator()); // <== this is it!
    }

    /* snip */
}
```

## Help / Contribution

If you found a bug, please create an issue. Want to contribute? Yes, please! Create a pull request!