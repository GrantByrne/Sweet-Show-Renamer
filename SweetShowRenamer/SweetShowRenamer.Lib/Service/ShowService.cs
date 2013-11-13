using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShowRenamer.Lib.Service
{
    public class ShowService
    {
        public string GetUrl(string showName)
        {
            return "http://epguides.com/" + showName.Replace("The ", "").Replace(" ", "") + "/";
        }

        public string GetShowNumberString(int season, int episode)
        {
            return "s" + season.ToString("00") + "e" + episode.ToString("00");
        }
    }
}
