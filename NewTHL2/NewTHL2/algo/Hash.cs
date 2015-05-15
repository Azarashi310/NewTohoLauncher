using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace NewTHL2.algo
{
    class Hash
    {
        //MD5の排出
        public static string exportMD5(string FilePath)
        {
            //こっからファイルを配列バイト型へ変換
            FileStream FS = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            //バイト配列の宣言
            byte[] md5byte = new byte[FS.Length];
            FS.Read(md5byte, 0, md5byte.Length);
            FS.Close();
            //MD5を使う宣言
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            //byte配列を16進数に変換
            StringBuilder SB = new StringBuilder();
            md5byte = MD5.ComputeHash(md5byte);
            MD5.Clear();
            foreach (byte b in md5byte)
            {
                SB.Append(b.ToString("x2"));
            }
            return SB.ToString();
        }
        //MD5の比較
        public static bool compairMD5(string FilePath, string md5)
        {
            //リターン用
            bool flag;
            //こっからファイルを配列バイト型へ変換
            FileStream FS = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            //バイト配列の宣言
            byte[] md5byte = new byte[FS.Length];
            FS.Read(md5byte, 0, md5byte.Length);
            FS.Close();
            //MD5を使う宣言
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            //byte配列を16進数に変換
            StringBuilder SB = new StringBuilder();
            md5byte = MD5.ComputeHash(md5byte);
            MD5.Clear();
            foreach (byte b in md5byte)
            {
                SB.Append(b.ToString("x2"));
            }
            if (SB.ToString() == md5)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }
    }
}
