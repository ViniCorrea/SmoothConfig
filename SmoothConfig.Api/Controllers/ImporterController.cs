using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmoothConfig.Api.Importer;
using SmoothConfig.Api.Repositories;
using SmoothConfig.Api.Services;
using SmoothConfig.Api.ViewModel;


namespace SmoothConfig.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImporterController : ControllerBase
    {
        private readonly IImporterService _importerService;

        public ImporterController(IImporterService importerService)
        {
            _importerService = importerService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult GenerateTokenizedConfig(IFormFile file)
        {
            var contentType = "application/octet-stream";

            if (file == null || file.ContentType != contentType) return BadRequest();

            var importer = new TokenizedImporter(file.OpenReadStream());
            var newFile = importer.GetXMLTokenized();

            var ms = new MemoryStream();
            newFile.Save(ms);
            byte[] bytes = ms.ToArray();

            return new FileContentResult(bytes, contentType)
            {
                FileDownloadName = file.FileName
            };
        }

        [HttpPost]
        [Authorize]
        public IActionResult ImportConfig([FromForm] ImportConfigViewModel importConfigViewModel)
        {
            if (importConfigViewModel is null)
                return UnprocessableEntity();

            _importerService.ImportConfig(importConfigViewModel);

            return Ok();
        }
    }
}
