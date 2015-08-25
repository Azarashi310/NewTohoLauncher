using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Windows.Forms;

namespace NewTHL2.algo
{
    class FileManage
    {
        /// <summary>
        /// フォルダのコピーを行います
        /// BackupPath　バックアップする場所を入れます
        /// SourcePath  元データがあるパスを入れます
        /// </summary>
        /// <param name="BackupPath"></param>
        /// <param name="SourcePath"></param>
        public static void folderCopy(string BackupPath,string SourcePath)
        {
            string backup = BackupPath;
            //ファイルをコピー
            string[] files = Directory.GetFiles(SourcePath);
            if(files.Length != 0)
            {
                //フォルダを作成する
                if (!Directory.Exists(backup))
                {
                    folderCreate(backup);
                }

                //バックアップパスにセパレータをつける( / これね)
                backup = backup + Path.DirectorySeparatorChar;


                foreach (string file in files)
                {
                    File.Copy(file, backup + Path.GetFileName(file), true);
                }
            }
        }

        //フォルダ作成する奴（正直必要かどうか怪しい）
        public static void folderCreate(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// バックアップする際にフォルダが存在しているかや、
        /// 日付の設定などをここでやります。
        /// ていうかバックアップもさせちゃう
        /// 
        /// source バックアップするファイルのある場所を指定します
        /// backup バックアップする先のフォルダパスを指定します
        /// </summary>
        /// <param name="source"></param>
        /// <param name="backup"></param>
        public static void backupManageandFolderCopy(string source,string backup)
        {
            string backupFolderPath = "";
            string sourcePath = source;
            string backupPath = backup;


            //ソースパスが存在するか？
            if (Directory.Exists(sourcePath))
            {
                //バックアップパスを代入
                backupFolderPath = backup;
            }
            backupFolderPath = backupFolderCheck(backupFolderPath);
            #region 以前のコード
            /*

            //バックアップフォルダが存在しているか
            if (Directory.Exists(backupFolderPath))
            {
                backupFolderPath = Path.Combine(backupFolderPath, DateTime.Now.ToString("yyyy年MM月dd日"));
            }
            else
            {
                DialogResult result = MessageBox.Show("バックアップフォルダが存在しません" + Environment.NewLine +
                           "設定しているパス : " + backupFolderPath + Environment.NewLine +
                           "フォルダを作成しますか？", "お知らせ", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //フォルダの作成
                    algo.FileManage.folderCreate(backupFolderPath);
                    MessageBox.Show("作成しました", "お知らせ");
                }
                else
                {
                    MessageBox.Show("フォルダを作成しませんでした。" + Environment.NewLine +
                        "後ほど、ランチャーからバックアップフォルダパスを再設定してください", "お知らせ");
                    
                    //フォルダを作成しなかった為、バックアップフォルダを初期化させる
                    backupFolderPath = "";
                }
            }
            */
            #endregion
            //バックアップファイルパスに問題がなければ
            if(backupFolderPath != "")
            {
                //フォルダコピーを実行する
                folderCopy(backupFolderPath, sourcePath);
            }
        }

        //バックアップフォルダの確認だけ別枠にする
        public static string backupFolderCheck(string path)
        {
            string backupFolderPath = path;
            //バックアップフォルダが存在しているか
            if (Directory.Exists(backupFolderPath))
            {
                backupFolderPath = Path.Combine(backupFolderPath, DateTime.Now.ToString("yyyy年MM月dd日"));
            }
            else
            {
                DialogResult result = MessageBox.Show("バックアップフォルダが存在しません" + Environment.NewLine +
                           "設定しているパス : " + backupFolderPath + Environment.NewLine +
                           "フォルダを作成しますか？", "お知らせ", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //フォルダの作成
                    algo.FileManage.folderCreate(backupFolderPath);
                    MessageBox.Show("作成しました", "お知らせ");
                }
                else
                {
                    MessageBox.Show("フォルダを作成しませんでした。" + Environment.NewLine +
                        "後ほど、ランチャーからバックアップフォルダパスを再設定してください", "お知らせ");

                    //フォルダを作成しなかった為、バックアップフォルダを初期化させる
                    backupFolderPath = "";
                }
            }

            return backupFolderPath;
        }
    }
}
