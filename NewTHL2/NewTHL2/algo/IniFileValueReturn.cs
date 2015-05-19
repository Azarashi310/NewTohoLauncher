using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace NewTHL2.algo
{
    class IniFileValueReturn:Form1
    {
        /// <summary>
        /// 引数にはiniファイルのファイルパスをいれろください。
        /// 戻り値はDictionary　string,string　やで
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<string,string> getIniFileValue(string path)
        {
            //一時的にkeyを格納する(String)
            List<string> keyTemp = new List<string>();

            //実際にリターンするもの
            Dictionary<string, string> vpatch_key_Value = new Dictionary<string, string>();

            //セクションの取得
            List<string> section = new List<string>();

            //ファイルパス
            string FilePath = path;

            /*
             * ここ改善した
             * セクションを取得していちいち"Window"とか書かなくてもいいようにした
             */
            //セクションの取得
            //取得用のバイトを用意
            byte[] sectionByte = new byte[1024];
            //バイトとしてセクションを取得
            uint sectionResult_uint = GetPrivateProfileStringByByteArray(null, null, "default", sectionByte, (uint)sectionByte.Length, path);
            //文字列として変数に入れる
            string resultSection = Encoding.Default.GetString(sectionByte, 0, (int)sectionResult_uint - 1);
            //まとまってるセクションを分けて入れるようの配列を用意
            string[] sections = resultSection.Split('\0');
            //セクションを分けて配列に入れる
            foreach(string tempSection in sections)
            {
                section.Add(tempSection);
            }

            //Keyの取得
            foreach(string temp_section in section)
            {
                //Vpatch.iniのWindowのkeyを取得する
                byte[] byteArr = new byte[1024];
                uint resultSize = GetPrivateProfileStringByByteArray(temp_section, null, "", byteArr, (uint)byteArr.Length, path);
                string result = Encoding.Default.GetString(byteArr, 0, (int)resultSize - 1);
                string[] keys = result.Split('\0');
                
                //Vpatch.iniのWindow部分のkeyを取得する
                foreach (string key in keys)
                {
                    keyTemp.Add(key);
                }
            }

            //取得したValue(Window)格納用
            //StringBuilder Value_window = new StringBuilder(1024);
            //取得したValue(Option)格納用
            //StringBuilder Value_Option = new StringBuilder(1024);

            //取得したValueの格納用
            StringBuilder value = new StringBuilder(1024);
            //キーの番号を保存する
            int keys_temp = 0;

            //valueを取得し、配列にぶち込む
            for (int sectionCount = 0; sectionCount < section.Count; sectionCount++ )
            {
                for (int keys = 0; keys < keyTemp.Count; keys++ )
                {
                    keys = keys_temp;
                    GetPrivateProfileString(section[sectionCount], keyTemp[keys], "null", value, Convert.ToUInt32(value.Capacity), FilePath);
                    if (value.ToString() != "null")
                    {
                        //Dictionaryに追加
                        vpatch_key_Value.Add(keyTemp[keys], value.ToString());
                        keys_temp++;
                    }
                    else if(value.ToString() == "null")
                    {
                        keys_temp = keys;
                        break;
                    }
                }
                    
            }

            /*
            for (int i = 0; i < keyTemp.Count; i++ )
            {
                //ArrayListの要素を変換して格納
                tempKeys_Vpatch = Convert.ToString((string)keyTemp[i]);

                GetPrivateProfileString("Window", tempKeys_Vpatch, "", Value_window, Convert.ToUInt32(Value_window.Capacity), vpatchPath);
                GetPrivateProfileString("Option", tempKeys_Vpatch, "", Value_Option, Convert.ToUInt32(Value_Option.Capacity), vpatchPath);
                if(Value_window.ToString() != "")
                {
                    //Dictionaryに追加
                    vpatchIni.Add(tempKeys_Vpatch, int.Parse(Value_window.ToString()));
                }
                else if(Value_Option.ToString() != "")
                {
                    vpatchIni.Add(tempKeys_Vpatch, int.Parse(Value_Option.ToString()));
                }
            }
            */
            return vpatch_key_Value;
        }
    }
}
