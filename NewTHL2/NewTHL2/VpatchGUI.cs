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
    public partial class VpatchGUI : Form
    {
        //書き込み用
        public static Dictionary<string, int> vpatchValues;
        public VpatchGUI()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ControlBox = !this.ControlBox;
            //this.trackBar1.ValueChanged += trackBar1_ValueChanged;
        }
        private void VpatchGUI_Load(object sender, EventArgs e)
        {

        }
        //値をセットする
        public void setValues(Dictionary<string,int> values,string titleName)
        {
            //値を取得
            vpatchValues = values;
            //Dictionary<string, int> vpatchValues = values;           

            #region Window
            /*
             * ここからWindow設定
             * 
             */

            //起動時にWindow or FullScreenを尋ねる
            if(vpatchValues["AskWindowMode"] == 1)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            //Vsync側でWindowのサイズを指定する
            if(vpatchValues["enabled"] == 1)
            {
                checkBox2.Checked = true;
                //trueであるならば詳細設定をenabledにする
                詳細設定.Enabled = true;
            }
            else
            {
                checkBox2.Checked = false;
                //trueであるならば詳細設定をdiableにする
                詳細設定.Enabled = false;
            }
            
            //描画位置X
            描画位置X_Text.Text = vpatchValues["X"].ToString();

            //描画位置_Y
            描画位置Y_Text.Text = vpatchValues["Y"].ToString();

            //Width
            Width_Text.Text = vpatchValues["Width"].ToString();

            //Height
            Height_Text.Text = vpatchValues["Height"].ToString();

            //タイトルバーを表示させるかどうか
            if(vpatchValues["TitleBar"] == 1)
            {
                ShowTitleBar.Checked = true;
            }
            else
            {
                ShowTitleBar.Checked = false;
            }

            //常に手前で表示させる
            if(vpatchValues["AlwaysOnTop"] == 1)
            {
                AlwaysOnTop.Checked = true;
            }
            else
            {
                AlwaysOnTop.Checked = false;
            }
            #endregion
            #region Vsync
            /*
             * 垂直同期
             * 
             */

            //垂直同期設定(とりあえず初期化)
            垂直同期設定.Enabled = false;
            垂直同期設定東方神霊廟.Enabled = false;
            垂直同期設定ダブルスポイラー以降.Enabled = false;
            
            //FPS設定（初期化）
            groupBox3.Enabled = true;

            //垂直同期設定（ダブルスポイラー）
            if(titleName == "Th125")
            {
                垂直同期設定ダブルスポイラー以降.Enabled = true;
                switch(vpatchValues["Vsync"])
                {
                    case -1:
                        {
                            //東方本家の描画方法で描画
                            G2_OriginalDrawing.Checked = true;
                            break;
                        }
                    case 0:
                        {
                            //垂直同期なし（Vpatch制御）
                            G2_NoVsync.Checked = true;
                            break;
                        }
                    case 1:
                        {
                            //Vpatch独自の描画形式
                            G2_VpatchOriginalDrawing.Checked = true;
                            break;
                        }
                    default:
                        {
                            //東方本家の描画方法で描画
                            G2_OriginalDrawing.Checked = true;
                            break;
                        }
                }
            }

            //垂直同期設定（神霊廟）
            else if(titleName == "Th13")
            {
                垂直同期設定東方神霊廟.Enabled = true;
                switch (vpatchValues["Vsync"])
                {
                    case -1:
                        {
                            //ハードウェア補助レベル１
                            G_3HardWareAssistLv1.Checked = true;
                            break;
                        }
                    case -2:
                        {
                            //ハードウェア補助レベル２
                            G_3HardWareAssistLv2.Checked = true;
                            break;
                        }
                    case 0:
                        {
                            //東方本家の描画方法で描画
                            G3_OriginalDrawing.Checked = true;
                            break;
                        }
                    case 1:
                        {
                            //Vpatch独自の描画形式
                            G_3VpatchOriginalDrawing.Checked = true;
                            break;
                        }
                    default:
                        {
                            //東方本家の描画方法で描画
                            G3_OriginalDrawing.Checked = true;
                            break;
                        }
                }
            }

            //垂直同期設定（通常）
            else
            {
                垂直同期設定.Enabled = true;
                switch (vpatchValues["Vsync"])
                {
                    case 0:
                        {
                            //東方本家の描画方法で描画
                            G1_OriginalDrawing.Checked = true;
                            break;
                        }
                    case 1:
                        {
                            //モニターのリフレッシュレートに合わせる
                            G1_MonitorRefreshLate.Checked = true;
                            break;
                        }
                    case 2:
                        {
                            //Vsync側で設定したFPSの値に近づけて描画
                            G1_VsyncFPS.Checked = true;
                            //FPSの設定はこの時の場合はOnにする
                            groupBox3.Enabled = true;
                            break;
                        }
                    case 3:
                        {
                            //画面の真ん中より下で操作する人向け
                            G1_UnderDrawing.Checked = true;
                            break;
                        }
                    default:
                        {
                            //東方本家の描画方法で描画
                            G1_OriginalDrawing.Checked = true;
                            break;
                        }
                }
            }
            #endregion
            #region drawSettings&BugFix
            /*
             * ここから描画設定&BugFix
             * 
             */

            //FPSの制御の設定（初期化）
            FPSAutoControl.Checked = false;
            FPSControlOff.Checked = false;
            FPSControlOn.Checked = false;
            //FPS制御の設定
            switch(vpatchValues["SleepType"])
            {
                case -1:
                    {
                        FPSAutoControl.Checked = true;
                        break;
                    }
                case 0:
                    {
                        FPSControlOff.Checked = true;
                        break;
                    }
                case 1:
                    {
                        FPSControlOn.Checked = true;
                        break;
                    }
                default:
                    {
                        FPSAutoControl.Checked = true;
                        break;
                    }
            }

            //GameFPS
            GameFPS_Text.Text = vpatchValues["GameFPS"].ToString();

            //ReplaySkipFPS
            textBox1.Text = vpatchValues["ReplaySkipFPS"].ToString();

            //ReplaySlowFPS
            textBox2.Text = vpatchValues["ReplaySlowFPS"].ToString();

            //描画設定
            if(vpatchValues["CalcFPS"] == 1)
            {
                //自動計測のチェックボックス
                checkBox3.Checked = true;
                //自動計測のチェックボックスがオンならばトラックバーは利用できない
                先行描画設定TrackBar.Enabled = false;
            }
            else
            {
                checkBox3.Checked = false;
                //自動計測のチェックボックスがオフなので先行描画設定を利用する
                先行描画設定の値.Text = "値 ＝ " + 先行描画設定TrackBar.Value.ToString();
            }

            if(vpatchValues["BltPrepareTime"] == -1)
            {
                //先行描画の値が -1 であるのならば、自動計測のチェックボックスをオンにする
                checkBox3.Checked = true;
            }
            else
            {
                //先行描画の値を取得
                先行描画設定TrackBar.Value = vpatchValues["BltPrepareTime"];
            }

            //FPSの独自の自動計測
            if (vpatchValues["CalcFPS"] == 1)
            {
                FPS値を独自方法で計算します.Checked = true;
            }
            else
            {
                FPS値を独自方法で計算します.Checked = false;
            }

            //非アクティブでも描画
            if(vpatchValues["AlwaysBlt"] == 1)
            {
                非アクティブでも描画.Checked = true;
            }
            else
            {
                非アクティブでも描画.Checked = false;
            }

            //妖々夢の桜点バグを修正
            if(vpatchValues["BugFixCherry"] == 1)
            {
                妖々夢の桜点バグを修正します.Checked = true;
            }
            else
            {
                妖々夢の桜点バグを修正します.Checked = false;
            }

            //風神録のバグマリを修正する
            if(vpatchValues["BugFixTh10Power3"] == 1)
            {
                風神録のバグマリを修正します.Checked = true;
            }
            else
            {
                風神録のバグマリを修正します.Checked = false;
            }

            //星蓮船の聖輦船の影を修正します
            if (vpatchValues["BugFixTh12Shadow"] == 1)
            {
                星蓮船の聖輦船の影を修正します.Checked = true;
            } 
            else
            {
                星蓮船の聖輦船の影を修正します.Checked = false;
            }

            //入力の暴走を修正します
            if (vpatchValues["BugFixGetDeviceState"] == 1)
            {
                入力の暴走を修正します.Checked = true;
            }
            else
            {
                入力の暴走を修正します.Checked = false;
            }

            //フレームスキップをなるべく感じさせないようにする
            if (vpatchValues["AllowShortDelay"] == 1)
            {
                shortDelay.Checked = true;
            }
            else
            {
                shortDelay.Checked = false;
            }
            #endregion
            #region HardCore
            /*
             * 上級者向け
             * 
             */
            
            //動作コア
            switch(vpatchValues["ProcessAffinityMask"])
            {
                case 0:
                    {
                        radioButton1.Checked = true;
                        break;
                    }
                case 1:
                    {
                        CPU0Work.Checked = true;
                        break;
                    }
                case 2:
                    {
                        CPU1Work.Checked = true;
                        break;
                    }
                case 3:
                    {
                        CPU0_CPU1Work.Checked = true;
                        break;
                    }
                default:
                    {
                        radioButton1.Checked = true;
                        break;
                    }
            }
            //プロセス優先度
            processPrimaryTrackBar.Value = vpatchValues["ProcessPriority"];
            switch (vpatchValues["ProcessPriority"])
            {
                case -2:
                    {
                        processPrimaryText.Text = "低";
                        break;
                    }
                case -1:
                    {
                        processPrimaryText.Text = "通常以下";
                        break;
                    }
                case 0:
                    {
                        processPrimaryText.Text = "通常";
                        break;
                    }
                case 1:
                    {
                        processPrimaryText.Text = "通常以上";
                        break;
                    }
                case 2:
                    {
                        processPrimaryText.Text = "高";
                        break;
                    }
                default:
                    {
                        processPrimaryText.Text = "通常";
                        break;
                    }
            }

            //Vsyncが正常に動作しない場合
            if(vpatchValues["LockBackBuffer"] == 1)
            {
                checkBox4.Checked = true;
            }
            else
            {
                checkBox4.Checked = false;
            }

            //ダブルスポイラー以降の特殊設定
            if(titleName == "Th125")
            {
                //Direct3Dをマルチスレッドで動かす
                if (vpatchValues["D3DMultiThread"] == 1)
                {
                    D3DMultiThredforDubleSpoiler.Checked = true;
                }
                else
                {
                    D3DMultiThredforDubleSpoiler.Checked = false;
                }
                //DirectInputをVpatch側で制御
                if(vpatchValues["HookDirectInput"] == 1)
                {
                    DirectInput_DoubleSpoiler.Checked = true;
                }
                else
                {
                    DirectInput_DoubleSpoiler.Checked = false;
                }
            }
            else
            {
                ダブルスポイラー以降.Enabled = false;
            }
            
            //東方神霊廟のみ
            if(titleName == "Th13")
            {
                //Direct3Dをマルチスレッドで動かす
                if(vpatchValues["D3DMultiThread"] == 1)
                {
                    D3DMultiThredforTenDesire.Enabled = true;
                }
                else
                {
                    D3DMultiThredforTenDesire.Enabled = false;
                }
                
                //バイリニアフィルタリングで拡大する
                if(vpatchValues["MagnificationMethod"] == 1)
                {
                    checkBox7.Checked = true;
                }
                else
                {
                    checkBox7.Checked = false;
                }

                //th13.exeのチェックサムを無効にする
                if(vpatchValues["DisableChecksum"] == 1)
                {
                    checksum.Enabled = true;
                }
                else
                {
                    checksum.Enabled = false;
                }
            }
            else
            {
                東方神霊廟のみ.Enabled = false;
            }

            #endregion
        }

        /*
         * 
         * ここからボタンが押された時の挙動
         * 
         */

        //決定ボタン
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("設定を変更致しますよろしいですか？", "お知らせ",MessageBoxButtons.OKCancel);
            if(DialogResult == DialogResult.OK)
            {
                //ここでVpatch.iniへの書き込み処理です
                this.Close();
            }
        }
        //キャンセルボタン
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //起動時にW or F を尋ねる
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == Enabled)
            {
                vpatchValues["AskWindowMode"] = 1;
            }
            else
            {
                vpatchValues["AskWindowMode"] = 0;
            }
        }
        //Vsync側でWindowのサイズをしていするか
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked == true)
            {
                vpatchValues["enabled"] = 1;
                詳細設定.Enabled = true;
            }
            else
            {
                vpatchValues["enabled"] = 0;
                詳細設定.Enabled = false;
            }
        }

        //描画位置とかをいじるためのチェックボックス
        private void 詳細設定_Enter(object sender, EventArgs e)
        {
            if(詳細設定.Enabled == false)
            {
                MessageBox.Show("設定をいじるには Vsync側でWindowのサイズを指定するにチェックを入れてください。", "お知らせ");
            }
        }

        //描画位置の指定(X)
        private void 描画位置X_Text_TextChanged(object sender, EventArgs e)
        {
            int valueX;
            Int32.TryParse(描画位置X_Text.Text ,out valueX);
            vpatchValues["X"] = valueX;
        }

        //描画位置の指定（Y）
        private void 描画位置Y_Text_TextChanged(object sender, EventArgs e)
        {
            int valueY;
            Int32.TryParse(描画位置Y_Text.Text, out valueY);
            vpatchValues["Y"] = valueY;
        }

        //ウィンドウの幅
        private void Width_Text_TextChanged(object sender, EventArgs e)
        {
            int valueW;
            Int32.TryParse(Width_Text.Text, out valueW);
            vpatchValues["Width"] = valueW;
        }
        
        //ウィンドウの高さ
        private void Height_Text_TextChanged(object sender, EventArgs e)
        {
            int valueH;
            Int32.TryParse(Height_Text.Text, out valueH);
            vpatchValues["Height"] = valueH;
        }
        
        //タイトルバーの表示の有無
        private void ShowTitleBar_CheckedChanged(object sender, EventArgs e)
        {
            if(ShowTitleBar.Checked == true)
            {
                vpatchValues["TitleBar"] = 1;
            }
            else
            {
                vpatchValues["TitleBar"] = 0;
            }
        }

        //常に手前に表示
        private void AlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            if(AlwaysOnTop.Checked == true)
            {
                vpatchValues["AlwaysOnTop"] = 1;
            }
            else
            {
                vpatchValues["AlwaysOnTop"] = 0;
            }
        }

        //垂直同期設定（本家）
        private void G1_OriginalDrawing_CheckedChanged(object sender, EventArgs e)
        {
            if(G1_OriginalDrawing.Checked == true)
            {
                vpatchValues["Vsync"] = 0;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //非アクティブのやつ
        private void 非アクティブでも描画_CheckedChanged(object sender, EventArgs e)
        {
            if (非アクティブでも描画.Checked == true)
            {
                vpatchValues["AlwaysBlt"] = 1;
            }
            else
            {
                vpatchValues["AlwaysBlt"] = 0;
            }
        }

        //Th13バイリニアフィルタリング
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox7.Checked == true)
            {
                vpatchValues["D3DMultiThread"] = 1;
            }
            else
            {
                vpatchValues["D3DMultiThread"] = 0;
            }
        }

        //th13Checksum
        private void checksum_CheckedChanged(object sender, EventArgs e)
        {
            //th13.exeのチェックサムを無効にする
            if(checksum.Checked == true)
            {
                vpatchValues["DisableChecksum"] = 1;
            }
            else
            {
                vpatchValues["DisableChecksum"] = 0;
            }
        }

        //DirectInput_DoubleSpoiler
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if(DirectInput_DoubleSpoiler.Checked == true)
            {
                vpatchValues["HookDirectInput"] = 1;
            }
            else
            {
                vpatchValues["HookDirectInput"] = 0;
            }
        }

        //D3DMultiTread(DS)
        private void D3DMultiThredforDubleSpoiler_CheckedChanged(object sender, EventArgs e)
        {
            //Direct3Dをマルチスレッドで動かす
            if (D3DMultiThredforDubleSpoiler.Checked == true)
            {
                vpatchValues["D3DMultiThread"] = 1;
            }
            else
            {
                vpatchValues["D3DMultiThread"] = 0;
            }
        }
        
        //D3DMultiTread(TedDesire)
        private void D3DMultiThredforDubleSpoilerforTenDesire_CheckedChanged(object sender, EventArgs e)
        {
            //Direct3Dをマルチスレッドで動かす
            if (D3DMultiThredforTenDesire.Checked == true)
            {
                vpatchValues["D3DMultiThread"] = 1;
            }
            else
            {
                vpatchValues["D3DMultiThread"] = 0;
            }
        }

        //LockBackBuffer
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox4.Checked == true)
            {
                vpatchValues["LockBackBuffer"] = 1;
            }
            else
            {
                vpatchValues["LockBackBuffer"] = 0;
            }
        }
        
        //プロセス優先度の変更
        private void processPrimaryTrackBar_Scroll(object sender, EventArgs e)
        {
            //値の変更
            vpatchValues["ProcessPriority"] = processPrimaryTrackBar.Value;

            //下のラベルの文字の変更 
            switch (processPrimaryTrackBar.Value)
             {
                case -2:
                    {
                        processPrimaryText.Text = "低";
                        break;
                    }
                case -1:
                    {
                        processPrimaryText.Text = "通常以下";
                        break;
                    }
                case 0:
                    {
                        processPrimaryText.Text = "通常";
                        break;
                    }
                case 1:
                    {
                        processPrimaryText.Text = "通常以上";
                        break;
                    }
                case 2:
                    {
                        processPrimaryText.Text = "高";
                        break;
                    }
                default:
                    {
                        processPrimaryText.Text = "通常";
                        break;
                    }
             }
        }

        

        

        

        

        

        

        





        

        
    }
}
