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
            keySentences.Add(new string[] { "go forward", "forward"});
            keySentences.Add(new string[] { "back", "go back", "come back" });
            keySentences.Add(new string[] { "stop", "wait", "come back" });

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
            context.logmMssage("Speech recognized: Z" + e.Result.Text);
        }
        // Create a simple handler for the SpeechRecognized event.
        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            context.logmMssage("Speech recognized: " + e.Result.Text);
        }
    }
}
