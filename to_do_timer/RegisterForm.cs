﻿using System;
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

            //タスク編集
            if(task_name.Trim().Length > 0)
            {
                textBox1.Text = task_name;
                monthCalendar1.SetDate(DateTime.Parse(date));
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
