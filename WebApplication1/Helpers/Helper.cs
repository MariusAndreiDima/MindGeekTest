using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            await DownloadRess(jsonContent);
        }




        public async static Task DownloadRess(List<MoviesDetails> movieDetailsList)
        {
            Directory.CreateDirectory($"wwwroot/CardImages/");
            Directory.CreateDirectory($"wwwroot/KeyArtImages/");
            Directory.CreateDirectory($"wwwroot/Videos/");
            Directory.CreateDirectory($"wwwroot/AlternativeVideos/");




            List<Task> tasks = new List<Task>();

            foreach (MoviesDetails md in movieDetailsList)
            {
                foreach (CardImages cd in md.CardImages)
                {
                    string fileName = $"{md.Id}{md.CardImages.IndexOf(cd).ToString()}.jpg";
                    CardImagesD cid = new CardImagesD(md.Headline, md.Id, cd.Url, cd.H, cd.W, "NotOk", $@"CardImages/{fileName}");
                    cardImagesDownloaded.Add(cid);

                    tasks.Add(Task.Run(async () =>
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                try
                                {
                                    byte[] file = await client.GetByteArrayAsync(new Uri($@"{cd.Url}"));
                                    if (file.Length == 0) return;
                                    using (FileStream SourceStream = File.Open($@"wwwroot/CardImages/{fileName}", FileMode.OpenOrCreate))
                                    {
                                        SourceStream.Seek(0, SeekOrigin.End);
                                        await SourceStream.WriteAsync(file, 0, file.Length);
                                    }
                                }
                                catch (WebException we)
                                {
                                    Debug.WriteLine($"{we.Message}");
                                }
                                catch (Exception e)
                                {

                                }
                            }
                        }));
                }
                foreach (KeyArtImages kai in md.KeyArtImages)
                {
                    string fileName = $"{md.Id}{md.KeyArtImages.IndexOf(kai).ToString()}.jpg";
                    KeyArtImagesD kaid = new KeyArtImagesD(md.Headline, md.Id, kai.Url, kai.H, kai.W, "NotOk", $@"KeyArtImages/{fileName}");
                    keyArtImagesDownloaded.Add(kaid);
                    tasks.Add(Task.Run(async () =>
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            try
                            {
                                byte[] file = await client.GetByteArrayAsync(new Uri($@"{kai.Url}"));
                                if (file.Length == 0) return;
                                using (FileStream SourceStream = File.Open($@"wwwroot/KeyArtImages/{fileName}", FileMode.OpenOrCreate))
                                {
                                    SourceStream.Seek(0, SeekOrigin.End);
                                    await SourceStream.WriteAsync(file, 0, file.Length);
                                }
                            }
                            catch (WebException we)
                            {
                                Debug.WriteLine($"{we.Message}");
                            }
                            catch (Exception e)
                            {

                            }
                        }

                    }));
                }


            }
            //await Task.WhenAll(tasks);
            //tasks.Clear();



            foreach (MoviesDetails md in movieDetailsList)
            {
                if (md.Videos != null)
                {
                    foreach (Videos v in md.Videos)
                        if (v.Alternatives != null)
                        {
                            foreach (Alternatives a in v.Alternatives)
                            {
                                string fileName = $"{md.Id}{md.Videos.IndexOf(v)}{v.Alternatives.IndexOf(a).ToString()}.mp4";
                                AlternativeVideosD avd = new AlternativeVideosD(md.Headline, md.Id, a.Quality, a.Url, "NotOk", $@"AlternativeVideos/{fileName}");
                                alternativeVideosDownloaded.Add(avd);

                                tasks.Add(Task.Run(async () =>
                                {
                                    using (HttpClient client = new HttpClient())
                                    {
                                        try
                                        {

                                            //client.DownloadFileCompleted += Client_DownloadFileCompleted;
                                            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.155 Safari/537.36");
                                            byte[] file = await client.GetByteArrayAsync(new Uri($@"{a.Url}"));
                                            if (file.Length == 0) return;
                                            using (FileStream SourceStream = File.Open($@"wwwroot/AlternativeVideos/{fileName}", FileMode.OpenOrCreate))
                                            {
                                                SourceStream.Seek(0, SeekOrigin.End);
                                                await SourceStream.WriteAsync(file, 0, file.Length);
                                            }


                                           
                                        }
                                        catch (WebException we)
                                        {
                                            //byte[] file = await client.GetByteArrayAsync(new Uri($@"{we.Response.ResponseUri}"));
                                            //if (file.Length == 0) return;
                                            //using (FileStream SourceStream = File.Open($@"wwwroot/AlternativeVideos/{fileName}", FileMode.OpenOrCreate))
                                            //{
                                            //    SourceStream.Seek(0, SeekOrigin.End);
                                            //    await SourceStream.WriteAsync(file, 0, file.Length);
                                            //}
                                        }
                                        catch (Exception e)
                                        {

                                        }
                                    }
                                }));
                            }
                        }
                }
            }
            

            foreach (MoviesDetails md in movieDetailsList)
            {
                if (md.Videos != null)
                {
                    foreach (Videos v in md.Videos)
                    {
                        List<AlternativeVideosD> avdList = new List<AlternativeVideosD>();
                        avdList.AddRange(alternativeVideosDownloaded.FindAll(a => a.Id == md.Id));
                        string fileName = $"{md.Id}{md.Videos.IndexOf(v)}.mp4";
                        VideosD vd = new VideosD(md.Headline, md.Id, v.Title, avdList, v.Type, v.Url, "NotOk", $@"Videos/{fileName}");
                        videosDownloaded.Add(vd);

                        tasks.Add(Task.Run(async () =>
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                try
                                {

                                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.155 Safari/537.36");
                                    byte[] file = await client.GetByteArrayAsync(new Uri($@"{v.Url}"));
                                    if (file.Length == 0) return;
                                    using (FileStream SourceStream = File.Open($@"wwwroot/Videos/{fileName}", FileMode.OpenOrCreate))
                                    {
                                        SourceStream.Seek(0, SeekOrigin.End);
                                        await SourceStream.WriteAsync(file, 0, file.Length);
                                    }
                                   
                                }
                                catch (WebException we)
                                {

                                    //byte[] file = await client.GetByteArrayAsync(new Uri($@"{we.Response.ResponseUri}"));
                                    //if (file.Length == 0) return;
                                    //using (FileStream SourceStream = File.Open($@"wwwroot/Videos/{fileName}", FileMode.OpenOrCreate))
                                    //{
                                    //    SourceStream.Seek(0, SeekOrigin.End);
                                    //    await SourceStream.WriteAsync(file, 0, file.Length);
                                    //}
                                    
                                }
                                catch(Exception e)
                                {

                                }

                            }

                        }));
                    }

                }
            }
            await Task.WhenAll(tasks);
            tasks.Clear();

            Thread.Sleep(1000);
            Helper.StatusKeyArtImages();
            Helper.StatusCardImages();
            Helper.StatusVideos();
            Helper.StatusAlternativeVideos();  
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

                foreach (CardImagesD cid in cardImagesDownloaded)
                {
                    if (fi.Length == 0)
                    {
                        if (cid != null)
                        {
                            if (fi.FullName.Split("\\")[fi.FullName.Split("\\").Length - 1] == cid.Source.Split('/')[cid.Source.Split('/').Length - 1])
                                cid.Status = "NotOk";
                        }
                    }
                    else
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
