using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.ServerManager
{
    [Serializable]
    public class UserPreferences
    {
        public bool MinimizeInTrayOnClose { get; set; }
        public bool DisplayDisconnectedNodes { get; set; }

        public UserPreferences()
        {
            MinimizeInTrayOnClose = false;
            DisplayDisconnectedNodes = false;
        }
    }
}
