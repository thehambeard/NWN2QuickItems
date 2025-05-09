using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWN2QuickItems.Settings
{
    public interface ISetting
    {

    }

    public interface ISettingWrapper<T> : ISetting where T : new()
    {
        public T ToValue();
    }
}
