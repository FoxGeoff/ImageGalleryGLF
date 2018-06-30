using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Client.Models;
using ImageGallery.Client.Services;
using ImageGallery.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ImageGallery.Client.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IImageGalleryHttpClient _imageGalleryHttpClient;

        public GalleryController(IImageGalleryHttpClient imageGalleryHttpClient)
        {
            _imageGalleryHttpClient = imageGalleryHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            //call the API
            var httpClient = await _imageGalleryHttpClient.GetClient();
            var response = await httpClient.GetAsync("api/images").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var imagesAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var galleryIndexViewModel = new GalleryIndexViewModel(
                    JsonConvert.DeserializeObject<IList<Image>>(imagesAsString).ToList());

                return View(galleryIndexViewModel);
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }
    }
}