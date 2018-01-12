using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;


namespace Desktop_Server
{
    class SpeechHelper
    {
        MainWindow context;
        SpeechRecognitionEngine recognizer;

        String action_down = "DOWN";
        String action_up = "UP";


        int duration = 3;
        KeyboardController keyboardController = null;

        public SpeechHelper(MainWindow ctx) {

            context = ctx;
            keyboardController = new KeyboardController(ctx);
        }

        public void initalizeEngine(){
            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            
            // Create a simple grammar that recognizes "red", "green", or "blue".


            Choices keySentences = new Choices();

 
            keySentences.Add(new string[] { "zoom in", "zoom in"});
            keySentences.Add(new string[] { "zoom out", "zoom out" });
            keySentences.Add(new string[] { "capture", "capture" });
            keySentences.Add(new string[] { "remove", "remove" });

            keySentences.Add(new string[] { "goto Germany" });
            keySentences.Add(new string[] { "goto Nigeria" });


            keySentences.Add(new string[] { "go higher" });
            keySentences.Add(new string[] { "enough" });
            keySentences.Add(new string[] { "go lower"});
            keySentences.Add(new string[] { "open fire" });
            keySentences.Add(new string[] { "give me guns" });
            keySentences.Add(new string[] { "give me rockets" });
            keySentences.Add(new string[] { "bank left" });
            keySentences.Add(new string[] { "bank right" });

 


            // Create a GrammarBuilder object and append the Choices object.
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(keySentences);

            // Create the Grammar instance and load it into the speech recognition engine.
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);

            recognizer.SetInputToDefaultAudioDevice();


            recognizer.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(SRE_RecognizeCompleted);
        
            // Register a handler for the SpeechRecognized event.
            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);

            // recognize speech asynchronous
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

        }

        void SRE_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            String capture = "capture";
           
        }
        // Create a simple handler for the SpeechRecognized event.
        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (Preference.ActiveMode == Preference.SUPPORTED_MODES.Mouse)
            {

                if (e.Result != null)
                {
                    if (e.Result.Text.CompareTo("zoom in") == 0)
                    {
                        KeyboardController.processGlobeAction("zoom in");
                    }
                    else if (e.Result.Text.CompareTo("zoom out") == 0)
                    {
                        KeyboardController.processGlobeAction("zoom out");
                    }
                    else if (e.Result.Text.CompareTo("capture") == 0)
                    {
                        KeyboardController.processGlobeAction("capture");
                    }
                    else if (e.Result.Text.CompareTo("remove") == 0)
                    {
                        KeyboardController.processGlobeAction("remove");
                    }
                    else if (e.Result.Text.CompareTo("goto Germany") == 0)
                    {
                        KeyboardController.processGlobeAction("Germany");
                    }
                }

            }

            else if (Preference.ActiveMode == Preference.SUPPORTED_MODES.Vjoy_Controller)
            {

                if(e.Result.Text == "give me guns")
                {
                    context.VJOY.PressButton3(action_down); 
                    context.VJOY.PressButton3(action_up);
                }


                if (e.Result.Text == "give me rockets")
                {
                    context.VJOY.PressButton4(action_down);
                    context.VJOY.PressButton4(action_up);
                }

                if (e.Result.Text == "open fire")
                {
                    context.VJOY.PressButton1(action_down);
                }

                if (e.Result.Text == "enough")
                {
                    KeyboardController.simulateButton(WindowsInput.VirtualKeyCode.UP, action_up);
                    KeyboardController.simulateButton(WindowsInput.VirtualKeyCode.DOWN, action_up);
                    KeyboardController.simulateButton(WindowsInput.VirtualKeyCode.LEFT, action_up);
                    KeyboardController.simulateButton(WindowsInput.VirtualKeyCode.RIGHT, action_up);
                 
                    context.VJOY.operatePOV(0xFFFFFFFF);
                    context.VJOY.PressButton1(action_up);
                }



                if (e.Result.Text == "go higher")
                {
                      keyboardController.simulatePressWithTimer(WindowsInput.VirtualKeyCode.UP, duration);
                        //KeyboardController.simulateButton(WindowsInput.VirtualKeyCode.UP, action_down);
                }

                if(e.Result.Text == "go lower")
                {
                    keyboardController.simulatePressWithTimer(WindowsInput.VirtualKeyCode.DOWN, duration);
                }

                if (e.Result.Text == "bank left")
                {
                    keyboardController.simulatePressWithTimer(WindowsInput.VirtualKeyCode.LEFT, duration);
                }


                if (e.Result.Text == "bank right")
                {
                    keyboardController.simulatePressWithTimer(WindowsInput.VirtualKeyCode.RIGHT, duration);
                }
            }


            context.logmMssage("Speech recognized: " + e.Result.Text);

        }
    }
}
