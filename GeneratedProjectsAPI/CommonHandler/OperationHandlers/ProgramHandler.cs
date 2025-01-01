using GeneratedProjectsAPI.CommonHandler.Models;

using System.Text;

namespace GeneratedProjectsAPI.CommonHandler.OperationHandlers
{
    public class ProgramHandler : BaseHandler
    {
        public override void Handle(RequestContext context)
        {
            Console.WriteLine("Generating ProgramHandler and setting up domain models");

            AddServicesToProgram(context.ProjectPath, context.Tables, context.ProjectName);

            base.Handle(context);
        }

        private void AddServicesToProgram(string projectPath, IEnumerable<Table> tables, string projectName)
        {
            var programFilePath = Path.Combine(projectPath, $"{projectName}.API", "Program.cs");

            if (!System.IO.File.Exists(programFilePath))
            {
                throw new FileNotFoundException($"Program.cs dosyası bulunamadı: {programFilePath}");
            }


            var usingStatements = $@"
using Microsoft.EntityFrameworkCore;
using {projectName}.Service;
using {projectName}.Repository;
";

            foreach (var item in tables)
            {
                usingStatements += $@"using {projectName}.Repository.{item.TableName}_Repository;
using {projectName}.Service.{item.TableName}_Service;
";

            }

            var programContent = usingStatements + @"
" +
System.IO.File.ReadAllText(programFilePath);

            // AddScoped satırlarının ekleneceği yeri belirle
            var servicesSection = "// Add services to the container.";
            if (!programContent.Contains(servicesSection))
            {
                throw new InvalidOperationException($"Program.cs içinde `{servicesSection}` etiketi bulunamadı.");
            }

            var scopedRegistrations = new StringBuilder();

            scopedRegistrations.AppendLine($"builder.Services.AddDbContext<AppDbContext>(options =>\r\n        options.UseSqlServer(builder.Configuration.GetConnectionString(\"DBConnection\")));");

            foreach (var table in tables)
            {
                scopedRegistrations.AppendLine($"    builder.Services.AddScoped<I{table.TableName}Service,{table.TableName}Service>();");
                scopedRegistrations.AppendLine($"    builder.Services.AddScoped<I{table.TableName}Repository,{table.TableName}Repository>();");
            }

            scopedRegistrations.AppendLine($"builder.Services.AddControllers();\r\n");

            // Yeni içerik ile Program.cs dosyasını güncelle
            var updatedProgramContent = programContent.Replace(servicesSection, $"{servicesSection}\n{scopedRegistrations}");
            updatedProgramContent = AddUseControllerLine(updatedProgramContent);
            System.IO.File.WriteAllText(programFilePath, updatedProgramContent);
        }

        private string AddUseControllerLine(string content)
        {
            var scopedRegistrations = new StringBuilder();

            var servicesSection = "app.UseHttpsRedirection();";

            scopedRegistrations.AppendLine("app.MapControllers();");

            content = content.Replace(servicesSection, $"{servicesSection}\n{scopedRegistrations}");

            return content;
        }
    }
}
