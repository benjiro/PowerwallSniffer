using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace PowerwallSniffer.DatabaseProvider.TimescaleDB.Repository
{
    using Npgsql;
    using NpgsqlTypes;
    using Model;

    public class CreateDatabase
    {
        private readonly string _connectionString;

        public CreateDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        private NpgsqlConnection Connection => new (_connectionString);

        public async Task Create()
        {
            await using var dbConnection = Connection;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "PowerwallSniffer.DatabaseProvider.TimescaleDB.CreateDatabase.sql";

            await using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            
            var script = await reader.ReadToEndAsync();
            var createDatabase = new NpgsqlCommand(script, dbConnection);
            
            await dbConnection.OpenAsync();
            await createDatabase.ExecuteNonQueryAsync();
            await dbConnection.CloseAsync();
        }
    }
}