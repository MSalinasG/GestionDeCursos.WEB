using GestionDeCursos.Data.MongoModels;

namespace GestionDeCursos.Data.Services
{
    public interface IPdfFileServices
    {
        Task<List<PdfFile>> GetAll();
        Task<PdfFile> GetById(string id);
        Task Create(PdfFile file);
        Task Update(string id, PdfFile file);
        Task Delete(string id);

    }
}
