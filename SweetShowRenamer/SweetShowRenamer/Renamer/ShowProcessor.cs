using HtmlAgilityPack;
using SweetShowRenamer.Lib.Service;
using SweetShowRenamer.Lib.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SweetShowRenamer.Renamer
{
    public class ShowProcessor
    {
        private Dictionary<string, HtmlDocument> episodePages;

        /// <summary>
        ///     ctor
        /// </summary>
        public ShowProcessor()
        {
            ShowsList = new ObservableCollection<ShowData>();
            ShowNames = new ObservableCollection<string>();
            episodePages = new Dictionary<string, HtmlDocument>();
        }

        /// <summary>
        /// List of the Show Names used to identify the shows in the filename
        /// </summary>
        public ObservableCollection<String> ShowNames { get; set; }

        /// <summary>
        /// List of the shows to be updated
        /// </summary>
        public ObservableCollection<ShowData> ShowsList { get; set; }

        /// <summary>
        /// Loads up all of the show information in a single directory
        /// </summary>
        /// <param name="path">The directory the show information is being pulled from</param>
        public void LoadDirectory(string path)
        {
            this.ShowsList.Clear();

            string[] filepaths = Directory.GetFiles(path);

            foreach (var file in filepaths)
            {
                if (file.ToLower().EndsWith(".mp4") || file.ToLower().EndsWith(".avi") || file.ToLower().EndsWith(".mkv"))
                {
                    var tempShow = new ShowData();
                    tempShow.Path = path;
                    tempShow.OldFilename = file.Substring(path.Length + 1, file.Length - path.Length - 1);

                    ShowsList.Add(tempShow);
                }
            }
        }

        /// <summary>
        /// Takes all of the Show Name Stored in a File and Brings it
        /// into the object
        /// </summary>
        public void LoadShowNames()
        {
            const string f = "shows.txt";
            List<string> shows = new List<string>();

            try
            {
                using (StreamReader r = new StreamReader(f))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        ShowNames.Add(line);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Takes all the show information stored in the object and tries
        /// to generate the correct filename for each show
        /// </summary>
        public void ProcessShows()
        {
            foreach (var someShow in ShowsList)
            {
                // Find the name of the Show
                someShow.FindShowName(ShowNames);

                // Find the Season and the Episode Number
                someShow.findShowNum();

                // Put in the File Extension
                someShow.findFileExtension();

                // Find the episode names for the shows
                if (someShow.checkValidInformation())
                {
                    // Check if the page is already loaded in memory,
                    // if not, pull it from the internet
                    if (!episodePages.ContainsKey(someShow.ShowName))
                    {
                        // Download the page from the internet,
                        // so we can do the scraping
                        HtmlWeb webGet = new HtmlWeb();
                        webGet.UserAgent = "Mozilla/6.0 (Windows NT 6.2; WOW64; rv:16.0.1) Gecko/20121011 Firefox/16.0.1";
                        var doc = webGet.Load(someShow.generateURL());
                        episodePages.Add(someShow.ShowName, doc);
                    }
                    someShow.FindEpisodeName(episodePages[someShow.ShowName]);
                }

                // Process this information to create a new filename
                someShow.createNewFilename();

                someShow.Processed = true;

                if (!string.IsNullOrEmpty(someShow.NewFilename))
                {
                    someShow.Ready = true;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void WriteFilenames()
        {
            foreach (var show in ShowsList)
            {
                show.writeNewFilename();
            }
        }
    }
}