using System;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;


namespace Mechanect.Classes
{  
    /// <summary>
    /// Voice Command Class allow you to check if certain word was said or not by user.
    /// </summary>
     /// <remarks>
    /// <para>AUTHOR: Tamer Nabil </para>
    /// </remarks>
    class VoiceCommands
    {     
        KinectAudioSource _kinectAudio;
        SpeechRecognitionEngine _speechRecognitionEngine;
        Stream _stream;
        readonly KinectSensor _kinect;
        string _hearedstring= " ";  
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
            _kinect = kinect;
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
            _speechRecognitionEngine = new SpeechRecognitionEngine(recognizerInfo.Id);
            var choices = new Choices();
            foreach (var command in arrayOfCommands)
           {
               choices.Add(command);
           }
           var grammarBuilder = new GrammarBuilder { Culture = recognizerInfo.Culture};
           grammarBuilder.Append(choices);
           var grammar = new Grammar(grammarBuilder);
            _speechRecognitionEngine.LoadGrammar(grammar);
            _speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngineSpeechRecognized;
        }
        /// <summary>
        /// StartAudioStream is amethod that the engine start listening to the user.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>

        public void StartAudioStream()
        {
            _kinectAudio = _kinect.AudioSource;
            _stream = _kinectAudio.Start();
            _speechRecognitionEngine.SetInputToAudioStream(_stream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            _speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);   
        }
         
        /// <summary>
        /// getHeared take Expectedstring as input and compare it with the Heared string from kinect and returns true
        /// if equal  and false otherwise.
        /// </summary>
         /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="expectedstring"></param>
        /// <returns>returns boolean ,true if he heared expectedstring,false otherwise</returns>

        public Boolean GetHeared(string expectedstring)
        {
            return expectedstring.Equals(_hearedstring);
        }
        
        //to be deleted !
         public Boolean getHeared(string expectedstring)
        {
            return expectedstring.Equals(_hearedstring);
        }

        /// <summary>
        /// This method store value of what said to kinect in the instance variable 
        /// "Hearedstring"
        /// </summary>
         /// <remarks>
           /// <para>AUTHOR: Tamer Nabil </para>
           /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeechRecognitionEngineSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.4)
                _hearedstring = e.Result.Text;
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


