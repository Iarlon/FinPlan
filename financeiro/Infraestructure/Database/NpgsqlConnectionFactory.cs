using financeiro.Infraestructure.Database;
using Npgsql;
using System.Data;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public NpgsqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não foi encontrada.");

        return new NpgsqlConnection(connectionString);
    }
}
