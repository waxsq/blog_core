using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Utils;
using Blog.FileStorage.Core;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : Controller
    {
        private readonly FileUploadStrategy _strategy;

        public FileController(FileUploadContext context)
        {
            _strategy = context.GetStrategy();
        }

        [Route("Img")]
        [HttpPost]
        public async Task<FileResponseResult> UploadFile([FromForm(Name = "editormd-image-file")] IFormFile file)
        {
            return await _strategy.UploadAsync(file, "upload/img");
        }
    }
}
