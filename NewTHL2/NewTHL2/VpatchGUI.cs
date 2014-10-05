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
        //値をセットする
        public void setValues(Dictionary<string,int> values,string titleName)
        {
            //値を取得
            Dictionary<string, int> vpatchValues = values;
            //起動時にWindow or FullScreenを尋ねる
            if(vpatchValues["AskWindowMode"] == 1)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            //Vsync側でWindowのサイズを指定する
            if(vpatchValues["enabled"] == 1)
            {
                checkBox2.Checked = true;
                //trueであるならば詳細設定をenabledにする
                詳細設定.Enabled = true;
            }
            else
            {
                checkBox2.Checked = false;
                //trueであるならば詳細設定をdiableにする
                詳細設定.Enabled = false;
            }
            
            //描画位置X
            描画位置X_Text.Text = vpatchValues["X"].ToString();

            //描画位置_Y
            描画位置Y_Text.Text = vpatchValues["Y"].ToString();

            //Width
            Width_Text.Text = vpatchValues["Width"].ToString();

            //Height
            Height_Text.Text = vpatchValues["Height"].ToString();

            //タイトルバーを表示させるかどうか
            if(vpatchValues["TitleBar"] == 1)
            {
                ShowTitleBar.Checked = true;
            }
            else
            {
                ShowTitleBar.Checked = false;
            }

            //常に手前で表示させる
            if(vpatchValues["AlwaysonTop"] == 1)
            {
                AlwaysOnTop.Checked = true;
            }
            else
            {
                AlwaysOnTop.Checked = false;
            }



            //垂直同期設定(とりあえず初期化)
            垂直同期設定.Enabled = false;
            垂直同期設定東方神霊廟.Enabled = false;
            垂直同期設定ダブルスポイラー以降.Enabled = false;
            
            //FPS設定（初期化）
            groupBox3.Enabled = true;

            //垂直同期設定（ダブルスポイラー）
            if(titleName == "Th125")
            {
                垂直同期設定ダブルスポイラー以降.Enabled = true;
                switch(vpatchValues["Vsync"])
                {
                    case -1:
                        {
                            //東方本家の描画方法で描画
                            G2_OriginalDrawing.Checked = true;
                            break;
                        }
                    case 0:
                        {
                            //垂直同期なし（Vpatch制御）
                            G2_NoVsync.Checked = true;
                            break;
                        }
                    case 1:
                        {
                            //Vpatch独自の描画形式
                            G2_VpatchOriginalDrawing.Checked = true;
                            break;
                        }
                }
            }

            //垂直同期設定（神霊廟）
            else if(titleName == "Th13")
            {
                垂直同期設定東方神霊廟.Enabled = true;
                switch (vpatchValues["Vsync"])
                {
                    case -1:
                        {
                            //ハードウェア補助レベル１
                            G_3HardWareAssistLv1.Checked = true;
                            break;
                        }
                    case -2:
                        {
                            //ハードウェア補助レベル２
                            G_3HardWareAssistLv2.Checked = true;
                            break;
                        }
                    case 0:
                        {
                            //東方本家の描画方法で描画
                            G3_OriginalDrawing.Checked = true;
                            break;
                        }
                    case 1:
                        {
                            //Vpatch独自の描画形式
                            G_3VpatchOriginalDrawing.Checked = true;
                            break;
                        }
                }
            }

            //垂直同期設定（通常）
            else
            {
                垂直同期設定.Enabled = true;
                switch (vpatchValues["Vsync"])
                {
                    case 0:
                        {
                            //東方本家の描画方法で描画
                            G1_OriginalDrawing.Checked = true;
                            break;
                        }
                    case 1:
                        {
                            //モニターのリフレッシュレートに合わせる
                            G1_MonitorRefreshLate.Checked = true;
                            break;
                        }
                    case 2:
                        {
                            //Vsync側で設定したFPSの値に近づけて描画
                            G1_VsyncFPS.Checked = true;
                            //FPSの設定はこの時の場合はOnにする
                            groupBox3.Enabled = true;
                            break;
                        }
                    case 3:
                        {
                            //画面の真ん中より下で操作する人向け
                            G1_UnderDrawing.Checked = true;
                            break;
                        }
                }
            }

            //FPS制御の設定
            switch(vpatchValues["SleepType"])
            {
                case -1:
                    {
                        break;
                    }
            }

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
