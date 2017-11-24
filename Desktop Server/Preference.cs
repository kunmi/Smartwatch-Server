using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Server
{
    class Preference
    {

        public enum SUPPORTED_MODES
        {
            Mouse,
            Keyboard,
            Vjoy_Controller
        }

        public static SUPPORTED_MODES ActiveMode
        {
            get;
            set;
        }

        private static bool logEnabled = true;
        private static int mouse_speed = 1;

        private static bool invertY = false;
        private static bool invertAxis = false;
        public static bool InvertY
        {
            get {
                return invertY;
            }
            set {
                invertY = value;
            }
        }


        public static bool InvertAxis {
            get 
            {
                return invertAxis;
            }
            set
            {
                invertAxis = value;
            }
        }


        public static int MOUSE_SPEED {
            set {
                mouse_speed = value;
            }

            get {
                return mouse_speed;
            }
        }

        public static bool LOG_ENEBLED {
            set {
                logEnabled = value;
            }
            get
            {
                return logEnabled;
            }
        }
    }
}
