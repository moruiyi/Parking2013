using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGLedInterface
{
    public delegate void DisplayLed1TitleDelegate(string message);
    public delegate void DisplayLed2TitleDelegate(string message);
    public delegate void DisplayLed3TitleDelegate(string message);
    public delegate void DisplayLed4TitleDelegate(string message);

    public class CDelegate
    {
        public CDelegate()
        { }

        public event DisplayLed1TitleDelegate DisplayLed1TitleEvent;
        public event DisplayLed2TitleDelegate DisplayLed2TitleEvent;
        public event DisplayLed3TitleDelegate DisplayLed3TitleEvent;
        public event DisplayLed4TitleDelegate DisplayLed4TitleEvent;

        public void DisplayLed1Title(string message)
        {
            if (DisplayLed1TitleEvent != null)
            {
                DisplayLed1TitleEvent(message);
            }
        }

        public void DisplayLed2Title(string message)
        {
            if (DisplayLed2TitleEvent != null)
            {
                DisplayLed2TitleEvent(message);
            }
        }

        public void DisplayLed3Title(string message)
        {
            if (DisplayLed3TitleEvent != null)
            {
                DisplayLed3TitleEvent(message);
            }
        }

        public void DisplayLed4Title(string message)
        {
            if (DisplayLed4TitleEvent != null)
            {
                DisplayLed4TitleEvent(message);
            }
        }

    }
}
