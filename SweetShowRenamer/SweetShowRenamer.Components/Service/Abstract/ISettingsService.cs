using SweetShowRenamer.Components.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SweetShowRenamer.Components.Service.Abstract
{
    public interface ISettingsService
    {
        Settings Get();
        Settings Update(Settings settings);
    }
}
