using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;

namespace UI
{
    public static class UILib
    {
        private static SpeechSynthesizer speechSynthesizer;
        /// <summary>
        /// This method takes a string of text and reads it to the user.
        /// </summary>
        ///  <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 13 </para>
        /// <para>DATE MODIFIED: May, 13  </para>
        /// </remarks>
        /// <param name="text">The string of text to be read out loud.</param>
        /// <returns> true if the string was said and false if it wasn't.</returns>
        public static bool SayText(string text)
        {
            if (speechSynthesizer == null)
            {
                speechSynthesizer = new SpeechSynthesizer();
            }
            try
            {
                speechSynthesizer.Volume = 100;
                speechSynthesizer.SpeakAsync(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        
    }
}
