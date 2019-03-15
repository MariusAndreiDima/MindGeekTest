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

            return View();
        }
       
        public IActionResult Movie(string id)
        {
            IEnumerable<MoviesDetails> result = Helper.jsonContent.Where(s => s.Id == id);
            if (result.Count() > 0)
            {
                ViewBag.Movies = result;
                var movieDetails = Helper.jsonContent.FirstOrDefault(s => s.Id == id); 
                ViewData["Headline"] = movieDetails.Headline;
                ViewData["Body"] = movieDetails.Body;
                ViewBag.Cast = movieDetails.Cast;
                ViewData["Cert"] = movieDetails.Cert;
                ViewData["Class"] = movieDetails.Class;
                ViewBag.Directors = movieDetails.Directors;
                var durationS = Convert.ToInt32(movieDetails.Duration);
                ViewData["Duration"] = $"{durationS / 3600}H {durationS % 3600 / 60}min {durationS % 3600 % 60}sec";
                ViewBag.Genres = movieDetails.Genres;

                ViewData["LastUpdated"] = movieDetails.LastUpdated;
                ViewData["Quote"] = movieDetails.Quote;
                ViewData["Rating"] = movieDetails.Rating;
                ViewData["ReviewAuthor"] = movieDetails.ReviewAuthor;
                ViewData["SkyGoId"] = movieDetails.SkyGoId;
                ViewData["SkyGoUrl"] = movieDetails.SkyGoUrl;
                ViewData["Sum"] = movieDetails.Sum;
                ViewData["Url"] = movieDetails.Url;

                ViewingWindow viewingResult = movieDetails.ViewingWindow;
                if (viewingResult != null)
                {
                    ViewData["Title"] = viewingResult.Title;
                    ViewData["StartDate"] = viewingResult.StartDate;
                    ViewData["WayToWatch"] = viewingResult.WayToWatch;
                    ViewData["EndDate"] = viewingResult.EndDate;
                }
                ViewData["Year"] = movieDetails.Year;

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
