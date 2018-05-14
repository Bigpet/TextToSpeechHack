using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace TTSTest
{
    public class TTSHelper
    {
        public SpeechSynthesizer synth;

        public TTSHelper() {
            synth = new SpeechSynthesizer();
        }

        public void test()
        {
            synth.Speak("This is a test");
        }
    }
}
