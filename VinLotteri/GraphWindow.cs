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
    public partial class GraphWindow : Form
    {
        public GraphWindow()
        {
            InitializeComponent();
        }

        public GraphWindow(int[]draws, String[] names, String Winner)
        {
            InitializeComponent();
            chart1.Series["Series1"].ChartType = SeriesChartType.Bar;
            chart1.Series["Series1"].Color = Color.Blue;
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            ConcatinateTicketHolders(draws, names);

            for(int i = 0; i < draws.Length; i++){
                if (names[i].Equals(Winner))
                {
                    chart1.Series["Series1"].Points.Add(draws[i]).AxisLabel = names[i]+ " Vant";          
                }
                else
                {
                    if (draws[i] > 0)
                    {
                        chart1.Series["Series1"].Points.Add(draws[i]).AxisLabel = names[i];
                    }
                }
            }
          }

        private void ConcatinateTicketHolders(int[]draws, string[]names)
        {
            for (int last = names.Length-1; last > 0; last--)
            {
                string name = names[last];
                for(int i = 0; i < last; i++){
                    if(names[i].Equals(name)){
                        draws[i] += draws[last];
                        draws[last] = 0;
                        names[last] = "";
                        break;
                    }
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
