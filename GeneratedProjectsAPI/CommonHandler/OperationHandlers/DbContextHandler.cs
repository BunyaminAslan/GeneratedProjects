using GeneratedProjectsAPI.CommonHandler.Models;

using System.Text;

namespace GeneratedProjectsAPI.CommonHandler.OperationHandlers
{
    public class DbContextHandler : BaseHandler
    {
        public override void Handle(RequestContext context)
        {
            Console.WriteLine("Generating DbContext and setting up domain models started");

            CreateDbContext(context.ProjectPath, context.Tables, context.ProjectName);

            base.Handle(context);
        }
        private void CreateDbContext(string projectPath, IEnumerable<Table> tables, string projectName)
        {
            var dbContextCode = $@"
using Microsoft.EntityFrameworkCore;
using {projectName}.Model;


namespace {projectName}.Repository
{{
    public class AppDbContext : DbContext
    {{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {{ }}

        {GenerateDbSets(tables)}
    }}
}}";

            var repositoryFolderPath = Path.Combine(projectPath, $"{projectName}.Repository");

            // Klasör yoksa oluştur
            if (!Directory.Exists(repositoryFolderPath))
            {
                Directory.CreateDirectory(repositoryFolderPath);
            }

            var dbContextFilePath = Path.Combine(repositoryFolderPath, "AppDbContext.cs");

            if (File.Exists(dbContextFilePath))
            {
                File.Delete(dbContextFilePath); // Eski dosyayı sil
            }

            File.WriteAllText(dbContextFilePath, dbContextCode);
        }
        private string GenerateDbSets(IEnumerable<Table> tables)
        {
            var dbSets = new StringBuilder();
            foreach (var table in tables)
            {
                dbSets.AppendLine($"        public DbSet<{table.TableName}> {table.TableName} {{ get; set; }}");
            }
            return dbSets.ToString();
        }

    }
}
