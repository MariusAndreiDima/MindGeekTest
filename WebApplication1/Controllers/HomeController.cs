using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MindGeekTest.Models;

namespace MindGeekTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Helper.StatusKeyArtImages();
            return View();
        }
       
        public IActionResult Movie(string id)
        {
            Helper.StatusCardImages();
            Helper.StatusVideos();
            Helper.StatusAlternativeVideos();  
            IEnumerable<MoviesDetails> result = Helper.jsonContent.Where(s => s.id == id);
            if (result.Count() > 0)
            {
                ViewBag.Movies = result;
                ViewData["Headline"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).headline;
                ViewData["Body"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).body;
                ViewBag.Cast = Helper.jsonContent.FirstOrDefault(s => s.id == id).cast;
                ViewData["Cert"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).cert;
                ViewData["Class"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).cclass;
                ViewBag.Directors = Helper.jsonContent.FirstOrDefault(s => s.id == id).directors;
                var durationS = Convert.ToInt32(Helper.jsonContent.FirstOrDefault(s => s.id == id).duration);
                ViewData["Duration"] = $"{durationS / 3600}H {durationS % 3600 / 60}min {durationS % 3600 % 60}sec";
                ViewBag.Genres = Helper.jsonContent.FirstOrDefault(s => s.id == id).genres;

                ViewData["LastUpdated"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).lastUpdated;
                ViewData["Quote"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).quote;
                ViewData["Rating"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).rating;
                ViewData["ReviewAuthor"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).reviewAuthor;
                ViewData["SkyGoId"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).skyGoId;
                ViewData["SkyGoUrl"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).skyGoUrl;
                ViewData["Sum"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).sum;
                ViewData["Url"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).url;

                ViewingWindow viewingResult = Helper.jsonContent.FirstOrDefault(s => s.id == id).viewingWindow;
                if (viewingResult != null)
                {
                    ViewData["Title"] = viewingResult.title;
                    ViewData["StartDate"] = viewingResult.startDate;
                    ViewData["WayToWatch"] = viewingResult.wayToWatch;
                    ViewData["EndDate"] = viewingResult.endDate;
                }
                ViewData["Year"] = Helper.jsonContent.FirstOrDefault(s => s.id == id).year;

                IEnumerable<VideosD> vdResult = Helper.videosDownloaded.Where(s => s != null && s.Id == id);
                if (vdResult != null)
                {
                    ViewBag.Videos = vdResult.Where(a => a.Status == "Ok");
                }

                IEnumerable<AlternativeVideosD> avdResult = Helper.alternativeVideosDownloaded.Where(s => s != null && s.Id == id);
                if (avdResult != null)
                {
                    ViewBag.AlternativeVideos = avdResult.Where(a => a.Status == "Ok");
                }

                IEnumerable<CardImagesD> cdResult = MindGeekTest.Models.Helper.cardImagesDownloaded.Where(s => s !=null && s.Id == id);
                if (cdResult != null)
                {
                    ViewBag.CardImages = cdResult.Where(a => a.Status == "Ok");
                }
            }
            
            if (result.Count() > 0)
            {
                return View();
            }
            else
            {
               Response.StatusCode = 404;
               return View("Error404");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
