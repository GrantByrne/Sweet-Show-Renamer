using SweetShowRenamer.Components.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SweetShowRenamer.Components.Domain;
using System.IO;

namespace SweetShowRenamer.Components.Service
{
    public class SettingsService : ISettingsService
    {
        public Settings Get()
        {
            return GetDefaultSettings();
        }

        public Settings Update(Settings settings)
        {
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
