using AutoMapper;
using ImageGallery.API.Entities;
using ImageGallery.API.Helpers;
using ImageGallery.API.Services;
using ImageGallery.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

//TODO: update using async

namespace ImageGallery.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Images")]
    public class ImagesController : Controller
    {
        private readonly IGalleryRepository _repo;
        private readonly IHostingEnvironment _env;

        public ImagesController(IGalleryRepository repo,
            IHostingEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        [HttpGet]
        public IEnumerable<Model.Image> GetImages()
        {
            // get from repo
            var imagesFromRepo = _repo.GetImages();

            // map to model
            var imagesToReturn = Mapper.Map<IEnumerable<Model.Image>>(imagesFromRepo);

            // return
            return imagesToReturn;
        }

        
        // GET: api/Images/5
        [HttpGet("{id}", Name = "GetImage")]
        public async Task<IActionResult> GetImage([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageFromRepo = await _repo.GetImageAsync(id);

            if (imageFromRepo == null)
            {
                return NotFound();
            }

            var imageToReturn = Mapper.Map<Model.Image>(imageFromRepo);

            return Ok(imageToReturn);
        }
        
        // PUT: api/Images/5 (UpdateImage)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage([FromRoute] Guid id, [FromBody] ImageForUpdate imageForUpdate)
        {
            if (imageForUpdate == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // return 422 - Unprocessable Entity when validation fails
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var imageFromRepo = _repo.GetImageAsync(id);
            if (imageFromRepo == null)
            {
                return NotFound();
            }

            await Mapper.Map(imageForUpdate, imageFromRepo);

            _repo.UpdateImage(imageFromRepo.Result);

            if (!_repo.Save())
            {
                throw new Exception($"Updating image with {id} failed on save.");
            }

            return NoContent();
        }
        

        // POST: api/Images (CreateImage)
        [HttpPost]
        public IActionResult PostImage([FromBody] ImageForCreation imageForCreation)
        {
            if (imageForCreation == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // return 422 - Unprocessable Entity when validation fails
                return new UnprocessableEntityObjectResult(ModelState);
            }

            // Automapper maps only the Title in our configuration
            var imageEntity = Mapper.Map<Entities.Image>(imageForCreation);

            // Create an image from the passed-in bytes (Base64), and 
            // set the filename on the image

            // get this environment's web root path (the path
            // from which static content, like an image, is served)
            var webRootPath = _env.WebRootPath;

            // create the filename
            string fileName = Guid.NewGuid().ToString() + ".jpg";

            // the full file path
            var filePath = Path.Combine($"{webRootPath}/images/{fileName}");

            // write bytes and auto-close stream
            System.IO.File.WriteAllBytes(filePath, imageForCreation.Bytes);

            // fill out the filename
            imageEntity.FileName = fileName;

            _repo.AddImage(imageEntity);

            if (!_repo.Save())
            {
                throw new Exception($"Adding an image failed on save.");
            }

            var imageToReturn = Mapper.Map<Model.Image>(imageEntity);

            return CreatedAtRoute("GetImage", new { id = imageToReturn.Id }, imageToReturn);
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public IActionResult DeleteImage([FromRoute] Guid id)
        {
            var imageFromRepo = _repo.GetImage(id);

            if (imageFromRepo == null)
            {
                return NotFound();
            }

            _repo.DeleteImage(imageFromRepo);

            if (!_repo.Save())
            {
                throw new Exception($"Deleting image with {id} failed on save.");
            }

            return NoContent();
        }

        private bool ImageExists(Guid id)
        {
            return _repo.ImageExists(id);
        }
        
    }
}