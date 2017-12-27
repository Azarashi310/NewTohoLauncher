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
        //作品用enum（あくまで作品名を返す用）
        enum ThXXGames
        {
            alcostg, th06, th07, th075, th08, th09, th095, th10, th105, th11, th12, th123, th125, th128, th13, th135, th14, th143, th145, th15, th16
        }
        ThXXGames[] Thxx = new ThXXGames[21]
        {ThXXGames.alcostg,ThXXGames.th06,ThXXGames.th07,ThXXGames.th075,ThXXGames.th08,ThXXGames.th09,ThXXGames.th095,ThXXGames.th10,ThXXGames.th105,ThXXGames.th11,ThXXGames.th12,
         ThXXGames.th123,ThXXGames.th125,ThXXGames.th128,ThXXGames.th13,ThXXGames.th135,ThXXGames.th14,ThXXGames.th143,ThXXGames.th145,ThXXGames.th15,ThXXGames.th16};

        //作品名から選択番号を取りたい場合
        private Dictionary<string, int> thxxGamesNumbers = new Dictionary<string, int>();

        //選択時変更用画像
        //private Bitmap BMP = NewTHL2.Properties.Resources.PBG;
        //設定ファイル格納用フォルダパス
        private const string settingFolderPath = @"./resource";
        //設定用iniファイルパス
        private const string settingFilePath = @"./resource/settings.ini";
        //ハッシュファイルパスs
        private const string hashFilePath = @"./resource/hash.ini";
        //R/W用のエンコード
        public static Encoding sjis = Encoding.GetEncoding("Shift-JIS");
        //ファイルパスが通ってるかどうかの記憶
        private string[] FP_switch = new string[21];
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
            //フォルダが無ければ作る
            if (!Directory.Exists(settingFolderPath))
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


            //ファイルパスが通ってるかどうかの設定です。
            filePathInitialize();
            //パネルの配色の初期化
            panelColorInitialize();
            //パネルのアイコンの初期化
            panelIconInitialize();
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
            th16_P.Click += gamePanel_Click;
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
            th16_L.Click += gamePanel_Click;
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
            th16_I.Click += gamePanel_Click;
            alcostg_I.Click += gamePanel_Click;
            #endregion
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            //D&D許可
            textBox1.AllowDrop = true;
            //D&D処理
            textBox1.DragEnter += textBox1_DragEnter;
            textBox1.DragDrop += textBox1_DragDrop;
        }

        //D&Dに入る処理
        void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        //ドロップ完了処理
        void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            //ファイルが渡されていない場合
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }
            else
            {
                StringBuilder THhash = new StringBuilder(1024);
                string[] THEXE;
                string thexeName;
                foreach(var FilePath in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    //ハッシュ値の取得
                    GetPrivateProfileString("HASH", Thxx[select].ToString(), "", THhash, Convert.ToUInt32(THhash.Capacity), hashFilePath);
                    //検索するEXEファイルの種別
                    if(Thxx[select].ToString() == "th06")
                    {
                        thexeName = "東方紅魔郷.exe";
                    }
                    else
                    {
                        thexeName = Thxx[select].ToString() + ".exe";
                    }
                    //exeファイルを掘削して検索
                    THEXE = System.IO.Directory.GetFiles(FilePath, thexeName, SearchOption.AllDirectories);
                    foreach(string exes in THEXE)
                    {
                        bool flag = NewTHL2.algo.Hash.compairMD5(exes, THhash.ToString());
                        //ファイルが正しかった場合
                        if(flag)
                        {
                            FileInfo Finfo = new FileInfo(exes);
                            DirectoryInfo Dinfo = Finfo.Directory;
                            textBox1.Text = Dinfo.FullName;

                            //iniファイルに書き込み
                            WritePrivateProfileString("FilePath", Thxx[select].ToString(), Dinfo.FullName, settingFilePath);

                            //新規追加した場合には初期化式を走らせる
                            //ファイルパスが通ってるかどうかの設定です。
                            filePathInitialize();
                            //パネルの配色の初期化
                            panelColorInitialize();
                            //パネルのアイコンの初期化
                            panelIconInitialize();

                        }
                    }

                }
            }
        }

        #region 初期化
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
                    case 20:
                        {
                            EXE = thxx_EXE(FP_switch[i].ToString(), i);
                            if (File.Exists(EXE))
                            {
                                th16_I.Image = algo.GetIcon.returnPanelIcon(EXE, th16_I.Width, th16_I.Height);
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
            string[] FilePath = new string[21];
            for (int i = 0; i < Thxx.Length; i++)
            {
                //設定ファイルから参照
                GetPrivateProfileString("FilePath", Thxx[i].ToString(), "", FP, Convert.ToUInt32(FP.Capacity), settingFilePath);
                //ここでついでにthxxgamesNumbersに番号を入れておく
                thxxGamesNumbers.Add(Thxx[i].ToString(), i);
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
                    case 20:
                        {
                            if (Directory.Exists(FP_switch[i]))
                            {
                                th16_P.BackColor = Color.Transparent;
                            }
                            else
                            {
                                th16_P.BackColor = Color.LightGray;
                            }
                            break;
                        }
                }
            }
        }
        #endregion
        //複数のイベントハンドラを処理する（左のメニューをクリックした場合のやつ）
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
            if (sender.Equals(th16_P) | sender.Equals(th16_L) | sender.Equals(th16_I))
            {
                titleName.Text = th16_L.Text;
                th16_P.BackColor = Color.LightPink;
                select = 20;
                textBox1.Text = FP_switch[select].ToString();
                EXE = thxx_EXE(FP_switch[select].ToString(), select);
                if (File.Exists(EXE))
                {
                    rightPainIcon.Image = algo.GetIcon.returnRightPainIcon(EXE, rightPainIcon.Width, rightPainIcon.Height);
                }
            }

            //Vpatchのチェックボックスの設定
            if((Thxx[select].ToString() == "th06") || (Thxx[select].ToString() == "th07") || (Thxx[select].ToString() == "th08") || (Thxx[select].ToString() == "th09")
                ||(Thxx[select].ToString() == "th095") || (Thxx[select].ToString() == "th10") || (Thxx[select].ToString() == "th11") || (Thxx[select].ToString() == "th12")
                ||(Thxx[select].ToString() == "th125") || (Thxx[select].ToString() == "th128") || (Thxx[select].ToString() == "th13") || (Thxx[select].ToString() == "alcostg"))
            {
                //vpatchのトグルスイッチの有効化
                vpatch_Toggle.Enabled = true;
                //setting.iniのkeyとvalueを取得
                Dictionary<string, string> settingIniValue = algo.IniFileValueReturn.getIniFileValue(settingFilePath);
                if(settingIniValue[Thxx[select].ToString() + "_V"] == "True")
                {
                    vpatch_Toggle.Checked = true;
                }
                else
                {
                    vpatch_Toggle.Checked = false;
                }
            }
            else
            {
                vpatch_Toggle.Enabled = false;
                vpatch_Toggle.Checked = false;
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
            if (FP == "")
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
            //まず初期化(可視化不可視化をちょいと搭載してみる)
            バックアップフォルダの設定ToolStripMenuItem1.Visible = true;
            バックアップフォルダの設定ToolStripMenuItem1.Enabled = true;
            特殊な設定ToolStripMenuItem1.Visible = true;
            特殊な設定ToolStripMenuItem1.Enabled = true;
            リプレイのユーザーデータ化ToolStripMenuItem1.Visible = true;
            リプレイのユーザーデータ化ToolStripMenuItem1.Enabled = true;
            ゲームのフォルダを開くToolStripMenuItem1.Visible = true;
            ゲームのフォルダを開くToolStripMenuItem1.Enabled = true;
            vpatchの設定ToolStripMenuItem1.Visible = true;
            vpatchの設定ToolStripMenuItem1.Enabled = true;
            ゲームの設定を開くToolStripMenuItem1.Visible = true;
            ゲームの設定を開くToolStripMenuItem1.Enabled = true;
            adonisの設定ToolStripMenuItem.Visible = true;
            adonisの設定ToolStripMenuItem.Enabled = true;
            casterの設定ToolStripMenuItem.Visible = true;
            casterの設定ToolStripMenuItem.Enabled = true;
            updaterの起動ToolStripMenuItem.Visible = true;
            updaterの起動ToolStripMenuItem.Enabled = true;
            セーブデータ等の場所を開くth125以降ToolStripMenuItem.Visible = true;
            セーブデータ等の場所を開くth125以降ToolStripMenuItem.Enabled = true;
            if(select == 999)
            {
                バックアップフォルダの設定ToolStripMenuItem1.Visible = false;
                バックアップフォルダの設定ToolStripMenuItem1.Enabled = false;
                ゲームの設定を開くToolStripMenuItem1.Visible = false;
                ゲームの設定を開くToolStripMenuItem1.Enabled = false;
                ゲームのフォルダを開くToolStripMenuItem1.Visible = false;
                ゲームのフォルダを開くToolStripMenuItem1.Enabled = false;
                ランチャの背景設定ToolStripMenuItem1.Visible = false;
                ランチャの背景設定ToolStripMenuItem1.Enabled = false;
                特殊な設定ToolStripMenuItem1.Visible = false;
                特殊な設定ToolStripMenuItem1.Enabled = false;
                adonisの設定ToolStripMenuItem.Visible = false;
                adonisの設定ToolStripMenuItem.Enabled = false;
                casterの設定ToolStripMenuItem.Visible = false;
                casterの設定ToolStripMenuItem.Enabled = false;
                updaterの起動ToolStripMenuItem.Visible = false;
                updaterの起動ToolStripMenuItem.Enabled = false;
                セーブデータ等の場所を開くth125以降ToolStripMenuItem.Visible = false;
                セーブデータ等の場所を開くth125以降ToolStripMenuItem.Enabled = false;
                return;
            }
            if(FP_switch[select] != "")
            {
                バックアップフォルダの設定ToolStripMenuItem1.Visible = true;
                バックアップフォルダの設定ToolStripMenuItem1.Enabled = true;
                ゲームの設定を開くToolStripMenuItem1.Visible = true;
                ゲームの設定を開くToolStripMenuItem1.Enabled = true;
                ゲームのフォルダを開くToolStripMenuItem1.Visible = true;
                ゲームのフォルダを開くToolStripMenuItem1.Enabled = true;
            }
            //特殊な設定に関わりがないもの
            if((Thxx[select].ToString() == "th075")|(Thxx[select].ToString() == "th105")|(Thxx[select].ToString() == "th123")|
                (Thxx[select].ToString() == "th135"))
            {
                特殊な設定ToolStripMenuItem1.Visible = false;
                特殊な設定ToolStripMenuItem1.Enabled = false;
            }
            //リプレイのユーザーデータ化
            if(Thxx[select].ToString() == "th10")
            {
                リプレイのユーザーデータ化ToolStripMenuItem1.Visible = false;
                リプレイのユーザーデータ化ToolStripMenuItem1.Enabled = false;
            }
            //Vpatchの設定
            if ((Thxx[select].ToString() == "th075") | (Thxx[select].ToString() == "th105")|(Thxx[select].ToString() == "th123") | (Thxx[select].ToString() == "th135")|
                (Thxx[select].ToString() == "th14") | (Thxx[select].ToString() == "th143") | (Thxx[select].ToString() == "th145") | (Thxx[select].ToString() == "th15") | (Thxx[select].ToString() == "th16"))
            {
                vpatchの設定ToolStripMenuItem1.Visible = false;
                vpatchの設定ToolStripMenuItem1.Enabled = false;
            }
            //Adonisの設定
            if(Thxx[select].ToString() == "th09")
            {
                adonisの設定ToolStripMenuItem.Visible = true;
                adonisの設定ToolStripMenuItem.Enabled = true;
            }
            if(Thxx[select].ToString() != "th09")
            {
                adonisの設定ToolStripMenuItem.Visible = false;
                adonisの設定ToolStripMenuItem.Enabled = false;
            }
            //ゲームの設定を開く
            if ((Thxx[select].ToString() == "th105") | (Thxx[select].ToString() == "th123") | (Thxx[select].ToString() == "th135") | (Thxx[select].ToString() == "th145"))
            {
                ゲームの設定を開くToolStripMenuItem1.Visible = false;
                ゲームの設定を開くToolStripMenuItem1.Enabled = false;
            }
            //アップデーターがあるやつ
            if(Thxx[select].ToString() == "th145")
            {
                updaterの起動ToolStripMenuItem.Visible = true;
                updaterの起動ToolStripMenuItem.Enabled = true;
            }
            if(Thxx[select].ToString() != "th145")
            {
                updaterの起動ToolStripMenuItem.Visible = false;
                updaterの起動ToolStripMenuItem.Enabled = false;
            }
            //casterの設定（萃夢想）
            if(Thxx[select].ToString() == "th075")
            {
                casterの設定ToolStripMenuItem.Visible = true;
                casterの設定ToolStripMenuItem.Enabled = true;
            }
            if(Thxx[select].ToString() != "th075")
            {
                casterの設定ToolStripMenuItem.Visible = false;
                casterの設定ToolStripMenuItem.Enabled = false;
            }
            //セーブデータが特殊な場所のやつ
            if((Thxx[select].ToString() == "th125")||(Thxx[select].ToString() == "th128")||(Thxx[select].ToString() == "th13")||(Thxx[select].ToString() == "th14")
                || (Thxx[select].ToString() == "th143") || (Thxx[select].ToString() == "th15") || (Thxx[select].ToString() == "th16"))
            {
                セーブデータ等の場所を開くth125以降ToolStripMenuItem.Visible = true;
                セーブデータ等の場所を開くth125以降ToolStripMenuItem.Enabled = true;
            }
            else
            {
                セーブデータ等の場所を開くth125以降ToolStripMenuItem.Visible = false;
                セーブデータ等の場所を開くth125以降ToolStripMenuItem.Enabled = false;
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

        //Vpatchを使うかどうかのチェックボックス
        private void vpatch_Toggle_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                vpatch_Toggle.Checked = false;
                return;
            }
            Dictionary<string, string> runVpatch = new Dictionary<string, string>();
            if(vpatch_Toggle.Checked)
            {
                runVpatch.Add(Thxx[select].ToString() + "_V", "True");
                algo.IniFileValueWrite.IniFileWriter(runVpatch, settingFilePath);
            }
            else
            {
                runVpatch.Add(Thxx[select].ToString() + "_V", "False");
                algo.IniFileValueWrite.IniFileWriter(runVpatch, settingFilePath);
            }
        }

        //ゲームの起動準備
        private void button2_Click(object sender, EventArgs e)
        {
            string startEXE = "";
            if(vpatch_Toggle.Checked)
            {
                startEXE = Path.Combine(textBox1.Text, "vpatch.exe");
            }
            else
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("何かゲームを選択してから押してください。", "お知らせ");
                    return;
                }
                else
                {
                    startEXE = thxx_EXE(textBox1.Text, select);
                }
            }
            
            //ゲームのバックアップ
            gamebackup();
            
            if(Thxx[select].ToString() == "th123")
            {
                th123_Inisetting();
            }

            //ゲームを起動する
            bool runBool = run(startEXE);
            if(runBool)
            {
                this.Close();
            }
        }

        //非想天則のみの特殊設定
        private void th123_Inisetting()
        {
            string configexFilePath = Path.Combine(FP_switch[select],"configex123.ini");
            string th105FilePath = FP_switch[thxxGamesNumbers["th105"]];

            Dictionary<string, string> iniContents = algo.IniFileValueReturn.getIniFileValue(configexFilePath);
            //configex.iniにパスがないか、パスのディレクトリが存在しなければ
            if ((iniContents["path"] == "") || (!Directory.Exists(iniContents["path"])))
            {
                iniContents["path"] = th105FilePath;
                algo.IniFileValueWrite.IniFileWriter(iniContents, configexFilePath);
                MessageBox.Show("非想天則においての、緋想天の引き継ぎのファイルパスを設定しました" + Environment.NewLine +
                                    "変更したパス : " + FP_switch[thxxGamesNumbers["th105"]]);
            }

        }

        //ゲームの起動
        private bool run(string exePath)
        {
            Process P = new Process();
            P.StartInfo.FileName = exePath;
            P.StartInfo.WorkingDirectory = FP_switch[select];
            try
            {
                P.Start();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return false;
            }
            return true;
        }

        //ゲーム起動時のバックアップ処理
        private void gamebackup()
        {
            //バックアップファイルパス
            string backupFilePath = "";
            //元のパス
            string sourcePath = "";

            //体験版がある場合のみの処理（黄昏作品以外）
            string trial = ""; 
            //紅魔郷のみ
            if(Thxx[select].ToString() == "th06")
            {
                trial = Path.Combine(FP_switch[select], "taiken.txt");
            }
            //紅魔郷以外
            else
            {
                trial = Path.Combine(FP_switch[select], Thxx[select].ToString() + "tr.dat");
            }

            //setting.iniから情報を取得
            Dictionary<string, string> settingIniValue = algo.IniFileValueReturn.getIniFileSectionValue(settingFilePath, Thxx[select].ToString().ToUpper() + "BU");
            
            //keyを取得
            Dictionary<string, string>.KeyCollection keys = settingIniValue.Keys;
            foreach(string key in keys)
            {
                //キーをアンダースコアで分割
                string splitKey = key.Remove(0, key.IndexOf('_') + 1);
                #region Save
                //セーブ
                if(splitKey == "Save")
                {
                    /*
                     * セーブデータのファイルの場所の指定をする
                     */
                    //score.dat　のみのもの
                    if ((Thxx[select].ToString() == "th06") || (Thxx[select].ToString() == "th07") || (Thxx[select].ToString() == "th07") || 
                        (Thxx[select].ToString() == "th08") || (Thxx[select].ToString() == "th09") || (Thxx[select].ToString() == "th105") ||
                        (Thxx[select].ToString() == "th135") || (Thxx[select].ToString() == "th145"))

                    {
                        sourcePath = Path.Combine(FP_switch[select], "score.dat");
                    }
                    //ファイルの場所がroamingの奴
                    else if((Thxx[select].ToString() == "th125") || (Thxx[select].ToString() == "th128") || (Thxx[select].ToString() == "th13") ||
                        (Thxx[select].ToString() == "th14") || (Thxx[select].ToString() == "th143") || (Thxx[select].ToString() == "th15"))
                    {
                        //体験版がある場合
                        if (File.Exists(trial))
                        {
                            //体験版のみtr
                            sourcePath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ShanghaiAlice", 
                                Thxx[select].ToString() + "tr", "score" + Thxx[select] + ".dat");
                        }
                        else
                        {
                            sourcePath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ShanghaiAlice",
                                Thxx[select].ToString(), "score" + Thxx[select] + ".dat");
                        }
                    }
                    //作品名 + score.dat の場合
                    else
                    {
                        //score123.datのようにthがないもの用
                        if(Thxx[select].ToString() == "th123")
                        {
                            sourcePath = Path.Combine(FP_switch[select], "score" + Thxx[select].ToString().Remove(0,2) + ".dat");
                        }
                        else
                        {
                            sourcePath = Path.Combine(FP_switch[select], Thxx[select] + "score.dat");
                        }
                    }

                    //ソースファイルはあるか？
                    if (File.Exists(sourcePath))
                    {
                        //バックアップパスを代入
                        backupFilePath = settingIniValue[key];
                    }

                    //バックアップフォルダはあるか
                    if (Directory.Exists(backupFilePath))
                    {
                        //ここでバックアップファイルパスにファイルの名前と日付のスタンプ
                        backupFilePath = Path.Combine(backupFilePath, DateTime.Now.ToString("yyyy年MM月dd日") + "_" + DateTime.Now.ToString("HH時mm分ss秒") + "_" + 
                            Path.GetFileName(sourcePath));
                        //ファイルをバックアップ
                        File.Copy(sourcePath, backupFilePath);
                    }
                    else
                    {
                        if(backupFilePath != "")
                        {
                            backupFilePath = algo.FileManage.backupFolderCheck(backupFilePath);
                            //ファイルをバックアップ
                            File.Copy(sourcePath, backupFilePath);
                        }                        
                    }
                }
                #endregion
                #region autoSave
                //東方紺珠伝の自動セーブ
                else if(splitKey == "AutoSave")
                {
                    //体験版だけ tr つける（製品版出たら変える）
                    if(File.Exists(trial))
                    {
                        sourcePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ShanghaiAlice",
                            Thxx[select].ToString() + "tr", "autosave");
                    }
                    //体験版じゃない場合
                    else
                    {
                        sourcePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ShanghaiAlice",
                            Thxx[select].ToString(), "autosave");
                    }
                    
                    //まず先にバックアップパスにフォルダをつくる

                    //ソースパスが存在するか
                    if(Directory.Exists(sourcePath))
                    {
                        //バックアップパスを代入
                        backupFilePath = settingIniValue[key];
                        //バックアップフォルダの確認
                        if (backupFilePath != "") backupFilePath = algo.FileManage.backupFolderCheck(backupFilePath);
                        else return;

                        //ソースフォルダのファイル一覧を取得
                        string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
                        //ファイルを一つづつ処理
                        foreach (string file in files)
                        {
                            //拡張子のついていないファイル名
                            string fileWithoutExt = Path.GetFileNameWithoutExtension(file);
                            //上記の文字列を分割
                            string[] strArray = fileWithoutExt.Split('_');
                            //自動セーブのフォルダパス
                            string autoSaveFolderPath = "";
                            
                            //ファイルによって変える
                            switch(strArray[0])
                            {
                                //霊夢
                                case "save0":
                                    {
                                        //パスの結合
                                        autoSaveFolderPath = Path.Combine(backupFilePath, "霊夢");
                                        break;
                                    }
                                //魔理沙
                                case "save1":
                                    {
                                        autoSaveFolderPath = Path.Combine(backupFilePath, "魔理沙");
                                        break;
                                    }
                                //早苗
                                case "save2":
                                    {
                                        autoSaveFolderPath = Path.Combine(backupFilePath, "早苗");
                                        break;
                                    }
                                //優曇華
                                case "save3":
                                    {
                                        autoSaveFolderPath = Path.Combine(backupFilePath, "優曇華");
                                        break;
                                    }
                            }

                            //体験版はフォルダにtrをつける
                            if(File.Exists(trial))
                            {
                                autoSaveFolderPath = autoSaveFolderPath + "tr";
                            }

                            //フォルダがなければ
                            if (!Directory.Exists(autoSaveFolderPath))
                            {
                                Directory.CreateDirectory(autoSaveFolderPath);
                            }
                            //ファイルをパスに含めてコピーする下準備
                            string autoSaveBackupFilePath = Path.Combine(autoSaveFolderPath, DateTime.Now.ToString("HH時mm分ss秒") + "_" + Path.GetFileName(file));
                            //コピーする
                            File.Copy(file, autoSaveBackupFilePath);
                        }
                    }
                }
                #endregion
                #region macro
                //東方深秘録のマクロ
                else if(splitKey == "Macro")
                {
                    //マクロフォルダの取得
                    sourcePath = Path.Combine(FP_switch[select], "macro");

                    //ソースパスがあるかの確認
                    if(Directory.Exists(sourcePath))
                    {
                        //バックアップフォルダの取得
                        backupFilePath = settingIniValue[key];
                        //存在の確認
                        if (backupFilePath != "") backupFilePath = algo.FileManage.backupFolderCheck(backupFilePath);
                        else return;

                        //ソースフォルダのファイル一覧を取得
                        string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);

                        //foreachの中で使う準備
                        string fileWithoutExt;
                        string[] strArray;
                        string macroFolderPath;
                        
                        //ファイルを一つづつ処理
                        foreach (string file in files)
                        {
                            //拡張子のついていないファイル名
                            fileWithoutExt = Path.GetFileNameWithoutExtension(file);
                            //上記の文字列を分割
                            strArray = fileWithoutExt.Split('_');
                            //自動セーブのフォルダパス
                            macroFolderPath = Path.Combine(backupFilePath,strArray[0]);

                            //フォルダがなければ
                            if (!Directory.Exists(macroFolderPath))
                            {
                                Directory.CreateDirectory(macroFolderPath);
                            }
                            //ファイルをパスに含めてコピーする下準備
                            string autoSaveBackupFilePath = Path.Combine(macroFolderPath, DateTime.Now.ToString("HH時mm分ss秒") + "_" + Path.GetFileName(file));
                            //コピーする
                            File.Copy(file, autoSaveBackupFilePath);
                        }
                    }

                }
                #endregion
                /*
                * 
                * 
                * フォルダのバックアップだけですむ奴
                * 
                */
                #region フォルダ作成系
                else
                {
                    #region snapshot
                    //スナップショット
                    if(splitKey == "SnapShot")
                    {
                        //ソースパスの設定
                        if ((Thxx[select].ToString() == "th123") || (Thxx[select].ToString() == "th125") || (Thxx[select].ToString() == "th128") ||
                            (Thxx[select].ToString() == "th13") || (Thxx[select].ToString() == "th14") || (Thxx[select].ToString() == "th143") ||
                             (Thxx[select].ToString() == "th15"))
                        {
                            if(File.Exists(trial))
                            {
                                sourcePath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    "ShanghaiAlice", Thxx[select].ToString() + "tr", "snapshot");
                            }
                            else
                            {
                                sourcePath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    "ShanghaiAlice", Thxx[select].ToString(), "snapshot");
                            }
                        }
                        //東方深秘録のみ
                        else if(Thxx[select].ToString() == "th145")
                        {
                            sourcePath = Path.Combine(FP_switch[select], "ss");
                        }
                        else
                        {
                            sourcePath = Path.Combine(FP_switch[select], "snapshot");
                        }

                    }
                    #endregion
                    #region replay
                    //リプレイ
                    if(splitKey == "Replay")
                    {
                        //ソースパスの設定(ローミングの場合)
                        if ((Thxx[select].ToString() == "th125") || (Thxx[select].ToString() == "th128") ||
                            (Thxx[select].ToString() == "th13") || (Thxx[select].ToString() == "th14") || (Thxx[select].ToString() == "th143") ||
                            (Thxx[select].ToString() == "th15"))
                        {
                            if(File.Exists(trial))
                            {
                                sourcePath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    "ShanghaiAlice", Thxx[select].ToString() + "tr", "replay");
                            }
                            else
                            {
                                sourcePath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    "ShanghaiAlice", Thxx[select].ToString(), "replay");
                            }
                        }
                        //ローミングではなくカレントの場合
                        else
                        {
                            if(File.Exists(trial))
                            {
                                sourcePath = Path.Combine(FP_switch[select], "replay");
                            }
                            else
                            {
                                sourcePath = Path.Combine(FP_switch[select], "replay");
                            }
                        }
                    }
#endregion
                    #region hint
                    //ヒント
                    if(splitKey == "Hint")
                    {
                        sourcePath = Path.Combine(FP_switch[select], "hint");
                    }
                    #endregion
                    #region profile
                    //プロフィール
                    if(splitKey == "Profile")
                    {
                        sourcePath = Path.Combine(FP_switch[select], "profile");
                    }
                    #endregion
                    #region icon
                    //アイコン
                    if(splitKey == "Icon")
                    {
                        sourcePath = Path.Combine(FP_switch[select], "icon");
                    }
                    #endregion
                    #region okubi
                    //御首頂戴帳
                    if(splitKey == "Okubi")
                    {
                        sourcePath = Path.Combine(FP_switch[select], "御首頂戴帳");
                    }
                    #endregion
                    #region bestshot
                    //ベストショット
                    if(splitKey == "BestShot")
                    {
                        if(Thxx[select].ToString() == "th125")
                        {
                            if(File.Exists(trial))
                            {
                                sourcePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    "ShanghaiAlice", Thxx[select].ToString() + "tr", "bestshot");
                            }
                            else
                            {
                                sourcePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    "ShanghaiAlice", Thxx[select].ToString(), "bestshot");
                            }
                        }
                        else
                        {
                            sourcePath = Path.Combine(FP_switch[select], "bestshot");
                        }
                    }
                    #endregion
                    //フォルダの存在や、バックアップパスの指定などをやってもらう
                    if (settingIniValue[key] != "") algo.FileManage.backupManageandFolderCopy(sourcePath, settingIniValue[key]);
                    else return;
                }
                #endregion
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

        //ダブルスポイラー以降のセーブデータフォルダを開く
        private void セーブデータ等の場所を開くth125以降ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //特殊フォルダのフォルダパスを取得
            string folderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //ShanghaiAliceパスを結合、作品の名前を結合
            folderPath = Path.Combine(folderPath, "ShanghaiAlice", Thxx[select].ToString());
            if(Directory.Exists(folderPath))
            {
                Process.Start(folderPath);
            }
            else
            {
                MessageBox.Show("一度でも起動しましたか？してない場合フォルダが作成されていません。", "フォルダがないです");
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
            Dictionary<string,string> settingsFileIni = NewTHL2.algo.IniFileValueReturn.getIniFileSectionValue(settingFilePath, backupSection);

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
            //キャンセル処理（ここまずいのであとで変える）
            if (FP == "")
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
                return;
            }
            else
            {
                DL = Path.GetPathRoot(FP);
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

        private void タブを増やすToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void th16_L_Click(object sender, EventArgs e)
        {

        }
    }
}