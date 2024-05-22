using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лр25
{
    public partial class UCGrid : UserControl
    {
        public UCGrid()
        {
            InitializeComponent();
            dataGridView1.CellMouseUp += DataGridView1_CellMouseUp;
        }

        public object DataSource
        {
            get { return dataGridView1.DataSource; }
            set { dataGridView1.DataSource = value; dataGridView1.Refresh(); }
        }

        public DataGridViewRow SelectedRow
        {
            get { return dataGridView1.CurrentRow; }
        }

        public int ColumnIndex { get; private set; }

        public void LoadData(object data)
        {
            dataGridView1.DataSource = data;
        }

        private void DataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnIndex = e.ColumnIndex;
        }
    }
}
