using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NewTHL2.algo
{
    class IniFileValueWrite:Form1
    {
        /// <summary>
        /// 取得したVpatch.iniの中身（Dictionary型）のものを第一引数、
        /// Vpatch.iniの中身を引き継ぐ先のファイルパス(String)を第二引数へ。
        /// </summary>
        /// <param name="Dic"></param>
        /// <param name="FilePath"></param>
        public static void IniFileWriter(Dictionary<string, string> Dic, string FilePath)
        {
            //セクションを保存するよう
            List<string> section = new List<string>();
            //セクションの取得
            //取得用のバイトを用意
            byte[] sectionByte = new byte[1024];
            //バイトとしてセクションを取得
            uint sectionResult_uint = GetPrivateProfileStringByByteArray(null, null, "default", sectionByte, (uint)sectionByte.Length, FilePath);
            //文字列として変数に入れる
            string resultSection = Encoding.Default.GetString(sectionByte, 0, (int)sectionResult_uint - 1);
            //まとまってるセクションを分けて入れるようの配列を用意
            string[] sections = resultSection.Split('\0');
            //セクションを分けて配列に入れる
            foreach (string tempSection in sections)
            {
                section.Add(tempSection);
            }

            //Value格納用
            StringBuilder getValue = new StringBuilder(1024);

            //キーのカウントのtemp
            int key_temp = 0;
            //消化した分のキー数
            int usedKey = 0;
            //全部キーを消化した場合のフラグ
            bool endKey = false;
            //キーのみを抽出
            List<string> keyList = new List<string>(Dic.Keys);
            //バリューのみ抽出
            List<string> valueList = new List<string>(Dic.Values);


            for (int sec_count = 0; sec_count <= section.Count -1; sec_count++)
            {
                for (int key_count = 0; key_count <= Dic.Keys.Count -1; key_count++)
                {
                    key_count = key_temp;

                    //現在のセクションにそのキーが存在するか確認する用
                    GetPrivateProfileString(section[sec_count], keyList[key_count], "null", getValue, Convert.ToUInt32(getValue.Capacity), FilePath);

                    if (getValue.ToString() != "null")
                    {
                        //iniへの書き込み
                        WritePrivateProfileString(section[sec_count], keyList[key_count], valueList[key_count], FilePath);
                        
                        //使用したキー数を監視
                        usedKey++;
                        if(usedKey == Dic.Keys.Count)
                        {
                            endKey = true;
                        }
                        
                        key_temp++;
                    }
                    //セクション移動用
                    else if (getValue.ToString() == "null")
                    {
                        key_temp = key_count;
                        break;
                    }

                }
                //キーをすべて消化した時用
                if (endKey)
                {
                    break;
                }
            }
        }
    }
}
