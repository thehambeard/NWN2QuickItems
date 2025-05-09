using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Assets.UI.Common;
using Kingmaker.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using NWN2QuickItems.Utility.Helpers;

namespace NWN2QuickItems.UI.WindowControl
{
    public class DraggableWindowExt : ControlBase
    {
        public override void MoveAction(Vector2 vector)
        {
            Vector2 nPos = _containerStartPos + vector;
            _ownRectTransform.anchoredPosition = UIUtility.LimitPositionRectInRect(nPos, _parentRectTransform, _ownRectTransform);
            _lastMousePos = vector;
        }
    }
}
