using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class Helper
    {
        public static List<MoviesDetails> jsonContent;
        public static List<CardImagesD> cardImagesDownloaded = new List<CardImagesD>();
        public static List<KeyArtImagesD> keyArtImagesDownloaded = new List<KeyArtImagesD>();
        public static List<AlternativeVideosD> alternativeVideosDownloaded = new List<AlternativeVideosD>();
        public static List<VideosD> videosDownloaded = new List<VideosD>();
        
        public static async Task DeserializeJSON(string url)
        {
            System.Net.HttpWebRequest request = null;
            System.Net.HttpWebResponse response = null;
            request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            response = (System.Net.HttpWebResponse)request.GetResponse();
            string contentJson = "";
            if (request.HaveResponse)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    using (StreamReader br = new StreamReader(receiveStream))
                    {
                        contentJson = br.ReadToEnd();
                        br.Close();
                    }
                }
            }

            jsonContent = JsonConvert.DeserializeObject<List<MoviesDetails>>(contentJson);
            DownloadRess(jsonContent);
        }




        public async static void DownloadRess(List<MoviesDetails> movieDetailsList)
        {
            Directory.CreateDirectory($"wwwroot/CardImages/");
            Directory.CreateDirectory($"wwwroot/KeyArtImages/");
            Directory.CreateDirectory($"wwwroot/Videos/");
            Directory.CreateDirectory($"wwwroot/AlternativeVideos/");




            List<Task> tasks = new List<Task>();

            foreach (MoviesDetails md in movieDetailsList)
            {
                foreach (CardImages cd in md.cardImages)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                        {
                            using (WebClient client = new WebClient())
                            {
                                try
                                {

                                    client.DownloadFileCompleted += Client_DownloadFileCompletedKA;

                                    string fileName = $"{md.id}{md.cardImages.IndexOf(cd).ToString()}.jpg";
                                    CardImagesD cid = new CardImagesD(md.headline, md.id, cd.url, cd.h, cd.w, "Ok", $@"CardImages/{fileName}");
                                    cardImagesDownloaded.Add(cid);
                                    client.DownloadFileTaskAsync(new Uri($@"{cd.url}"), $@"wwwroot/CardImages/{fileName}");
                                }
                                catch (WebException we)
                                {
                                    Debug.WriteLine($"{we.Message}");
                                }

                            }

                        }));
                }
                foreach (KeyArtImages kai in md.keyArtImages)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        using (WebClient client = new WebClient())
                        {
                            try
                            {

                                client.DownloadFileCompleted += Client_DownloadFileCompleted;

                                string fileName = $"{md.id}{md.keyArtImages.IndexOf(kai).ToString()}.jpg";
                                KeyArtImagesD kaid = new KeyArtImagesD(md.headline, md.id, kai.url, kai.h, kai.w, "Ok", $@"KeyArtImages/{fileName}");
                                keyArtImagesDownloaded.Add(kaid);
                                client.DownloadFileTaskAsync(new Uri($@"{kai.url}"), $@"wwwroot/KeyArtImages/{fileName}");
                            }
                            catch (WebException we)
                            {
                                Debug.WriteLine($"{we.Message}");
                            }

                        }

                    }));
                }


            }
            await Task.WhenAll(tasks);



            foreach (MoviesDetails md in movieDetailsList)
            {
                if (md.videos != null)
                {
                    foreach (Videos v in md.videos)
                        if (v.alternatives != null)
                        {
                            foreach (Alternatives a in v.alternatives)
                            {
                                tasks.Add(Task.Factory.StartNew(() =>
                                {
                                    using (WebClient client = new WebClient())
                                    {
                                        try
                                        {

                                            //client.DownloadFileCompleted += Client_DownloadFileCompleted;

                                            string fileName = $"{md.id}{md.videos.IndexOf(v)}{v.alternatives.IndexOf(a).ToString()}.mp4";
                                            AlternativeVideosD avd = new AlternativeVideosD(md.headline, md.id, a.quality, a.url, "Ok", $@"AlternativeVideos/{fileName}");
                                            alternativeVideosDownloaded.Add(avd);
                                            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(a.url);
                                            req.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.155 Safari/537.36";
                                            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                                            client.DownloadFileTaskAsync(new Uri($@"{a.url}"), $@"wwwroot/AlternativeVideos/{fileName}");
                                        }
                                        catch (WebException we)
                                        {
                                            using (WebResponse response = we.Response)
                                            {
                                                string fileName = $"{md.id}{md.videos.IndexOf(v)}{v.alternatives.IndexOf(a).ToString()}.mp4";
                                                HttpWebResponse httpResponse = (HttpWebResponse)response;
                                                client.DownloadFileTaskAsync(new Uri($@"{httpResponse.ResponseUri}"), $@"wwwroot/AlternativeVideos/{fileName}");
                                            }
                                        }

                                    }

                                }));
                            }
                        }
                }
            }
            await Task.WhenAll(tasks);

            foreach (MoviesDetails md in movieDetailsList)
            {
                if (md.videos != null)
                {
                    foreach (Videos v in md.videos)

                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            using (WebClient client = new WebClient())
                            {
                                try
                                {
                                    List<AlternativeVideosD> avdList = new List<AlternativeVideosD>();
                                    avdList.AddRange(alternativeVideosDownloaded.FindAll(a => a.Id == md.id));
                                    string fileName = $"{md.id}{md.videos.IndexOf(v)}.mp4";
                                    VideosD vd = new VideosD(md.headline, md.id, v.title, avdList, v.type, v.url, "Ok", $@"Videos/{fileName}");
                                    videosDownloaded.Add(vd);
                                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(v.url);
                                    req.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.155 Safari/537.36";
                                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                                   client.DownloadFileTaskAsync(new Uri($@"{v.url}"), $@"wwwroot/Videos/{fileName}");
                                }
                                catch (WebException we)
                                {
                                    using (WebResponse response = we.Response)
                                    {
                                        string fileName = $"{md.id}{md.videos.IndexOf(v)}.mp4";
                                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                                        client.DownloadFileTaskAsync(new Uri($@"{httpResponse.ResponseUri}"), $@"wwwroot/Videos/{fileName}");    
                                    }
                                }

                            }

                        }));


                }
            }
            await Task.WhenAll(tasks);

        }
        private static void Client_DownloadFileCompletedKA(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //Thread.Sleep(15);
                ////StatusCardImages();
                //int indexKA = keyArtImagesDownloaded.IndexOf(keyArtImagesDownloaded.Find(s => s.Url == ((System.Net.WebException)e.Error).Response.ResponseUri.AbsoluteUri));
                //if (indexKA > 0)
                //{
                //    keyArtImagesDownloaded[indexKA].Status = "NotOk";
                //}
                //else
                //    Debug.WriteLine($"{((System.Net.WebException)e.Error).Response.ResponseUri.AbsoluteUri}");


            }
        }
        private static void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
               // StatusKeyArtImages();
                //int index = cardImagesDownloaded.IndexOf(cardImagesDownloaded.Find(s => s.Url == ((System.Net.WebException)e.Error).Response.ResponseUri.AbsoluteUri));
                //if (index > 0)
                //    cardImagesDownloaded[index].Status = "Error";
                //else
                //    Debug.WriteLine($"{((System.Net.WebException)e.Error).Response.ResponseUri.AbsoluteUri}");


            }
        }
        public static void StatusCardImages()
        {
            DirectoryInfo d = new DirectoryInfo("wwwroot/CardImages/");
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis) 
            {      
               if(fi.Length == 0)
               {
                    foreach(CardImagesD cid in cardImagesDownloaded)
                    {
                        if (cid != null)
                        {
                            if (fi.FullName.Split("\\")[fi.FullName.Split("\\").Length - 1] == cid.Source.Split('/')[cid.Source.Split('/').Length - 1])
                                cid.Status = "NotOk";
                        }
                    }
               }
               else
               {
                    foreach (CardImagesD cid in cardImagesDownloaded)
                    {
                        if (cid != null)
                        {
                            if (fi.FullName.Split("\\")[fi.FullName.Split("\\").Length - 1] == cid.Source.Split('/')[cid.Source.Split('/').Length - 1])
                                cid.Status = "Ok";
                        }
                    }
                }
            }
        }

        public static void StatusKeyArtImages()
        {
            DirectoryInfo d = new DirectoryInfo("wwwroot/KeyArtImages/");
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                if (fi.Length == 0)
                {
                    foreach (KeyArtImagesD kai in keyArtImagesDownloaded)
                    {
                        if (fi.FullName.Split("\\")[fi.FullName.Split("\\").Length - 1] == kai.Source.Split('/')[kai.Source.Split('/').Length - 1])
                            kai.Status = "NotOk";
                    }
                }
                else
                {
                    foreach (KeyArtImagesD kai in keyArtImagesDownloaded)
                    {
                        if (fi.FullName.Split("\\")[fi.FullName.Split("\\").Length - 1] == kai.Source.Split('/')[kai.Source.Split('/').Length - 1])
                            kai.Status = "Ok";
                    }
                }

            }
        }

        public static void StatusVideos()
        {
            DirectoryInfo d = new DirectoryInfo("wwwroot/Videos/");
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                if (fi.Length == 0)
                {
                    foreach (VideosD vd in videosDownloaded)
                    {
                        if (fi.FullName.Split("\\")[fi.FullName.Split("\\").Length - 1] == vd.Source.Split('/')[vd.Source.Split('/').Length - 1])
                            vd.Status = "NotOk";
                    }
                }
                else
                {
                    foreach (VideosD vd in videosDownloaded)
                    {
                        if (fi.FullName.Split("\\")[fi.FullName.Split("\\").Length - 1] == vd.Source.Split('/')[vd.Source.Split('/').Length - 1])
                            vd.Status = "Ok";
                    }
                }

            }
        }

        public static void StatusAlternativeVideos()
        {
            DirectoryInfo d = new DirectoryInfo("wwwroot/AlternativeVideos/");
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                if (fi.Length == 0)
                {
                    foreach (AlternativeVideosD avd in alternativeVideosDownloaded)
                    {
                        if (fi.FullName.Split("\\")[fi.FullName.Split("\\").Length - 1] == avd.Source.Split('/')[avd.Source.Split('/').Length - 1])
                            avd.Status = "NotOk";
                    }
                }
                else
                {
                    foreach (AlternativeVideosD avd in alternativeVideosDownloaded)
                    {
                        if (fi.FullName.Split("\\")[fi.FullName.Split("\\").Length - 1] == avd.Source.Split('/')[avd.Source.Split('/').Length - 1])
                            avd.Status = "Ok";
                    }
                }

            }
        }
    }
}
