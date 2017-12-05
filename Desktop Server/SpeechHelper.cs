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

        public SpeechHelper(MainWindow ctx) {

            context = ctx;
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
                context.logmMssage("Speech recognized: " + e.Result.Text);
            }
        }
    }
}
