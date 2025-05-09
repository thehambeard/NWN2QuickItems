using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NWN2QuickItems.Settings
{
    class ColorSetting : ISettingWrapper<Color>
    {
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }
        public float Alpha { get; set; }

        public ColorSetting()
        {
            
        }

        public ColorSetting(float red, float green, float blue, float alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public ColorSetting(Color color)
        {
            Red = color.r;
            Green = color.g;
            Blue = color.b;
            Alpha = color.a;
        }

        public Color ToValue() => new Color(Red, Green, Blue, Alpha);
    }
}
