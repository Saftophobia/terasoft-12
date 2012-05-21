using System;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;


namespace Mechanect.Common
{  
    /// <summary>
    /// Voice Command Class allow you to check if certain word was said or not by user.
    /// </summary>
     /// <remarks>
    /// <para>AUTHOR: Tamer Nabil </para>
    /// </remarks>
   public class VoiceCommands
    {     
        KinectAudioSource kinectAudio;
        SpeechRecognitionEngine speechRecognitionEngine;
        Stream stream;
        readonly KinectSensor kinect;
        string hearedString= " ";  
        /// <summary>
        /// Constructor takes as input Kinect Sensor and use it to initialize the instance variable 
        ///"Kinect" and call InitalizeKinectAudio() to initiate the audio and string Command contains commands.
        ///Seperated By "," if acceptable commands.
        /// </summary>
         /// <remarks>
          /// <para>AUTHOR: Tamer Nabil </para>
         /// </remarks>
        /// <param name="kinect">kinect</param>
        /// <param name="commands">commands</param>
   
        public VoiceCommands(KinectSensor kinect,string commands)
        {
            this.kinect = kinect;
            InitalizeKinectAudio(commands);
        }  
       /// <summary>
       /// InitalizeKinectAudio()   Get called by the constructor to initialize current Kinect audio Souce and 
       /// add grammers which can be accepted.
       /// </summary>
       /// <remarks>
       /// <para>AUTHOR: Tamer Nabil </para>
       /// </remarks>
       /// <param name="commands">list of commands separated by ,</param>
         
        private void InitalizeKinectAudio(string commands)
        {
            string [] arrayOfCommands = commands.Split(',');
          //  KinectAudio = Kinect.AudioSource;
            RecognizerInfo recognizerInfo = GetKinectRecognizer();
            speechRecognitionEngine = new SpeechRecognitionEngine(recognizerInfo.Id);
            var choices = new Choices();
            foreach (var command in arrayOfCommands)
           {
               choices.Add(command);
           }
           var grammarBuilder = new GrammarBuilder { Culture = recognizerInfo.Culture};
           grammarBuilder.Append(choices);
           var grammar = new Grammar(grammarBuilder);
            speechRecognitionEngine.LoadGrammar(grammar);
            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngineSpeechRecognized;
        }
        /// <summary>
        /// StartAudioStream is amethod that the engine start listening to the user.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>

        public void StartAudioStream()
        {
            try
            {
                kinectAudio = kinect.AudioSource;
                stream = kinectAudio.Start();
                speechRecognitionEngine.SetInputToAudioStream(stream,
                                                              new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1,
                                                                                        32000, 2, null));
                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                
            }

        }
         
        /// <summary>
        /// getHeared take expectedString as input and compare it with the Heared string from kinect and returns true
        /// if equal  and false otherwise.
        /// </summary>
         /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="expectedString"></param>
        /// <returns>returns boolean ,true if he heared expectedString,false otherwise</returns>

        public bool GetHeared(string expectedString)
        {
            return expectedString.Equals(hearedString);
        }

        [Obsolete("getHeared is deprecated, please use GetHeared instead.")]
        public Boolean getHeared(string expectedString)
        {
            return expectedString.Equals(hearedString);
        }

        /// <summary>
        /// This method store value of what said to kinect in the instance variable 
        /// "hearedString"
        /// </summary>
         /// <remarks>
           /// <para>AUTHOR: Tamer Nabil </para>
           /// </remarks>
        /// <param name="sender">sender</param>
        /// <param name="e">Event argument</param>
        private void SpeechRecognitionEngineSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.4)
                hearedString = e.Result.Text;
        }

        /// <summary>
        /// a static method that returns a list of speech recognition engines on the system. 
        /// Speech uses a Language-Integrated Query (LINQ) to obtain the ID of the first recognizer in the list and 
        /// returns the results as a RecognizerInfo object. Speech 
        /// then uses RecognizerInfo.Id to create a SpeechRecognitionEngine object.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>returns RecognizerInfo</returns>
        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = matchFunction =>
            {
                string value;
                matchFunction.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(matchFunction.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

    }
}
