using GestionDeCursos.Data.MongoModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GestionDeCursos.Data.Database
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public IMongoCollection<ExcelFile> ExcelFiles => _database.GetCollection<ExcelFile>("ExcelFiles");

        public IMongoCollection<PdfFile> PdfFiles => _database.GetCollection<PdfFile>("PdfPostulantes");

        public MongoDbContext(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }
    }
}
