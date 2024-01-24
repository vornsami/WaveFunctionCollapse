using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveFunctionCollapse.Models
{
    internal class AppVerInfo
    {
        public string Title => AppInfo.Name;
        public string Version => AppInfo.VersionString;
    }
}
