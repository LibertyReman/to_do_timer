using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace to_do_timer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //ユーザが列や行のサイズを変更できないようにする
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            //一番下の新しい行（新規行）を非表示にして、ユーザーが新しい行を追加できないようにする
            dataGridView1.AllowUserToAddRows = false;
            //セルのカーソルを上に置いたときに表示されるツールヒントを非表示にする
            dataGridView1.ShowCellToolTips = false;
            //縦のスクロールバーを表示して、横のスクロールバーは非表示にする
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            //セル選択を行選択として設定
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // データグリッドビューの行を初期化 ※リロード用
            dataGridView1.Rows.Clear();

            //新規追加方法がセルクリックのためグリッドビューを10行表示する。
            for (int count = 1; count <= 10; count++)
                dataGridView1.Rows.Add(count, "脅威分析修正", "10/7", "50");
        }
    }
}
