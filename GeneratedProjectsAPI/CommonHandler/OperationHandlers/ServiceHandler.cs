using GeneratedProjectsAPI.CommonHandler.Models;


namespace GeneratedProjectsAPI.CommonHandler.OperationHandlers
{
    public class ServiceHandler : BaseHandler
    {
        public override void Handle(RequestContext context)
        {
            Console.WriteLine("Generating services setting up domain models");

            CreateBaseServiceInterface(context.ProjectPath, context.ProjectName);

            foreach (var table in context.Tables)
            {
                CreateServiceClass(context.ProjectPath, table, context.ProjectName);
            }

            base.Handle(context);
        }

        private void CreateBaseServiceInterface(string projectPath, string projectName)
        {

            var baseInterfaceRepo = $@"using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace {projectName}.Service.Base
{{
    public interface IBaseService<TEntity> where TEntity : class
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


            var repositoryFolderPath = Path.Combine(projectPath, $"{projectName}.Service", "Base");

            // Klasör yoksa oluştur
            if (!Directory.Exists(repositoryFolderPath))
            {
                Directory.CreateDirectory(repositoryFolderPath);
            }

            var repositoryInterfaceFilePath = Path.Combine(repositoryFolderPath, $"IBaseService.cs");


            File.WriteAllText(repositoryInterfaceFilePath, baseInterfaceRepo);

        }
        private void CreateServiceClass(string projectPath, Table table, string projectName)
        {
            var serviceCode = $@"
using {projectName}.Model;
using {projectName}.Repository.{table.TableName}_Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace {projectName}.Service.{table.TableName}_Service
{{
     public class {table.TableName}Service : I{table.TableName}Service
    {{
        readonly I{table.TableName}Repository _{table.TableName}Repo;
        readonly ILogger<{table.TableName}Service> _logger;
        public {table.TableName}Service(
            I{table.TableName}Repository {table.TableName}Service, ILogger<{table.TableName}Service> logger)
        {{
            _{table.TableName}Repo = {table.TableName}Service;
            _logger = logger;
        }}

        public EntityEntry<{table.TableName}> AddModel({table.TableName} entity)
        {{
            try
            {{
                return _{table.TableName}Repo.AddModel(entity);
            }}
            catch (Exception ex)
            {{
                _logger.LogError($""Add Model Error : {{ex.Message}}"");
                throw new Exception(ex.Message);
            }}
        }}

        public Task<EntityEntry<{table.TableName}>> AddModelAsync({table.TableName} entity)
        {{
            try
            {{
                return _{table.TableName}Repo.AddModelAsync(entity);
            }}
            catch (Exception ex)
            {{
                _logger.LogError($""Add Model Async Error : {{ex.Message}}"");
                throw new Exception(ex.Message);
            }}
        }}

        public IEnumerable<{table.TableName}> GetAllDatas()
        {{
            try
            {{
                return _{table.TableName}Repo.GetAllDatas();
            }}
            catch (Exception ex)
            {{
                _logger.LogError($""GetAllDatas Error : {{ex.Message}}"");
                throw new Exception(ex.Message);
            }}
        }}

        public Task<IEnumerable<{table.TableName}>> GetAllDatasAsync()
        {{
            try
            {{
                return _{table.TableName}Repo.GetAllDatasAsync();
            }}
            catch (Exception ex)
            {{
                _logger.LogError($""GetAllDatasAsync Error : {{ex.Message}}"");
                throw new Exception(ex.Message);
            }}
        }}

        public IQueryable<{table.TableName}> Query()
        {{
            try
            {{
                return _{table.TableName}Repo.Query();
            }}
            catch (Exception ex)
            {{
                _logger.LogError($""Query Error : {{ex.Message}}"");
                throw new Exception(ex.Message);
            }}
        }}

        public IQueryable<{table.TableName}> QueryAsNoTracking()
        {{
            try
            {{
                return _{table.TableName}Repo.QueryAsNoTracking();
            }}
            catch (Exception ex)
            {{
                _logger.LogError($""QueryAsNoTracking Error : {{ex.Message}}"");
                throw new Exception(ex.Message);
            }}
        }}

        public EntityEntry<{table.TableName}> UpdateModel({table.TableName} entity)
        {{
            try
            {{
                return _{table.TableName}Repo.UpdateModel(entity);
            }}
            catch (Exception ex)
            {{
                _logger.LogError($""UpdateModel Error : {{ex.Message}}"");
                throw new Exception(ex.Message);
            }}
        }}
    }}
}}";

            string serviceInterfaceCode = $@"using {projectName}.Model;
using {projectName}.Service.Base;

namespace {projectName}.Service.{table.TableName}_Service
{{
    public interface I{table.TableName}Service : IBaseService<{table.TableName}>
    {{
    }}
}}
";

            var serviceFolderPath = Path.Combine(projectPath, $"{projectName}.Service",$"{table.TableName}_Service");

            // Klasör yoksa oluştur
            if (!Directory.Exists(serviceFolderPath))
            {
                Directory.CreateDirectory(serviceFolderPath);
            }

            var serviceFilePath = Path.Combine(serviceFolderPath, $"{table.TableName}Service.cs");

            if (System.IO.File.Exists(serviceFilePath))
            {
                System.IO.File.Delete(serviceFilePath); // Eski dosyayı sil
            }

            var serviceFileInterfacePath = Path.Combine(serviceFolderPath, $"I{table.TableName}Service.cs");
            if (System.IO.File.Exists(serviceFileInterfacePath))
            {
                System.IO.File.Delete(serviceFileInterfacePath); // Eski dosyayı sil
            }

            System.IO.File.WriteAllText(serviceFileInterfacePath, serviceInterfaceCode);

            System.IO.File.WriteAllText(serviceFilePath, serviceCode);
        }

    }
}
