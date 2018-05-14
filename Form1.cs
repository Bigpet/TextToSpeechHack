using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.AudioFormat;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTSTest
{
    public partial class Form1 : Form
    {
        public TTSHelper TextToSpeech;
        public WaveOutEvent evt;

        public Form1()
        {
            InitializeComponent();
            this.TextToSpeech = new TTSHelper();
            var voices = TextToSpeech.synth.GetInstalledVoices();
            foreach (var voice in voices)
            {
                var item = new ToolStripMenuItem(voice.VoiceInfo.Name);
                item.Click += (s, e) => { TextToSpeech.synth.SelectVoice(voice.VoiceInfo.Name); };
                this.voiceToolStripMenuItem.DropDownItems.Add(item);
            }
            var speeds = new int[] { -10, -5, -2, 0, 2, 5, 10};
            foreach(var speed in speeds)
            {
                var item = new ToolStripMenuItem(speed.ToString());
                item.Click += (s, e) => { TextToSpeech.synth.Rate = speed; };
                this.speedToolStripMenuItem.DropDownItems.Add(item);
            }
            var volumes = new int[] { 10, 20, 50, 70, 90, 100 };
            foreach (var volume in volumes)
            {
                var item = new ToolStripMenuItem(volume.ToString());
                item.Click += (s, e) => { TextToSpeech.synth.Volume = volume; };
                this.volumeToolStripMenuItem.DropDownItems.Add(item);
            }
            int devnum = WaveOut.DeviceCount;
            for (int i = 0; i< devnum; ++i)
            {
                if(i == 0) this.evt = new WaveOutEvent() { DeviceNumber = 0 };
                var caps = WaveOut.GetCapabilities(i);
                var item = new ToolStripMenuItem(caps.ProductName);
                int n = i;
                item.Click += (s, e) => {
                    if (this.evt != null) this.evt.Dispose();
                    this.evt = new WaveOutEvent() { DeviceNumber = n };
                };
                this.outputToToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var text = textBox1.Text;
                var mem = new System.IO.MemoryStream(512000);
                this.TextToSpeech.synth.SetOutputToAudioStream(mem,new SpeechAudioFormatInfo(44100,  AudioBitsPerSample.Sixteen, AudioChannel.Stereo));
                this.TextToSpeech.synth.Speak(text);
                mem.Position = 0;
                if(this.evt.PlaybackState == PlaybackState.Playing)
                {
                    this.evt.Stop();
                }
                this.evt.Init(new RawSourceWaveStream(mem, new WaveFormat(44100, 16, 2)));
                this.evt.Play();
                //this.TextToSpeech.synth.Speak();
                textBox1.Text = "";
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

        }
    }
}
