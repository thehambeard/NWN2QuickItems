using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWN2QuickItems.UI.MVVM.Events
{
    interface IMetaMagicHandler : IGlobalSubscriber
    {
        public void OnMetaMagicAdd(Feature metamagic, int heightenLevel = -1);
        public void OnMetaMagicRemove(Feature metamagic);
    }
}
