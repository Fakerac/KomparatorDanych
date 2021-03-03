using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Media;


namespace DataScience
{

    public partial class Komparator : Form
    {
        
        //Tworzy nowy odtwarzacz dźwięku, z dźwiękiem powiadomienia Windowsa.
        SoundPlayer errorSound = new SoundPlayer(@"C:\Windows\Media\Windows Notify System Generic.wav");

        //To jest wybrana ścieżka plików JSON, z automatu jest uznawana za C:\Data. W przyszłości będzie to folder w którym znajduje się nasz komparator.
        string enteredFilePath = @"C:\Data";

        //To jest nasz wybrany plik JSON. Z automatu jego ścieżka to "null" (nie mylić z brakiem zawartości)
        string chosenJson = "null";

        //A oto zawartość naszego naszego wybranego pliku JSON, jeśli owy nie został załadowany, użytkownik zostanie o tym poinformowany
        string chosenJsonContents = "No file selected.";
        public Komparator()
        {
            InitializeComponent();

        }

        //Zbiera wszystkie pliki z końcówką .json w wybranej ścieżce, sortuje i pokazuje na liście.
        private void button2_Click(object sender, EventArgs e)
        {
            string[] filePaths = Directory.GetFiles(enteredFilePath, "*.json");
            if (filePaths.Length > 0)
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(filePaths);
            }
            else
            {
                Console.WriteLine("No files found.");
                listBox1.Items.Clear();
            }
        }

        //Sprawdza czy została podana ścieżka, jak nie, to błąd. Jeśli tak, to zapisuje ją.
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBoxFilepath1.TextLength > 0)
            {
                enteredFilePath = listBoxFilepath1.Text;
            }
            else
            {
                Console.WriteLine("No path specified.");
                errorSound.Play();
                if (MessageBox.Show("Nie podano ścieżki plików.", "Błąd", MessageBoxButtons.OK) == DialogResult.OK)
                {
                }
            }
        }

        //Wybrany plik JSON jest czytany, jego ścieżka oraz zawartość zapisywana.
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                chosenJson = listBox1.SelectedItem.ToString();
                chosenJsonContents = System.IO.File.ReadAllText(chosenJson);
            }
            else
            {
                Console.WriteLine("No JSON file chosen.");
                errorSound.Play();
                if (MessageBox.Show("Nie wybrano pliku JSON.", "Błąd", MessageBoxButtons.OK) == DialogResult.OK)
                {
                }
            }
        }

        //Sprawdza czy został wybrany plik JSON w zakładce Wybierania, jeśli tak, wyczyszcza chwilową zawartość, a później pokazuje go w "Zaserializowany JSON".
        //Jeśli nie, wyskakuje błąd
        private void button1_Click(object sender, EventArgs e)
        {
                if (chosenJsonContents.Length > 0)
                {
                    richTextBox4.Clear();
                    richTextBox4.AppendText(chosenJsonContents);
                }
                else
                {
                    Console.WriteLine("No JSON file chosen.");
                    errorSound.Play();
                    if (MessageBox.Show("Plik JSON nie ma zawartości", "Błąd", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                    }
                }
            }

    }
}
