using Kingmaker.Assets.UI.Common;
using Kingmaker.UI.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NWN2QuickItems.UI.WindowControl
{
    public class ResizePanelComp : MonoBehaviour, IResizeElement
    {
        private RectTransform _rectTransform;

        private void Start() => _rectTransform = GetComponent<RectTransform>();

        public void SetSizeDelta(Vector2 size) => _rectTransform.sizeDelta = size;

        public Vector2 GetSize() => _rectTransform.sizeDelta;

        public RectTransform GetTransform() => _rectTransform;
    }
}
