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
using System.Text.RegularExpressions;


namespace DataScience
{

    public partial class Komparator : Form
    {

        //Boolean który sprawdza czy jakikolwiek plik został wybrany.
        bool bFileChosen = false;

        //File Stream który powoduje że wybrany plik nie może być edytowany.
        FileStream fs;

        //Tworzy nowy odtwarzacz dźwięku, z dźwiękiem powiadomienia Windowsa.
        SoundPlayer errorSound = new SoundPlayer(@"C:\Windows\Media\Windows Notify System Generic.wav");

        //To jest wybrana ścieżka plików JSON, z automatu jest uznawana za C:\Data. W przyszłości będzie to folder w którym znajduje się nasz komparator.
        string enteredFilePath = @"C:\Data";
        

        //To jest nasz wybrany plik JSON. Z automatu jego ścieżka to "null" (nie mylić z brakiem zawartości)
        string chosenJson = "null";

        //A oto zawartość naszego naszego wybranego pliku JSON, jeśli owy nie został załadowany, użytkownik zostanie o tym poinformowany
        string chosenJsonContents = "No file selected.";

        public void ErrorMessage(string consolemsg, string msgboxmsg, string msgboxtitle)
        {
            Console.WriteLine(consolemsg);
            errorSound.Play();
            if (MessageBox.Show(msgboxmsg, msgboxtitle, MessageBoxButtons.OK) == DialogResult.OK)
            {
            }
        }
        public Komparator()
        {
            InitializeComponent();

            listBoxFilepath1.Clear();
            listBoxFilepath1.AppendText(enteredFilePath);
        }

        //Sprawdza czy ścieżka pliku jest właściwa
        static public bool isValidPath (string path)
        {
            Regex r = new Regex(@"^(([a-zA-Z]:)|(\))(\{1}|((\{1})[^\]([^/:*?<>""|]*))+)$");
            return r.IsMatch(path);
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
                if (isValidPath(listBoxFilepath1.Text))
                {
                    enteredFilePath = listBoxFilepath1.Text;
                }
            }
            else
            {
                ErrorMessage("No path specified", "Nie podano ścieżki plików.", "Błąd");
            }
        }

        //Wybrany plik JSON jest czytany, jego ścieżka oraz zawartość zapisywana.
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                if (bFileChosen != true)
                {
                    chosenJson = listBox1.SelectedItem.ToString();
                    chosenJsonContents = System.IO.File.ReadAllText(chosenJson);
                    fs = new FileStream(chosenJson, FileMode.Open, FileAccess.Read, FileShare.None);
                    bFileChosen = true;
                }
                else if (bFileChosen == true)
                {
                    ErrorMessage("JSON file already in use.", "Plik JSON został już wybrany.", "Błąd");
                }
            }
            else
            {
                ErrorMessage("No JSON file chosen.", "Nie wybrano pliku JSON.", "Błąd");
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
                ErrorMessage("No JSON file contents.", "Plik JSON nie ma zawartości", "Błąd");
                }
            }



        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.Description = "Wybierz Folder";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                listBoxFilepath1.Clear();
                listBoxFilepath1.AppendText(fbd.SelectedPath);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (bFileChosen == true)
            {
                fs.Close();
                chosenJson = "null";
                chosenJsonContents = "No file selected.";
                listBox1.Items.Clear();
                bFileChosen = false;
            } else
            {
                listBox1.Items.Clear();
            }
        }
    }
}
