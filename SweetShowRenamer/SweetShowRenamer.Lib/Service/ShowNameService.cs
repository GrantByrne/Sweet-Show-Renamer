using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShowRenamer.Lib.Service
{
    public class ShowNameService
    {
        /// <summary>
        ///     Persists the data to a text file on the disk
        /// </summary>
        private readonly IFileStorageService<List<string>> _fileStorageService;
        
        /// <summary>
        ///     ctor
        /// </summary>
        public ShowNameService()
        {
            _fileStorageService = new FileStorageService<List<string>>();
        }

        /// <summary>
        ///     Gets the list of user defined shows
        /// </summary>
        /// <returns></returns>
        public List<string> Get()
        {
            return _fileStorageService.Get();
        }

        /// <summary>
        ///     Sets the list of user defined shows
        /// </summary>
        /// <param name="showsList">
        ///     The list of shows to be saved to the hard drive 
        /// </param>
        /// <returns>
        ///     The same list of shows that were saved to the hard drive
        /// </returns>
        public List<string> Set(List<string> showsList)
        {
            _fileStorageService.Set(showsList);
            return showsList;
        }
    }
}
