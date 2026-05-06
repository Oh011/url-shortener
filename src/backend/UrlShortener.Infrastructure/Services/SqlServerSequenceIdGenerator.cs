using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using UrlShortener.Application.Abstrcations.Persistence;
using UrlShortener.Infrastructure.Persistence.Context;

namespace UrlShortener.Infrastructure.Services
{
    internal class SqlServerSequenceIdGenerator : IUniqueIdGenerator
    {

        private readonly ApplicationDbContext _context;


        public SqlServerSequenceIdGenerator(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<long> GetNextId()
        {

            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT NEXT VALUE FOR UrlShortenerSeq";

            if (_context.Database.CurrentTransaction != null)
            {
                command.Transaction = _context.Database.CurrentTransaction.GetDbTransaction();
            }

            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt64(result);

        }
    }
}
