using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrydayProject
{
    public partial class RecognizeOnLine : Form {
        SpeechSynthesizer Friday;
        private CultureInfo culture;
        private SpeechRecognitionEngine _sre;
        ListBox ListBox1=new ListBox();
        Emulight emulightForm = new Emulight();
        
        public RecognizeOnLine()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                culture = new CultureInfo("en-US");
                _sre = new SpeechRecognitionEngine(culture);
  

                // Setup event handlers
                _sre.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(sr_SpeechDetected);
                _sre.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(sr_RecognizeCompleted);
                _sre.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(sr_SpeechHypothesized);
                _sre.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sr_SpeechRecognitionRejected);
                _sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);

                // select input source
                _sre.SetInputToDefaultAudioDevice();

                // load grammar
                _sre.LoadGrammar(SwitchOnGrammar());
                _sre.LoadGrammar(SwitchOffGrammar());
                _sre.LoadGrammar(CheckRoomLight());
                _sre.LoadGrammar(Presentation());
                _sre.LoadGrammar(Presentation());

                Friday = new SpeechSynthesizer();
                
                


                AppendLine(Friday.Voice.Name.ToString());
                AppendLine(Friday.Voice.Gender.ToString());
                AppendLine(Friday.Voice.Age.ToString());
                AppendLine(Friday.Voice.Culture.Name.ToString());

                // start recognition
                _sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            try
            {
                Friday = new SpeechSynthesizer();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            emulightForm.Show();
        }


        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //listBox1.Items.Add("Work " + DateTime.Now.ToString() + ";");
            //Распознанная фраза
            string recoString = e.Result.Text;

            /*
            //Имя команды
            string cmdName = e.Result.Semantics["cmd"].Value.ToString();
            //Точка А маршрута
            string pointA = e.Result.Semantics["from"].Value.ToString();
            //Точка Б маршрута
            string pointB = e.Result.Semantics["to"].Value.ToString();
            //Показываем сообщение
            listBox1.Items.Add($"Маршрут от точки А: {pointA} до точки Б: {pointB}");
            */
            ListBox1.Items.Add(recoString + " " + DateTime.Now.ToString() + ";");
        }

       
        private Choices roomlight()
        {
            var val1 = new SemanticResultValue("top light", "top light");
            var val3 = new SemanticResultValue("back light", "back light");
            var val4 = new SemanticResultValue("side light", "side light");

            return new Choices(val1, val3, val4);
        }

       


        private Grammar SwitchOnGrammar()
        {
            var roomLight = roomlight();

            var grammarBuilder = new GrammarBuilder("Friday, switch on", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("switch on", roomLight));
            return new Grammar(grammarBuilder);
        }

        private Grammar SwitchOffGrammar()
        {
            var roomLight = roomlight();

            var grammarBuilder = new GrammarBuilder("Friday, switch off", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("switch off", roomLight));
            return new Grammar(grammarBuilder);
        }

        private Grammar CheckRoomLight()
        {
            var roomLight = roomlight();

            var grammarBuilder = new GrammarBuilder("Friday, can you check", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("can you check", roomLight));
            return new Grammar(grammarBuilder);
        }
        //-------------------------------------- grammar for general ----------------------------------------------
         private Choices selfPresentation()
        {
            var val1 = new SemanticResultValue("you", "you");
            var val3 = new SemanticResultValue("what can you do?", "what can you do?");
            var val4 = new SemanticResultValue("Thanks", "Thanks");

            return new Choices(val1, val3, val4);
        }
        private Grammar Presentation()
        {
            var cpresentation = selfPresentation();

            var grammarBuilder = new GrammarBuilder("Friday, say", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("Friday, say", cpresentation));
            return new Grammar(grammarBuilder);
        }
        private Choices ThanksWords()
        {
            var val1 = new SemanticResultValue("Thank you", "Thank you");
            var val3 = new SemanticResultValue("Oh my, thanks", "Thank you");
            var val4 = new SemanticResultValue("Thanks", "Thanks");

            return new Choices(val1, val3, val4);
        }
        private Grammar Thanks()
        {
            var thankses = ThanksWords();

            var grammarBuilder = new GrammarBuilder("Friday, ", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("Thank", thankses));
            return new Grammar(grammarBuilder);
        }





        private void sr_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            AppendLine("Распознавание речи отклонено: " + e.Result.Text);
        }

        private void sr_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            AppendLine("Вы хотели сказать: " + e.Result.Text + " (" + e.Result.Confidence + ")");
        }

        private void sr_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            AppendLine("Распознано: " + e.Result.Text);
        }

        private void sr_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            AppendLine("Речь обнаружена: audio pos " + e.AudioPosition);
        }

        private void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            AppendLine("\t" + "Распознано:");

            AppendLine(e.Result.Text + " (" + e.Result.Confidence + ")");

            if (e.Result.Confidence < 0.1f)
                return;

            for (var i = 0; i < e.Result.Alternates.Count; ++i)
            {
                AppendLine("\t" + "Альтернатива: " + e.Result.Alternates[i].Text + " (" + e.Result.Alternates[i].Confidence + ")");
            }

            for (var i = 0; i < e.Result.Words.Count; ++i)
            {
                AppendLine("\t" + "Слово: " + e.Result.Words[i].Text + " (" + e.Result.Words[i].Confidence + ")");

                if (e.Result.Words[i].Confidence < 0.1f)
                    return;
            }
             
            foreach (var s in e.Result.Semantics)
            {
                var SKey = (string)s.Key;
                var SValue = (string)s.Value.Value;
                int EMstate = 0;
                switch (SKey)
                {
                    case "switch on":
                        switch (SValue)
                        {
                            // EMState is 1 - light is already on 
                            // EMState is 2 - Normal stste for on, can switch
                            // EMState is 2 - Lamp broken 

                            case "top light":
                                EMstate = emulightForm.switchLightTop(true);
                                if (EMstate == 1) Say("Overhead light is already on");
                                if (EMstate == 3) Say("Overhead light can not be switched. Lamp is broken");
                                else Say("Overhead light on");
                                break;
                            case "back light":
                                EMstate = emulightForm.switchLightBottom(true);
                                if (EMstate == 1) Say("Back light is already on");
                                if (EMstate == 3) Say("Back light can not be switched. Lamp is broken");
                                else Say("Back light on");
                                break;
                            case "side light":
                                EMstate = emulightForm.switchLightSide(true);
                                if (EMstate == 1) Say("Side light is already on");
                                if (EMstate == 3) Say("Side light can not be switched. Lamp is broken");
                                else Say("Side light on");
                                break;
                        }
                        
                        break;
                    case "switch off":
                        switch (SValue)
                        {
                            case "top light":
                                EMstate = emulightForm.switchLightTop(false);
                                if (EMstate == 1) Say("overhead light is already off");
                                if (EMstate == 3) Say("Overhead light can not be switched. Lamp is broken");
                                else Say("overhead light off");
                                break;
                            case "back light":
                                EMstate = emulightForm.switchLightBottom(true);
                                if (EMstate == 1) Say("Back light is already off");
                                if (EMstate == 3) Say("Back light can not be switched. Lamp is broken");
                                else Say("Back light off");
                                break;
                            case "side light":
                                EMstate = emulightForm.switchLightSide(true);
                                if (EMstate == 1) Say("Side light is already off");
                                if (EMstate == 3) Say("Side light can not be switched. Lamp is broken");
                                else Say("Side light off");
                                break;
                        }
                        break;

                    case "can you check":
                        switch (SValue)
                        {
                            case "top light":
                                if (emulightForm.CheckTopLamp()== true)
                                    Say("Lighting works, no problems detected");
                                else
                                    Say("I found a problem. Top light don't work");
                                break;
                            case "back light":
                                if (emulightForm.CheckBottomLamp())
                                    Say("Lighting works, no problems detected");
                                else
                                    Say("I found a problem. Bottom light don't work");

                                break;
                            case "side light":
                                if (emulightForm.CheckSideLamp())
                                    Say("Lighting works, no problems detected");
                                else
                                    Say("I found a problem. Side light don't work");
                                break;
                        }

                        break;

                    case "Friday, say":
                        switch (SValue)
                        {
                            case "you":
                                Say("Hi, my name is Friday. I am a smart system that can diagnose any system, quickly notify you of faults and malfunctions. I also want to learn how to talk");

                                break;
                            case "what can you do": 
                                    Say("I am in charge of monitoring, diangostics and alerting systems ");
                                break;
                        }

                        break;
                    case "Thank":
                        switch (SValue)
                        {
                            case "Thank you":
                                Say("I am so happy to help you");

                                break;
                            case "Oh my, thanks":
                                Say("Oh my, this so cute. I want to help you more");
                                break;
                            case "Thanks":
                                Say("No problem");
                                break;
                        }

                        break;
                }
            }
        }

        private void AppendLine(string text)  // for logging all speake recognize
        {
            listBox2.Items.Add(text + Environment.NewLine);
            listBox2.TopIndex = ListBox1.Items.Count - 1;
        }

        public void AddPandora(string Text)
        {
            PandoraBox.Items.Add(Text);
            PandoraBox.TopIndex = PandoraBox.Items.Count - 1;
        }
        private void Say(string Frase)  // for speak any frases
        {
            Friday.SelectVoice("Microsoft Zira Desktop");
            Friday.SpeakAsync(Frase);
            AddPandora(Frase);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.Green;
            TimerCheck.Interval = Int32.Parse(textBox1.Text);
            TimerCheck.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.LightGray;
            TimerCheck.Stop();
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        int Sec1 = 0;
        private void TimerCheck_Tick(object sender, EventArgs e)
        {
            Sec1 = Int32.Parse(textBox1.Text);
            SecTimer.Stop();
            SecTimer.Start();
        }

        private void SecTimer_Tick(object sender, EventArgs e)
        {
            Sec1--;
            SecIndicator.Text = Sec1.ToString();
        }
    }
}

