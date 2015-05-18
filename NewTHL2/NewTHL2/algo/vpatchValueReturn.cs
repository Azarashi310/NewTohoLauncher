using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace NewTHL2.algo
{
    class vpatchValueReturn:Form1
    {
        /// <summary>
        /// 引数にはVpatchのファイルパスをいれろください。
        /// 戻り値はDictionary　string,int　やで
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<string,int> getVpatchValue(string path)
        {
            //一時的にkeyを格納する(String)
            ArrayList keyTemp = new ArrayList();
            
            //vpatch.iniのキーごとの値格納数
            ArrayList valueTemp = new ArrayList();

            //実際にリターンするもの
            Dictionary<string, int> vpatchIni = new Dictionary<string, int>();

            //ファイルパス
            string vpatchPath = path;

            //Vpatch.iniのWindowのkeyを取得する
            byte[] byteArr_Window = new byte[1024];
            uint resultSize_Window = GetPrivateProfileStringByByteArray("Window", null, "", byteArr_Window, (uint)byteArr_Window.Length, path);
            string result_Window = Encoding.Default.GetString(byteArr_Window, 0, (int)resultSize_Window - 1);
            string[] keys_Window = result_Window.Split('\0');
            
            ////Vpatch.iniのOptionのkeyを取得する
            byte[] byteArr_Option = new byte[1024];
            uint resultSize_Option = GetPrivateProfileStringByByteArray("Option", null, "", byteArr_Option, (uint)byteArr_Option.Length, path);
            string result_Option = Encoding.Default.GetString(byteArr_Option, 0, (int)resultSize_Option - 1);
            string[] keys_Option = result_Option.Split('\0');
            
            //Vpatch.iniのWindow部分のkeyを取得する
            foreach (string key in keys_Window)
            {
                keyTemp.Add(key);
            }
            //Vpatch.iniのOption部分のkeyを取得する
            foreach (string key in keys_Option)
            {
                keyTemp.Add(key);
            }

            //取得したValue(Window)格納用
            StringBuilder Value_window = new StringBuilder(1024);
            //取得したValue(Option)格納用
            StringBuilder Value_Option = new StringBuilder(1024);
            //ArrayListから要素を取得したものを一時的に格納するもの
            string tempKeys_Vpatch = "";

            //valueを取得し、配列にぶち込む
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
                return vpatchIni;
        }
    }
}
