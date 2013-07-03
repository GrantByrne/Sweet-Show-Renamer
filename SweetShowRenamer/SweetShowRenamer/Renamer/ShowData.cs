using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using HtmlAgilityPack;

namespace SweetShowRenamer.Renamer
{
    public class ShowData
    {

        /// <summary>
        /// Based off the content of the old filename and the list of show names,
        /// it tries to find the name of the show
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public void FindShowName(ObservableCollection<string> showNames)
        {
            var filename = OldFilename.ToLower().Replace("_", " ").Replace(".", " ").Replace("'", "");

            foreach (var showName in showNames)
            {
                if (filename.Contains(showName.ToLower()))
                {
                    this.ShowName = showName;
                    break;
                }
            }
        }

        /// <summary>
        /// Finds the Show Number from the filename
        /// </summary>
        /// <param name="show"></param>
        public void findShowNum()
        {

            /////
            // Clean up the filename to reduce false positives
            /////
            var lowerFilename = OldFilename.ToLower();
            string[] scrublist = { "1080p", "720p", "300mb", "x264" };

            foreach (var scrub in scrublist)
            {
                lowerFilename = lowerFilename.Replace(scrub, " ");
            }

            // Checks the episode in the ##x## format
            string pattern = @"s\d{2}e\d{2}";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(lowerFilename);
            if (matches.Count >= 1)
            {
                string tempSeason = matches[0].Value.Substring(1, 2);
                int.TryParse(tempSeason, out Season);

                string tempEpisode = matches[0].Value.Substring(4, 2);
                int.TryParse(tempEpisode, out Episode);
            }

            // TODO Add support for the ### and #### format
        }

        /// <summary>
        /// Creates the URL to find the show information
        /// </summary>
        /// <param name="showName">takes in a string with the show name</param>
        /// <returns>string of the url for epguides.com</returns>
        public string generateURL()
        {
            string url = "http://epguides.com/" + ShowName.Replace("The ", "").Replace(" ", "") + "/";

            return url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public void FindEpisodeName(HtmlDocument doc)
        {
            var browser = doc.DocumentNode.SelectNodes(@"//body//div[@id='eplist']//pre//a");

            foreach (var node in browser)
            {
                if (node.OuterHtml.Contains("season " + this.Season) &&
                    node.OuterHtml.Contains("episode " + this.Episode))
                {
                    EpisodeName = node.InnerHtml;
                    break;
                }
            }

        }

        /// <summary>
        /// Takes the season number and the episode number and format the show 
        /// number to s##e## format
        /// </summary>
        /// <returns>Season number in the s##e## format</returns>
        string formatShowNumber()
        {
            string result = "";

            result = "s" + Season.ToString("D2") + "e" + Episode.ToString("D2");

            return result;
        }

        /// <summary>
        /// Checks if the class has all the data to create a filename
        /// </summary>
        public bool checkValidInformation()
        {
            bool result = true;

            result &= !string.IsNullOrEmpty(ShowName);
            result &= (Season >= 1);
            result &= (Episode >= 1);
            result &= !string.IsNullOrEmpty(FileExtension);

            return result;
        }


        /// <summary>
        /// Based on the attributes, assembles tho new filename
        /// </summary>
        public void createNewFilename()
        {
            if (checkValidInformation())
            {
                NewFilename = ShowName + " " + formatShowNumber();

                if (!string.IsNullOrEmpty(EpisodeName))
                {
                    NewFilename = NewFilename + " - " + EpisodeName;
                }

                NewFilename = NewFilename + FileExtension;

                // Clean up the character information so the filename is valid
                foreach (var character in System.IO.Path.GetInvalidFileNameChars())
                {
                    NewFilename = NewFilename.Replace(character.ToString(), "");
                }
            }
        }

        /// <summary>
        /// Computes the file extension based on the old filename
        /// </summary>
        public void findFileExtension()
        {
            List<string> filetypes = new List<string> { ".mp4", ".avi", ".mkv" };

            foreach (var filetype in filetypes)
            {
                if (OldFilename.ToLower().EndsWith(filetype))
                {
                    this.FileExtension = filetype;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void writeNewFilename()
        {

            string newFilenamePath = this.Path + "\\" + this.NewFilename;
            string oldFilenamePath = this.Path + "\\" + this.OldFilename;
            
            if (this.checkValidInformation() && 
                this.Ready && this.Processed &&
                !System.IO.File.Exists(newFilenamePath))
            {
                System.IO.File.Move(oldFilenamePath, newFilenamePath);
            }

        }

        /// <summary>
        /// Location of the folder for the show file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Original Show Filename
        /// </summary>
        public string OldFilename { get; set; }

        /// <summary>
        /// Name of the Show
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// The season number for the show
        /// </summary>
        public int Season;

        /// <summary>
        /// The episode number for the show
        /// </summary>
        public int Episode;

        /// <summary>
        /// The episode name for the show
        /// </summary>
        public string EpisodeName { get; set; }

        /// <summary>
        /// The file extension for the file
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// The computed new filename for the show
        /// </summary>
        public string NewFilename { get; set; }

        /// <summary>
        /// Flag to show whether the show has been processed or not
        /// </summary>
        public bool Processed { get; set; }
        
        /// <summary>
        /// Flag to determine the show is ready to be written
        /// </summary>
        public bool Ready { get; set; }
    }
}
