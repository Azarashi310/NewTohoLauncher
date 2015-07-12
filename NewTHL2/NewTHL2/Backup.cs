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
    public partial class Backup : Form
    {
        //ローカルに一度保存
        private Dictionary<string, string> settingIniFile;
        //ファイルパスをもってくる
        private string FilePath;
        //選択されている作品
        private string selectedWork;
        //フォルダ参照がキャンセルされているか
        private bool cancel = false;

        public Backup()
        {
            InitializeComponent();
            //閉じるボタンとか消すやつ
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ControlBox = !this.ControlBox;
        }

        private void Backup_Load(object sender, EventArgs e)
        {

        }
        
        //ゲームごとに選択できるものを変更する
        /// <summary>
        /// setFileIniにはkeyとvalueを取得したDictionaryクラスのものを入れます
        /// selectには現在ランチャー側で選択されている作品の名前を入れます。
        /// </summary>
        /// <param name="setFileIni"></param>
        /// <param name="select"></param>
        public void initialize(Dictionary<string,string>setFileIni,string select,string FP)
        {
            //ローカルに保存
            settingIniFile = setFileIni;
            FilePath = FP;
            selectedWork = select;
            //セーブデータとリプレイ以外をfalseにして初期化
            screenShot_Group.Enabled = false;
            bestShot_Group.Enabled = false;
            hint_Group.Enabled = false;
            autoSave_Group.Enabled = false;
            profile_Group.Enabled = false;
            Okubi_Group.Enabled = false;
            macro_Group.Enabled = false;
            icon_Group.Enabled = false;

            #region Enableの設定

            //テキストボックスに入れる（基本的なやつ）
            savedata_TextBox.Text = settingIniFile[selectedWork + "_Save"];
            replay_Textbox.Text = settingIniFile[selectedWork + "_Replay"];

            //紅魔郷と萃夢想はスクリーンショットがないため（ていうか黄昏作品ぜんぶねぇじゃん！！！ふざけんな！！！！）
            if((selectedWork != "th06") & (selectedWork != "th075") & (selectedWork != "th105") & (selectedWork != "th123")&
                (selectedWork != "th135") & (selectedWork != "th145"))
            {
                screenShot_Group.Enabled = true;

                //テキストボックスに入れる
                screenShot_Textbox.Text = settingIniFile[selectedWork + "_SnapShot"];

                //文花帖系
                if ((selectedWork == "th095") || (selectedWork == "th125"))
                {
                    bestShot_Group.Enabled = true;
                    
                    //テキストボックスに入れる
                    bestShot_TextBox.Text = settingIniFile[selectedWork + "_BestShot"];
                }
                //風神録 & 地霊殿
                if ((selectedWork == "th10") || (selectedWork == "th11"))
                {
                    hint_Group.Enabled = true;

                    //テキストボックスにいれる
                    hint_TextBox.Text = settingIniFile[selectedWork + "_Hint"];
                }
                //東方紺珠伝 ～ Legacy of Lunatic Kingdom.
                if(selectedWork == "th15")
                {
                    autoSave_Group.Enabled = true;
                    
                    //テキストボックスに入れる
                    autoSave_TextBox.Text = settingIniFile[selectedWork + "_AutoSave"];
                }
            }
            //プロファイルのみの黄昏作品
            if ((selectedWork == "th105") || (selectedWork == "th123"))
            {
                profile_Group.Enabled = true;

                //テキストボックスに入れる
                profile_Textbox.Text = settingIniFile[selectedWork + "_Profile"];
            }
            //心綺楼
            if (selectedWork == "th135")
            {
                profile_Group.Enabled = true;
                Okubi_Group.Enabled = true;
                icon_Group.Enabled = true;

                //テキストボックスに入れる
                profile_Textbox.Text = settingIniFile[selectedWork + "_Profile"];
                okubi_Textbox.Text = settingIniFile[selectedWork + "_Okubi"];
                icon_Textbox.Text = settingIniFile[selectedWork + "_Icon"];
            }
            //深秘録
            if (selectedWork == "th145")
            {
                profile_Group.Enabled = true;
                Okubi_Group.Enabled = true;
                macro_Group.Enabled = true;
                icon_Group.Enabled = true;

                //テキストボックスに入れる
                profile_Textbox.Text = settingIniFile[selectedWork + "_Profile"];
                okubi_Textbox.Text = settingIniFile[selectedWork + "_Okubi"];
                macro_TextBox.Text = settingIniFile[selectedWork + "_Macro"];
                icon_Textbox.Text = settingIniFile[selectedWork + "_Icon"];
            }
            #endregion
        }

        //決定ボタン
        private void decideButton_Click(object sender, EventArgs e)
        {
            //基本のもの
            settingIniFile[selectedWork + "_Save"] = savedata_TextBox.Text;
            settingIniFile[selectedWork + "_Replay"] = replay_Textbox.Text;
            
            //その他のもの
            if ((selectedWork != "th06") & (selectedWork != "th075"))
            {
                screenShot_Group.Enabled = true;

                //テキストボックスに入れる
                settingIniFile[selectedWork + "_SnapShot"] = screenShot_Textbox.Text;

                //文花帖系
                if ((selectedWork == "th095") || (selectedWork == "th125"))
                {
                    bestShot_Group.Enabled = true;

                    //テキストボックスに入れる
                     settingIniFile[selectedWork + "_BestShot"] = bestShot_TextBox.Text;
                }
                //風神録 & 地霊殿
                if ((selectedWork == "th10") || (selectedWork == "th11"))
                {
                    hint_Group.Enabled = true;

                    //テキストボックスにいれる
                    settingIniFile[selectedWork + "_Hint"] = hint_TextBox.Text;
                }
                //プロファイルのみの黄昏作品
                if ((selectedWork == "th105") || (selectedWork == "th123"))
                {
                    profile_Group.Enabled = true;

                    //テキストボックスに入れる
                    settingIniFile[selectedWork + "_Profile"] = profile_Textbox.Text;
                }
                //心綺楼
                if (selectedWork == "th135")
                {
                    profile_Group.Enabled = true;
                    Okubi_Group.Enabled = true;
                    icon_Group.Enabled = true;

                    //テキストボックスに入れる
                    settingIniFile[selectedWork + "_Profile"] = profile_Textbox.Text;
                    settingIniFile[selectedWork + "_Okubi"] = okubi_Textbox.Text;
                    settingIniFile[selectedWork + "_Icon"] = icon_Textbox.Text;
                }
                //深秘録
                if (selectedWork == "th145")
                {
                    profile_Group.Enabled = true;
                    Okubi_Group.Enabled = true;
                    macro_Group.Enabled = true;
                    icon_Group.Enabled = true;

                    //テキストボックスに入れる
                    settingIniFile[selectedWork + "_Profile"] = profile_Textbox.Text;
                    settingIniFile[selectedWork + "_Okubi"] = okubi_Textbox.Text;
                    settingIniFile[selectedWork + "_Macro"] = macro_TextBox.Text;
                    settingIniFile[selectedWork + "_Icon"] = icon_Textbox.Text;
                }
                //東方紺珠伝 ～ Legacy of Lunatic Kingdom.
                if (selectedWork == "th15")
                {
                    autoSave_Group.Enabled = true;

                    //テキストボックスに入れる
                    settingIniFile[selectedWork + "_AutoSave"] = autoSave_TextBox.Text;
                }
            }

            //iniファイルに書き込む
            algo.IniFileValueWrite.IniFileWriter(settingIniFile,FilePath);

            this.Close();
        }

        //キャンセルボタン
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("キャンセルしますか？", "お知らせ", MessageBoxButtons.YesNo);
            if(DialogResult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        //セーブデータのフォルダパス参照
        private void savedataBuckupFolder_Browse_button_Click(object sender, EventArgs e)
        {
            string folderPath = getFolderPath();
            if(!cancel)
            {
                savedata_TextBox.Text = folderPath;
            }
            else
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
            }
        }
        //リプレイのフォルダパス参照
        private void replayBackupFolder_Browse_Button_Click(object sender, EventArgs e)
        {
            string folderPath = getFolderPath();
            if (!cancel)
            {
                replay_Textbox.Text = folderPath;
            }
            else
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
            }
        }
        //スクリーンショットのフォルダパス参照
        private void screenShotBackupFolder_Browse_Button_Click(object sender, EventArgs e)
        {
            string folderPath = getFolderPath();
            if (!cancel)
            {
                screenShot_Textbox.Text = folderPath;
            }
            else
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
            }
        }
        //ベストショットのフォルダパス参照
        private void bestShotBackupFolder_Browse_Button_Click(object sender, EventArgs e)
        {
            string folderPath = getFolderPath();
            if (!cancel)
            {
                bestShot_TextBox.Text = folderPath;
            }
            else
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
            }
        }
        //ヒントのフォルダパス参照
        private void hintBackupFolder_Browse_Button_Click(object sender, EventArgs e)
        {
            string folderPath = getFolderPath();
            if (!cancel)
            {
                hint_TextBox.Text = folderPath;
            }
            else
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
            }
        }
        //自動セーブのフォルダパス参照
        private void autoSaveBackUpFolder_Browse_Button_Click(object sender, EventArgs e)
        {
            string folderPath = getFolderPath();
            if (!cancel)
            {
                autoSave_TextBox.Text = folderPath;
            }
            else
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
            }
        }
        //プロフィールのフォルダパス参照
        private void profileBackupFolder_Browse_Button_Click(object sender, EventArgs e)
        {
            string folderPath = getFolderPath();
            if (!cancel)
            {
                profile_Textbox.Text = folderPath;
            }
            else
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
            }
        }
        //御首頂戴帳のフォルダパス参照
        private void OkubiBackupFolder_Browse_Button_Click(object sender, EventArgs e)
        {
            string folderPath = getFolderPath();
            if (!cancel)
            {
                okubi_Textbox.Text = folderPath;
            }
            else
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
            }
        }
        //アイコンのフォルダパス参照
        private void button1_Click(object sender, EventArgs e)
        {
            string folderPath = getFolderPath();
            if (!cancel)
            {
                icon_Textbox.Text = folderPath;
            }
            else
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
            }
        }
        //マクロのフォルダパス参照
        private void macroBackupFolder_Browse_Button_Click(object sender, EventArgs e)
        {
            string folderPath = getFolderPath();
            if (!cancel)
            {
                macro_TextBox.Text = folderPath;
            }
            else
            {
                MessageBox.Show("キャンセルされました", "お知らせ");
            }
        }
        
        //フォルダパスを取得する
        private string getFolderPath()
        {
            string folderPath = "";

            folderPath = algo.OFDandSFD.FBD_Run();
            if(folderPath == "")
            {
                cancel = true;
            }
            return folderPath;
        }

    }
}
