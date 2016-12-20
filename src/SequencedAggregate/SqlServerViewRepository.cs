using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace SequencedAggregate
{
    internal class SqlServerViewRepository : IViewRepository
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private readonly string _pkName;

        internal SqlServerViewRepository(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
            _pkName = tableName.Replace("-", string.Empty);
        }

        internal SqlServerViewRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ProjectionRepository"].ConnectionString;
            _tableName = "ProjectionRepository";
            _pkName = "IdType";
        }

        public void Commit<TView>(string id, TView view) where TView : class
        {
            var type = typeof(TView).ToString();
            var json = JsonConvert.SerializeObject(view);

            var sql = $@"IF EXISTS (SELECT * FROM [dbo].[{_tableName}] WHERE [Id] = '{id}' AND [Type] = '{type}')
                         BEGIN
                             UPDATE [dbo].[{_tableName}] SET [Json] = '{json}'
                             WHERE [Id] = '{id}' AND [Type] = '{type}'
                         END
                         ELSE
                         BEGIN
                             INSERT INTO [dbo].[{_tableName}] ([Id], [Type], [Json])
                             VALUES ('{id}', '{type}', '{json}')
                         END";

            Execute(sql);
        }

        public TView Read<TView>(string id) where TView : class
        {
            var type = typeof(TView).ToString();

            var sql = $@"SELECT [Json] FROM [dbo].[{_tableName}] 
                         WHERE [Id] = '{id}' AND [Type] = '{type}'";

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var command = new SqlCommand(sql, sqlConnection))
                {
                    string json = command.ExecuteScalar() as string;

                    if (string.IsNullOrEmpty(json)) return null;

                    return JsonConvert.DeserializeObject<TView>(json);
                }
            }
        }

        public void CreateProjectionsTable()
        {
            var sql = $@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{_tableName}]') AND type in (N'U'))
                         BEGIN
                         CREATE TABLE [dbo].[{_tableName}] (
                             [Id] [nvarchar] (255),
                             [Type] [nvarchar] (255),
                             [Json] [nvarchar] (max) NOT NULL,
                             CONSTRAINT PK_{_pkName} PRIMARY KEY NONCLUSTERED ([Id], [Type])
                         ) ON [PRIMARY];
                         END";

            Execute(sql);
        }

        private void Execute(string sql)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand(sql, sqlConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
