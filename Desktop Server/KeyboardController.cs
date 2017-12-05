using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
//STEP 1
using WindowsInput;


namespace Desktop_Server
{
    class KeyboardController
    {

        public static void PerformMovement(float x, float y)
        {
            if(y<0)
            {
          InputSimulator.SimulateKeyPress(VirtualKeyCode.UP);
            }
            else if(y>0)
            {
          InputSimulator.SimulateKeyPress(VirtualKeyCode.DOWN);
            }

            if(x > 0)
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.RIGHT);
            }
            else if(x<0)
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.LEFT);
            }

        }

        public static void ProcessSpeech(string text)
        {
            if(text == "save")
            {
                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, new[] { VirtualKeyCode.VK_S});
            }
            else if(text == "previous")
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.PRIOR);
            }

            else if(text == "next")
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.NEXT);
            }

            else if(text == "exit")
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.ESCAPE);
                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, new[] { VirtualKeyCode.F4 });
            }

            else if(text == "Return")
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
            }

            //For Test Sake   |  |  |  | white | black | purple | indigo | small | big
            else if(text.ToLower() == "green".ToLower())
            {
                InputSimulator.SimulateTextEntry("green");
            }

            else if (text.ToLower() == "yellow".ToLower())
            {
                InputSimulator.SimulateTextEntry("yellow");
            }
            else if (text.ToLower() == "blue".ToLower())
            {
                InputSimulator.SimulateTextEntry("blue");
            }
            else if (text.ToLower() == "orange".ToLower())
            {
                InputSimulator.SimulateTextEntry("orange");
            }
            else if (text.ToLower() == "red".ToLower())
            {
                InputSimulator.SimulateTextEntry("red");
            }
            else if (text.ToLower() == "white".ToLower())
            {
                InputSimulator.SimulateTextEntry("white");
            }
            else if (text.ToLower() == "black".ToLower())
            {
                InputSimulator.SimulateTextEntry("black");
            }
            else if (text.ToLower() == "purple".ToLower())
            {
                InputSimulator.SimulateTextEntry("purple");
            }
            else if (text.ToLower() == "indigo".ToLower())
            {
                InputSimulator.SimulateTextEntry("indigo");
            }
            else if (text.ToLower() == "small".ToLower())
            {
                InputSimulator.SimulateTextEntry("small");
            }
            else if (text.ToLower() == "big".ToLower())
            {
                InputSimulator.SimulateTextEntry("big");
            }

            else if (text.ToLower() == "clear".ToLower())
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.DELETE);
            }


            else
            {
                string[] parts = text.Split(new char[] {' '});
                
                if(parts[0].ToLower() == "action" )
                {
                    if (parts[1].ToLower() == "save")
                    {
                        InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, new[] { VirtualKeyCode.VK_S });
                    }
                    else if (parts[1].ToLower() == "previous")
                    {
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.BACK);
                    }

                    else if (parts[1].ToLower() == "next")
                    {
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.NEXT);
                    }

                    else if (parts[1].ToLower() == "exit")
                    {
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.ESCAPE);
                        InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, new[] { VirtualKeyCode.F4 });
                    }      
                }

                else if(parts[0].ToLower() == "enter")
                {
                    String input = String.Join(" ", parts, 1, parts.Length-1);
                    InputSimulator.SimulateTextEntry(input);
                }

            }

        }

  

        public static void simulateButton(VirtualKeyCode key, String action)
        {
            if (action == "DOWN")
            {
                InputSimulator.SimulateKeyDown(key);
            }

            else if (action == "UP")
            {
                InputSimulator.SimulateKeyUp(key);                
            }
        }


        public static void simulateButtonPress(string key)
        {
           InputSimulator.SimulateTextEntry(key);
        }
        

        // EXPERIMENTAL
        //Only For Keys 
        public static void processGlobeAction(string action)
        {
            if(action.ToLower() == "g".ToLower())
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_G);
            }
            else if (action == "m".ToLower())
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_M);
            }

            else if (action == "l".ToLower())
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L);
            }

            else if (action == "j".ToLower())
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J);
            }

            else if (action == "zoom in".ToLower())
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I);
            }


            else if (action == "zoom out".ToLower())
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K);
            }

            else if (action == "capture".ToLower())
            {
                Console.WriteLine("We are Capturing");
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_P);
            }

            else if (action == "remove".ToLower())
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_R);
            }

            else if(action.ToLower() == "germany")
            {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Q);
                
                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LSHIFT, new[] { VirtualKeyCode.VK_3 });
                Thread.Sleep(100);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_G);
                Thread.Sleep(100);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E);
                Thread.Sleep(100);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_R);
                Thread.Sleep(100);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_M);
                Thread.Sleep(100);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A);
                Thread.Sleep(100);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_N);
                Thread.Sleep(100);
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Y);
                Thread.Sleep(100);
                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LSHIFT, new[] { VirtualKeyCode.VK_3 });
                Thread.Sleep(100);

                //                InputSimulator.SimulateTextEntry("q#germany#");
            }




        }



        public static void ProcessShortCuts(string text)
        {
            if (text == "chrome-lasttab")
            {
                InputSimulator.SimulateModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, new[] { VirtualKeyCode.VK_T });
            }
            else if (text == "chrome-savepage")
            {
                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, new[] { VirtualKeyCode.VK_S });
            }
            else if (text == "chrome-incognito")
            {
                InputSimulator.SimulateModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, new[] { VirtualKeyCode.VK_N });
            }
            else if (text == "chrome-viewbook")
            {
                InputSimulator.SimulateModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, new[] { VirtualKeyCode.VK_B });
            }
            else if (text == "chrome-viewbookman")
            {
                InputSimulator.SimulateModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, new[] { VirtualKeyCode.VK_O });
            }
            else if (text == "chrome-viewhist")
            {
                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, new[] { VirtualKeyCode.VK_H });
            }
            else if (text == "chrome-dls")
            {
                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, new[] { VirtualKeyCode.VK_J });
            }
            else if (text == "chrome-cleard")
            {
                InputSimulator.SimulateModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, new[] { VirtualKeyCode.DELETE });
            }
            else if (text == "chrome-devt")
            {
                InputSimulator.SimulateModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, new[] { VirtualKeyCode.VK_J });
            }

            else if (text == "chrome-feedback")
            {
                InputSimulator.SimulateModifiedKeyStroke(new[] { VirtualKeyCode.MENU, VirtualKeyCode.SHIFT }, new[] { VirtualKeyCode.VK_I });
            }

            //
        }





    }
}
