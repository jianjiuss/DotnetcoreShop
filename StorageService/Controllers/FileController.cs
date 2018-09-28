using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        private string token = "ea79d8a0-c031-40a3-b439-eed6518520d9";

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm]IFormFile file, string imageUseType)
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Headers["token"]))
            {
                return Unauthorized();
            }

            if(!HttpContext.Request.Headers["token"].Equals(token))
            {
                return Unauthorized();
            }

            if (!file.ContentType.ToLower().Contains("image"))
            {
                return BadRequest("不支持的类型");
            }

            if (!imageUseTypes.Contains(imageUseType))
            {
                return BadRequest("请指定好该图片用途");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", imageUseType);
            string fileName = Guid.NewGuid().ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));

            string fullPath = Path.Combine(filePath, fileName);

            if (file.Length > 0)
            {
                using (var stream = new FileStream(fullPath, FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return Ok(new { path = "/Images/" + imageUseType + "/" + fileName });
        }

        [HttpGet]
        public string Index()
        {
            return "Upload";
        }

        [HttpPost]
        public IActionResult DeleteFile(string filenames)
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Headers["token"]))
            {
                return Unauthorized();
            }

            if (!HttpContext.Request.Headers["token"].Equals(token))
            {
                return Unauthorized();
            }

            List<string> usefulFilename = filenames.Split(';').ToList();

            var IconImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ProductIcon");
            var InfoImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ProductInfo");
            var TitleImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ProductTitle");
            DeleteFileInPath(usefulFilename, IconImagePath);
            DeleteFileInPath(usefulFilename, InfoImagePath);
            DeleteFileInPath(usefulFilename, TitleImagePath);

            return Ok();
        }

        private static void DeleteFileInPath(List<string> usefulFilename, string imagePath)
        {
            var localFilenamePath = Directory.GetFiles(imagePath);
            var localFilename = new List<string>();
            foreach(var item in localFilenamePath)
            {
                localFilename.Add(Path.GetFileName(item));
            }
            var removeFilename = localFilename.Except(usefulFilename).ToList();
            
            foreach(string name in removeFilename)
            {
                var filePath = Path.Combine(imagePath, name);
                System.IO.File.Delete(filePath);
            }
        }

        [HttpPost]
        public string Test1(string name)
        {
            return "Test1";
        }

        [HttpPost]
        public IActionResult Test2(string name)
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Headers["token"]))
            {
                return Unauthorized();
            }

            if (!HttpContext.Request.Headers["token"].Equals(token))
            {
                return Unauthorized();
            }

            return Ok("Test2");
        }
    }
}
