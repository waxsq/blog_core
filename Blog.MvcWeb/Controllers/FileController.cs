using Blog.Core.Commons;
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

        public Task<EditReponse<FileResult>> UploadFile([FromForm] IFormFile file)
        {
            return null;
        }
    }
}
