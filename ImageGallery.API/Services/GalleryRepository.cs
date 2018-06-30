using ImageGallery.API.Entities;
using ImageGallery.API.Entitiies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.API.Services
{
    public class GalleryRepository : IGalleryRepository, IDisposable
    {
        GalleryContext _ctx;

        public GalleryRepository(GalleryContext ctx)
        {
            _ctx = ctx;
        }
        public bool ImageExists(Guid id)
        {
            return _ctx.Images.Any(i => i.Id == id);
        }

        public async Task<Image> GetImageAsync(Guid id)
        {
            return await _ctx.Images.FirstOrDefaultAsync(i => i.Id == id);
        }

        public Image GetImage(Guid id)
        {
            return _ctx.Images.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Image> GetImages()
        {
            return _ctx.Images
                .OrderBy(i => i.Title).ToList();
        }

        public void AddImage(Image image)
        {
            _ctx.Images.Add(image);
        }

        public void UpdateImage(Image image)
        {
            // no code in this implementation
        }

        public void DeleteImage(Image image)
        {
            _ctx.Images.Remove(image);

            // Note: in a real-life scenario, the image itself should also 
            // be removed from disk.  We don't do this in this demo
            // scenario, as we refill the DB with image URIs (that require
            // the actual files as well) for demo purposes.
        }

        public bool Save()
        {
            return (_ctx.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_ctx != null)
                {
                    _ctx.Dispose();
                    _ctx = null;
                }
            }
        }
    }
}
