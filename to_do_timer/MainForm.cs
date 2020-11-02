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
            //前回フォームを閉じたときの座標にフォームを表示
            this.Location = Properties.Settings.Default.StartFormPosition;
            // データグリッドビューの行を初期化 ※リロード用
            dataGridView1.Rows.Clear();

            //ファイル読み込み
            string[] read_lines = System.IO.File.ReadAllLines("task.csv", Encoding.GetEncoding("shift_jis"));


            //タスク表示
            DateTime today = DateTime.Today;
            foreach (string line in read_lines)
            {
                string[] row = line.Split(',');

                if (row[0] != "")
                {
                    DateTime due_date = DateTime.Parse(row[1]);
                    double limit = (due_date - today).TotalDays;
                    dataGridView1.Rows.Add("", row[0], row[1], row[1].Remove(0, 5), limit);
                }
            }

            //limit列でデータ並び替え
            dataGridView1.Sort(dataGridView1.Columns[4], ListSortDirection.Ascending);
            for (int count = 0; count < dataGridView1.Rows.Count; count++)
                dataGridView1.Rows[count].Cells[0].Value = count + 1;

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームの座標を保存
            Properties.Settings.Default.StartFormPosition = this.Location;
            Properties.Settings.Default.Save();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //ヘッダー以外のセルがダブルクリックされた場合
            if (e.RowIndex != -1) {
                //選択された行のタスク名と日付を取得
                DataGridView data_grid_view = (DataGridView)this.Controls.Find(((DataGridView)sender).Name, false)[0];
                string task_name = data_grid_view[1, e.RowIndex].Value.ToString();
                string full_date = data_grid_view[2, e.RowIndex].Value.ToString();

                //登録フォームを表示
                var f = new RegisterForm(task_name, full_date);
                f.ShowDialog();
                //データグリッドビューをリロード
                this.OnLoad(e);
            }
        }
    }
}
