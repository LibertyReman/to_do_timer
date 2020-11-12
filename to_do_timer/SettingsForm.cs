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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            //ウィンドウフロート設定
            this.TopMost = Properties.Settings.Default.TopMostSetting;

            //チェックボックスの初期値を設定
            checkBox1.Checked = Properties.Settings.Default.TopMostSetting;
            checkBox2.Checked = Properties.Settings.Default.MinimizeBoxSetting;
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TopMostの設定を保存
            Properties.Settings.Default.TopMostSetting = checkBox1.Checked;
            //最小化の設定を保存
            Properties.Settings.Default.MinimizeBoxSetting = checkBox2.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
