using ImageGallery.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageGallery.API.Entitiies
{
    public class GalleryContext : DbContext
    {
        public GalleryContext(DbContextOptions<GalleryContext> options)
            :base(options)
        { }

        public DbSet<Image> Images { get; set; }
    }
}
