using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SweetShowRenamer.Renamer
{
    public class Show
    {

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessShows()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public void FindShowName(List<string> showNames)
        {
            var filename = oldFilename.ToLower().Replace("_", " ").Replace(".", " ").Replace("'", "");

            foreach (var showName in showNames)
            {
                if (filename.Contains(showName.ToLower()))
                {
                    this.showName = showName;
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
            var lowerFilename = oldFilename.ToLower();
            string[] scrublist = {"1080p", "720p", "300mb", "x264"};

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
                int.TryParse(tempSeason, out season);

                string tempEpisode = matches[0].Value.Substring(4, 2);
                int.TryParse(tempEpisode, out episode);
            }

            // TODO Add support for the ### and #### format
        }

        /// <summary>
        /// Creates the URL to find the show information
        /// </summary>
        /// <param name="showName">takes in a string with the show name</param>
        /// <returns>string of the url for epguides.com</returns>
        static string generateURL(string showName)
        {
            string url = "http://epguides.com/" + showName.Replace("The ", "").Replace(" ", "") + "/";

            return url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string formatShowNumber()
        {
            string result = "";

            result = "s" + season.ToString("D2") + "e" + episode.ToString("D2");

            return result;
        }

        /// <summary>
        /// Check if the class has all the data to create a filename
        /// </summary>
        bool checkValidInformation()
        {
            bool result = true;

            result &= !string.IsNullOrEmpty(showName);
            result &= (season >= 1);
            result &= (episode >= 1);
            result &= !string.IsNullOrEmpty(fileExtension);

            return result;
        }

        public void createNewFilename()
        {
            if (checkValidInformation() )
            {
                newFilename = showName + " " + formatShowNumber() + fileExtension;
            }
        }

        public void findFileExtension()
        {
             if (oldFilename.ToLower().EndsWith(".mp4"))
             {
                 fileExtension = ".mp4";
             }            
             if(oldFilename.ToLower().EndsWith(".avi"))
             {
                 fileExtension = ".avi";
             }
        }
        
        public string path;            // Location of the folder for the show file

        public string oldFilename;     // Original Show Filename
        
        public string showName;        // Name of the Show

        public int season;             // The season number for the show
        
        public int episode;            // The episode number for the show

        public string episodeName;     // The episode name for the show

        public string fileExtension;   // The file extension for the file

        public string newFilename;     // The computed new filename for the show

        public bool ready;             // Flag to determine the show is ready to be written

    }
}
