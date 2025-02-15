using GeneratedProjectsAPI.CommonHandler.Models;


namespace GeneratedProjectsAPI.CommonHandler.OperationHandlers
{
    public class RepositoryHandler : BaseHandler
    {
        public override void Handle(RequestContext context)
        {
            Console.WriteLine("Generating repositories setting up domain models started");

            CreateBaseRepo(context.ProjectPath, context.ProjectName);

            foreach (var table in context.Tables)
            {
                CreateRepositoryClass(context.ProjectPath, table, context.ProjectName);
            }

            base.Handle(context);
        }
        private void CreateBaseRepo(string projectPath,string projectName)
        {
            var baseRepoCode = $@"using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace {projectName}.Repository.Base
{{
    public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
    {{
        protected readonly TContext _context;
        protected readonly ILogger _logger;

        public BaseRepository(TContext context, ILogger logger)
        {{
            _context = context;
            _logger = logger;
        }}


        public IQueryable<TEntity> Query()
        {{
            _logger.LogInformation(""Query Method Trigger"");
            try
            {{
               return _context.Set<TEntity>().AsQueryable<TEntity>();
            }}
            catch (Exception ex)
            {{
                _logger.LogError($""Query method get exp. Exp Detail : {{ex.Message}}"");
                throw new Exception($""Query method get exp. Exp Detail : {{ex.Message}}"");
            }}
            finally
            {{
                _logger.LogInformation(""Query Method Done"");
            }}
        }}

        public async Task<IEnumerable<TEntity>> GetAllDatasAsync()
        {{
            try
            {{
                return await _context.Set<TEntity>().ToListAsync();
            }}
            catch (Exception ex)
            {{
                throw new Exception($""GetAllDatas method get exp. Exp Detail : {{ex.Message}}"");
            }}
        }}

        public IEnumerable<TEntity> GetAllDatas()
        {{
            try
            {{
                return _context.Set<TEntity>().ToList();
            }}
            catch (Exception ex)
            {{
                throw new Exception($""GetAllDatas method get exp. Exp Detail : {{ex.Message}}"");
            }}
        }}

        public IQueryable<TEntity> QueryAsNoTracking()
        {{
            try
            {{
                return _context.Set<TEntity>().AsNoTracking().AsQueryable<TEntity>();
            }}
            catch (Exception ex)
            {{

                throw new Exception($""QueryAsNoTracking method get exp. Exp Detail : {{ex.Message}}"");
            }}
        }}

        public EntityEntry<TEntity> AddModel(TEntity entity)
        {{
            try
            {{
                var result = _context.Set<TEntity>().Add(entity);

                _context.SaveChanges();

                return result;
            }}
            catch (Exception ex)
            {{

                throw new Exception($""AddModel method get exp. Exp Detail : {{ex.Message}}"");
            }}
        }}
        public async Task<EntityEntry<TEntity>> AddModelAsync(TEntity entity)
        {{
            try
            {{
                var result = await _context.Set<TEntity>().AddAsync(entity);

                await _context.SaveChangesAsync();

                return result;
            }}
            catch (Exception ex)
            {{

                throw new Exception($""AddModel method get exp. Exp Detail : {{ex.Message}}"");
            }}
        }}

        public EntityEntry<TEntity> UpdateModel(TEntity entity)
        {{
            try
            {{
                return _context.Set<TEntity>().Update(entity);
            }}
            catch (Exception ex)
            {{

                throw new Exception($""AddModel method get exp. Exp Detail : {{ex.Message}}"");
            }}
        }}
    }}
}}

";

            var baseInterfaceRepo = $@"using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace {projectName}.Repository.Base
{{
    public interface IBaseRepository <TEntity, TContext> where TEntity : class where TContext : DbContext
    {{
        Task<IEnumerable<TEntity>> GetAllDatasAsync();
        IEnumerable<TEntity> GetAllDatas();
        IQueryable<TEntity> QueryAsNoTracking();
        IQueryable<TEntity> Query();

        Task<EntityEntry<TEntity>> AddModelAsync(TEntity entity);
        EntityEntry<TEntity> AddModel(TEntity entity);

        EntityEntry<TEntity> UpdateModel(TEntity entity);
    }}
}}
";


            var repositoryFolderPath = Path.Combine(projectPath , $"{projectName}.Repository","Base");

            // Klasör yoksa oluştur
            if (!Directory.Exists(repositoryFolderPath))
            {
                Directory.CreateDirectory(repositoryFolderPath);
            }

            var repositoryInterfaceFilePath = Path.Combine(repositoryFolderPath, $"IBaseRepository.cs");

            var repositoryFilePath = Path.Combine(repositoryFolderPath, $"BaseRepository.cs");


            File.WriteAllText(repositoryFilePath, baseRepoCode);
            File.WriteAllText(repositoryInterfaceFilePath, baseInterfaceRepo);

        }

        private void CreateRepositoryClass(string projectPath, Table table, string projectName)
        {
            var repositoryCode = $@"
using {projectName}.Model;
using {projectName}.Repository.Base;
using Microsoft.Extensions.Logging;

namespace {projectName}.Repository.{table.TableName}_Repository
{{
    public class {table.TableName}Repository : BaseRepository<{table.TableName}, AppDbContext>, I{table.TableName}Repository
    {{
        public {table.TableName}Repository(AppDbContext context, ILogger<{table.TableName}Repository> logger)
            : base(context, logger)
        {{
        }}
    }}
}}";

            var repositoryInterfaceCode = $@"using {projectName}.Repository.Base;
using {projectName}.Model;

namespace {projectName}.Repository.{table.TableName}_Repository
{{
    public interface I{table.TableName}Repository : IBaseRepository<{table.TableName}, AppDbContext>
    {{

    }}
}}
";

            var repositoryFolderPath = Path.Combine(projectPath, $"{projectName}.Repository", $"{table.TableName}_Repository");

            // Klasör yoksa oluştur
            if (!Directory.Exists(repositoryFolderPath))
            {
                Directory.CreateDirectory(repositoryFolderPath);
            }

            var repositoryFilePath = Path.Combine(repositoryFolderPath, $"{table.TableName}Repository.cs");
            var repositoryInterfaceFilePath = Path.Combine(repositoryFolderPath, $"I{table.TableName}Repository.cs");

            System.IO.File.WriteAllText(repositoryInterfaceFilePath, repositoryInterfaceCode);
            System.IO.File.WriteAllText(repositoryFilePath, repositoryCode);

        }
    }
}
