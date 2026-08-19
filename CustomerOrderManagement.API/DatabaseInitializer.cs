

using CustomerOrderManagement.Infrastructure.Data.Contexts;
using CustomerOrderManagement.Infrastructure.Migrations;
using Serilog;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace CustomerOrderManagement.API
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var configuration = new Configuration();

                    var migrator = new DbMigrator(configuration);

                    var pendingMigrations = migrator.GetPendingMigrations().ToList();

                    if (pendingMigrations.Any())
                    {
                        Log.Information("Found {Count} pending database migrations.",pendingMigrations.Count);

                        foreach (var migration in pendingMigrations)
                        {
                            Log.Information("Applying migration: {Migration}",migration);
                        }

                        migrator.Update();

                        Log.Information("Database migrations applied successfully.");
                    }
                    else
                    {
                        Log.Information("Database is up to date. No pending migrations.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex,"Database migration failed.");

                throw;
            }
        }
    }
}