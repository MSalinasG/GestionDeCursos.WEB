using GestionDeCursos.Data.Database;
using GestionDeCursos.Data.MongoModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GestionDeCursos.Data.Services
{
    public class PdfFileServices : IPdfFileServices
    {
        private readonly IMongoCollection<PdfFile> _pdfFiles;

        public PdfFileServices(MongoDbContext context)
        {
            _pdfFiles = context.PdfFiles;
        }

        public async Task<List<PdfFile>> GetAll()
        {
            return await _pdfFiles.Find(_ => true)
                .Sort(Builders<PdfFile>.Sort.Ascending(file => file.FileName))
                .ToListAsync();
        }

        public async Task<PdfFile> GetById(string id)
        {
            var mongoId = ObjectId.Parse(id);
            return await _pdfFiles.Find(file => file.Id == mongoId)
                .FirstOrDefaultAsync();
        }

        public async Task Create(PdfFile file)
        {
            await _pdfFiles.InsertOneAsync(file);
        }        

        public async Task Update(string id, PdfFile file)
        {
            var mongoId = ObjectId.Parse(id);
            await _pdfFiles.ReplaceOneAsync(file => file.Id == mongoId, file);
        }
        public async Task Delete(string id)
        {
            var mongoId = ObjectId.Parse(id);
            await _pdfFiles.DeleteOneAsync(file => file.Id == mongoId);
        }
    }
}
