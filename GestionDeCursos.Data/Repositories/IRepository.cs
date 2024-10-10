using System.Data.Common;

namespace GestionDeCursos.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> Get(int? id);
        Task<TEntity?> Get(Guid? id);
        Task Add(TEntity entity);
        void Remove(TEntity entity);



        Task<List<T>> ExecuteStoredProcedureAsync<T>(Action<DbCommand> cmd);
        Task ExecuteStoredProcedureNoResultsAsync(Action<DbCommand> cmd);

        void SetParameterCommand(DbCommand cmd, string name, object? value);
    }
}
