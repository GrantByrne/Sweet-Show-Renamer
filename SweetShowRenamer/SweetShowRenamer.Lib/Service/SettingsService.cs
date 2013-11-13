using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SweetShowRenamer.Lib.Service.Abstract;
using SweetShowRenamer.Lib.Domain;

namespace SweetShowRenamer.Lib.Service
{
    public class SettingsService : ISettingsService
    {

        private readonly IFileStorageService<Settings> _fileStorageService;

        /// <summary>
        ///     ctor
        /// </summary>
        public SettingsService()
        {
            _fileStorageService = new FileStorageService<Settings>();
        }

        public Settings Get()
        {
            var settings = _fileStorageService.Get();

            if(settings == null || string.IsNullOrEmpty(settings.LastUsedDirectory))
            {
                settings = GetDefaultSettings();
                Update(settings);
            }

            return settings;
        }

        public Settings Update(Settings settings)
        {
            _fileStorageService.Set(settings);
            return settings;
        }

        private Settings GetDefaultSettings()
        {
            var settings = new Settings();

            settings.LastUsedDirectory = System.IO.Directory.GetCurrentDirectory();

            return settings;
        }
    }
}
