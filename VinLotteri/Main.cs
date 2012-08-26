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
    public partial class Main : Form
    {
        private static String NAMEFILE = "Names.txt";
        private static IList list = new ArrayList();
        private String name;
        private int week;

        CultureInfo myCI;
        Calendar myCal;
        CalendarWeekRule myCWR;
        DayOfWeek myFirstDOW;

        //Initiate stuff
        public Main()
        {
            InitializeComponent();

            button2.Enabled = false;
            this.comboBox1.Items.AddRange(loadNames());

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

        /*
         * Load names from the Names.txt file, the names are used in the dropdown list
         */
        private String[] loadNames()
        {
            ArrayList tempNames = new ArrayList();
            using (StreamReader sr = new StreamReader(NAMEFILE))
            {
                String s;
                while ((s = sr.ReadLine()) != null)
                {
                    tempNames.Add(s);
                }
            }
            return tempNames.ToArray(typeof(string)) as string[];
        }
        /*
         * Writes the names to the Names.txt file
         */
        private void saveNames()
        {
            using (StreamWriter sw = new StreamWriter(NAMEFILE))
            {
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    sw.WriteLine(comboBox1.Items[i]);
                }
            }
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
                    Console.WriteLine(e.StackTrace);
                }
            }
            sumSales();
            buttonEnabling();
        }
        //Manages the Draw buton so that one can not draw while noone have any tickets
        private void buttonEnabling()
        {
            if (richTextBox1.Lines.Length > 0)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }

        //Register sale
        private void button1_Click(object sender, EventArgs e)
        {
            name = comboBox1.Text;
            if (name.Length > 2)
            {
                CheckIfNameInListAndAdd(name);

                int number = (int)numericUpDown1.Value;
                for (int i = 0; i < number; i++)
                {
                    richTextBox1.Text = name + "\n" + richTextBox1.Text;
                }
                richTextBox1.Text = richTextBox1.Text.Trim();
                saveInformation();
                sumSales();
            }
        }

        //Adds new names to the dropdown list
        private void CheckIfNameInListAndAdd(string name)
        {
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i].Equals(name))
                {
                    return;
                }
            }
            comboBox1.Items.Add(name);
            saveNames(); 
        }
        //Sums and updates a label
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
            buttonEnabling();
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

            WinnerWindow ww = new WinnerWindow(arr, this);
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

        //Removes the winning ticked from the pile of tickets, renders it impossibru to win twice on one ticket
        internal void RemoveWinner(string Winner)
        {
            int index = richTextBox1.Text.IndexOf(Winner);
            try
            {
                richTextBox1.Text = richTextBox1.Text.Remove(index, Winner.Length + 1); //This is the dirtiest bugfix ever
            }
            catch (Exception)
            {
                richTextBox1.Text = richTextBox1.Text.Remove(index, Winner.Length);
            }
            finally
            {
                richTextBox1.Text = richTextBox1.Text.Insert(index, "");
                richTextBox1.Text.Trim();
                saveInformation();
                updateLoddCounter();
            }
            buttonEnabling();
        }

        private void updateLoddCounter()
        {
            label7.Text = "" + richTextBox1.Lines.Length + " å trekke fra";
        }

        //SAves the name of the winner to a file
        internal void SaveVinner(string Winner)
        {
            using (TextWriter tw = new StreamWriter(week + "_vinnere.txt", true))
            {
                tw.Write(Winner + '\n');
            }
        }
    }
}
