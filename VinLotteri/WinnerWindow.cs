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
        Form1 form;
        int drawPlace = 3;

		//Default constructor
		public WinnerWindow()
		{
			InitializeComponent();
		}

		//Bacon constructor
		public WinnerWindow(string[] arr, String[]All, Form1 form)
		{
			InitializeComponent();
			random = new Random();
			AllNames = All;
			NumberOfDraws = random.Next(100, 250);
			Draws = arr;
            this.form = form;
			delLabelText = Label_Text;

			DrawsArray = new int[All.Length];
			timer  = new System.Timers.Timer();
			timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			timer.Enabled = true;
           // button1.Enabled = false;
		}


		//Whenever the timer finds that it is time to do something, it does this:
		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			draw();
			timer.Interval = random.Next(5, 60);
			NumberOfDraws--;
			//If done drawing, find WINNER
			if (NumberOfDraws == 0)
			{
				timer.Enabled = false;
				label3.ForeColor = System.Drawing.Color.Green;
               // button1.Enabled = true;
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
            PublishDraw(r);
		}

        private void PublishDraw(int r)
        {

            if (drawPlace == 3)
            {
                Label_Text(label3, Draws[r]);
            }
            else if (drawPlace == 4)
            {
                Label_Text(label4, Draws[r]);
            }
            else if (drawPlace == 5)
            {
                Label_Text(label5, Draws[r]);
            }
            else if (drawPlace == 6)
            {
                Label_Text(label6, Draws[r]);
            }
            else if (drawPlace ==7)
            {
                Label_Text(label7, Draws[r]);
            }
            else if (drawPlace == 8)
            {
                Label_Text(label8, Draws[r]);
            }
            else if (drawPlace == 9)
            {
                Label_Text(label9, Draws[r]);
            }
            else if (drawPlace == 10)
            {
                Label_Text(label10, Draws[r]);
            }
            else if (drawPlace == 11)
            {
                Label_Text(label11, Draws[r]);
            }
            else if (drawPlace == 12)
            {
                Label_Text(label12, Draws[r]);
            }
            else if (drawPlace == 13)
            {
                Label_Text(label13, Draws[r]);
            }
            else if (drawPlace == 14)
            {
                Label_Text(label14, Draws[r]);
            }
            else if (drawPlace ==15)
            {
                Label_Text(label15, Draws[r]);
            }
            else if (drawPlace == 16)
            {
                Label_Text(label16, Draws[r]);
            }
            else if (drawPlace == 17)
            {
                Label_Text(label17, Draws[r]);
            }
            else if (drawPlace == 18)
            {
                Label_Text(label18, Draws[r]);
            }
            else if (drawPlace == 19)
            {
                Label_Text(label19, Draws[r]);
            }
            else if (drawPlace == 20)
            {
                Label_Text(label20, Draws[r]);
            }
            drawPlace++;

            if (drawPlace > 20) {
                drawPlace = 3;
            }
        }

		//Method for changing the winner name label
		public void Label_Text(Label label, string text)
		{
			if (label.InvokeRequired)
			{
				label.Invoke(delLabelText, new object[] { label, text });
			}
			else
			{
				label.Text = text;
			}
		}
	   
		//Listen for the birds and the clicks
		private void button1_Click(object sender, EventArgs e)
		{
			timer.Enabled = false;
			timer.Dispose();
			Form2 f = new Form2(DrawsArray, AllNames, label3.Text, form);
			f.Show();
			Close();
		}




		
	}
}
