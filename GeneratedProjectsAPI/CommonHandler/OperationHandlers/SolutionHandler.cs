using GeneratedProjectsAPI.CommonHandler.Models;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GeneratedProjectsAPI.CommonHandler.Solution
{
    public class SolutionHandler : BaseHandler
    {
        public override void Handle(RequestContext context)
        {
            try
            {
                Console.WriteLine($"Creating solution: {context.SolutionName} in {context.ProjectPath}");
                // Logic to create solution

                CreateSolution(context);

                base.Handle(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateSolution(RequestContext request)
        {
            var projectPath = request.ProjectPath;

            if (string.IsNullOrEmpty(projectPath) || !Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }

            try
            {
                // Çözüm dosyasını oluştur
                var slnCommand = $"dotnet new sln -n {request.SolutionName} -o \"{projectPath}\"";
                RunCommand(slnCommand);

                // Model Katmanını oluştur
                var modelCommand = $"dotnet new classlib -n {request.ProjectName}.Model -o \"{Path.Combine(projectPath, $"{request.ProjectName}.Model")}\"";
                RunCommand(modelCommand);

                // Repository Katmanını oluştur
                var repoCommand = $"dotnet new classlib -n {request.ProjectName}.Repository -o \"{Path.Combine(projectPath, $"{request.ProjectName}.Repository")}\"";
                RunCommand(repoCommand);

                // Service Katmanını oluştur
                var serviceCommand = $"dotnet new classlib -n {request.ProjectName}.Service -o \"{Path.Combine(projectPath, $"{request.ProjectName}.Service")}\"";
                RunCommand(serviceCommand);

                // API Projesini oluştur
                var apiCommand = $"dotnet new webapi -n {request.ProjectName}.API -o \"{Path.Combine(projectPath, $"{request.ProjectName}.API")}\"";
                RunCommand(apiCommand);


                // Projeleri çözüme ekle
                var addModelCommand = $"dotnet sln \"{Path.Combine(projectPath, $"{request.SolutionName}.sln")}\" add \"{Path.Combine(projectPath, $"{request.ProjectName}.Model", $"{request.ProjectName}.Model.csproj")}\"";
                RunCommand(addModelCommand);

                var addRepoCommand = $"dotnet sln \"{Path.Combine(projectPath, $"{request.SolutionName}.sln")}\" add \"{Path.Combine(projectPath, $"{request.ProjectName}.Repository", $"{request.ProjectName}.Repository.csproj")}\"";
                RunCommand(addRepoCommand);

                var addServiceCommand = $"dotnet sln \"{Path.Combine(projectPath, $"{request.SolutionName}.sln")}\" add \"{Path.Combine(projectPath, $"{request.ProjectName}.Service", $"{request.ProjectName}.Service.csproj")}\"";
                RunCommand(addServiceCommand);

                var addApiCommand = $"dotnet sln \"{Path.Combine(projectPath, $"{request.SolutionName}.sln")}\" add \"{Path.Combine(projectPath, $"{request.ProjectName}.API", $"{request.ProjectName}.API.csproj")}\"";
                RunCommand(addApiCommand);

                // Projeler arası bağımlılıkları ayarla
                var referenceServiceCommand = $"dotnet add \"{Path.Combine(projectPath, $"{request.ProjectName}.Service", $"{request.ProjectName}.Service.csproj")}\" reference \"{Path.Combine(projectPath, $"{request.ProjectName}.Repository", $"{request.ProjectName}.Repository.csproj")}\"";
                RunCommand(referenceServiceCommand);

                var referenceApiCommand = $"dotnet add \"{Path.Combine(projectPath, $"{request.ProjectName}.API", $"{request.ProjectName}.API.csproj")}\" reference \"{Path.Combine(projectPath, $"{request.ProjectName}.Service", $"{request.ProjectName}.Service.csproj")}\"";
                RunCommand(referenceApiCommand);

                var referenceModelRepoCommand = $"dotnet add \"{Path.Combine(projectPath, $"{request.ProjectName}.Repository", $"{request.ProjectName}.Repository.csproj")}\" reference \"{Path.Combine(projectPath, $"{request.ProjectName}.Model", $"{request.ProjectName}.Model.csproj")}\"";
                RunCommand(referenceModelRepoCommand);
                // Projeler arası bağımlılıkları ayarla


                // API Projesinine SQL paket ekle
                var sqlPackageName = "Microsoft.EntityFrameworkCore.SqlServer";
                var installSqlCommand = $"dotnet add \"{Path.Combine(projectPath, $"{request.ProjectName}.API", $"{request.ProjectName}.API.csproj")}\" package {sqlPackageName}";
                RunCommand(installSqlCommand);


                //Repo Katmanına EF ekle
                var packageName = "Microsoft.EntityFrameworkCore";
                var installCommand = $"dotnet add \"{Path.Combine(projectPath, $"{request.ProjectName}.Repository", $"{request.ProjectName}.Repository.csproj")}\" package {packageName}";
                RunCommand(installCommand);


                // Migration icin Repo katmanına da ekliyor
                var sqlRepositoryPackageName = "Microsoft.EntityFrameworkCore.SqlServer";
                var installSqlRepositoryCommand = $"dotnet add \"{Path.Combine(projectPath, $"{request.ProjectName}.Repository", $"{request.ProjectName}.Repository.csproj")}\" package {sqlRepositoryPackageName}";
                RunCommand(installSqlRepositoryCommand);

                // Migration icin Disegnerı repo katmanına da ekliyor
                var designRepositoryPackageName = "Microsoft.EntityFrameworkCore.Design";
                var installDesignRepositoryCommand = $"dotnet add \"{Path.Combine(projectPath, $"{request.ProjectName}.Repository", $"{request.ProjectName}.Repository.csproj")}\" package {designRepositoryPackageName}";
                RunCommand(installDesignRepositoryCommand);

                // Migration icin Tools repo katmanına da ekliyor
                var toolRepositoryPackageName = "Microsoft.EntityFrameworkCore.Tools";
                var installToolRepositoryCommand = $"dotnet add \"{Path.Combine(projectPath, $"{request.ProjectName}.Repository", $"{request.ProjectName}.Repository.csproj")}\" package {toolRepositoryPackageName}";
                RunCommand(installToolRepositoryCommand);

                // Migration icin Disegnerı Api katmanına da ekliyor
                var designAPIPackageName = "Microsoft.EntityFrameworkCore.Design";
                var installDesignAPICommand = $"dotnet add \"{Path.Combine(projectPath, $"{request.ProjectName}.API", $"{request.ProjectName}.API.csproj")}\" package {designRepositoryPackageName}";
                RunCommand(installDesignAPICommand);
            }
            catch (Exception ex)
            {
                throw new Exception("Solution ve proje oluşturulurken bir hata oluştu.");
            }
        }

        private void RunCommand(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Komut başarısız oldu: {command}\nHata: {process.StandardError.ReadToEnd()}");
            }
        }

    }
}
