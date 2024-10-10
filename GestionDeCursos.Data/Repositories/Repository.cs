
using AutoMapper;
using GestionDeCursos.Data.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Data;
using System.Data.Common;

namespace GestionDeCursos.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext Context;
        protected readonly IDbConnection DbDapper;
        protected readonly IMapper Mapper;

        public Repository(ApplicationDbContext contextParam, IMapper mapperParam, IDbConnection dbDapper)
        {
            Context = contextParam;
            Mapper = mapperParam;
            DbDapper = dbDapper;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> Get(int? id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> Get(Guid? id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task Add(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }




        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(Action<DbCommand> cmd)
        {
            DbConnection conn = null;
            try
            {
                conn = Context.Database.GetDbConnection();
                var records = new List<Dictionary<string, object>>();

                var newCmd = conn.CreateCommand();
                newCmd.CommandType = System.Data.CommandType.StoredProcedure;

                await conn.OpenAsync();
                cmd(newCmd);

                using (var reader = await newCmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection))
                {
                    while(await reader.ReadAsync())
                    {
                        records.Add(Enumerable.Range(0, reader.FieldCount)
                            .ToDictionary(reader.GetName, reader.GetValue));
                    }
                        
                }

                var jsonStr = JsonConvert.SerializeObject(records, GetPascalCaseSettings());
                var jsonArray = JArray.Parse(jsonStr);
                var finalResult = jsonArray.ToObject<List<T>>();

                return finalResult; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public async Task ExecuteStoredProcedureNoResultsAsync(Action<DbCommand> cmd)
        {
            DbConnection conn = null;
            try
            {
                conn = Context.Database.GetDbConnection();                

                var newCmd = conn.CreateCommand();
                newCmd.CommandType = System.Data.CommandType.StoredProcedure;

                await conn.OpenAsync();
                cmd(newCmd);
                await newCmd.ExecuteNonQueryAsync();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public void SetParameterCommand(DbCommand cmd, string name, object? value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            cmd.Parameters.Add(p);
        }

        private JsonSerializerSettings GetPascalCaseSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            };
        }
    }
}
