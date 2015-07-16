using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;

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
            
            //日付つきのフォルダを作成する
            if(!Directory.Exists(backup))
            {
                Directory.CreateDirectory(backup);
            }

            //バックアップパスにセパレータをつける
            backup = backup + Path.DirectorySeparatorChar;

            //ファイルをコピー
            string[] files = Directory.GetFiles(SourcePath);
            foreach(string file in files)
            {
                File.Copy(file, backup + Path.GetFileName(file), true);
            }
        }

        public static void folderCreate(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
