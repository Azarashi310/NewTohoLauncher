using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace NewTHL2
{
    public partial class Form1 : Form
    {
        #region iniファイルを使いたい
        // Win32APIの GetPrivateProfileString を使う宣言
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);
        [DllImport("kernel32.dll")]
        private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpstring, string lpFileName);
        #endregion
        //作品用enum
        enum ThXXGames
        {
            Th06,Th07,Th075,Th08,Th09,Th095,Th10,Th105,Th11,Th12,Th123,Th125,Th128,Th13,Th135,Th14,Th143,alocstg
        }
        ThXXGames[] Thxx = new ThXXGames[18]
        {ThXXGames.Th06,ThXXGames.Th07,ThXXGames.Th075,ThXXGames.Th08,ThXXGames.Th09,ThXXGames.Th095,ThXXGames.Th10,ThXXGames.Th105,ThXXGames.Th11,ThXXGames.Th12,
         ThXXGames.Th123,ThXXGames.Th125,ThXXGames.Th128,ThXXGames.Th13,ThXXGames.Th135,ThXXGames.Th14,ThXXGames.Th143,ThXXGames.alocstg};
        
        //選択時変更用画像
        private Bitmap BMP = NewTHL2.Properties.Resources.PBG;
        //設定ファイル格納用フォルダパス
        public const string settingFolderPath = @"resource";
        //設定用iniファイルパス
        public const string settingFilePath = @"resource/settings.ini";
        //R/W用のエンコード
        private Encoding sjis = Encoding.GetEncoding("Shift-JIS");
        //ファイルパスが通ってるかどうかの記憶
        private bool[] FP_switch = new bool[18];

        //コンストラクタ
        public Form1()
        {
            InitializeComponent();
            //イベントハンドラの初期化
            eventHandlerInitialize();
            //パネルの配色の初期化
            panelColorInitialize();
            //起動時初期化設定
            Initialize();
        }
        //設定初期化
        private void Initialize()
        {
            //設定ファイル格納用フォルダと設定ファイルは存在するか？
            if(Directory.Exists(settingFolderPath) & File.Exists(settingFilePath))
            {
                filePathInitialize();
            }
            else
            {
                //フォルダが無ければ作る
                if(!Directory.Exists(settingFilePath))
                {
                    //フォルダ作成
                    Directory.CreateDirectory(settingFolderPath);
                    //設定ファイル作成
                    File.Create(settingFilePath).Close();
                    //設定ファイルの中身を作成
                    StreamWriter SW = new StreamWriter(settingFilePath , true, sjis);
                    SW.Write(NewTHL2.Properties.Resources.setteingTemplate);
                    SW.Close();
                    MessageBox.Show("設定ファイル及びフォルダが無いため新たに作成しました。", "お知らせ");
                }
                //設定ファイルがなければ作る
                if(!File.Exists(settingFilePath))
                {
                    File.Create(settingFilePath).Close();
                    //設定ファイルの中身を作成
                    StreamWriter SW = new StreamWriter(settingFilePath, true, sjis);
                    SW.Write(NewTHL2.Properties.Resources.setteingTemplate);
                    SW.Close();
                    MessageBox.Show("設定ファイルがないため新たに作成しました。", "お知らせ");
                }
            }
        }

        //ファイルパス設定の初期化
        private void filePathInitialize()
        {
            StringBuilder SB = new StringBuilder(1024);
            string[] FilePath = new string[18];
            for(int i = 0; i < 18; i++)
            {
                GetPrivateProfileString("FilePath", Thxx[i].ToString(), "", SB, Convert.ToUInt32(SB.Capacity), settingFilePath);
                FilePath[i] = SB.ToString();
                if(File.Exists(FilePath[i]))
                {

                }
            }
        }
        //イベントハンドラ一式の作成
        private void eventHandlerInitialize()
        {
            //スクロール用のイベントハンドラ
            tabControl1.Click += tabControl1_Click;
            #region パネルクリックハンドラ
            th06_P.Click += gamePanel_Click;
            th07_P.Click += gamePanel_Click;
            th075_P.Click += gamePanel_Click;
            th08_P.Click += gamePanel_Click;
            th09_P.Click += gamePanel_Click;
            th095_P.Click += gamePanel_Click;
            th10_P.Click += gamePanel_Click;
            th105_P.Click += gamePanel_Click;
            th11_P.Click += gamePanel_Click;
            th12_P.Click += gamePanel_Click;
            th123_P.Click += gamePanel_Click;
            th125_P.Click += gamePanel_Click;
            th128_P.Click += gamePanel_Click;
            th13_P.Click += gamePanel_Click;
            th135_P.Click += gamePanel_Click;
            th14_P.Click += gamePanel_Click;
            th143_P.Click += gamePanel_Click;
            alcostg_P.Click += gamePanel_Click;
            #endregion
            #region ラベルクリックハンドラ
            th06_L.Click += gamePanel_Click;
            th07_L.Click += gamePanel_Click;
            th075_L.Click += gamePanel_Click;
            th08_L.Click += gamePanel_Click;
            th09_L.Click += gamePanel_Click;
            th095_L.Click += gamePanel_Click;
            th10_L.Click += gamePanel_Click;
            th105_L.Click += gamePanel_Click;
            th11_L.Click += gamePanel_Click;
            th12_L.Click += gamePanel_Click;
            th123_L.Click += gamePanel_Click;
            th125_L.Click += gamePanel_Click;
            th128_L.Click += gamePanel_Click;
            th13_L.Click += gamePanel_Click;
            th135_L.Click += gamePanel_Click;
            th14_L.Click += gamePanel_Click;
            th143_L.Click += gamePanel_Click;
            alcostg_L.Click += gamePanel_Click;
            #endregion
            #region イメージクリックハンドラ
            th06_I.Click += gamePanel_Click;
            th07_I.Click += gamePanel_Click;
            th075_I.Click += gamePanel_Click;
            th08_I.Click += gamePanel_Click;
            th09_I.Click += gamePanel_Click;
            th095_I.Click += gamePanel_Click;
            th10_I.Click += gamePanel_Click;
            th105_I.Click += gamePanel_Click;
            th11_I.Click += gamePanel_Click;
            th12_I.Click += gamePanel_Click;
            th123_I.Click += gamePanel_Click;
            th125_I.Click += gamePanel_Click;
            th128_I.Click += gamePanel_Click;
            th13_I.Click += gamePanel_Click;
            th135_I.Click += gamePanel_Click;
            th14_I.Click += gamePanel_Click;
            th143_I.Click += gamePanel_Click;
            alcostg_I.Click += gamePanel_Click;
            #endregion
        }
        
        //パネルの配色の初期化
        private void panelColorInitialize()
        {
            th06_P.BackColor = Color.LightGray;
            th07_P.BackColor = Color.LightGray;
            th075_P.BackColor = Color.LightGray;
            th08_P.BackColor = Color.LightGray;
            th09_P.BackColor = Color.LightGray;
            th095_P.BackColor = Color.LightGray;
            th10_P.BackColor = Color.LightGray;
            th105_P.BackColor = Color.LightGray;
            th11_P.BackColor = Color.LightGray;
            th12_P.BackColor = Color.LightGray;
            th123_P.BackColor = Color.LightGray;
            th125_P.BackColor = Color.LightGray;
            th128_P.BackColor = Color.LightGray;
            th13_P.BackColor = Color.LightGray;
            th135_P.BackColor = Color.LightGray;
            th14_P.BackColor = Color.LightGray;
            th143_P.BackColor = Color.LightGray;
            alcostg_P.BackColor = Color.LightGray;
        }
        //複数のイベントハンドラを処理する
        void gamePanel_Click(object sender, EventArgs e)
        {
            tabPage1.Focus();
            panelBG_reflesh();
            #region パネル群&ラベル群&イメージ群
            if (sender.Equals(th06_P) | sender.Equals(th06_L) | sender.Equals(th06_I))
            {
                //rightPainのタイトルを変える
                titleName.Text = th06_L.Text;
                //パネルの背景色を変える
                th06_P.BackColor = Color.LightPink;
            }
            if (sender.Equals(th07_P) | sender.Equals(th07_L) | sender.Equals(th07_I))
            {
                titleName.Text = th07_L.Text;
                th07_P.BackColor = Color.LightPink;
            }
            if (sender.Equals(th075_P) | sender.Equals(th075_L) | sender.Equals(th075_I))
            {
                th075_P.BackColor = Color.LightPink;
            }
            if (sender.Equals(th08_P) | sender.Equals(th08_L) | sender.Equals(th08_I))
            {
                th08_P.BackColor = Color.LightPink;
            }
            if (sender.Equals(th09_P) | sender.Equals(th09_L) | sender.Equals(th09_I))
            {
                th09_P.BackColor = Color.LightPink;
            }
            if (sender.Equals(th095_P) | sender.Equals(th095_L) | sender.Equals(th095_I))
            {
                th095_P.BackColor = Color.LightPink;
            }
            if(sender.Equals(th10_P) | sender.Equals(th10_L) | sender.Equals(th10_I))
            {
                th10_P.BackColor = Color.LightPink;
            }
            if (sender.Equals(th105_P) | sender.Equals(th105_L) | sender.Equals(th105_I))
            {
                th105_P.BackColor = Color.LightPink;
            }
            if(sender.Equals(th11_P) | sender.Equals(th11_L) | sender.Equals(th11_I))
            {
                th11_P.BackColor = Color.LightPink;
            }
            if(sender.Equals(th12_P) | sender.Equals(th12_L) | sender.Equals(th12_I))
            {
                th12_P.BackColor = Color.LightPink;
            }
            if (sender.Equals(th123_P) | sender.Equals(th123_L) | sender.Equals(th123_I))
            {
                th123_P.BackColor = Color.LightPink;
            }
            if(sender.Equals(th125_P) | sender.Equals(th125_L) | sender.Equals(th125_I))
            {
                th125_P.BackColor = Color.LightPink;
            }
            if(sender.Equals(th128_P) | sender.Equals(th128_L) | sender.Equals(th128_I))
            {
                th128_P.BackColor = Color.LightPink;
            }
            if(sender.Equals(th13_P) | sender.Equals(th13_L) | sender.Equals(th13_I))
            {
                th13_P.BackColor = Color.LightPink;
            }
            if(sender.Equals(th135_P) | sender.Equals(th135_L) | sender.Equals(th135_I))
            {
                th135_P.BackColor = Color.LightPink;
            }
            if (sender.Equals(th14_P) | sender.Equals(th14_L) | sender.Equals(th14_I))
            {
                th14_P.BackColor = Color.LightPink;
            }
            if (sender.Equals(th143_P) | sender.Equals(th143_L) | sender.Equals(th143_I))
            {
                th143_P.BackColor = Color.LightPink;
            }
            if(sender.Equals(alcostg_P) | sender.Equals(alcostg_L) | sender.Equals(alcostg_I))
            {
                alcostg_P.BackColor = Color.LightPink;
            }
            #endregion
        }
        //全パネルの背景をリフレッシュ
        private void panelBG_reflesh()
        {
            /* ファイルパスが通ってるかどうかで色変えたいので先にそっちの実装 */
            //th07_P.BackgroundImage = null;
            //th06_P.BackgroundImage = null;
            //th10_P.BackgroundImage = null;
            //th095_P.BackgroundImage = null;
            //th09_P.BackgroundImage = null;
            //th08_P.BackgroundImage = null;
            //th11_P.BackgroundImage = null;
            //th12_P.BackgroundImage = null;
            //th125_P.BackgroundImage = null;
            //th128_P.BackgroundImage = null;
            //th13_P.BackgroundImage = null;
            //th14_P.BackgroundImage = null;
            //th143_P.BackgroundImage = null;
            //th075_P.BackgroundImage = null;
            //th105_P.BackgroundImage = null;
            //th123_P.BackgroundImage = null;
            //th135_P.BackgroundImage = null;
            //alcostg_P.BackgroundImage = null;
            
        }
        

        //タブをクリックしたら
        void tabControl1_Click(object sender, EventArgs e)
        {
            //スクロール用
            tabPage1.Focus();
        }
        //東方タブ
        private void tabPage1_Click(object sender, EventArgs e)
        {
            tabPage1.Focus();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void メニューToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        
    }
}
