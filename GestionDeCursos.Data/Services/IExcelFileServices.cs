using GestionDeCursos.Data.MongoModels;

namespace GestionDeCursos.Data.Services
{
    public interface IExcelFileServices
    {
        Task<List<ExcelFile>> GetAll();
        Task<ExcelFile> GetById(string id);
        Task Create(ExcelFile file);
        Task Update(string id, ExcelFile file);
        Task Delete(string id);

    }
}
