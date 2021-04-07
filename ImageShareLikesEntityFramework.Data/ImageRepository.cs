using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageShareLikesEntityFramework.Data
{
    public class ImageRepository
    {
        private readonly string _connectionString;
        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Image> GetAll()
        {
            using var context = new ImageDbContext(_connectionString);
            return context.Images.OrderByDescending(i => i.Date).ToList();
        }
        public void Add(Image image)
        {
            using var context = new ImageDbContext(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();
        }
        public Image GetById(int id)
        {
            using var context = new ImageDbContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }
        public void Update (int id)
        {
            using var context = new ImageDbContext(_connectionString);
            var image = GetById(id);
            image.Likes += 1;
            context.Images.Attach(image);
            context.Entry(image).State = EntityState.Modified;
            context.SaveChanges();
        }
        public int GetLikes (int id)
        {
            using var context = new ImageDbContext(_connectionString);
            var image = GetById(id);
            return image.Likes;
        }
    }
}
