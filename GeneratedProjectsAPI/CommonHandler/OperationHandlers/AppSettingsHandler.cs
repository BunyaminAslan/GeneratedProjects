using GeneratedProjectsAPI.CommonHandler.Models;

namespace GeneratedProjectsAPI.CommonHandler.OperationHandlers
{
    public class AppSettingsHandler : BaseHandler
    {
        public override void Handle(RequestContext context)
        {
            try
            {
                Console.WriteLine("Generating AppSettings started");

                CreateAppSettings(context.ProjectPath, context.ConnectionString, context.ProjectName);

                base.Handle(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;
            }
            finally
            {
                Console.WriteLine("Generating AppSettings done");
            }
        }
        private void CreateAppSettings(string projectPath, string connectionString, string projectName)
        {
            var appSettingsPath = Path.Combine(projectPath, $"{projectName}.API", "appsettings.json");

            var appSettingsContent = $@"
{{
    ""Logging"": {{
        ""LogLevel"": {{
            ""Default"": ""Information"",
            ""Microsoft.AspNetCore"": ""Warning""
        }}
    }},
    ""ConnectionStrings"": {{
        ""DBConnection"": ""{connectionString}""
    }}
}}";

            File.WriteAllText(appSettingsPath, appSettingsContent);
        }
    }
}
