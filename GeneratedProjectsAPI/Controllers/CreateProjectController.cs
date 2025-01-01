using GeneratedProjectsAPI.CommonHandler.Models;
using GeneratedProjectsAPI.CommonHandler.OperationHandlers;
using GeneratedProjectsAPI.CommonHandler.Solution;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace GeneratedProjectsAPI.Controllers
{
    public class CreateProjectController : Controller
    {
        [HttpPost("delete-folder")]
        public async Task<IActionResult> DeleteFolder([FromBody] RequestContext request) {

            try
            {
                if (string.IsNullOrEmpty(request.ProjectPath))
                {
                    return BadRequest(new { message = "Geçerli bir proje yolu belirtilmelidir." });
                }

                Directory.Delete(request.ProjectPath, true);

                return await Task.FromResult(Ok(new { message = "Klasor basarili silindi." }));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Klasor silinirken bir hata oluştu.", error = ex.Message });
            }
        }

        [HttpPost("create-solution")]
        public async Task<IActionResult> GenerateSolution([FromBody] RequestContext request)
        {
            var projectPath = request.ProjectPath;
            var projReq = new RequestContext { ProjectPath = projectPath, ProjectName = request.ProjectName, SolutionName = request.SolutionName };
            if (string.IsNullOrEmpty(projectPath) || request.Tables == null || !request.Tables.Any())
            {
                return BadRequest(new { message = "Geçerli bir proje yolu ve tablo bilgileri belirtilmelidir." });
            }

            try
            {

                var createSolutionHandler = new SolutionHandler();
                var createRepositoryHandler = new RepositoryHandler();
                var createServiceHandler = new ServiceHandler();
                var createEntityHandler = new EntityHandler();
                var createDbContextHandler = new DbContextHandler();
                var createProgramHandler = new ProgramHandler();
                var createAppSettingsHandler = new AppSettingsHandler();
                var createControllerHandler = new ControllerHandler();
                
                createSolutionHandler.SetNext(createRepositoryHandler);
                createRepositoryHandler.SetNext(createServiceHandler);
                createServiceHandler.SetNext(createEntityHandler);
                createEntityHandler.SetNext(createDbContextHandler);
                createDbContextHandler.SetNext(createProgramHandler);
                createProgramHandler.SetNext(createAppSettingsHandler);
                createAppSettingsHandler.SetNext(createControllerHandler);

                createSolutionHandler.Handle(request);


                return await Task.FromResult(Ok(new { message = "Solution basarili oluşturuldu." }));
            }
            catch (Exception ex)
            {
                await DeleteFolder(projReq);

                return await Task.FromResult(StatusCode(500, new { message = "Repository sınıfları oluşturulurken bir hata oluştu.", error = ex.Message }));
            }
        }

    }
}
