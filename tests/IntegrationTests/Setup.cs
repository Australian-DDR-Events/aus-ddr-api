using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests
{
    public static class Setup
    {
        public static EFDatabaseContext Connect()
        {
            var connectionString =
                "Username=admin;Password=password;Host=localhost;Port=1235;Database=IntegrationTests";
            var options = new DbContextOptionsBuilder<EFDatabaseContext>();
            options
                .UseNpgsql(connectionString);
            return new EFDatabaseContext(options.Options);
        }
        
        public static void Migrate(EFDatabaseContext context)
        {
            context.Database.Migrate();
        }
        
        public static void DropAllRows(EFDatabaseContext context)
        {
            if (context.Database.GetDbConnection().Database != "IntegrationTests")
            {
                throw new ArgumentException("can only drop all rows on IntegrationTests database");
            }

            var query = @"
CREATE OR REPLACE FUNCTION truncate_schema(_schema character varying)
  RETURNS void AS
$BODY$
declare
    selectrow record;
begin
for selectrow in
select 'TRUNCATE TABLE ' || quote_ident(_schema) || '.' ||quote_ident(t.table_name) || ' CASCADE;' as qry 
from (
     SELECT table_name 
     FROM information_schema.tables
     WHERE table_type = 'BASE TABLE' AND table_schema = _schema AND table_name != '__EFMigrationsHistory'
     )t
loop
execute selectrow.qry;
end loop;
end;
$BODY$
  LANGUAGE plpgsql;

SELECT truncate_schema('public');";
            
            context.Database.ExecuteSqlRaw(query);
        }
    }
}