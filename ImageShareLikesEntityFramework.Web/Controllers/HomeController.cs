using ImageShareLikesEntityFramework.Data;
using ImageShareLikesEntityFramework.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageShareLikesEntityFramework.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _environment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _connectionString  = configuration.GetConnectionString("ConStr");
            _environment = environment;
        }

        public IActionResult Index()
        {
            var connectionString = _connectionString;
            var repo = new ImageRepository(connectionString);
            List<Image> images = repo.GetAll();
            HomeViewModel vm = new HomeViewModel
            {
                Images = images
            };
            return View(vm);
        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(Image image, IFormFile imageFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                imageFile.CopyTo(stream);
            }
            image.FileName = fileName;
            image.Date = DateTime.Now;
            var connectionString = _connectionString;
            var repo = new ImageRepository(connectionString);
            repo.Add(image);
            return Redirect("/");
        }
        public IActionResult ViewImage (int id)
        {
            var connectionString = _connectionString;
            var repo = new ImageRepository(connectionString);
            var Ids = HttpContext.Session.Get<List<int>>("Ids");
            bool liked = false;
            if (Ids != null)
            {
                liked = Ids.Contains(id);
            }
            ViewImagesViewModel vm = new ViewImagesViewModel
            {
                Image = repo.GetById(id),
                AlreadyLiked = liked
            };
            return View(vm);
        }
        [HttpPost]
        public void Update (int id)
        {
            var Ids = HttpContext.Session.Get<List<int>>("Ids") ?? new List<int>();
            Ids.Add(id);
            HttpContext.Session.Set("Ids", Ids);
            var connectionString = _connectionString;
            var repo = new ImageRepository(connectionString);
            repo.Update(id);
        }
        public IActionResult GetLikes (int id)
        {
            var connectionString = _connectionString;
            var repo = new ImageRepository(connectionString);
            int likes = repo.GetLikes(id);
            return Json (likes);
        }
    }
}


public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T Get<T>(this ISession session, string key)
    {
        string value = session.GetString(key);
        return value == null ? default(T) :
            JsonConvert.DeserializeObject<T>(value);
    }
}
