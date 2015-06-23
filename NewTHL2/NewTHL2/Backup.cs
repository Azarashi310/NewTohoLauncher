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
        public Backup()
        {
            InitializeComponent();
        }

        private void Backup_Load(object sender, EventArgs e)
        {

        }

        //ゲームごとに選択できるものを変更する
        public void initialize(Dictionary<string,string>setFileIni,string select)
        {
            if((select == "th06") || (select == "th07")||(select == "th075"))
            {
                screenShot_Group.Enabled = false;
                bestShot_Group.Enabled = false;
                hint_Group.Enabled = false;
                autoSave_Group.Enabled = false;
                profile_Group.Enabled = false;
                Okubi_Group.Enabled = false;
                macro_Group.Enabled = false;
            }
            else
            {
                if((select == "th095") || (select == "th125"))
                {
                    hint_Group.Enabled = false;
                    autoSave_Group.Enabled = false;
                    profile_Group.Enabled = false;
                    Okubi_Group.Enabled = false;
                    macro_Group.Enabled = false;
                }
                if(select == "th10")
                {
                    autoSave_Group.Enabled = false;
                    profile_Group.Enabled = false;
                    Okubi_Group.Enabled = false;
                    macro_Group.Enabled = false;
                }
                if(select == "")
                bestShot_Group.Enabled = false;
                hint_Group.Enabled = false;
                autoSave_Group.Enabled = false;
                profile_Group.Enabled = false;
                Okubi_Group.Enabled = false;
                macro_Group.Enabled = false;
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
