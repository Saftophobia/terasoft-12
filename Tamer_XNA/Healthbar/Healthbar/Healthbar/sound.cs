using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Speech;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;

namespace Healthbar
{

    class sound
    {
        KinectAudioSource kinectSource;
        SpeechRecognitionEngine sre;
        Stream stream;
        KinectSensor nui;
        public sound()
        {

        }
        private void InitalizeKinect()
        {

           nui = KinectSensor.KinectSensors[0];

            nui.Start();

            kinectSource = nui.AudioSource;

            RecognizerInfo ri = GetKinectRecognizer();

           sre = new SpeechRecognitionEngine(ri.Id);

            var choices = new Choices();
            choices.Add("up");
            choices.Add("down");
            
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ri.Culture;
            gb.Append(choices);

            var g = new Grammar(gb);

            sre.LoadGrammar(g);
            sre.SpeechRecognized += SreSpeechRecognized;

            Console.WriteLine("Recognizing Speech");

            stream = kinectSource.Start();

            sre.SetInputToAudioStream(stream,
                          new SpeechAudioFormatInfo(
                              EncodingFormat.Pcm, 16000, 16, 1,
                              32000, 2, null));

            sre.RecognizeAsync(RecognizeMode.Multiple);

        }

  
        private  void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("up"))
            {
               
            }
            else 
            {
                Console.WriteLine(e.Result.Text);
           
            }

           
        }
        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }
        
        }
    }

