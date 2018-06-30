using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


//Online Ref: https://stackoverflow.com/questions/10343632/httpclient-getasync-never-returns-when-using-await-async#10351400

namespace ImageGallery.Client.Services
{
    public class ImageGalleryHttpClient : IImageGalleryHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _httpClient = new HttpClient();

        public ImageGalleryHttpClient(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpClient> GetClient()
        {
            //TODO: await _httpClient.GetAsync("http://localhost:49873/");
            _httpClient.BaseAddress = new Uri("http://localhost:49873/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;
        }
    }
}
