using ImageGallery.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageGallery.API.Services
{
    public interface IGalleryRepository
    {
        IEnumerable<Image> GetImages();
        Task<Image> GetImageAsync(Guid id);
        Image GetImage(Guid id);
        bool ImageExists(Guid id);
        void AddImage(Image image);
        void UpdateImage(Image image);
        void DeleteImage(Image image);
        bool Save();
    }
}