using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTHL2
{
    public partial class VpatchGUI : Form
    {
        public VpatchGUI()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ControlBox = !this.ControlBox;
            //this.trackBar1.ValueChanged += trackBar1_ValueChanged;
        }
        //決定ボタン
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("設定を変更致しますよろしいですか？", "お知らせ",MessageBoxButtons.OKCancel);
            if(DialogResult == DialogResult.OK)
            {
                //ここでVpatch.iniへの書き込み処理です
                this.Close();
            }
        }

        private void VpatchGUI_Load(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked == true)
            {
                詳細設定.Enabled = true;
            }
            else
            {
                詳細設定.Enabled = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void 非アクティブでも描画_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checksum_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void D3DMultiThredforDubleSpoilerforTenDesire_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void D3DMultiThredforDubleSpoiler_CheckedChanged(object sender, EventArgs e)
        {

        }

        
    }
}
