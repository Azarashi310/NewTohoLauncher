using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THlauncher
{
    public partial class adonis : Form
    {
        public adonis()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.ControlBox = !this.ControlBox;
        }

        private void adonis_Load(object sender, EventArgs e)
        {
            
            if (checkBox1.Checked == true)
            {
                radioButton1.AutoCheck = true;
                radioButton2.AutoCheck = true;
                radioButton3.AutoCheck = true;
            }
            else
            {
                radioButton1.AutoCheck = false;
                radioButton2.AutoCheck = false;
                radioButton3.AutoCheck = false;
            }
            if (checkBox2.Checked == true)
            {
                this.textBox3.ReadOnly = false;
                this.textBox4.ReadOnly = false;
                this.textBox5.ReadOnly = false;
                this.textBox6.ReadOnly = false;
                this.checkBox3.AutoCheck = true;
                this.checkBox4.AutoCheck = true;
            }
            else
            {
                this.textBox3.ReadOnly = true;
                this.textBox4.ReadOnly = true;
                this.textBox5.ReadOnly = true;
                this.textBox6.ReadOnly = true;
                this.checkBox3.AutoCheck = false;
                this.checkBox4.AutoCheck = false;
            }
            //switch (trackBar1.Value)
            //{
            //    case -2:
            //        {
            //            label8.Text = "低";
            //            break;
            //        }
            //    case -1:
            //        {
            //            label8.Text = "通常以下";
            //            break;
            //        }
            //    case 0:
            //        {
            //            label8.Text = "通常";
            //            break;
            //        }
            //    case 1:
            //        {
            //            label8.Text = "通常以上";
            //            break;
            //        }
            //    case 2:
            //        {
            //            label8.Text = "高";
            //            break;
            //        }
            //}
            if (checkBox9.Checked == true)
            {
                textBox14.ReadOnly = false;
                textBox15.ReadOnly = false;
                textBox16.ReadOnly = false;
            }
            else
            {
                textBox14.ReadOnly = true;
                textBox15.ReadOnly = true;
                textBox16.ReadOnly = true;
            }
        }
        //確定
        private void button2_Click(object sender, EventArgs e)
        {
            /*
             * 
             * 
             * 判別処理ずらずら
             * 
             * 
             * 
             */
            //playername
            if (textBox1.TextLength != 0)
            {
                int wordbyte;
                Encoding SJISENC = Encoding.GetEncoding("Shift-JIS");
                wordbyte = SJISENC.GetByteCount(textBox1.Text);
                if (wordbyte > 32)
                {
                    MessageBox.Show("文字数が限度を超えています半角なら32文字全角なら16文字以内（32byte）で調整してください", "変更してください");
                    return;
                }
            }
            //X
            if (textBox3.TextLength != 0)
            {
                bool flag = false;
                foreach (char c in textBox3.Text)
                {
                    if (c < '0' || '9' < c)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    MessageBox.Show("数字のみ入力できます", "お知らせ");
                    return;
                }
            }
            //Y
            if (textBox4.TextLength != 0)
            {
                bool flag = false;
                foreach (char c in textBox4.Text)
                {
                    if (c < '0' || '9' < c)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    MessageBox.Show("数字のみ入力できます", "お知らせ");
                    return;
                }
            }
            //W
            if (textBox5.TextLength != 0)
            {
                bool flag = false;
                foreach (char c in textBox5.Text)
                {
                    if (c < '0' || '9' < c)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    MessageBox.Show("数字のみ入力できます", "お知らせ");
                    return;
                }
            }
            //H
            if (textBox6.TextLength != 0)
            {
                bool flag = false;
                foreach (char c in textBox6.Text)
                {
                    if (c < '0' || '9' < c)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    MessageBox.Show("数字のみ入力できます", "お知らせ");
                    return;
                }
            }
            this.Close();
        }
        //X
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        //Y
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        //W
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        //H
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        //ReplayAutoSave
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                radioButton1.AutoCheck = true;
                radioButton2.AutoCheck = true;
                radioButton3.AutoCheck = true;
            }
            else
            {
                radioButton1.AutoCheck = false;
                radioButton2.AutoCheck = false;
                radioButton3.AutoCheck = false;
            }
        }
        //なにもしない
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
        //日付ごとに～
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
        //対戦者毎に～
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
        //ファイルパス参照
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                FolderBrowserDialog FBD = new FolderBrowserDialog();
                //上部説明
                FBD.Description = "Adonis用のreplayファイル保存先を指定してください";
                //ルートフォルダ(デフォルトはデスクトップ)
                FBD.RootFolder = Environment.SpecialFolder.Desktop;
                //最初に選択するフォルダー(Rootfolder以下である必要性)
                FBD.SelectedPath = @"C:\Windows";
                //新規フォルダーを作れるか。
                FBD.ShowNewFolderButton = true;
                //ダイアログの表示
                if (FBD.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = FBD.SelectedPath;
                }
            }
            else
            {
                MessageBox.Show("ReplayAutoSaveにチェックを入れてから押してください", "お知らせ");
            }
        }
        //リプレイ保存先
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        //画面設定を有効にする
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                this.textBox3.ReadOnly = false;
                this.textBox4.ReadOnly = false;
                this.textBox5.ReadOnly = false;
                this.textBox6.ReadOnly = false;
                this.checkBox3.AutoCheck = true;
                this.checkBox4.AutoCheck = true;
            }
            else
            {
                this.textBox3.ReadOnly = true;
                this.textBox4.ReadOnly = true;
                this.textBox5.ReadOnly = true;
                this.textBox6.ReadOnly = true;
                this.checkBox3.AutoCheck = false;
                this.checkBox4.AutoCheck = false;
            }
        }
        //タイトルバーを表示させない
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }
        //常に手前に表示する
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }
        //スクロールさせるアレ。
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            switch (trackBar1.Value)
            {
                case -2:
                    {
                        label8.Text = "低";
                        break;
                    }
                case -1:
                    {
                        label8.Text = "通常以下";
                        break;
                    }
                case 0:
                    {
                        label8.Text = "通常";
                        break;
                    }
                case 1:
                    {
                        label8.Text = "通常以上";
                        break;
                    }
                case 2:
                    {
                        label8.Text = "高";
                        break;
                    }
            }
        }
        //花映塚の優先度
        private void label8_Click(object sender, EventArgs e)
        {

        }
        //プレイヤーネーム
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
        //Begin
        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                try
                {
                    OFD(0);
                }
                catch (NullReferenceException)
                {

                }
            }
            else
            {
                MessageBox.Show("Playsoundを有効にするにチェックを入れてください", "お知らせ");
            }
        }
        //BeginTEXT
        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
        //End
        private void button4_Click(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                try
                {
                    OFD(1);
                }
                catch (NullReferenceException)
                {

                }
            }
            else
            {
                MessageBox.Show("Playsoundを有効にするにチェックを入れてください", "お知らせ");
            }
            
        }
        //EndTEXT
        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
        //Abort
        private void button5_Click(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                try
                {
                    OFD(2);
                }
                catch (NullReferenceException)
                {

                }
            }
            else
            {
                MessageBox.Show("Playsoundを有効にするにチェックを入れてください", "お知らせ");
            }
        }
        //AbortTEXT
        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
        //SyncFilure
        private void button6_Click(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                try
                {
                    OFD(3);
                }
                catch (NullReferenceException)
                {

                }
            }
            else
            {
                MessageBox.Show("Playsoundを有効にするにチェックを入れてください", "お知らせ");
            }
        }
        //SyncFailure
        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }
        private void OFD(int value)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.FileName = "default.wav";
            OFD.InitialDirectory = @"C:\";
            OFD.Filter = "WAVファイル(*.wav;*.WAV;*.wave;*.WAVE)|*.wav;*.WAV;*.wave;*.WAVE";
            OFD.FilterIndex = 0;
            OFD.Title = "waveファイルを指定してください";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                switch (value)
                {
                    case 0:
                        {
                            textBox7.Text = OFD.FileName;
                            break;
                        }
                    case 1:
                        {
                            textBox8.Text = OFD.FileName;
                            break;
                        }
                    case 2:
                        {
                            textBox9.Text = OFD.FileName;
                            break;
                        }
                    case 3:
                        {
                            textBox10.Text = OFD.FileName;
                            break;
                        }
                }
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked == true)
            {
                textBox14.ReadOnly = false;
                textBox15.ReadOnly = false;
                textBox16.ReadOnly = false;
            }
            else
            {
                textBox14.ReadOnly = true;
                textBox15.ReadOnly = true;
                textBox16.ReadOnly = true;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        

        


        

        

        


        





        

    }

        
}

