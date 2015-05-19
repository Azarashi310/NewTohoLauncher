using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NewTHL2.algo
{
    class FileCopy:Form1
    {
        /// <summary>
        /// PathにはVpatchを作成したい場所。
        /// KindにはVpatchの種類を入れます
        /// Th06-th11 = "other"
        /// th125 = "Th125"
        /// th13 = "Th13"
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Kind"></param>
        /// <returns></returns>
        public static void makeVpatchIni(string Path/*,string Kind*/)
        {
            string FilePath = Path;
            File.Create(FilePath).Close();
            StreamWriter SW = new StreamWriter(FilePath, true, sjis);

            /*
             * 
             * 試験的に全部のVpatch.iniファイルを共通化させる
             * 
             */

            SW.WriteLine(NewTHL2.Properties.Resources.vpatch_All);

            /*
            if(Kind == "Th12")
            {
                SW.Write(NewTHL2.Properties.Resources.vpatch_th13);
            }
            else if(Kind == "Th125")
            {
                SW.Write(NewTHL2.Properties.Resources.vpatch_th125);
            }
            else if(Kind == "Th13")
            {
                SW.Write(NewTHL2.Properties.Resources.vpatch_th13);                
            }
            else
            {
                SW.Write(NewTHL2.Properties.Resources.vpatch_th06_th11);
            }
             */
            SW.Close();
        }

        /// <summary>
        /// ファイルパスの設定をアップデートします
        /// </summary>
        /// <param name="path"></param>
        public static void updateFilePath(string path)
        {
            string FilePath = path;
            StreamWriter SW = new StreamWriter(FilePath, false, sjis);
            SW.WriteLine(NewTHL2.Properties.Resources.setteingTemplate);
        }
    }
}
