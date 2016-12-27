using System;
using System.Data.SqlClient;
using NUnit.Framework;

namespace SequencedAggregate.Tests.Integration
{
    [TestFixture]
    public class SqlServerViewRepositoryTests
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariables().Contains("APPVEYOR")
                ? @"Server=(local)\SQL2014;Initial Catalog=SequencedAggregate;User ID=sa;Password=Password12!"
                : @"Data Source=SE-UTV28172; Initial Catalog=SequencedAggregate; Integrated Security=True";

        [Test]
        public void CreateTable_WhenTableDoesNotExist_TableCreatedCorrectly()
        {
            // Arrange
            var tableName = Guid.NewGuid().ToString();

            var configuration = new SqlServerViewRepositoryConfiguration(_connectionString, tableName);
            var viewRepository = new SqlServerViewRepository(configuration);

            // Act
            viewRepository.CreateProjectionsTable();

            // Assert
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var sql = $@"SELECT name FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{tableName}]') AND type in (N'U')";
                using (var command = new SqlCommand(sql, sqlConnection))
                {
                    string name = command.ExecuteScalar() as string;
                    Assert.That(name, Is.EqualTo(tableName));
                }
            }
        }
    }
}
