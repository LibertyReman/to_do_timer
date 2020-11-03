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
    public partial class RegisterForm : Form
    {
        public RegisterForm(string task_name, string date)
        {
            InitializeComponent();

            //ファイル読み込み
            string[] read_lines = System.IO.File.ReadAllLines("task.csv", Encoding.GetEncoding("shift_jis"));

            //タスク編集
            if(task_name.Trim().Length > 0)
            {
                //選択されたタスクを削除
                int i = 0;
                foreach (string line in read_lines)
                {
                    string[] row = line.Split(',');
                    if (row[0].Contains(task_name) && row[1].Contains(date))
                    {
                        read_lines = read_lines.Where((source, index) => index != i).ToArray();
                        //foreach文を抜ける
                        break;
                    }
                    i++;
                }
                System.IO.File.WriteAllLines("task.csv", read_lines, Encoding.GetEncoding("shift_jis"));
                //テキストボックスとカレンダーに値を代入
                textBox1.Text = task_name;
                monthCalendar1.SetDate(DateTime.Parse(date));

                //カーソル位置を行末へ
                textBox1.Select(textBox1.Text.Length, 0);
            }

        }

        private void RegisterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //タスク名が空じゃない場合はファイルに書き込む
            if (textBox1.Text.Trim().Length > 0)
            {
                using (var sw = new System.IO.StreamWriter("task.csv", true, Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(textBox1.Text.Trim() + ",");
                    sw.Write(monthCalendar1.SelectionStart.ToShortDateString());
                    sw.WriteLine("");
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //カンマの入力を禁止する
            if (e.KeyChar == ',')
                e.Handled = true;
        }
    }
}
