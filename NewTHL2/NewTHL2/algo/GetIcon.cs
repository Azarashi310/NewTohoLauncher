using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Drawing;

namespace NewTHL2.algo
{
    class GetIcon
    {
        #region ThXXX.exeのアイコン取得
        /*
         * 
         * ココからアイコン取得
         * 
         * 
         */
        // SHGetFileInfo関数
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        // SHGetFileInfo関数で使用するフラグ
        private const uint SHGFI_ICON = 0x100; // アイコン・リソースの取得
        private const uint SHGFI_LARGEICON = 0x0; // 大きいアイコン
        private const uint SHGFI_SMALLICON = 0x1; // 小さいアイコン

        // SHGetFileInfo関数で使用する構造体
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }
        /*
         * 
         * ここまで
         * 
         * 
         */
        #endregion

        //アイコン
        private static Bitmap canvas;
        
        //アイコンを取得し、パネル用のものを返す
        public static Bitmap returnPanelIcon(string FP, int PICBOX_Width,int PICBOX_Height)
        {
            SHFILEINFO shinfo = new SHFILEINFO(); 
            IntPtr hSuccess = SHGetFileInfo(FP, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
            if (hSuccess != IntPtr.Zero)
            {
                Icon appIcon = Icon.FromHandle(shinfo.hIcon);
                //画像の拡大　ここから
                //アイコンのBMP化
                Bitmap bmp = appIcon.ToBitmap();
                canvas = new Bitmap(PICBOX_Width, PICBOX_Height);
                Graphics Gra = Graphics.FromImage(canvas);
                Gra.DrawImage(bmp, 0, 0, bmp.Width * 2, bmp.Height * 2);
                Gra.Dispose();
                ////ピクチャーボックスにアプリケーションアイコンをセット
                //this.pictureBox1.Image = canvas;
                //this.panel1.BackColor = Color.WhiteSmoke;
                ////set.FilePathGroupへの追加
                //set.FilePathGroup[i] = SB.ToString();
            }
            return canvas;
        }

        //アイコンを取得し、ライトペイン用の画像を返す
        public static Bitmap returnRightPainIcon(string FP, int PICBOX_Width, int PICBOX_Height)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hSuccess = SHGetFileInfo(FP, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
            if (hSuccess != IntPtr.Zero)
            {
                Icon appIcon = Icon.FromHandle(shinfo.hIcon);
                //画像の拡大　ここから
                //アイコンのBMP化
                Bitmap bmp = appIcon.ToBitmap();
                
                //描画先のImageオブジェクトを作成する
                canvas = new Bitmap(PICBOX_Width, PICBOX_Height);
                
                //ImageオブジェクトのGraphicsオブジェクトを作成する
                Graphics Gra = Graphics.FromImage(canvas);
                
                //Bitmapオブジェクトの作成
                //Bitmap image = new Bitmap(FP);
                //補間法としてバイキュービック法を使う
                Gra.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //画像を拡大
                Gra.DrawImage(bmp, 0, 0, bmp.Width * 4, bmp.Height * 4);

                Gra.Dispose();
                ////ピクチャーボックスにアプリケーションアイコンをセット
                //this.pictureBox1.Image = canvas;
                //this.panel1.BackColor = Color.WhiteSmoke;
                ////set.FilePathGroupへの追加
                //set.FilePathGroup[i] = SB.ToString();
            }
            return canvas;
        }
    }
}
