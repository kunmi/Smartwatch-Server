using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyInterfaceWrap;

namespace Desktop_Server
{
    public class VJoyController
    {
        // Declaring one joystick (Device id 1) and a position structure. 
        static public vJoy joystick;
        static public vJoy.JoystickState iReport;
        static public uint id = 1;

        //Joysticks maximum value
        long joyStickMaxval = 0;
        //no of analogue sticks
        int ContPovNumber;
        // no of DPad
        int DiscPovNumber;
        //no of buttons
        int nButtons;

        private MainWindow context;

        public VJoyController(MainWindow window)
        {
            context = window;
            joystick = new vJoy();
            iReport = new vJoy.JoystickState();
        }

        public bool isVjoyEnabled() {
            return joystick.vJoyEnabled();
        }

        public bool activateVJoy()
        {
            VjdStat status = joystick.GetVJDStatus(id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    context.logmMssage("vJoy Device "+ id+" Connected Successfully");
                    break;
                case VjdStat.VJD_STAT_FREE:
                    context.logmMssage("vJoy Device " + id + " Connected Successfully");
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    context.logmMssage("vJoy Device " + id + " is already owned by another feeder\nCannot continue\n");
                    return false;
                case VjdStat.VJD_STAT_MISS:
                    context.logmMssage("vJoy Device is not installed or disabled\nCannot continue\n");
                    return false;
                default:
                    context.logmMssage("vJoy Device general error\nCannot continue\n");
                    return false;
            };

            bool AxisX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_X);
            bool AxisY = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Y);
            bool AxisZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Z);
            bool AxisRX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RX);
            bool AxisRZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RZ);
            // Get the number of buttons and POV Hat switchessupported by this vJoy device
            nButtons = joystick.GetVJDButtonNumber(id);
            ContPovNumber = joystick.GetVJDContPovNumber(id);
            DiscPovNumber = joystick.GetVJDDiscPovNumber(id);

            // Print results
            Console.WriteLine("\nvJoy Device {0} capabilities:\n", id);
            Console.WriteLine("Numner of buttons\t\t{0}\n", nButtons);
            Console.WriteLine("Numner of Continuous POVs\t{0}\n", ContPovNumber);
            Console.WriteLine("Numner of Descrete POVs\t\t{0}\n", DiscPovNumber);
            Console.WriteLine("Axis X\t\t{0}\n", AxisX ? "Yes" : "No");
            Console.WriteLine("Axis Y\t\t{0}\n", AxisX ? "Yes" : "No");
            Console.WriteLine("Axis Z\t\t{0}\n", AxisX ? "Yes" : "No");
            Console.WriteLine("Axis Rx\t\t{0}\n", AxisRX ? "Yes" : "No");
            Console.WriteLine("Axis Rz\t\t{0}\n", AxisRZ ? "Yes" : "No");


            UInt32 DllVer = 0, DrvVer = 0;
            bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
            if (match)
                Console.WriteLine("Version of Driver Matches DLL Version ({0:X})\n", DllVer);
            else
                Console.WriteLine("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer);


            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
            {
                context.logmMssage("Failed to acquire vJoy device number "+ id);
                return false;
            }
            else
                context.logmMssage("Acquired: vJoy device number " + id);

            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref joyStickMaxval);

            Console.WriteLine("max val is: " + joyStickMaxval);
            
            iReport.bDevice = (byte)id;

            joystick.ResetVJD(id);


            OperateJoyStick(0, 0);
            operatePOV(0xFFFFFFFF);

            joystick.UpdateVJD(id, ref iReport);


            return true;

        }

        public void OperateJoyStick(double X, double Y)
        {

            double xVal = interpolate(X);
            double yVal = interpolate(Y);

            if (xVal > joyStickMaxval)
                xVal = joyStickMaxval;
            if(yVal > joyStickMaxval)
                yVal = joyStickMaxval;
           
 
            if(xVal < 0)
                xVal = 0;

            if (yVal < 0)
                yVal = 0;

            iReport.AxisX = (int) xVal;
            iReport.AxisY = (int) yVal;
            //iReport.Buttons = (uint)(0x1 << (int)(5 / 20));
            //joystick.SetAxis(X, id, HID_USAGES.HID_USAGE_X);
            //joystick.SetAxis(Y, id, HID_USAGES.HID_USAGE_Y);
            //joystick.SetAxis(Z, id, HID_USAGES.HID_USAGE_Z);

            joystick.UpdateVJD(id, ref iReport);

        }

        public void operatePOV(uint val)
        {
            iReport.bHats = val;
            joystick.UpdateVJD(id, ref iReport);
        }

        public void PressButton1 (String action)
        {
            if (action == "DOWN")
            {
                iReport.Buttons = (uint)(0x1 << 1);
                //joystick.SetBtn(true, id, 1);
            }

            else if (action == "UP")
            {
                iReport.Buttons = (uint)(0x0 << 1);
                //joystick.SetBtn(false, id, 1);
            }
            joystick.UpdateVJD(id, ref iReport);
        }

        public void PressButton2(String action)
        {
            if (action == "DOWN")
            {
                iReport.Buttons = (uint)(0x1 << 2);
                //joystick.SetBtn(true, id, 1);
            }

            else if (action == "UP")
            {
                iReport.Buttons = (uint)(0x0 << 2);
                //joystick.SetBtn(false, id, 1);
            }
            joystick.UpdateVJD(id, ref iReport);
        }
        public void PressButton3(String action)
        {
            if (action == "DOWN")
            {
                iReport.Buttons = (uint)(0x1 << 3);
                //joystick.SetBtn(true, id, 1);
            }

            else if (action == "UP")
            {
                iReport.Buttons = (uint)(0x0 << 3);
                //joystick.SetBtn(false, id, 1);
            }
            joystick.UpdateVJD(id, ref iReport);
        }
        public void PressButton4(String action)
        {
            if (action == "DOWN")
            {
                iReport.Buttons = (uint)(0x1 << 4);
                //joystick.SetBtn(true, id, 1);
            }

            else if (action == "UP")
            {
                iReport.Buttons = (uint)(0x0 << 4);
                //joystick.SetBtn(false, id, 1);
            }
            joystick.UpdateVJD(id, ref iReport);
        }


        public void ReleaseController()
        {
            joystick.RelinquishVJD(id);
            context.logmMssage("Released VJOY");
        }


        float minGyro = -5;
        float maxGyro = +5;

        private double interpolate(double gyroVal)
        {
            double val = ((joyStickMaxval * (gyroVal - minGyro)) / (maxGyro - minGyro));
            return val;
        }
    }
}
