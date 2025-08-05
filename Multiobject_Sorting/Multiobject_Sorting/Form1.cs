using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multiobject_Sorting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitGridview();
        }

        private void InitGridview()
        {
            dataGridView1.ColumnCount = 4;  
            dataGridView1.Columns[0].Name = "图像x轴";
            dataGridView1.Columns[1].Name = "图像y轴";
            dataGridView1.Columns[2].Name = "现实x轴";
            dataGridView1.Columns[3].Name = "现实y轴";

        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
