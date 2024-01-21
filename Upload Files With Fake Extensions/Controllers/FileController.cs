using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Upload_Files_With_Fake_Extensions.Model;

namespace Upload_Files_With_Fake_Extensions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ApplicationDbcontext _context;
        private readonly IWebHostEnvironment  _webHostEnvironment;
        public FileController(ApplicationDbcontext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetFile()
        {
           var AllFile = _context.uploadFiles.ToList();
            return Ok(AllFile);
        }
       

        [HttpPost]
        public IActionResult UploadFiles([FromForm]List<IFormFile> files)
        {
            List<UploadFile> uploadFiles = new List<UploadFile>();

            foreach (var file in files)
            {
                var fakeFileName = Path.GetRandomFileName();

                UploadFile uploadFile = new UploadFile
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    StoredFileName = fakeFileName
                };

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsfiles", fakeFileName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                uploadFiles.Add(uploadFile);
            }

            _context.uploadFiles.AddRange(uploadFiles);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet ("DownloadFile/{filename}")]
        public IActionResult DownloadFile(string filename)
        {
            var uploadFile = _context.uploadFiles.FirstOrDefault(f => f.StoredFileName == filename);

            if (uploadFile == null)
            {
                return NotFound();
            }

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsfiles", filename);

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                MemoryStream memoryStream = new MemoryStream();
                fileStream.CopyTo(memoryStream);

                memoryStream.Position = 0;

                return File(memoryStream, uploadFile.ContentType, uploadFile.FileName);
            }
        }


    }
}
