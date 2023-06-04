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
        bool initialized = false;

        public MainForm()
        {
            InitializeComponent();

            //ウィンドウフロート設定
            this.TopMost = Properties.Settings.Default.TopMostSetting;
            //最小化表示設定
            this.MinimizeBox = Properties.Settings.Default.MinimizeBoxSetting;
            //ウィンドウ高さ設定
            this.Height = Properties.Settings.Default.FormHeightSetting;

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
            //ヘッダーとすべてのセルの内容に合わせて、列の幅を自動調整する
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //ヘッダーとすべてのセルの内容に合わせて、行の高さを自動調整する
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            initialized = true;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //前回フォームを閉じたときの座標にフォームを表示
            if (initialized)
            {
                this.Location = Properties.Settings.Default.StartFormPosition;
                initialized = false;
            }

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
                    if (7 < limit && limit < 15)
                        dataGridView1[4, dataGridView1.Rows.Count -1].Style.BackColor = Color.Yellow;
                    else if (0 < limit && limit < 8)
                        dataGridView1[4, dataGridView1.Rows.Count -1].Style.BackColor = Color.Red;
                    else if (limit < 1)
                        dataGridView1.Rows[dataGridView1.Rows.Count -1].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }

            //limit列でデータ並び替え
            dataGridView1.Sort(dataGridView1.Columns[4], ListSortDirection.Ascending);
            //タスク個数分No.列を追加
            for (int count = 0; count < dataGridView1.Rows.Count; count++)
                dataGridView1.Rows[count].Cells[0].Value = count + 1;

            //タスクは最大20個まで登録できるようにする
            for (int count = dataGridView1.Rows.Count + 1; count <= 20; count++)
                dataGridView1.Rows.Add(count, "", "", "");
        }

        //フォームが表示された後に一度だけ呼ばれる
        private void MainForm_Shown(object sender, EventArgs e)
        {
            //セル未選択状態にする
            dataGridView1.CurrentCell = null;

            //縦方向にだけフォームサイズ変更できるようにする  微調整：dataGridView1.Columns["Date"].Width * 2 + 2
            this.MinimumSize = new Size(dataGridView1.Columns["Number"].Width + dataGridView1.Columns["TaskName"].Width + dataGridView1.Columns["Date"].Width * 2 + 2, dataGridView1.Rows[1].Height * 4);
            this.MaximumSize = new Size(dataGridView1.Columns["Number"].Width + dataGridView1.Columns["TaskName"].Width + dataGridView1.Columns["Date"].Width * 2 + 2, dataGridView1.Rows[1].Height * 13);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームの座標を保存
            Properties.Settings.Default.StartFormPosition = this.Location;
            //ウィンドウ高さを保存
            Properties.Settings.Default.FormHeightSetting = this.Height;
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

        //右クリックしたら設定メニューを表示
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add("設定", this.contextMenu_click);
                m.Show(dataGridView1, new Point(e.X, e.Y));
            }
        }

        //設定メニューがクリックされたら設定フォームを表示
        private void contextMenu_click(object sender, EventArgs e)
        {
            var f = new SettingsForm();
            f.ShowDialog();
        }

        //セルクリックしてスクロールしたときに文字が重ならないように修正
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = false;
        }

        // アプリ起動から24時間経ったら再起動し日付更新する
        private void timer1_Tick(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
