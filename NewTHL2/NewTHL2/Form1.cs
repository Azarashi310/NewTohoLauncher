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
            alcostg, Th06, Th07, Th075, Th08, Th09, Th095, Th10, Th105, Th11, Th12, Th123, Th125, Th128, Th13, Th135, Th14, Th143
        }
        ThXXGames[] Thxx = new ThXXGames[18]
        {ThXXGames.alcostg,ThXXGames.Th06,ThXXGames.Th07,ThXXGames.Th075,ThXXGames.Th08,ThXXGames.Th09,ThXXGames.Th095,ThXXGames.Th10,ThXXGames.Th105,ThXXGames.Th11,ThXXGames.Th12,
         ThXXGames.Th123,ThXXGames.Th125,ThXXGames.Th128,ThXXGames.Th13,ThXXGames.Th135,ThXXGames.Th14,ThXXGames.Th143};

        //選択時変更用画像
        //private Bitmap BMP = NewTHL2.Properties.Resources.PBG;
        //設定ファイル格納用フォルダパス
        public const string settingFolderPath = @"resource";
        //設定用iniファイルパス
        public const string settingFilePath = @"resource/settings.ini";
        //ハッシュファイルパス
        public const string hashFilePath = @"resource/hash.ini";
        //R/W用のエンコード
        private Encoding sjis = Encoding.GetEncoding("Shift-JIS");
        //ファイルパスが通ってるかどうかの記憶
        private string[] FP_switch = new string[18];
        //今選択しているもの
        private int select = 999;

        //コンストラクタ
        public Form1()
        {
            InitializeComponent();
            //起動時の読み込みダイアログを出す
            NewTHL2.WalkUp WU = new WalkUp();
            WU.Show();
            //イベントハンドラの初期化
            eventHandlerInitialize();
            //起動時初期化設定
            Initialize();
            //ここで起動時ダイアログを殺す
            WU.Dispose();
        }
        //設定初期化
        private void Initialize()
        {
            //設定ファイル格納用フォルダと設定ファイルは存在するか？
            if (Directory.Exists(settingFolderPath) & File.Exists(settingFilePath) & File.Exists(hashFilePath))
            {
                //ファイルパスが通ってるかどうかの設定です。
                filePathInitialize();
                //パネルの配色の初期化
                panelColorInitialize();
                //パネルのアイコンの初期化
                panelIconInitialize();
            }
            else
            {
                //フォルダが無ければ作る
                if (!Directory.Exists(settingFilePath))
                {
                    //フォルダ作成
                    Directory.CreateDirectory(settingFolderPath);
                    //設定ファイル作成
                    File.Create(settingFilePath).Close();
                    //設定ファイルの中身を作成
                    StreamWriter SW = new StreamWriter(settingFilePath, true, sjis);
                    SW.Write(NewTHL2.Properties.Resources.setteingTemplate);
                    SW.Close();
                    MessageBox.Show("設定ファイル及びフォルダが無いため新たに作成しました。", "お知らせ");
                }
                //設定ファイルがなければ作る
                if (!File.Exists(settingFilePath))
                {
                    File.Create(settingFilePath).Close();
                    //設定ファイルの中身を作成
                    StreamWriter SW = new StreamWriter(settingFilePath, true, sjis);
                    SW.Write(NewTHL2.Properties.Resources.setteingTemplate);
                    SW.Close();
                    MessageBox.Show("設定ファイルがないため新たに作成しました。", "お知らせ");
                }
                //ハッシュファイルがなければつくる
                if (!File.Exists(hashFilePath))
                {
                    File.Create(hashFilePath).Close();
                    StreamWriter SW = new StreamWriter(hashFilePath, true, sjis);
                    SW.Write(NewTHL2.Properties.Resources.hash);
                    SW.Close();
                    MessageBox.Show("ハッシュファイルを作成しました。", "お知らせ");
                }
            }
        }
        //パネルのアイコンの初期化
        private void panelIconInitialize()
        {
            for(int i = 0; i < Thxx.Length;  i++)
            {
                switch(i)
                {
                    case 0:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                alcostg_I.Image = algo.GetIcon.returnPanelIcon(EXE, alcostg_I.Width, alcostg_I.Height);
                            }
                            break;
                        }
                    case 1:
                        {
                            string EXE;
                            //実行ファイルはちゃんと存在するか
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                //ピクチャボックスの変更
                                th06_I.Image = algo.GetIcon.returnPanelIcon(EXE, th06_I.Width, th06_I.Height);
                            }
                            break;
                        }
                    case 2:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th07_I.Image = algo.GetIcon.returnPanelIcon(EXE, th07_I.Width, th07_I.Height);
                            }
                            break;
                        }
                    case 3:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th075_I.Image = algo.GetIcon.returnPanelIcon(EXE, th075_I.Width, th075_I.Height);
                            }
                            break;
                        }
                    case 4:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th08_I.Image = algo.GetIcon.returnPanelIcon(EXE, th08_I.Width, th08_I.Height);
                            }
                            break;
                        }
                    case 5:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th09_I.Image = algo.GetIcon.returnPanelIcon(EXE, th09_I.Width, th09_I.Height);
                            }
                            break;
                        }
                    case 6:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th095_I.Image = algo.GetIcon.returnPanelIcon(EXE, th095_I.Width, th095_I.Height);
                            }
                            break;
                        }
                    case 7:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th10_I.Image = algo.GetIcon.returnPanelIcon(EXE, th10_I.Width, th10_I.Height);
                            }
                            break;
                        }
                    case 8:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th105_I.Image = algo.GetIcon.returnPanelIcon(EXE, th105_I.Width, th105_I.Height);
                            }
                            break;
                        }
                    case 9:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th11_I.Image = algo.GetIcon.returnPanelIcon(EXE, th11_I.Width, th11_I.Height);
                            }
                            break;
                        }
                    case 10:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th12_I.Image = algo.GetIcon.returnPanelIcon(EXE, th12_I.Width, th12_I.Height);
                            }
                            break;
                        }
                    case 11:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th123_I.Image = algo.GetIcon.returnPanelIcon(EXE, th123_I.Width, th123_I.Height);
                            }
                            break;
                        }
                    case 12:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th125_I.Image = algo.GetIcon.returnPanelIcon(EXE, th125_I.Width, th125_I.Height);
                            }
                            break;
                        }
                    case 13:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th128_I.Image = algo.GetIcon.returnPanelIcon(EXE, th128_I.Width, th128_I.Height);
                            }
                            break;
                        }
                    case 14:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th13_I.Image = algo.GetIcon.returnPanelIcon(EXE, th13_I.Width, th13_I.Height);
                            }
                            break;
                        }
                    case 15:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th135_I.Image = algo.GetIcon.returnPanelIcon(EXE, th135_I.Width, th135_I.Height);
                            }
                            break;
                        }
                    case 16:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th14_I.Image = algo.GetIcon.returnPanelIcon(EXE, th14_I.Width, th14_I.Height);
                            }
                            break;
                        }
                    case 17:
                        {
                            string EXE;
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th143_I.Image = algo.GetIcon.returnPanelIcon(EXE, th143_I.Width, th143_I.Height);
                            }
                            break;
                        }
                    
                }
            }
        }

        //ファイルパス設定の初期化
        private void filePathInitialize()
        {
            StringBuilder FP = new StringBuilder(1024);
            string[] FilePath = new string[18];
            for (int i = 0; i < Thxx.Length; i++)
            {
                GetPrivateProfileString("FilePath", Thxx[i].ToString(), "", FP, Convert.ToUInt32(FP.Capacity), settingFilePath);
                FilePath[i] = FP.ToString();
                if (File.Exists(FilePath[i]))
                {
                    FP_switch[i] = FP.ToString();
                }
                else
                {
                    FP_switch[i] = FP.ToString();
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

        //パネルの配色の初期化(もっといい方法をみつけること)
        private void panelColorInitialize()
        {
            for (int i = 0; i < 18; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                alcostg_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                alcostg_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th06_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th06_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th07_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th07_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th075_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th075_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th08_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th08_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th09_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th09_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 6:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th095_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th095_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 7:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th10_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th10_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 8:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th105_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th105_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 9:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th11_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th11_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 10:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th12_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th12_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 11:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th123_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th123_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 12:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th125_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th125_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 13:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th128_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th128_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 14:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th13_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th13_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 15:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th135_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th135_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 16:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th14_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th14_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 17:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th143_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th143_P.BackColor = Color.LightGray;
                            }
                            break;
                        }

                }
            }
        }
        //複数のイベントハンドラを処理する
        void gamePanel_Click(object sender, EventArgs e)
        {
            tabPage1.Focus();
            //配色を戻す
            panelColorInitialize();
            //一度パネルを初期化させる
            rightPainIcon.Image = null;

            #region パネル群&ラベル群&イメージ群
            if (sender.Equals(alcostg_P) | sender.Equals(alcostg_L) | sender.Equals(alcostg_I))
            {
                string EXE;
                //rightPainのタイトルを変える
                titleName.Text = alcostg_L.Text;
                //パネルの背景色を変える
                alcostg_P.BackColor = Color.LightPink;
                //選択しているものを記憶させる
                select = 0;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width,rightPainIcon.Height);
                }
            }
            if (sender.Equals(th06_P) | sender.Equals(th06_L) | sender.Equals(th06_I))
            {
                string EXE;
                //rightPainのタイトルを変える
                titleName.Text = th06_L.Text;
                //パネルの背景色を変える
                th06_P.BackColor = Color.LightPink;
                //選択しているものを記憶させる
                select = 1;
                //テキストボックスの変更
                textBox1.Text = FP_switch[select].ToString();
                //実行ファイルはちゃんと存在するか
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    //ライトペインの画像を変更
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th07_P) | sender.Equals(th07_L) | sender.Equals(th07_I))
            {
                string EXE;
                titleName.Text = th07_L.Text;
                th07_P.BackColor = Color.LightPink;
                select = 2;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th075_P) | sender.Equals(th075_L) | sender.Equals(th075_I))
            {
                string EXE;
                titleName.Text = th075_L.Text;
                th075_P.BackColor = Color.LightPink;
                select = 3;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th08_P) | sender.Equals(th08_L) | sender.Equals(th08_I))
            {
                string EXE;
                titleName.Text = th08_L.Text;
                th08_P.BackColor = Color.LightPink;
                select = 4;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th09_P) | sender.Equals(th09_L) | sender.Equals(th09_I))
            {
                string EXE;
                titleName.Text = th09_L.Text;
                th09_P.BackColor = Color.LightPink;
                select = 5;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th095_P) | sender.Equals(th095_L) | sender.Equals(th095_I))
            {
                string EXE;
                titleName.Text = th095_L.Text;
                th095_P.BackColor = Color.LightPink;
                select = 6;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th10_P) | sender.Equals(th10_L) | sender.Equals(th10_I))
            {
                string EXE;
                titleName.Text = th10_L.Text;
                th10_P.BackColor = Color.LightPink;
                select = 7;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th105_P) | sender.Equals(th105_L) | sender.Equals(th105_I))
            {
                string EXE;
                titleName.Text = th105_L.Text;
                th105_P.BackColor = Color.LightPink;
                select = 8;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th11_P) | sender.Equals(th11_L) | sender.Equals(th11_I))
            {
                string EXE;
                titleName.Text = th11_L.Text;
                th11_P.BackColor = Color.LightPink;
                select = 9;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnPanelIcon(EXE, th11_I.Width, th11_I.Height);
                }
            }
            if (sender.Equals(th12_P) | sender.Equals(th12_L) | sender.Equals(th12_I))
            {
                string EXE;
                titleName.Text = th12_L.Text;
                th12_P.BackColor = Color.LightPink;
                select = 10;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th123_P) | sender.Equals(th123_L) | sender.Equals(th123_I))
            {
                string EXE;
                titleName.Text = th123_L.Text;
                th123_P.BackColor = Color.LightPink;
                select = 11;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th125_P) | sender.Equals(th125_L) | sender.Equals(th125_I))
            {
                string EXE;
                titleName.Text = th125_L.Text;
                th125_P.BackColor = Color.LightPink;
                select = 12;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {                    
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th128_P) | sender.Equals(th128_L) | sender.Equals(th128_I))
            {
                string EXE;
                titleName.Text = th128_L.Text;
                th128_P.BackColor = Color.LightPink;
                select = 13;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width,rightPainIcon.Height);
                }
            }
            if (sender.Equals(th13_P) | sender.Equals(th13_L) | sender.Equals(th13_I))
            {
                string EXE;
                titleName.Text = th13_L.Text;
                th13_P.BackColor = Color.LightPink;
                select = 14;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th135_P) | sender.Equals(th135_L) | sender.Equals(th135_I))
            {
                string EXE;
                titleName.Text = th135_L.Text;
                th135_P.BackColor = Color.LightPink;
                select = 15;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th14_P) | sender.Equals(th14_L) | sender.Equals(th14_I))
            {
                string EXE;
                titleName.Text = th14_L.Text;
                th14_P.BackColor = Color.LightPink;
                select = 16;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th143_P) | sender.Equals(th143_L) | sender.Equals(th143_I))
            {
                string EXE;
                titleName.Text = th143_L.Text;
                th143_P.BackColor = Color.LightPink;
                select = 17;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            #endregion
        }
        //ファイルパスの参照
        private void button1_Click(object sender, EventArgs e)
        {
            //ファイルパス
            string FP = "";
            //.exeのパス取得
            string EXE = "";
            //ハッシュ取得用
            StringBuilder hash = new StringBuilder(1024);
            //比較用
            bool flag;
            //何も選択してない場合は参照させない形にするのがいいね
            if (select == 999)
            {
                MessageBox.Show("とりあえず左の一覧からなにか選択してください", "話はそれからだ");
                return;
            }
            //ファイルダイアログを開き、ファイルパスを取得
            FP = algo.OFDandSFD.FBD_Run();
            if (FP == @"C:\Windows")
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
                return;
            }
            else
            {
                //ハッシュを取得
                GetPrivateProfileString("HASH", Thxx[select].ToString(), "", hash, Convert.ToUInt32(hash.Capacity), hashFilePath);
                //何故かハッシュ値が消えている場合の処理
                if (hash.ToString() == "")
                {
                    MessageBox.Show("ハッシュファイルエラー" + Environment.NewLine + "メニューからハッシュ値の更新を押してください", "お知らせ");
                    return;
                }
                //ハッシュを比較
                EXE = thxx_EXE(FP,select);
                //EXEファイルが存在するか
                if(!File.Exists(EXE))
                {
                    MessageBox.Show("そこに東方の実行ファイルはありますか？正しい場所を選択してください", "お知らせ");
                    return;
                }
                flag = algo.Hash.compairMD5(EXE, hash.ToString());
                //選択したものが正しいか
                if (flag == true)
                {
                    //iniファイルに書き込み
                    WritePrivateProfileString("FilePath", Thxx[select].ToString(), FP, settingFilePath);
                    textBox1.Text = FP;
                }
                else
                {
                    MessageBox.Show("違うファイルが選択されているか、ハッシュ値表が古いです" + Environment.NewLine +
                        "ファイルが違う場合は再度参照しなおしてください。" + Environment.NewLine + "ハッシュ値表が古い場合はメニューからハッシュ値の更新を押してください（要ネット環境）", "お知らせ");
                    return;
                }
            }
        }
        //ゲームの実行ファイル名を返す
        private string thxx_EXE(string FP,int value)
        {
            string EXE;
            
            if(value == 1)
            {
                EXE = Path.Combine(FP, "東方紅魔郷.exe");
            }
            else
            {
                EXE = Path.Combine(FP, Thxx[value].ToString() + ".exe");
            }
            return EXE;
        }
        //ファイル一括登録
        private void 一括登録ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ファイルパス
            string FP = "";
            //エラー回避用ドライブレター
            string DL;
            MessageBox.Show("東方が全部（一部でも可）入っているであろうフォルダを選択してください。" + Environment.NewLine + "時間がそこそこかかります。", "お知らせ");
            //ファイルパスの取得
            FP = algo.OFDandSFD.FBD_Run();
            DL = Path.GetPathRoot(FP);
            //キャンセル処理
            if(FP == @"C:\Windows")
            {
                MessageBox.Show("キャンセルされました","お知らせ");
                return;
            }
            //ルートドライブを指定されてしまった場合
            if(FP == DL)
            {
                MessageBox.Show("ドライブレターを直接は指定しないでね", "お知らせ");
                return;
            }
            
            NewTHL2.Search SC = new Search();
            SC.Show(this);
            //ここで検索しつつ登録
            for (int i = 0; i < Thxx.Length; i++)
            {
                //プログレスバーの値を変動させる
                SC.progressBar1.Value = i;
                //EXEファイルの名前を取得する
                string EXEName = thxx_EXE("", i);
                //検索してヒットしたものを格納する配列
                string[] THEXE = Directory.GetFiles(FP, EXEName, SearchOption.AllDirectories);
                //比較するべきハッシュ値を格納するもの
                StringBuilder hash = new StringBuilder(1024);
                //正否用のbool
                bool flag = true;
                //ハッシュ値の取得
                GetPrivateProfileString("HASH", Thxx[i].ToString(), "", hash, Convert.ToUInt32(hash.Capacity), hashFilePath);
                foreach (string temp in THEXE)
                {
                    //東方花映塚　～ Phantasmagoria of Flower View.の場合
                    if (Thxx[i].ToString() == "Th09")
                    {
                        //2種のハッシュ格納用
                        string[] hashArray = new string[2];
                        //分割すべき位置を取得
                        int separated = hash.ToString().IndexOf("_");
                        //花映塚Ver1.00の方の格納
                        hashArray[0] = hash.ToString().Substring(0, separated);
                        //花映塚Ver1.50の方の格納
                        hashArray[1] = hash.ToString().Substring(separated + 1);
                        for(int _i = 0; _i < 2; _i++)
                        {
                            //MD5の比較
                            flag = algo.Hash.compairMD5(temp, hashArray[_i]);
                            //一発目から正しければさっさと抜けたほうが効率いいので。
                            if(flag)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        flag = algo.Hash.compairMD5(temp, hash.ToString());
                    }

                    //フラグが正しければ設定に格納
                    if (flag)
                    {
                        //検索した結果のファイル（実行ファイルの場所などの）情報を格納
                        FileInfo Finfo = new FileInfo(temp);
                        //ファイル情報からディレクトリの情報を抜き取る
                        DirectoryInfo Dinfo = Finfo.Directory;
                        WritePrivateProfileString("FilePath", Thxx[i].ToString(), Dinfo.FullName, settingFilePath);
                    }
                }
            }
            MessageBox.Show("アプリケーションを再起動させます", "終了しました");
            //検索ダイアログを閉める
            SC.Close();
            //アプリケーションの再起動
            Application.Restart();
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
        //スパナをクリックしたら
        private void button3_Click(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(button3, new Point(button3.Width / 2, button3.Height / 2));
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

        private void titleName_Click(object sender, EventArgs e)
        {

        }

        


    }
}
