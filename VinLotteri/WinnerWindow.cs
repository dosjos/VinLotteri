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
        Main form;


		//Default constructor
		public WinnerWindow()
		{
			InitializeComponent();
		}

		//Bacon constructor
		public WinnerWindow(string[] arr, String[]All, Main form)
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
           Label_Text(label3, Draws[r]);
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

            form.RemoveWinner(label3.Text);
            form.SaveVinner(label3.Text);
			Close();
		}

        private void button2_Click(object sender, EventArgs e)
        {
            GraphWindow f = new GraphWindow(DrawsArray, AllNames, label3.Text);
            f.Show();
        }




		
	}
}
