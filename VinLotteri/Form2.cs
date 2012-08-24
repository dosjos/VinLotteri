using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace VinLotteri
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(int[]draws, String[] names, String Winner)
        {
            InitializeComponent();
            chart1.Series["Series1"].ChartType = SeriesChartType.Bar;
            chart1.Series["Series1"].Color = Color.Blue;
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            
            for(int i = 0; i < draws.Length; i++){
                if (names[i].Equals(Winner))
                {
                    chart1.Series["Series1"].Points.Add(draws[i]).AxisLabel = names[i]+ " Vant";          
                }
                else
                {
                    chart1.Series["Series1"].Points.Add(draws[i]).AxisLabel = names[i];
                }
            }
          }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }


    //THIS IS MY MIGHTY COMMENT
}
