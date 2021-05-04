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
            await AddSongDifficulties(context);
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

        private static async Task AddSongDifficulties(DatabaseContext context)
        {
            await context.AddRangeAsync(new List<SongDifficulty>
            {
                new()
                {
                    Id = Guid.Parse("22210e1e-f8fc-484d-a5e4-5680ff84d6b5"),
                    SongId = Guid.Parse("f47d376b-67d7-4789-b880-371ca9ea5da8")
                },
                new()
                {
                    Id = Guid.Parse("e715e642-dc8d-4b3e-bde5-aeda22f4cc18"),
                    SongId = Guid.Parse("1fe2344a-dde0-466f-aa76-2c9784783670")
                },
                new()
                {
                    Id = Guid.Parse("89321afb-cf74-4907-871b-0228487b3889"),
                    SongId = Guid.Parse("1fe2344a-dde0-466f-aa76-2c9784783670")
                },
                new()
                {
                    Id = Guid.Parse("5d5186ce-661e-4f55-bd10-84eb614cf2ce"),
                    SongId = Guid.Parse("b005f39f-de77-4939-965f-9852a1ed9efc")
                },
                new()
                {
                    Id = Guid.Parse("0565d83b-44fd-4780-b4e9-47fd765e837b"),
                    SongId = Guid.Parse("b005f39f-de77-4939-965f-9852a1ed9efc")
                },
                new()
                {
                    Id = Guid.Parse("7506966e-9307-435a-8671-d655487a1221"),
                    SongId = Guid.Parse("b005f39f-de77-4939-965f-9852a1ed9efc")
                },
                new()
                {
                    Id = Guid.Parse("b5fda847-c8b7-47a8-bef6-3e7a069e342f"),
                    SongId = Guid.Parse("0665bd6e-a39c-417e-9be1-4e34c6eca781")
                },
                new()
                {
                    Id = Guid.Parse("239ec546-7c0e-40ec-8fd9-391fd9a27d60"),
                    SongId = Guid.Parse("0665bd6e-a39c-417e-9be1-4e34c6eca781")
                },
                new()
                {
                    Id = Guid.Parse("0a62587e-0c30-4d8a-a6c6-e103a21d8db1"),
                    SongId = Guid.Parse("0665bd6e-a39c-417e-9be1-4e34c6eca781")
                },
                new()
                {
                    Id = Guid.Parse("c124199e-2645-4467-b413-97ad1b9c4997"),
                    SongId = Guid.Parse("0665bd6e-a39c-417e-9be1-4e34c6eca781")
                }
            });
            
            await context.SaveChangesAsync();
        }
    }
}