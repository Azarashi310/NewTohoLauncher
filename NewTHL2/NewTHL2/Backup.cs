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
        Dictionary<string, string> settingIniFile;
        
        public Backup()
        {
            InitializeComponent();
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
        public void initialize(Dictionary<string,string>setFileIni,string select)
        {
            settingIniFile = setFileIni;
            //セーブデータとリプレイ以外をfalseにして初期化
            screenShot_Group.Enabled = false;
            bestShot_Group.Enabled = false;
            hint_Group.Enabled = false;
            autoSave_Group.Enabled = false;
            profile_Group.Enabled = false;
            Okubi_Group.Enabled = false;
            macro_Group.Enabled = false;
            
            
            //紅魔郷と萃夢想はスクリーンショットがないため
            if((select != "th06") & (select != "th075"))
            {
                screenShot_Group.Enabled = true;
                
                //文花帖系
                if ((select == "th095") || (select == "th125"))
                {
                    bestShot_Group.Enabled = true;
                }
                //風神録 & 地霊殿
                if ((select == "th10") || (select == "th11"))
                {
                    hint_Group.Enabled = true;
                }
                //プロファイルのみの黄昏作品
                if((select == "th105")||(select == "th123"))
                {
                    profile_Group.Enabled = true;
                }
                //心綺楼
                if(select == "th135")
                {
                    profile_Group.Enabled = true;
                    Okubi_Group.Enabled = true;
                }
                //深秘録
                if(select == "th145")
                {
                    profile_Group.Enabled = true;
                    Okubi_Group.Enabled = true;
                    macro_Group.Enabled = true;
                }
                //東方紺珠伝 ～ Legacy of Lunatic Kingdom.
                if(select == "th15")
                {
                    autoSave_Group.Enabled = true;
                }
            }
        }

        //決定ボタン
        private void decideButton_Click(object sender, EventArgs e)
        {
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
    }
}
