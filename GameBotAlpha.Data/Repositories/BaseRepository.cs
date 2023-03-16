using Microsoft.Data.SqlClient;

namespace GameBotAlpha.Data.Repositories
{
    public class BaseRepository
    {
        private readonly string _connectionString;
        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected SqlConnection Connection => new(_connectionString);
    }
}
