using DbUp;
using System.Reflection;

namespace Financeiro.Infraestructure.Database;

public static class MigrationRunner
{
    public static void Run(string connectionString)
    {
        var upgrader =
        DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(
                Assembly.GetExecutingAssembly(),
                s => s.Contains("Database.Scripts"))
            .JournalToPostgresqlTable("public", "schemaversions")
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
            throw new Exception("Erro ao executar migrations", result.Error);
    }
}
