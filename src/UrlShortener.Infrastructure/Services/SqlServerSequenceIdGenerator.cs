using Microsoft.EntityFrameworkCore;
using Project.Application.Common.Interfaces;
using Project.Infrastructure.Persistence.Context;

namespace Project.Infrastructure.Services
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
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT NEXT VALUE FOR UrlShortenerSeq";

            var result = await command.ExecuteScalarAsync();
            long nextId = Convert.ToInt64(result);



            return nextId;

        }
    }
}
