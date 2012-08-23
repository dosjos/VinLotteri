using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace VinLotteri
{
    public partial class WinnerWindow : Form
    {

        int NumberOfDraws;
        Random random;
        string[] Draws;
        String[] AllNames;
        int[] DrawsArray;
        System.Timers.Timer timer;
        public delegate void DelLabelText(Label l, string s);
        public DelLabelText delLabelText;

        //Default constructor
        public WinnerWindow()
        {
            InitializeComponent();
        }

        //Bacon constructor
        public WinnerWindow(string[] arr, String[]All)
        {
            InitializeComponent();
            random = new Random();
            AllNames = All;
            NumberOfDraws = random.Next(200, 300);
            Draws = arr;
            delLabelText = Label_Text;

            DrawsArray = new int[12];
            timer  = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }


        //Whenever the timer finds that it is time to do something, it does this:
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            draw();
            timer.Interval = random.Next(5, 30);
            NumberOfDraws--;
            //If done drawing, find WINNER
            if (NumberOfDraws == 0)
            {
                timer.Enabled = false;
                label3.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                timer.Enabled = true;
            }
        }

        //Do actuall drawing of possible winners
        private void draw()
        {
            int r = random.Next(Draws.Length);

            for (int i = 0; i < DrawsArray.Length; i++)
            {
                if (AllNames[i].Equals(Draws[r]))
                {
                    DrawsArray[i]++;
                }
            }
            Label_Text(label3, Draws[r]); 
        }

        //Method for changing the winner name label
        public void Label_Text(Label label, string text)
        {
            if (label3.InvokeRequired)
            {
                label3.Invoke(delLabelText, new object[] { label, text });
            }
            else
            {
                label3.Text = text;
            }
        }
       
        //Listen for the birds and the clicks
        private void button1_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            timer.Dispose();
            Form2 f = new Form2(DrawsArray, AllNames, label3.Text);
            f.Show();
            Close();
        }
    }
}
