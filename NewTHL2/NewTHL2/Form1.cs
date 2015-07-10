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
using System.Diagnostics;

namespace NewTHL2
{
    public partial class Form1 : Form
    {
        /*
         * 
         * メモ
         * 新作を追加したらパネル関連のものと、FP_switchの配列の枠を増やしたりすること
         * 
         */
        
        #region iniファイルを使いたい
        // iniの読み込み
        [DllImport("kernel32.DLL")]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);
        // iniの書き込み
        [DllImport("kernel32.dll")]
        public static extern uint WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpstring, string lpFileName);
        // iniのkey一覧を取得
        [DllImport("kernel32.DLL",EntryPoint="GetPrivateProfileStringA")]
        public static extern uint GetPrivateProfileStringByByteArray(string lpAppName, string lpKeyName, string lpDefault, byte[] lpReturnedString, uint nSize, string lpFileName);
        #endregion
        //作品用enum
        enum ThXXGames
        {
            alcostg, th06, th07, th075, th08, th09, th095, th10, th105, th11, th12, th123, th125, th128, th13, th135, th14, th143, th145, th15
        }
        ThXXGames[] Thxx = new ThXXGames[20]
        {ThXXGames.alcostg,ThXXGames.th06,ThXXGames.th07,ThXXGames.th075,ThXXGames.th08,ThXXGames.th09,ThXXGames.th095,ThXXGames.th10,ThXXGames.th105,ThXXGames.th11,ThXXGames.th12,
         ThXXGames.th123,ThXXGames.th125,ThXXGames.th128,ThXXGames.th13,ThXXGames.th135,ThXXGames.th14,ThXXGames.th143,ThXXGames.th145,ThXXGames.th15};

        //選択時変更用画像
        //private Bitmap BMP = NewTHL2.Properties.Resources.PBG;
        //設定ファイル格納用フォルダパス
        private const string settingFolderPath = @"resource";
        //設定用iniファイルパス
        private const string settingFilePath = @"resource/settings.ini";
        //ハッシュファイルパスs
        private const string hashFilePath = @"resource/hash.ini";
        //R/W用のエンコード
        public static Encoding sjis = Encoding.GetEncoding("Shift-JIS");
        //ファイルパスが通ってるかどうかの記憶
        private string[] FP_switch = new string[20];
        //今選択しているもの
        private int select = 999;

        //コンストラクタ
        public Form1()
        {
            InitializeComponent();
            //起動時の読み込みダイアログを出す
            NewTHL2.WakeUp WU = new WakeUp();
            WU.Show();
            //イベントハンドラの初期化
            eventHandlerInitialize();
            //起動時初期化設定
            Initialize();
            //ここで起動時ダイアログを殺す
            WU.Dispose();
        }
        //フォームが呼び出される度
        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new System.Drawing.Size(System.Windows.Forms.Screen.GetBounds(this).Width,
                                                       System.Windows.Forms.Screen.GetBounds(this).Height);
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
            //押せるメニューの初期化
            gameSettingSelector();
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
            th145_P.Click += gamePanel_Click;
            th15_P.Click += gamePanel_Click;
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
            th145_L.Click += gamePanel_Click;
            th15_L.Click += gamePanel_Click;
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
            th145_I.Click += gamePanel_Click;
            th15_I.Click += gamePanel_Click;
            alcostg_I.Click += gamePanel_Click;
            #endregion
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
        }
        //パネルのアイコンの初期化
        private void panelIconInitialize()
        {
            string EXE;
            for(int i = 0; i < Thxx.Length;  i++)
            {
                switch(i)
                {
                    case 0:
                        {
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                alcostg_I.Image = algo.GetIcon.returnPanelIcon(EXE, alcostg_I.Width, alcostg_I.Height);
                            }
                            break;
                        }
                    case 1:
                        {
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
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th07_I.Image = algo.GetIcon.returnPanelIcon(EXE, th07_I.Width, th07_I.Height);
                            }
                            break;
                        }
                    case 3:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th075_I.Image = algo.GetIcon.returnPanelIcon(EXE, th075_I.Width, th075_I.Height);
                            }
                            break;
                        }
                    case 4:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th08_I.Image = algo.GetIcon.returnPanelIcon(EXE, th08_I.Width, th08_I.Height);
                            }
                            break;
                        }
                    case 5:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th09_I.Image = algo.GetIcon.returnPanelIcon(EXE, th09_I.Width, th09_I.Height);
                            }
                            break;
                        }
                    case 6:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th095_I.Image = algo.GetIcon.returnPanelIcon(EXE, th095_I.Width, th095_I.Height);
                            }
                            break;
                        }
                    case 7:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th10_I.Image = algo.GetIcon.returnPanelIcon(EXE, th10_I.Width, th10_I.Height);
                            }
                            break;
                        }
                    case 8:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th105_I.Image = algo.GetIcon.returnPanelIcon(EXE, th105_I.Width, th105_I.Height);
                            }
                            break;
                        }
                    case 9:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th11_I.Image = algo.GetIcon.returnPanelIcon(EXE, th11_I.Width, th11_I.Height);
                            }
                            break;
                        }
                    case 10:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th12_I.Image = algo.GetIcon.returnPanelIcon(EXE, th12_I.Width, th12_I.Height);
                            }
                            break;
                        }
                    case 11:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th123_I.Image = algo.GetIcon.returnPanelIcon(EXE, th123_I.Width, th123_I.Height);
                            }
                            break;
                        }
                    case 12:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th125_I.Image = algo.GetIcon.returnPanelIcon(EXE, th125_I.Width, th125_I.Height);
                            }
                            break;
                        }
                    case 13:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th128_I.Image = algo.GetIcon.returnPanelIcon(EXE, th128_I.Width, th128_I.Height);
                            }
                            break;
                        }
                    case 14:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th13_I.Image = algo.GetIcon.returnPanelIcon(EXE, th13_I.Width, th13_I.Height);
                            }
                            break;
                        }
                    case 15:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th135_I.Image = algo.GetIcon.returnPanelIcon(EXE, th135_I.Width, th135_I.Height);
                            }
                            break;
                        }
                    case 16:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th14_I.Image = algo.GetIcon.returnPanelIcon(EXE, th14_I.Width, th14_I.Height);
                            }
                            break;
                        }
                    case 17:
                        {
                            
                            EXE = thxx_EXE(FP_switch[i].ToString(),i);
                            if (File.Exists(EXE))
                            {
                                th143_I.Image = algo.GetIcon.returnPanelIcon(EXE, th143_I.Width, th143_I.Height);
                            }
                            break;
                        }
                    case 18:
                        {
                            EXE = thxx_EXE(FP_switch[i].ToString(), i);
                            if(File.Exists(EXE))
                            {
                                th145_I.Image = algo.GetIcon.returnPanelIcon(EXE, th145_I.Width, th145_I.Height);
                            }
                            break;
                        }
                    case 19:
                        {
                            EXE = thxx_EXE(FP_switch[i].ToString(), i);
                            if(File.Exists(EXE))
                            {
                                th15_I.Image = algo.GetIcon.returnPanelIcon(EXE, th15_I.Width, th15_I.Height);
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
            string[] FilePath = new string[20];
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



        //パネルの配色の初期化(もっといい方法をみつけること)
        private void panelColorInitialize()
        {
            for (int i = 0; i < Thxx.Length; i++)
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
                    case 18:
                        {
                            if(Directory.Exists(FP_switch[i]))
                            {
                                th145_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th145_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    case 19:
                        {
                            if(Directory.Exists(FP_switch[i]))
                            {
                                th15_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th15_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                }
            }
        }

        //複数のイベントハンドラを処理する
        void gamePanel_Click(object sender, EventArgs e)
        {
            string EXE;
            tabPage1.Focus();
            //配色を戻す
            panelColorInitialize();
            //一度パネルを初期化させる
            rightPainIcon.Image = null;

            #region パネル群&ラベル群&イメージ群
            if (sender.Equals(alcostg_P) | sender.Equals(alcostg_L) | sender.Equals(alcostg_I))
            {
                
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
                
                titleName.Text = th11_L.Text;
                th11_P.BackColor = Color.LightPink;
                select = 9;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if (sender.Equals(th12_P) | sender.Equals(th12_L) | sender.Equals(th12_I))
            {
                
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
            if( sender.Equals(th145_P) | sender.Equals(th145_L) | sender.Equals(th145_I))
            {
                titleName.Text = th145_L.Text;
                th145_P.BackColor = Color.LightPink;
                select = 18;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(),select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            if(sender.Equals(th15_P) | sender.Equals(th15_L) | sender.Equals(th15_I))
            {
                titleName.Text = th15_L.Text;
                th15_P.BackColor = Color.LightPink;
                select = 19;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(), select);
                if(File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }
            //コンテクストメニューの設定
            gameSettingSelector();
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
            bool flag = false;

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
                    DialogResult = MessageBox.Show("ハッシュファイルエラー" + Environment.NewLine + "キャンセルする場合は　はい" + Environment.NewLine +
                                    "そのまま、選択したフォルダの中の東方の実行ファイルを元にハッシュ値を取得し、" + Environment.NewLine +
                                    "ハッシュ値テーブルに書き加え、ファイル参照する場合は　いいえ　を押してください", "お知らせ", MessageBoxButtons.YesNo);
                    if (DialogResult == DialogResult.Yes)
                    {
                        return;
                    }
                    else if (DialogResult == DialogResult.No)
                    {
                        //ゲームの実行ファイル名(ファイルパスをくっつけたもの)を取得
                        EXE = thxx_EXE(FP, select);
                        //MD5の取得
                        string md5 = algo.Hash.exportMD5(EXE);
                        //ハッシュテーブルに書き込む
                        WritePrivateProfileString("HASH", Thxx[select].ToString(), md5, hashFilePath);
                        //比較用のハッシュ値を取得するために一度iniファイルからhashの再取得
                        GetPrivateProfileString("HASH", Thxx[select].ToString(), "", hash, Convert.ToUInt32(hash.Capacity), hashFilePath);
                    }
                }
                //ハッシュを比較(thxx_EXEでゲームの実行ファイル名を取得)
                EXE = thxx_EXE(FP, select);
                //EXEファイルが存在するか
                if (!File.Exists(EXE))
                {
                    MessageBox.Show("そこに東方の実行ファイルはありますか？正しい場所を選択してください", "お知らせ");
                    return;
                }

                //ここでハッシュ値が正しいか確認
                if(Thxx[select].ToString() == "th09")
                {
                    //2種のハッシュ格納用
                    string[] hashArray = new string[2];
                    //分割すべき位置を取得
                    int separated = hash.ToString().IndexOf("_");
                    //花映塚Ver1.00の方の格納
                    hashArray[0] = hash.ToString().Substring(0, separated);
                    //花映塚Ver1.50の方の格納
                    hashArray[1] = hash.ToString().Substring(separated + 1);
                    for (int _i = 0; _i < hashArray.Length; _i++)
                    {
                        //MD5の比較
                        flag = algo.Hash.compairMD5(EXE, hashArray[_i]);
                        //一発目から正しければさっさと抜けたほうが効率いいので。
                        if (flag)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    flag = algo.Hash.compairMD5(EXE, hash.ToString());
                }
                
                //選択したものが正しいか
                if (flag == true)
                {
                    //iniファイルに書き込み
                    WritePrivateProfileString("FilePath", Thxx[select].ToString(), FP, settingFilePath);
                    textBox1.Text = FP;

                    //新規追加した場合には初期化式を走らせる
                    //ファイルパスが通ってるかどうかの設定です。
                    filePathInitialize();
                    //パネルの配色の初期化
                    panelColorInitialize();
                    //パネルのアイコンの初期化
                    panelIconInitialize();
                }
                else
                {
                    MessageBox.Show("東方のバージョンが古いか、ハッシュ値表が古いです" + Environment.NewLine +
                                    "バージョンが古い場合は公式サイトでアップデートしてください。" + Environment.NewLine +
                                    "ハッシュ値表が古い場合はメニューからハッシュ値の更新を押してください（要ネット環境）", "お知らせ");
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

        //選択したゲーム毎の設定のDisableとAbleの設定
        private void gameSettingSelector()
        {
            //まず初期化
            バックアップフォルダの設定ToolStripMenuItem1.Enabled = true;
            特殊な設定ToolStripMenuItem1.Enabled = true;
            リプレイのユーザーデータ化ToolStripMenuItem1.Enabled = true;
            ゲームのフォルダを開くToolStripMenuItem1.Enabled = true;
            vpatchの設定ToolStripMenuItem1.Enabled = true;
            ゲームの設定を開くToolStripMenuItem1.Enabled = true;
            adonisの設定ToolStripMenuItem.Enabled = true;
            casterの設定ToolStripMenuItem.Enabled = true;
            updaterの起動ToolStripMenuItem.Enabled = true;
            if(select == 999)
            {
                バックアップフォルダの設定ToolStripMenuItem1.Enabled = false;
                ゲームの設定を開くToolStripMenuItem1.Enabled = false;
                ゲームのフォルダを開くToolStripMenuItem1.Enabled = false;
                ランチャの背景設定ToolStripMenuItem1.Enabled = false;
                特殊な設定ToolStripMenuItem1.Enabled = false;
                adonisの設定ToolStripMenuItem.Enabled = false;
                casterの設定ToolStripMenuItem.Enabled = false;
                updaterの起動ToolStripMenuItem.Enabled = false;
                return;
            }
            if(FP_switch[select] != "")
            {
                バックアップフォルダの設定ToolStripMenuItem1.Enabled = true;
                ゲームの設定を開くToolStripMenuItem1.Enabled = true;
                ゲームのフォルダを開くToolStripMenuItem1.Enabled = true;
            }
            if((Thxx[select].ToString() == "alcostg")|(Thxx[select].ToString() == "th075")|(Thxx[select].ToString() == "th105")|(Thxx[select].ToString() == "th123")|
                (Thxx[select].ToString() == "th135"))
            {
                特殊な設定ToolStripMenuItem1.Enabled = false;
            }
            if(Thxx[select].ToString() == "th10")
            {
                リプレイのユーザーデータ化ToolStripMenuItem1.Enabled = false;
            }
            if ((Thxx[select].ToString() == "th075") | (Thxx[select].ToString() == "th105")|(Thxx[select].ToString() == "th123") | (Thxx[select].ToString() == "th135")|
                (Thxx[select].ToString() == "th14") | (Thxx[select].ToString() == "th143") | (Thxx[select].ToString() == "th145") |(Thxx[select].ToString() == "th15"))
            {
                vpatchの設定ToolStripMenuItem1.Enabled = false;
            }
            if(Thxx[select].ToString() == "th09")
            {
                adonisの設定ToolStripMenuItem.Enabled = true;
            }
            if(Thxx[select].ToString() != "th09")
            {
                adonisの設定ToolStripMenuItem.Enabled = false;
            }
            if ((Thxx[select].ToString() == "th105") | (Thxx[select].ToString() == "th123") | (Thxx[select].ToString() == "th135") | (Thxx[select].ToString() == "th145"))
            {
                ゲームの設定を開くToolStripMenuItem1.Enabled = false;
            }
            if(Thxx[select].ToString() == "th145")
            {
                updaterの起動ToolStripMenuItem.Enabled = true;
            }
            if(Thxx[select].ToString() != "th145")
            {
                updaterの起動ToolStripMenuItem.Enabled = false;
            }
            if(Thxx[select].ToString() == "th075")
            {
                casterの設定ToolStripMenuItem.Enabled = true;
            }
            if(Thxx[select].ToString() != "th075")
            {
                casterの設定ToolStripMenuItem.Enabled = false;
            }
        }

        //custom.exe及びconfig.exe
        private void ゲームの設定を開くToolStripMenuItem1_Click(object sender, EventArgs e)
        {   
            //config及びcustomが存在するか
            if(Thxx[select].ToString() == "th075")
            {
                string config = Path.Combine(FP_switch[select], "config.exe");
                if(File.Exists(config))
                {
                    Process P = new Process();
                    P.StartInfo.FileName = config;
                    P.StartInfo.WorkingDirectory = FP_switch[select];
                    P.Start();
                }
            }
            else
            {
                string custom = Path.Combine(FP_switch[select], "custom.exe");
                if(File.Exists(custom))
                {
                    Process P = new Process();
                    P.StartInfo.FileName = custom;
                    P.StartInfo.WorkingDirectory = FP_switch[select];
                    P.Start();
                }
            }
        }

        //ゲームの起動
        private void button2_Click(object sender, EventArgs e)
        {
            string startEXE = "";
            //Vpatch起動の場合(あとで)
            if(vpatch_Toggle.Checked == true)
            {
                startEXE = Path.Combine(textBox1.Text, "vpatch.exe");
                Process P = new Process();
                P.StartInfo.FileName = startEXE;
                P.StartInfo.WorkingDirectory = FP_switch[select];
                P.Start();
                this.Close();
            }
            //通常起動の場合(あとでバックアップの機能とか追加)
            else
            {
                startEXE = thxx_EXE(textBox1.Text, select);
                Process P = new Process();
                P.StartInfo.FileName = startEXE;
                P.StartInfo.WorkingDirectory = FP_switch[select];
                P.Start();
                this.Close();
            }
        }

        //Vpatchの設定(GUI)
        private void vpatchの設定ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string VpatchIniPath = Path.Combine(FP_switch[select], "vpatch.ini");
            string VpatchExePath = Path.Combine(FP_switch[select], "vpatch.exe");
            
            //vpatch.iniは存在するか？
            if(File.Exists(VpatchIniPath))
            {
                //ここからVpatchの設定を読み取り
                Dictionary<string,string> vpatchValues = algo.IniFileValueReturn.getIniFileValue(VpatchIniPath);

                #region 以前のコード
                /*
                 * 試験的にvpatchのデーターをすべて共通のものにしてみる
                 * 
                 */


                /*
                if(Thxx[select].ToString() == "th12")
                {

                }
                else if(Thxx[select].ToString() == "th125")
                {
                    //要素が同じかどうかを確かめる(ダブスポ)
                    
                    //Dictionary<string, int> vpatchValues_DS = algo.vpatchValueReturn.getVpatchValue(uri);

                    //要素数が同じでなければオリジナルのものにする
                    //if(vpatchValues.Count != vpatchValues_DS.Count)
                    //{
                    //    algo.VpatchValueWrite.vpatchIniWrite(VpatchIniPath, NewTHL2.Properties.Resources.vpatch_th125);
                    //}
                }
                else if(Thxx[select].ToString() == "th13")
                {
                    Dictionary<string, int> vpatchValues_TenDesire = algo.vpatchValueReturn.getVpatchValue(NewTHL2.Properties.Resources.vpatch_th13);
                    if(vpatchValues.Count != vpatchValues_TenDesire.Count)
                    {
                        algo.VpatchValueWrite.vpatchIniWrite(VpatchIniPath, NewTHL2.Properties.Resources.vpatch_th13);
                    }
                }
                 */
                #endregion

                //VpatchGUIを参照
                VpatchGUI VGUI = new VpatchGUI();
                 
                //ここで設定のイニシャライズをする
                VGUI.setValues(vpatchValues,Thxx[select].ToString(),VpatchIniPath);

                VGUI.ShowDialog();
            }
            //存在しなければ
            else
            {
                if(File.Exists(VpatchExePath))
                {
                    DialogResult = MessageBox.Show("vpatch.iniが存在しない為、設定が使えません" + Environment.NewLine + "が vpatch.exeは存在している模様ですので"
                        + Environment.NewLine + "vpatch.ini　を作成することで利用できますが、作成しますか？（デフォルトの設定での作成になります）", "お知らせ", MessageBoxButtons.YesNo);
                    if(DialogResult == DialogResult.Yes)
                    {
                        //ここで作成
                        NewTHL2.algo.FileCopy.makeVpatchIni(VpatchIniPath/*,Thxx[select].ToString()*/);
                        MessageBox.Show("vpatch.iniを作成しました", "お知らせ");
                        
                    }
                    else
                    {
                        MessageBox.Show("作成をキャンセルしました", "お知らせ");
                    }
                }
                else
                {
                    MessageBox.Show("Vpatch.iniは存在しますが、Vpatch.exeが存在しません・・・（どういうことなの）", "お知らせ");
                }
                
            }
        }

        //Vpatchファイルの初期化（引き継ぎあり）
        private void vpatchファイルのToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("ファイルパスが通っているゲームでVpatch.iniが存在するゲームのVpatch.iniを初期化をします。"+ Environment.NewLine +
                            "よろしいですか？"+Environment.NewLine+
                            "※ もともとある設定値は引き継ぎ、共通化されたVpatch.iniファイルにしますのでご安心ください ※", "おしらせ", MessageBoxButtons.YesNo);
            if(DialogResult == DialogResult.No)
            {
                MessageBox.Show("キャンセルされました","おしらせ");
                return;
            }
            else if(DialogResult == DialogResult.Yes)
            {
                for(int i = 0; i < FP_switch.Length; i++)
                {
                    //Vpatch.iniのファイルパスを参照する
                    string VpatchIniPath = Path.Combine(FP_switch[i], "vpatch.ini");
                    if(File.Exists(VpatchIniPath))
                    {
                        //ここからVpatchの設定を読み取り
                        Dictionary<string, string> vpatchValues = algo.IniFileValueReturn.getIniFileValue(VpatchIniPath);
                        //ここで共通化したもので上書き
                        algo.FileCopy.makeVpatchIni(VpatchIniPath);
                        //ここで引き継ぎ
                        algo.IniFileValueWrite.IniFileWriter(vpatchValues, VpatchIniPath);
                    }
                    //存在しない場合は作る
                    else
                    {
                        //そもそもVpatchがない者達は除外した上で
                        if((Thxx[i].ToString() != "th075") & (Thxx[i].ToString() != "th105") & (Thxx[i].ToString() != "th123") &
                           (Thxx[i].ToString() != "th135") & (Thxx[i].ToString() != "th14") & (Thxx[i].ToString() != "th143") &
                           (Thxx[i].ToString() != "th145") & (Thxx[i].ToString() != "th15"))
                        {
                            //ここでファイルを作成する
                            algo.FileCopy.makeVpatchIni(VpatchIniPath);
                        }
                    }
                }
            }
        }

        //フォルダを開く
        private void ゲームのフォルダを開くToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //フォルダが存在すれば開く
            if(Directory.Exists(FP_switch[select]))
            {
                Process.Start(FP_switch[select]);
            }
            else
            {
                MessageBox.Show("ゲームのフォルダが存在しないようです。" + Environment.NewLine + "設定を再確認してください", "確認して下さい");
            }
        }

        //バックアップフォルダの設定
        private void バックアップフォルダの設定ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //セクションを用意
            string backupSection = Thxx[select].ToString() + "BU";
            //大文字化
            backupSection.ToUpper();
            //keyとvalueを取得
            Dictionary<string,string> settingsFileIni = NewTHL2.algo.IniFileValueReturn.getSettingsFileValue(settingFilePath, backupSection);

            //バックアップクラスをインスタンス化
            Backup BU = new Backup();
            //初期化
            BU.initialize(settingsFileIni, Thxx[select].ToString(),settingFilePath);
            //モーダルウィンドウで表示
            BU.ShowDialog();
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
            //キャンセル処理（ここまずいのであとで変える）
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
            //数に合わせてプログレスバーの最大値を変えとく
            SC.progressBar1.Maximum = Thxx.Length;
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
                    if (Thxx[i].ToString() == "th09")
                    {
                        //2種のハッシュ格納用
                        string[] hashArray = new string[2];
                        //分割すべき位置を取得
                        int separated = hash.ToString().IndexOf("_");
                        //花映塚Ver1.00の方の格納
                        hashArray[0] = hash.ToString().Substring(0, separated);
                        //花映塚Ver1.50の方の格納
                        hashArray[1] = hash.ToString().Substring(separated + 1);
                        for(int _i = 0; _i < hashArray.Length; _i++)
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
        
        //新作ができたとき用のファイルパス設定表の更新
        private void ファイルパスの設定の更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("設定ファイルを新しい物に引き継ぎます。よろしいですか？", "お知らせ", MessageBoxButtons.YesNo);
            if(DialogResult == DialogResult.Yes)
            {
                Dictionary<string, string> FilePathListSource = algo.IniFileValueReturn.getIniFileValue(settingFilePath);
                StreamWriter SW = new StreamWriter(settingFilePath, false,sjis);
                SW.WriteLine(NewTHL2.Properties.Resources.setteingTemplate);
                SW.Close();
                algo.IniFileValueWrite.IniFileWriter(FilePathListSource, settingFilePath);
                MessageBox.Show("完了しました", "お知らせ");
            }
            else
            {
                MessageBox.Show("キャセンルされました", "お知らせ");
                return;
            }
            
        }
        //th145のみ、アップデーターの起動
        private void updaterの起動ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UpdaterPath = Path.Combine(textBox1.Text,"updater.exe");
            if (File.Exists(UpdaterPath))
            {
                Process P = new Process();
                P.StartInfo.FileName = UpdaterPath;
                P.StartInfo.WorkingDirectory = FP_switch[select];
                P.Start();
            }
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
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(pictureBox1, new Point(pictureBox1.Width / 2, pictureBox1.Height / 2));
        }
        void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = NewTHL2.Properties.Resources.spana;
        }

        void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = NewTHL2.Properties.Resources.spana_click;
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
        //ヘルプ
        private void 困ったことがあったらToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTHL2.Help HP = new Help();
            HP.Show();
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }



        




    }
}
