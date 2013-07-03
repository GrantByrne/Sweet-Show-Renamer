using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SweetShowRenamer.Renamer
{
    /// <summary>
    /// Object used to manage the renaming of all the show in a directory
    /// </summary>
    public class Shows
    {

        public Shows()
        {
            showsList = new List<Show>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void LoadDirectory(string path)
        {
            string[] filepaths = Directory.GetFiles(path);

            foreach (var file in filepaths)
            {
                if (file.ToLower().EndsWith(".mp4") || file.ToLower().EndsWith(".avi") || file.ToLower().EndsWith(".mkv"))
                {
                    var tempShow = new Show();
                    tempShow.path = path;
                    tempShow.oldFilename = file.Substring(path.Length + 1, file.Length - path.Length - 1);

                    showsList.Add(tempShow);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadShowNames()
        {
            const string f = "shows.txt";
            List<string> shows = new List<string>();

            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    showNames.Add(line);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessShows()
        {
            foreach (var someShow in showsList)
            {
                // Find the name of the Show
                someShow.FindShowName(showNames);

                // Find the Season and the Episode Number
                someShow.findShowNum();

                // Put in the File Extension
                someShow.findFileExtension();

                // Process this information to create a new filename
                someShow.createNewFilename();

            }
        }

        /// <summary>
        /// List of the shows to be updated
        /// </summary>
        public List<Show> showsList;

        /// <summary>
        /// List of the Show Names used to identify the shows in the filename
        /// </summary>
        public List<String> showNames = new List<String>();
    }
}
