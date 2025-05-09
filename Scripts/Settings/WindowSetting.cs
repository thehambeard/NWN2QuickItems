using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWN2QuickItems.Settings
{
    class WindowSetting : ISetting
    {
        public bool WindowIsShown { get; set; }
        public float WindowPosX { get; set; }
        public float WindowPosY { get; set; }
        public float WindowSizeX { get; set; }
        public float WindowSizeY { get; set; }
        public float WindowScaleX { get; set; }
        public float WindowScaleY { get; set; }
        public float WindowScaleZ { get; set; }

        public WindowSetting()
        {
            
        }

        public WindowSetting
            (bool windowIsShown, 
            float windowPosX, 
            float windowPosY, 
            float windowSizeX, 
            float windowSizeY, 
            float windowScaleX, 
            float windowScaleY, 
            float windowScaleZ)
        {
            WindowIsShown = windowIsShown;
            WindowPosX = windowPosX;
            WindowPosY = windowPosY;
            WindowSizeX = windowSizeX;
            WindowSizeY = windowSizeY;
            WindowScaleX = windowScaleX;
            WindowScaleY = windowScaleY;
            WindowScaleZ = windowScaleZ;
        }
    }
}
