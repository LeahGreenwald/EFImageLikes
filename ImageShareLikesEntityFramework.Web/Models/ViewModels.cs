using ImageShareLikesEntityFramework.Data;
using System;
using System.Collections.Generic;

namespace ImageShareLikesEntityFramework.Web.Models
{
   public class HomeViewModel
    {
        public List<Image> Images { get; set; }
    }
    public class ViewImagesViewModel
    {
        public Image Image { get; set; }
        public bool AlreadyLiked { get; set; }
    }
}
