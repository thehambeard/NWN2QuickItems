using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWN2QuickItems.Settings
{
    class HeaderSetting : ISetting
    {
        public bool IsExpanded { get; set; }

        public HeaderSetting()
        {
            
        }

        public HeaderSetting(bool isExpanded)
        {
            IsExpanded = isExpanded;
        }
    }
}
