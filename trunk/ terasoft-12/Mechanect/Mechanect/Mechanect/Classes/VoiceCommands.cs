 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Speech;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;


namespace Mechanect.Classes
{
    /// <remarks>
    /// <para>AUTHOR: Tamer Nabil </para>
    /// </remarks>
    /// <summary>
    /// Voice Command Class allow you to check if certain word was said or not by user.
    /// </summary>
    class VoiceCommands
    {
    
        
        KinectAudioSource KinectAudio;
        SpeechRecognitionEngine sre;
        Stream stream;
        KinectSensor Kinect;
        String HearedString= " ";
        
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <summary>
        /// Constructor takes as input Kinect Sensor and use it to initialize the instance variable 
        ///"Kinect" and call InitalizeKinectAudio() to initiate the audio and String Command contains commands.
        ///Seperated By "," if acceptable commands.
        /// </summary>
        /// <param name="Kinect"></param>
        /// <param name="Command"></param>
   
        public VoiceCommands(KinectSensor Kinect,String Command)
        {
            this.Kinect = Kinect;
            InitalizeKinectAudio(Command);

        }

       /// <remarks>
       /// <para>AUTHOR: Tamer Nabil </para>
       /// </remarks>
       /// <summary>
       /// 
       /// InitalizeKinectAudio()   Get called by the constructor to initialize current Kinect audio Souce and 
       /// add grammers which can be accepted.
       /// 
       /// </summary>
       /// <param name="Command"></param>
         

        
        private void InitalizeKinectAudio(String Command)
        {
            String [] ArrayOfCommand = Command.Split(',');
            KinectAudio = Kinect.AudioSource;

            RecognizerInfo ri = GetKinectRecognizer();

            sre = new SpeechRecognitionEngine(ri.Id);

            var choices = new Choices();
           for (int i = 0; i < ArrayOfCommand.Length; i++)
            {
                choices.Add(ArrayOfCommand[i]);
            }
            

            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ri.Culture;
            gb.Append(choices);
            

           
            var g = new Grammar(gb);

            sre.LoadGrammar(g);
            sre.SpeechRecognized += SreSpeechRecognized;
           

            stream = KinectAudio.Start();

            sre.SetInputToAudioStream(stream,
                          new SpeechAudioFormatInfo(
                              EncodingFormat.Pcm, 16000, 16, 1,
                              32000, 2, null));

            sre.RecognizeAsync(RecognizeMode.Multiple);

        }

         /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <summary>
        /// getHeared take ExpectedString as input and compare it with the Heared String from kinect and returns true
        /// if equal  and false otherwise.
        /// </summary>
        /// <param name="ExpectedString"></param>
        /// <returns></returns>

        public Boolean getHeared(String ExpectedString)
        {
            if(ExpectedString.Equals(HearedString))
            {
                return true;
            }
            else{
                return false;
            }
          
        }
         /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <summary>
        /// This method store value of what said to kinect in the instance variable 
        /// "HearedString"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.5)
            {
                HearedString = e.Result.Text;

            }
            
        }
         /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <summary>
        /// a static method that returns a list of speech recognition engines on the system. 
        /// Speech uses a Language-Integrated Query (LINQ) to obtain the ID of the first recognizer in the list and 
        /// returns the results as a RecognizerInfo object. Speech 
        /// then uses RecognizerInfo.Id to create a SpeechRecognitionEngine object.
        /// </summary>
       
        /// <returns></returns>
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
