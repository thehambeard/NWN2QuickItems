using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.PubSubSystem;
using UnityEngine;

namespace NWN2QuickItems.UI.MVVM.Events
{
    public interface IConversionWindowHandler : IGlobalSubscriber
    {
        public void OpenConversionWindow(RectTransform buttonRect, SlotConversion slotConversion, UnitEntityData unit);
        public void CloseConversionWindow();
    }
}
