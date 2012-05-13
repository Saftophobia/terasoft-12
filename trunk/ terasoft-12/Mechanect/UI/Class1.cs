using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;

namespace UI
{
    public class Class1
    {

        static SpeechSynthesizer synth = new SpeechSynthesizer();

        /// <summary>
        /// This method takes a string of text and reads it to the user.
        /// </summary>
        ///  <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 13 </para>
        /// <para>DATE MODIFIED: May, 13  </para>
        /// </remarks>
        /// <param name="text">The string of text to be said.</param>
        /// <returns> True if the string was said and false if it wasn't.</returns>
        public static bool SayText(string text)
        {
            try
            {
                synth.Volume = 100;
                synth.SpeakAsync(text);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
