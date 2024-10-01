using Dapper;
using Library.Api.Commons.Interfaces.Persistance;

namespace Library.Api.Persistance
{
    public class DatabaseInitializer
    {
        private readonly IDbConnnectionFactory _connnectionFactory;

        public DatabaseInitializer(IDbConnnectionFactory connnectionFactory)
        {
            _connnectionFactory = connnectionFactory;
        }

        public async Task InitilizeDatabaseAsync()
        {
            using var connection = await _connnectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                @"CREATE TABLE IF NOT EXISTS Books(
                    Isbn TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Author TEXT NOT NULL,
                    ShortDescription TEXT NOT NULL,
                    PageCount INTEGER,
                    ReleaceDate TEXT NOT NULL)"
                );
        }
    }
}
