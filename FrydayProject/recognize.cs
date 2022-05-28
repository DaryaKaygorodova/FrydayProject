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
    public partial class recognize : Form
    {
        SpeechSynthesizer Friday = new SpeechSynthesizer();
        private CultureInfo _culture;
        private SpeechRecognitionEngine _sre;

        
        public recognize()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _culture = new CultureInfo("en-US");
                _sre = new SpeechRecognitionEngine(_culture);


                // Setup event handlers
                _sre.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(sr_SpeechDetected);
                _sre.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(sr_RecognizeCompleted);
                _sre.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(sr_SpeechHypothesized);
                _sre.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sr_SpeechRecognitionRejected);
                _sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);

                // select input source
                _sre.SetInputToDefaultAudioDevice();

                // load grammar
                _sre.LoadGrammar(CreateSampleGrammar1());
                _sre.LoadGrammar(CreateSampleGrammar2());
                _sre.LoadGrammar(CreateSampleGrammarAQS());





                // start recognition
                _sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            try {
                Friday = new SpeechSynthesizer();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            /*
            string grammarPath = @"C:\test\";
            //Компилируем наше грамматическое правило в файл Маршруты.cfg
            FileStream fs = new FileStream(grammarPath + "Маршруты.cfg", FileMode.Create);
            SrgsGrammarCompiler.Compile(grammarPath + "Маршруты.grxml", (Stream)fs);
            fs.Close();

            Grammar gr = new Grammar(grammarPath + "Маршруты.cfg", "Routes");

            //Загружаем скомпилированный файл грамматики
            sre.LoadGrammar(gr);
             */
            //Подписываемся на событие распознавания
            //sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);

            //Пусть голос поступает с устройства по умолчанию
            /// sre.SetInputToDefaultAudioDevice();

            //Запускаем асинхронно распознаватель
            // sre.RecognizeAsync();


        }

        

        //Процедура распознавания
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
            listBox1.Items.Add(recoString + " " + DateTime.Now.ToString() + ";");
        }

        private Choices CreateSampleChoices()
        {
            var val1 = new SemanticResultValue("calculator", "calc");
            //var val2 = new SemanticResultValue("explorer", "explorer");
            var val3 = new SemanticResultValue("notepad", "notepad");
            var val4 = new SemanticResultValue("paint", "mspaint");

            return new Choices(val1, val3, val4);
        }
        private Choices CreateSampleChoices2()
        {
            var val5 = new SemanticResultValue("Work pannel");
            //var val2 = new SemanticResultValue("explorer", "explorer");
            var val6 = new SemanticResultValue("MainFrame");
            var val7 = new SemanticResultValue("EnterForm");

            return new Choices(val5, val6, val7);
        }
            private Grammar CreateSampleGrammar1()
        {
            var programs = CreateSampleChoices();

            var grammarBuilder = new GrammarBuilder("Friday, start", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = _culture;
            grammarBuilder.Append(new SemanticResultKey("start", programs));

            return new Grammar(grammarBuilder);
        }

        private Grammar CreateSampleGrammar2()
        {
            var programs = CreateSampleChoices();

            var grammarBuilder = new GrammarBuilder("Friday, close", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = _culture;
            grammarBuilder.Append(new SemanticResultKey("close", programs));

            return new Grammar(grammarBuilder);
        }
        private Grammar CreateSampleGrammarAQS()
        {
            var programs = CreateSampleChoices2();
            var grammarBuilder = new GrammarBuilder("friday, open", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = _culture;
            grammarBuilder.Append(new SemanticResultKey("start", programs));

            return new Grammar(grammarBuilder);
        }
        private Grammar CreateSampleGrammar4()
        {
            var programs = CreateSampleChoices2();
            var grammarBuilder = new GrammarBuilder("friday, close", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = _culture;
            grammarBuilder.Append(new SemanticResultKey("close", programs));

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
                var program = (string)s.Value.Value;

                switch (s.Key)
                {
                    case "start":
                        Process.Start(program); 
                        break;
                    case "close":
                        var p = Process.GetProcessesByName(program);
                        if (p.Length > 0)
                            p[0].Kill();
                        break;
                        
                            
                }
            }
        }

        private void AppendLine(string text)
        {
            listBox1.Items.Add(text + Environment.NewLine);
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

        private void recognize_Shown(object sender, EventArgs e)
        {
            Friday.Speak("I listen you");
        }

    }
    
}
