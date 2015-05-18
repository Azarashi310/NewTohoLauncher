using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTHL2.algo
{
    class OFDandSFD
    {
        public static string FBD_Run()
        {
            //フォルダ参照ダイアログ
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            //上部説明
            FBD.Description = "ゲームのファイルパスを参照して下さい";
            //ルートフォルダ(デフォルトはデスクトップ)
            FBD.RootFolder = Environment.SpecialFolder.Desktop;
            //最初に選択するフォルダー(Rootfolder以下である必要性)
            FBD.SelectedPath = @"C:\Windows";
            //新規フォルダーを作れるか。
            FBD.ShowNewFolderButton = true;
            //ダイアログの表示
            if (FBD.ShowDialog() == DialogResult.OK)
            {

            }
            return FBD.SelectedPath;
        }
        public static void OFD_Run(string str)
        {
            //ファイル参照ダイアログ
            OpenFileDialog OFD = new OpenFileDialog();
            //上部説明
            OFD.Title = str;
            //ルートフォルダ
            OFD.InitialDirectory = @"C:\Windows";
            OFD.FileName = "";
            OFD.Filter = "すべての画像ファイル(*.png;*.PNG;*.jpg;*.jpeg;*.JPG;*.JPEG;*.bmp;*.BMP)|*.png;*.PNG;*.jpg;*.jpeg;*.JPG;*.JPEG;*.bmp;*.BMP|pngファイル(*.png;*.PNG)|*.png;*.PNG|jpegファイル(*.jpg;*.jpeg;*.JPG;*.JPEG)|*.jpg;*.jpeg;*.JPG;*.JPEG|bmpファイル(*.bmp;*.BMP)|*.bmp;*.BMP";
            OFD.FilterIndex = 1;
            if (OFD.ShowDialog() == DialogResult.OK)
            {

            }
        }
        public static void OFD_backup()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "インポートするバックアップファイルを選択してください";
            OFD.InitialDirectory = @"C:\";
            OFD.Filter = "バックアップファイル(*.thb)|*.thb";
            OFD.FilterIndex = 1;
            if (OFD.ShowDialog() == DialogResult.OK)
            {

            }
            else
            {

            }
        }
        //hashテーブル参照用
        public static void OFD_hash()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "インポートするハッシュテーブル.iniを選択してください";
            OFD.InitialDirectory = @"C:\";
            OFD.Filter = "ハッシュファイル(*.ini)|*.ini";
            OFD.FilterIndex = 1;
            if (OFD.ShowDialog() == DialogResult.OK)
            {

            }
            else
            {

            }
        }
        public static void SFD_Run()
        {
            DateTime DT = DateTime.Now;
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.FileName = "backup" + DT.ToString("yyyyMMdd") + ".thb";
            SFD.InitialDirectory = @"C:\";
            SFD.Filter = "バックアップファイル(*.thb)|*.thb";
            SFD.FilterIndex = 1;
            SFD.Title = "ファイルパスバックアップを保存する場所を選んで下さい";
            if (SFD.ShowDialog() == DialogResult.OK)
            {

            }
            else
            {

            }
        }
    }
}
