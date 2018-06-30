using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageGallery.Client.Services
{
    public interface IImageGalleryHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
