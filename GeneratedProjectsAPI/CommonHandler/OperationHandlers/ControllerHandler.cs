using GeneratedProjectsAPI.CommonHandler.Models;

namespace GeneratedProjectsAPI.CommonHandler.OperationHandlers
{
    public class ControllerHandler : BaseHandler
    {
        public override void Handle(RequestContext context)
        {
            Console.WriteLine("Generating Controller started");

            CreateControllerClasses(context.ProjectPath, context.Tables, context.ProjectName);

            base.Handle(context);
        }

        private void CreateControllerClasses(string projectPath, IEnumerable<Table> tables, string projectName)
        {
            var controllersFolderPath = Path.Combine(projectPath, $"{projectName}.API", "Controllers");

            if (!Directory.Exists(controllersFolderPath))
            {
                Directory.CreateDirectory(controllersFolderPath);
            }

            foreach (var table in tables)
            {
                var controllerCode = $@"
using Microsoft.AspNetCore.Mvc;
using {projectName}.Service;
using {projectName}.Service.{table.TableName}_Service;


namespace {projectName}.API.Controllers
{{
    [ApiController]
    [Route(""api/[controller]"")]
    public class {table.TableName}Controller : ControllerBase
    {{
        private readonly I{table.TableName}Service _service;

        public {table.TableName}Controller(I{table.TableName}Service service)
        {{
            _service = service;
        }}

        [HttpGet]
        public IActionResult GetAll()
        {{
            var result = _service.GetAllDatas();
            return Ok(result);
        }}
    }}
}}";

                var controllerFilePath = Path.Combine(controllersFolderPath, $"{table.TableName}Controller.cs");
                File.WriteAllText(controllerFilePath, controllerCode);
            }
        }
    }
}
