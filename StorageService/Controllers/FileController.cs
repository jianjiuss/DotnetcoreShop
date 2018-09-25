using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Controllers
{
    public class FileController : Controller
    {
        private string[] imageUseTypes = { "ProductIcon" , "ProductInfo", "ProductTitle", "UserHeadPhoto" };

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file, string imageUseType, string name)
        {
            if (!file.ContentType.ToLower().Contains("image"))
            {
                return BadRequest("不支持的类型");
            }

            if(!imageUseTypes.Contains(imageUseType))
            {
                return BadRequest("请指定好该图片用途");
            }
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", imageUseType);
            string fileName = (string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() : name) + file.FileName.Substring(file.FileName.IndexOf('.'));

            string fullPath = Path.Combine(filePath, fileName);

            if(System.IO.File.Exists(fullPath))
            {
                return BadRequest("该文件名字已经存在");
            }

            if (file.Length > 0)
            {
                using (var stream = new FileStream(fullPath, FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok(new { path = "/Images/" + imageUseType + "/" + fileName});
        }

        [HttpGet]
        public string Index()
        {
            return "Upload";
        }
    }
}
