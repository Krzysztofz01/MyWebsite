using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Repositories;
using PersonalWebsiteWebApi.Settings;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PersonalWebsiteWebApi.Services
{
    public interface IGalleryIndexerService
    {
        Task IndexGalleryImages();
    }

    public class GalleryIndexerService : IGalleryIndexerService
    {
        private readonly string[] ImageExtensions = {".jpg", ".png", ".jpeg"};
        private readonly CdsSettings cdsSettings;
        private readonly IGalleryImageRepository galleryImageRepository;
        private readonly IHttpClientFactory httpClientFactory;

        public GalleryIndexerService(
            IOptions<CdsSettings> cdsSettings,
            IGalleryImageRepository galleryImageRepository,
            IHttpClientFactory httpClientFactory)
        {
            this.cdsSettings = cdsSettings.Value;
            this.galleryImageRepository = galleryImageRepository;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task IndexGalleryImages()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, cdsSettings.BaseUrlImages);
            var client = httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                var images = new List<GalleryImage>();
                var responseString = await response.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                
                htmlDocument.LoadHtml(responseString);
                foreach(var node in htmlDocument.DocumentNode.SelectNodes("//a[@href]"))
                {
                    string href = node.Attributes["href"].Value;

                    foreach(var ext in ImageExtensions)
                    {
                        if(href.EndsWith(ext) || href.EndsWith(ext.ToUpper()))
                        {
                            var galleryImage = PrepareImageObject(href);
                            if(galleryImage != null)
                            {
                                images.Add(galleryImage);
                            }
                        }
                    } 
                }

                await galleryImageRepository.PushImages(images);   
            }
        }

        private GalleryImage PrepareImageObject(string href)
        {
            //Image naming scheme: /images/category-name.ext
            try
            {
                string catAndName = href.Split("images")[1];
                string cat = catAndName.Split("-")[0].Split("/")[1];

                var galleryImage = new GalleryImage();
                galleryImage.Category = cat;
                galleryImage.ImageUrl = cdsSettings.BaseUrlImages + catAndName;
                return galleryImage;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
