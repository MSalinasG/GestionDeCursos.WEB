using GestionDeCursos.Data.Database;
using GestionDeCursos.Data.MongoModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GestionDeCursos.Data.Services
{
    public class ExcelFileServices : IExcelFileServices
    {
        private readonly IMongoCollection<ExcelFile> _excelFiles;

        public ExcelFileServices(MongoDbContext context)
        {
            _excelFiles = context.ExcelFiles;
        }

        public async Task<List<ExcelFile>> GetAll()
        {
            return await _excelFiles.Find(_ => true)
                .Sort(Builders<ExcelFile>.Sort.Ascending(file => file.FileName))
                .ToListAsync();
        }

        public async Task<ExcelFile> GetById(string id)
        {
            var mongoId = ObjectId.Parse(id);
            return await _excelFiles.Find(file => file.Id == mongoId)
                .FirstOrDefaultAsync();
        }

        public async Task Create(ExcelFile file)
        {
            await _excelFiles.InsertOneAsync(file);
        }        

        public async Task Update(string id, ExcelFile file)
        {
            var mongoId = ObjectId.Parse(id);
            await _excelFiles.ReplaceOneAsync(file => file.Id == mongoId, file);
        }
        public async Task Delete(string id)
        {
            var mongoId = ObjectId.Parse(id);
            await _excelFiles.DeleteOneAsync(file => file.Id == mongoId);
        }
    }
}
