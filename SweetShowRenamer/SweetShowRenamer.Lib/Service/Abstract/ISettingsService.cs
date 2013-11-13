using SweetShowRenamer.Lib.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SweetShowRenamer.Lib.Service.Abstract
{
    public interface ISettingsService
    {
        Settings Get();
        Settings Update(Settings settings);
    }
}
