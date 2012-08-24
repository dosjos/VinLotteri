using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;
using System.IO;

namespace VinLotteri
{
    public partial class Form1 : Form
    {
        static IList list = new ArrayList();
        String name;
        int week;
        String[] AllNames;

        CultureInfo myCI;
        Calendar myCal;
        CalendarWeekRule myCWR;
        DayOfWeek myFirstDOW;

        //Initiate stuff
        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "Velg kjøper";

            AllNames = new String[] {
            "Alf",
            "Anders Emil",
            "Andreas",
            "Bente",
            "Christine",
            "Daniel",
            "Eivind",
            "Fredrick",
            "Geir",
            "Jan Ole",
            "Jon",
            "Ragne",
            "Connie",
            "Vibekke",
            "Petter",
            "Ludvigsen", //2
            "Helene", //1
            "Lars", //2
            "Morten", //5
            "Maria", //1
            "Helge", // 1
            "Bent", // 5
            "Ida", //1
            "Hege"
            };

            this.comboBox1.Items.AddRange(AllNames);

            myCI = new CultureInfo("en-US");
            myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            week = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);
            numericUpDown2.Value = week;

            SetTitle();
            loadWeekSales();
        }

        //Changes the title and week
        private void SetTitle()
        {
            label4.Text = "Vin og spritlotteri for uke : " + week;
        }

        //Loads sales from file
        private void loadWeekSales()
        {
            TextReader tr = null;
            try
            {
                tr = new StreamReader(week + "_lotto.txt");
                string s = "";
                string res = "";
                while ((s = tr.ReadLine()) != null)
                {
                    res += s + '\n';
                }
                richTextBox1.Text = res.Trim();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                try
                {
                    tr.Close();
                }
                catch (Exception e)
                {

                }
            }
            sumSales();
        }

        //Register sale
        private void button1_Click(object sender, EventArgs e)
        {
            name = comboBox1.Text;
            int number = (int)numericUpDown1.Value;
            for (int i = 0; i < number; i++)
            {
                richTextBox1.Text = name + "\n" + richTextBox1.Text;
            }
            richTextBox1.Text = richTextBox1.Text.Trim();
            saveInformation();
            sumSales();
        }

        private void sumSales()
        {
            label6.Text = "" + richTextBox1.Lines.Length + " Lodd er solgt (" + richTextBox1.Lines.Length * 20 + " kr)";
            updateLoddCounter();    
        }

        //Writes sales to file
        private void saveInformation()
        {
            TextWriter tw = null;
            using (tw = new StreamWriter(week + "_lotto.txt"))
            {
                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    tw.Write(richTextBox1.Lines[i] + '\n');
                }
            }
        }


        //STD method
        private void Form1_Load(object sender, EventArgs e)
        {
            //Exces comments goes here
            //WRAR
        }

        //Draws the winner
        private void button2_Click(object sender, EventArgs e)
        {
            string[] arr = new string[richTextBox1.Lines.Length];

            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                arr[i] = richTextBox1.Lines[i];
            }
            WinnerWindow ww = new WinnerWindow(arr, AllNames, this);
            ww.Show();
        }

        //Changes week
        private void button3_Click(object sender, EventArgs e)
        {
            changeWeek();
        }

        private void changeWeek()
        {
            week = (int)numericUpDown2.Value;
            richTextBox1.Text = "";
            loadWeekSales();
            SetTitle();
            updateLoddCounter();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            changeWeek();
        }

        internal void RemoveWinner(string Winner)
        {
            int index = richTextBox1.Text.IndexOf(Winner);
            richTextBox1.Text = richTextBox1.Text.Remove(index, Winner.Length+1); //TODO Fix bug if drawn player is the last in the list
            richTextBox1.Text = richTextBox1.Text.Insert(index, "");
            richTextBox1.Text.Trim();
            saveInformation();
            updateLoddCounter();
        }

        private void updateLoddCounter()
        {
            label7.Text = "" + richTextBox1.Lines.Length + " å trekke fra";
        }

        internal void SaveVinner(string Winner)
        {
            using (TextWriter tw = new StreamWriter(week + "_vinnere.txt", true))
            {
             tw.Write(Winner + '\n');
            }
        }
    }
}
