using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AusDdrApi.Helpers
{
    public class PopulateDatabase
    {
        public static async Task PopulateDatabaseWithDummyData(DatabaseContext context)
        {
            if (context.Database.GetDbConnection().Database != "local")
            {
                throw new ArgumentException("can only populate local database");
            }

            await DropTables(context);
            await AddSongs(context);
        }

        private static async Task DropTables(DatabaseContext context)
        {
            const string? query = @"
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

            await context.Database.ExecuteSqlRawAsync(query);
        }

        private static async Task AddSongs(DatabaseContext context)
        {
            await context.AddRangeAsync(new List<Song>
            {
                new()
                {
                    Id = Guid.Parse("94ddb821-5d1d-4eba-a848-3bd086f0fea5")
                },
                new()
                {
                    Id = Guid.Parse("f47d376b-67d7-4789-b880-371ca9ea5da8")
                },
                new()
                {
                    Id = Guid.Parse("1fe2344a-dde0-466f-aa76-2c9784783670")
                },
                new()
                {
                    Id = Guid.Parse("b005f39f-de77-4939-965f-9852a1ed9efc")
                },
                new()
                {
                    Id = Guid.Parse("0665bd6e-a39c-417e-9be1-4e34c6eca781")
                },
            });

            await context.SaveChangesAsync();
        }
    }
}