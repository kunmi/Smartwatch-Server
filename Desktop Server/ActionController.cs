using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WindowsInput;


namespace Desktop_Server
{
    class ActionController
    {
        MainWindow context;
        
        
        public enum DataType
        {
            ACCELEROMETER,
            GYRO,
            TOUCH,
            SPEECH,
            SCROLL,
            SHORTCUT,
            DPAD,
            KEYPAD
        }

        JavaScriptSerializer serializer;

        public ActionController(MainWindow program)
        {
            serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context = program;
        }

        public void decodeAction(String json)
        {
            
            try
            {
                dynamic dynamicObject = serializer.Deserialize<dynamic>(json);

                if (dynamicObject["type"] == DataType.ACCELEROMETER.ToString())
                {
                    Dictionary<String, object> values = dynamicObject["values"];
                    double x, y, z;
                    
                    x = double.Parse(values["x"].ToString());
                    y = double.Parse(values["y"].ToString());
                    z = double.Parse(values["z"].ToString());

                    MouseController.Move((int)x, (int)y);
                }

                
                if (dynamicObject["type"] == DataType.GYRO.ToString())
                {
                    Dictionary<String, object> values = dynamicObject["values"];
                    double x, y, z;

                    x = double.Parse(values["x"].ToString());
                    y = double.Parse(values["y"].ToString());
                    z = double.Parse(values["z"].ToString());
                    
                    if(Preference.InvertAxis)
                    {
                        double temp = z;
                        z = y;
                        y = -1 * temp;
                    }
              
                    if (Preference.ActiveMode == Preference.SUPPORTED_MODES.Mouse)
                    {
                        x = x * Preference.MOUSE_SPEED;
                        y = y * Preference.MOUSE_SPEED;
                        z = z * Preference.MOUSE_SPEED;


                        if (Preference.InvertY)
                            y = y * -1;

                        MouseController.Move((int)z, (int)y);
                    }
                    else if (Preference.ActiveMode == Preference.SUPPORTED_MODES.Keyboard)
                    {
                        KeyboardController.PerformMovement((int)z, (int)y);
                    }
                    else if (Preference.ActiveMode == Preference.SUPPORTED_MODES.Vjoy_Controller)
                    {
                        context.VJOY.OperateJoyStick(z,y);
                    }
                }

                if (dynamicObject["type"] == DataType.SPEECH.ToString())
                {
                    String text = dynamicObject["values"];
                    KeyboardController.ProcessSpeech(text.ToLower());
                }

                if (dynamicObject["type"] == DataType.DPAD.ToString())
                {
                    Dictionary<String, object> values = dynamicObject["values"];
                    //uint val = 0xFFFFFFFF;
                    int val = -1;
                    val = int.Parse(values["v"].ToString());
                    
                    if (Preference.ActiveMode == Preference.SUPPORTED_MODES.Vjoy_Controller)
                    {
                        if (val < 0) {
                            context.VJOY.operatePOV(0xFFFFFFFF);
                        }
                        else {
                            context.VJOY.operatePOV(Convert.ToUInt32(val));
                        }
                        
                    }
                }


                if (dynamicObject["type"] == DataType.SCROLL.ToString())
                {
                    Dictionary<String, object> values = dynamicObject["values"];
                    double x, y, z;

                    x = double.Parse(values["x"].ToString());
                    y = double.Parse(values["y"].ToString());
                    z = double.Parse(values["z"].ToString());

                    int scroll = (int)(-1 * y * Preference.MOUSE_SPEED);

                    MouseController.Scroll(scroll);                    
                }
                if (dynamicObject["type"] == DataType.SHORTCUT.ToString())
                {
                    String text = dynamicObject["values"];
                    KeyboardController.ProcessShortCuts(text.ToLower());
                }

                if (dynamicObject["type"] == DataType.KEYPAD.ToString())
                {
                    String text = dynamicObject["values"];
                    KeyboardController.processGlobeAction(text);
                    //KeyboardController.ProcessShortCuts(text.ToLower());
                }


                if (dynamicObject["type"] == DataType.TOUCH.ToString())
                {
                    String action = dynamicObject["values"];

                    if (Preference.ActiveMode == Preference.SUPPORTED_MODES.Mouse)
                    {

                        if (action == "DOWN")
                        {
                            if (dynamicObject["id"] == "L")
                                MouseController.LeftDown();
                            else if (dynamicObject["id"] == "R")
                                MouseController.RightDown();
                        }

                        else if (action == "UP")
                        {
                            if (dynamicObject["id"] == "L")
                                MouseController.LeftUp();
                            else if (dynamicObject["id"] == "R")
                                MouseController.RightUp();
                        }
                    }


                    else if (Preference.ActiveMode == Preference.SUPPORTED_MODES.Keyboard)
                    {
                        if (dynamicObject["id"] == "L")
                            KeyboardController.simulateButton(VirtualKeyCode.RETURN, action);
                        else if (dynamicObject["id"] == "R")
                            KeyboardController.simulateButton(VirtualKeyCode.ESCAPE, action);
                    }


                    else if (Preference.ActiveMode == Preference.SUPPORTED_MODES.Vjoy_Controller)
                    {
                        if (dynamicObject["id"] == "L")
                            context.VJOY.PressButton1(action);
                        else if (dynamicObject["id"] == "R")
                            context.VJOY.PressButton2(action);
                    }
                }
                

            }

            catch (Exception e)
            {
                Console.Out.WriteLine(e.StackTrace);
            }


        }

    }
}
