﻿using System;
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
        private Dictionary<string, string> vpatchValues;
        private string _FilePath;

        public VpatchGUI()
        {
            InitializeComponent();
            //閉じるボタンとか消すやつ
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ControlBox = !this.ControlBox;
            //this.trackBar1.ValueChanged += trackBar1_ValueChanged;
        }
        private void VpatchGUI_Load(object sender, EventArgs e)
        {

        }

        /*
         * 事前の設定
         * 
         * 
         */
        //イニシャライズ
        public void initialize(string title)
        {

        }

        //値をセットする
        public void setValues(Dictionary<string,string> values,string titleName,string FilePath)
        {
            //値を取得
            vpatchValues = values;
            this._FilePath = FilePath;
            //Dictionary<string, int> vpatchValues = values;           


            /*
             *ココらへんから取得した値をセットしたり
             * 使わない設定をenableとかします
             * 
             */
            #region Window
            /*
             * ここからWindow設定
             * 
             */

            //起動時にWindow or FullScreenを尋ねる
            if(vpatchValues["AskWindowMode"] == "1")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            //Vsync側でWindowのサイズを指定する
            if(vpatchValues["enabled"] == "1")
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
            if(vpatchValues["TitleBar"] == "1")
            {
                ShowTitleBar.Checked = true;
            }
            else
            {
                ShowTitleBar.Checked = false;
            }

            //常に手前で表示させる
            if(vpatchValues["AlwaysOnTop"] == "1")
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
            //rev7以降
            垂直同期設定ダブルスポイラー以降.Enabled = false;

            //垂直同期設定（rev7以降）
            if((titleName == "th095") || (titleName == "th10") ||(titleName == "alcostg") || (titleName == "th11") || (titleName == "th12")
                || (titleName == "th125") || (titleName == "th128"))
            {
                垂直同期設定ダブルスポイラー以降.Enabled = true;
                switch(vpatchValues["Vsync"])
                {
                    case "-1":
                        {
                            //東方本家の描画方法で描画
                            G2_OriginalDrawing.Checked = true;
                            break;
                        }
                    case "0":
                        {
                            //垂直同期なし（Vpatch制御）
                            G2_NoVsync.Checked = true;
                            break;
                        }
                    case "1":
                        {
                            //Vpatch独自の描画形式
                            G2_MonitorRefreshLate.Checked = true;
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
            else if(titleName == "th13")
            {
                垂直同期設定東方神霊廟.Enabled = true;
                switch (vpatchValues["Vsync"])
                {
                    case "-1":
                        {
                            //ハードウェア補助レベル１
                            G4_HardwareAssistLv1.Checked = true;
                            break;
                        }
                    case "-2":
                        {
                            //ハードウェア補助レベル２
                            G4_HardwareAssistLv2.Checked = true;
                            break;
                        }
                    case "0":
                        {
                            //東方本家の描画方法で描画
                            G4_OriginalDrawing.Checked = true;
                            break;
                        }
                    case "1":
                        {
                            //Vpatch独自の描画形式
                            G4_MonitorRefreshLate.Checked = true;
                            break;
                        }
                    default:
                        {
                            //東方本家の描画方法で描画
                            G4_OriginalDrawing.Checked = true;
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
                    case "0":
                        {
                            //東方本家の描画方法で描画
                            G1_OriginalDrawing.Checked = true;
                            break;
                        }
                    case "1":
                        {
                            //モニターのリフレッシュレートに合わせる
                            G1_MonitorRefreshLate.Checked = true;
                            break;
                        }
                    case "2":
                        {
                            //Vsync側で設定したFPSの値に近づけて描画
                            G1_VsyncFPS.Checked = true;
                            //FPSの設定はこの時の場合はOnにする
                            groupBox3.Enabled = true;
                            break;
                        }
                    case "3":
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
            //FPS設定（初期化）
            groupBox3.Enabled = false;

            //FPS制御の設定
            switch(vpatchValues["SleepType"])
            {
                case "-1":
                    {
                        FPSAutoControl.Checked = true;
                        break;
                    }
                case "0":
                    {
                        FPSControlOff.Checked = true;
                        break;
                    }
                case "1":
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
            //GameFPSの設定(vsyncが1ではない時のみ有効)
            if (vpatchValues["Vsync"] != "1")
            {
                groupBox3.Enabled = true;
            }

            //GameFPS
            GameFPS_Text.Text = vpatchValues["GameFPS"].ToString();

            //ReplaySkipFPS
            ReplaySkipFPS_Text.Text = vpatchValues["ReplaySkipFPS"].ToString();

            //ReplaySlowFPS
            ReplaySlowFPS_Text.Text = vpatchValues["ReplaySlowFPS"].ToString();

            //描画設定
            if(vpatchValues["CalcFPS"] == "1")
            {
                //自動計測のチェックボックス
                PrecedingDrawAutoMeasurement.Checked = true;
                //自動計測のチェックボックスがオンならばトラックバーは利用できない
                先行描画設定TrackBar.Enabled = false;
            }
            else
            {
                PrecedingDrawAutoMeasurement.Checked = false;
                //自動計測のチェックボックスがオフなので先行描画設定を利用する
                PrecedingDrawTrackBar_Value.Text = "値 ＝ " + 先行描画設定TrackBar.Value.ToString();
            }

            if(vpatchValues["BltPrepareTime"] == "-1")
            {
                //先行描画の値が -1 であるのならば、自動計測のチェックボックスをオンにする
                PrecedingDrawAutoMeasurement.Checked = true;
            }
            else
            {
                //先行描画の値を取得
                先行描画設定TrackBar.Value = int.Parse(vpatchValues["BltPrepareTime"]);
            }

            //FPSの独自の自動計測
            if (vpatchValues["CalcFPS"] == "1")
            {
                FPS値を独自方法で計算します.Checked = true;
            }
            else
            {
                FPS値を独自方法で計算します.Checked = false;
            }

            //非アクティブでも描画
            if(vpatchValues["AlwaysBlt"] == "1")
            {
                AlwaysBlt_CheckBox.Checked = true;
            }
            else
            {
                AlwaysBlt_CheckBox.Checked = false;
            }
            
            //妖々夢の桜点バグを修正
            if(titleName == "th07")
            {
                if (vpatchValues["BugFixCherry"] == "1")
                {
                    BugFixCherry_Checkbox.Checked = true;
                }
                else
                {
                    BugFixCherry_Checkbox.Checked = false;
                }
            }
            else
            {
                BugFixCherry_Checkbox.Enabled = false;
            }

            //風神録のバグマリを修正する
            if(titleName == "th10")
            {
                if (vpatchValues["BugFixTh10Power3"] == "1")
                {
                    BugFixTh10Power.Checked = true;
                }
                else
                {
                    BugFixTh10Power.Checked = false;
                }
            }
            else
            {
                BugFixTh10Power.Enabled = false;
            }

            //星蓮船の聖輦船の影を修正します
            if(titleName == "th12")
            {
                if (vpatchValues["BugFixTh12Shadow"] == "1")
                {
                    BugFixTh12Shadow_Checkbox.Checked = true;
                }
                else
                {
                    BugFixTh12Shadow_Checkbox.Checked = false;
                }
            }
            else
            {
                BugFixTh12Shadow_Checkbox.Enabled = false;
            }

            //入力の暴走を修正します
            if (vpatchValues["BugFixGetDeviceState"] == "1")
            {
                HookDirectInput_CheckBox.Checked = true;
            }
            else
            {
                HookDirectInput_CheckBox.Checked = false;
            }

            //フレームスキップをなるべく感じさせないようにする
            if (vpatchValues["AllowShortDelay"] == "1")
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
                case "0":
                    {
                        radioButton1.Checked = true;
                        break;
                    }
                case "1":
                    {
                        CPU0Work.Checked = true;
                        break;
                    }
                case "2":
                    {
                        CPU1Work.Checked = true;
                        break;
                    }
                case "3":
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
            processPrimaryTrackBar.Value = int.Parse(vpatchValues["ProcessPriority"]);
            switch (vpatchValues["ProcessPriority"])
            {
                case "-2":
                    {
                        processPrimaryText.Text = "低";
                        break;
                    }
                case "-1":
                    {
                        processPrimaryText.Text = "通常以下";
                        break;
                    }
                case "0":
                    {
                        processPrimaryText.Text = "通常";
                        break;
                    }
                case "1":
                    {
                        processPrimaryText.Text = "通常以上";
                        break;
                    }
                case "2":
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
            if(vpatchValues["LockBackBuffer"] == "1")
            {
                VpatchDoesnotWork.Checked = true;
            }
            else
            {
                VpatchDoesnotWork.Checked = false;
            }

            //rev7対応分の特殊設定
            if ((titleName == "th09") || (titleName == "th10") || (titleName == "alcostg") || (titleName == "th11") || (titleName == "th12")
                || (titleName == "th125") || (titleName == "th128"))
            {
                //Direct3Dをマルチスレッドで動かす
                if (vpatchValues["D3DMultiThread"] == "1")
                {
                    D3DMultiThredforDubleSpoiler.Checked = true;
                }
                else
                {
                    D3DMultiThredforDubleSpoiler.Checked = false;
                }
                //DirectInputをVpatch側で制御
                if(vpatchValues["HookDirectInput"] == "1")
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
                VsyncRev7_Vsyncth128_HardcoreSettings.Enabled = false;
            }
            
            //東方神霊廟のみ
            if(titleName == "th13")
            {
                D3DMultiThredforTenDesire.Enabled = true;
                Bilinearfiltering.Enabled = true;
                checksum.Enabled = true;
                //Direct3Dをマルチスレッドで動かす
                if(vpatchValues["D3DMultiThread"] == "1")
                {
                    D3DMultiThredforTenDesire.Checked = true;
                }
                else
                {
                    D3DMultiThredforTenDesire.Checked = false;
                }
                
                //バイリニアフィルタリングで拡大する
                if(vpatchValues["MagnificationMethod"] == "1")
                {
                    Bilinearfiltering.Checked = true;
                }
                else
                {
                    Bilinearfiltering.Checked = false;
                }

                //th13.exeのチェックサムを無効にする
                if(vpatchValues["DisableChecksum"] == "1")
                {
                    checksum.Checked = true;
                }
                else
                {
                    checksum.Checked = false;
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
        #region Window

        //起動時にWindow or FullScreen を尋ねる
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == Enabled)
            {
                vpatchValues["AskWindowMode"] = "1";
            }
            else
            {
                vpatchValues["AskWindowMode"] = "0";
            }
        }
        //Vsync側でWindowのサイズをしていするか
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked )
            {
                vpatchValues["enabled"] = "1";
                詳細設定.Enabled = true;
            }
            else
            {
                vpatchValues["enabled"] = "0";
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
            vpatchValues["X"] = 描画位置X_Text.Text;
        }

        //描画位置の指定（Y）
        private void 描画位置Y_Text_TextChanged(object sender, EventArgs e)
        {
            vpatchValues["Y"] = 描画位置Y_Text.Text;
        }

        //ウィンドウの幅
        private void Width_Text_TextChanged(object sender, EventArgs e)
        {
            vpatchValues["Width"] = Width_Text.Text;
        }
        
        //ウィンドウの高さ
        private void Height_Text_TextChanged(object sender, EventArgs e)
        {
            vpatchValues["Height"] = Height_Text.Text;
        }
        
        //タイトルバーの表示の有無
        private void ShowTitleBar_CheckedChanged(object sender, EventArgs e)
        {
            if(ShowTitleBar.Checked )
            {
                vpatchValues["TitleBar"] = "1";
            }
            else
            {
                vpatchValues["TitleBar"] = "0";
            }
        }

        //常に手前に表示
        private void AlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            if(AlwaysOnTop.Checked )
            {
                vpatchValues["AlwaysOnTop"] ="1";
            }
            else
            {
                vpatchValues["AlwaysOnTop"] ="0";
            }
        }
        #endregion
        #region 垂直同期設定VsyncPatchRev6
        //垂直同期設定（本家）
        private void G1_OriginalDrawing_CheckedChanged(object sender, EventArgs e)
        {
            if(G1_OriginalDrawing.Checked)
            {
                vpatchValues["Vsync"] = "0";
                if (!groupBox3.Enabled)
                {
                    groupBox3.Enabled = true;
                }
            }
        }
        //垂直同期設定（モニターのリフレッシュレートに合わせる）
        private void G1_MonitorRefreshLate_CheckedChanged(object sender, EventArgs e)
        {
            if(G1_MonitorRefreshLate.Checked)
            {
                vpatchValues["Vsync"] = "1";
                if(groupBox3.Enabled)
                {
                    groupBox3.Enabled = false;
                }
            }
        }
        //Vsync側で設定したFPS値に近づけて描画
        private void G1_VsyncFPS_CheckedChanged(object sender, EventArgs e)
        {
            if(G1_VsyncFPS.Checked)
            {
                vpatchValues["Vsync"] = "2";
                if (!groupBox3.Enabled)
                {
                    groupBox3.Enabled = true;
                }
            }

        }
        //画面の真ん中よりしたで操作する人向け
        private void G1_UnderDrawing_CheckedChanged(object sender, EventArgs e)
        {
            if(G1_UnderDrawing.Checked)
            {
                vpatchValues["Vsync"] = "3";
                if (!groupBox3.Enabled)
                {
                    groupBox3.Enabled = true;
                }
            }
        }
        #endregion
        #region 垂直同期設定VsyncPatchRev7
        //本家の描画設定
        private void G2_OriginalDrawing_CheckedChanged(object sender, EventArgs e)
        {
            if (G2_OriginalDrawing.Checked)
            {
                vpatchValues["Vsync"] = "-1";
                if (!groupBox3.Enabled)
                {
                    groupBox3.Enabled = true;
                }
            }
        }
        //垂直同期なし(Vpatch制御)
        private void G2_NoVsync_CheckedChanged(object sender, EventArgs e)
        {
            if (G2_NoVsync.Checked)
            {
                vpatchValues["Vsync"] = "0";
                if (!groupBox3.Enabled)
                {
                    groupBox3.Enabled = true;
                }
            }
        }
        //モニターのリフレッシュレートに合わせる
        private void G2_MonitorRefreshLate_CheckedChanged(object sender, EventArgs e)
        {
            if (G2_MonitorRefreshLate.Checked)
            {
                vpatchValues["Vsync"] = "1";
                if (groupBox3.Enabled)
                {
                    groupBox3.Enabled = false;
                }
            }
        }
        #endregion

        //th128はrev7と併用（使用せず）
        #region 垂直同期設定VsyncPatchRevth128
        /*
        //本家の描画
        private void G3_OriginalDraw_CheckedChanged(object sender, EventArgs e)
        {
            if (G3_OriginalDraw.Checked)
            {
                vpatchValues["Vsync"] = "-1";
            }
        }
        //垂直同期なし
        private void G3_NoVsync_CheckedChanged(object sender, EventArgs e)
        {
            if (G3_NoVsync.Checked)
            {
                vpatchValues["Vsync"] = "0";
            }
        }
        //モニターのリフレッシュレートに合わせる
        private void G3_MonitorRefreshLate_CheckedChanged(object sender, EventArgs e)
        {
            if (G3_MonitorRefreshLate.Checked)
            {
                vpatchValues["Vsync"] = "1";
            }
        }
         * */
        #endregion

        #region 垂直同期設定VsyncPathcRevth13

        //垂直同期なし
        private void G4_OriginalDrawing_CheckedChanged(object sender, EventArgs e)
        {
            if (G4_OriginalDrawing.Checked)
            {
                vpatchValues["Vsync"] = "0";
                if (!groupBox3.Enabled)
                {
                    groupBox3.Enabled = true;
                }
            }
        }
        //モニターのリフレッシュレートに合わせる
        private void G4_MonitorRefreshLate_CheckedChanged(object sender, EventArgs e)
        {
            if (G4_MonitorRefreshLate.Checked)
            {
                vpatchValues["Vsync"] = "1";
                if (groupBox3.Enabled)
                {
                    groupBox3.Enabled = false;
                }
            }
        }
        //ハードウェア補助レベル１
        private void G4_HardwareAssistLv1_CheckedChanged(object sender, EventArgs e)
        {
            if (G4_HardwareAssistLv1.Checked)
            {
                vpatchValues["Vsync"] = "-1";
                if (!groupBox3.Enabled)
                {
                    groupBox3.Enabled = true;
                }
            }
        }
        //ハードウェア補助レベル２
        private void G4_HardwareAssistLv2_CheckedChanged(object sender, EventArgs e)
        {
            if (G4_HardwareAssistLv2.Checked)
            {
                vpatchValues["Vsync"] = "-2";
                if (!groupBox3.Enabled)
                {
                    groupBox3.Enabled = true;
                }
            }
        }
        #endregion
        #region FPS制御の設定
        //FPS制御をONにする(SleepType)
        private void FPSControlOn_CheckedChanged(object sender, EventArgs e)
        {
            if (FPSControlOn.Checked)
            {
                vpatchValues["SleepType"] = "1";
            }
        }
        //FPS制御をしない
        private void FPSControlOff_CheckedChanged(object sender, EventArgs e)
        {
            if (FPSControlOff.Checked)
            {
                vpatchValues["SleepType"] = "0";
            }
        }
        //FPS制御を自動制御する
        private void FPSAutoControl_CheckedChanged(object sender, EventArgs e)
        {
            if (FPSAutoControl.Checked)
            {
                vpatchValues["SleepType"] = "-1";
            }
        }
        #endregion
        #region 再生FPS
        //ゲーム上のFPSの変更
        private void GameFPS_Text_TextChanged(object sender, EventArgs e)
        {
            vpatchValues["GameFPS"] = GameFPS_Text.Text;
        }


        //リプレイスキップ時のFPS
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            vpatchValues["ReplaySkipFPS"] = ReplaySkipFPS_Text.Text;
        }
        //リプレイスロー時のFPS
        private void ReplaySlowFPS_TextChanged(object sender, EventArgs e)
        {
            vpatchValues["ReplaySlowFPS"] = ReplaySlowFPS_Text.Text;
        }
        #endregion
        #region 描画設定
        //先行描画設定の自動計測
        private void PrecedingDrawAutoMeasurement_CheckedChanged(object sender, EventArgs e)
        {
            if (PrecedingDrawAutoMeasurement.Checked)
            {
                vpatchValues["AutoBltPrepareTime"] = "1";
            }
            else
            {
                vpatchValues["AutoBltPrepareTime"] = "0";
            }
        }
        //先行描画設定の値
        private void PrecedingDrawTrackBar_Scroll(object sender, EventArgs e)
        {
            vpatchValues["BltPrepareTime"] = 先行描画設定TrackBar.Value.ToString();
            PrecedingDrawTrackBar_Value.Text = "値 = " + 先行描画設定TrackBar.Value.ToString();
        }
        //値表示用
        private void PrecedingDrawTrackBar_Value_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region バグフィックス
        //妖々夢のFPSの値が変なとき
        private void FPS値を独自方法で計算します_CheckedChanged(object sender, EventArgs e)
        {
            if (FPS値を独自方法で計算します.Checked)
            {
                vpatchValues["CalcFPS"] = "1";
            }
            else
            {
                vpatchValues["CalcFPS"] = "0";
            }
        }
        //非アクティブのやつ
        private void 非アクティブでも描画_CheckedChanged(object sender, EventArgs e)
        {
            if (AlwaysBlt_CheckBox.Checked)
            {
                vpatchValues["AlwaysBlt"] = "1";
            }
            else
            {
                vpatchValues["AlwaysBlt"] = "0";
            }
        }
        //妖々夢の桜点バグ
        private void BugFixCherry_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (BugFixCherry_Checkbox.Checked)
            {
                vpatchValues["BugFixCherry"] = "1";
            }
            else
            {
                vpatchValues["BugFixCherry"] = "0";
            }
        }
        //風神録の魔理沙のバグ
        private void BugFixTh10Power_CheckedChanged(object sender, EventArgs e)
        {
            if (BugFixTh10Power.Checked)
            {
                vpatchValues["BugFixTh10Power3"] = "1";
            }
            else
            {
                vpatchValues["BugFixTh10Power3"] = "0";
            }
        }

        //星蓮船の影バグ
        private void BugFixTh12Shadow_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (BugFixTh12Shadow_Checkbox.Checked)
            {
                vpatchValues["BugFixTh12Shadow"] = "1";
            }
            else
            {
                vpatchValues["BugFixTh12Shadow"] = "0";
            }
        }
        //入力の暴走(rev7)
        private void HookDirectInput_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (HookDirectInput_CheckBox.Checked)
            {
                vpatchValues["HookDirectInput"] = "1";
            }
            else
            {
                vpatchValues["HookDirectInput"] = "0";
            }
        }
        #endregion
        #region ハードコア
        //動作させるcpuコア数
        //特に指定しない
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                vpatchValues["ProcessAffinityMask"] = "0";
            }
        }
        //CPU 0　で動作させる
        private void CPU0Work_CheckedChanged(object sender, EventArgs e)
        {
            if(CPU0Work.Checked)
            {
                vpatchValues["ProcessAffinityMask"] = "1";
            }
        }
        //CPU 1 で動作させる
        private void CPU1Work_CheckedChanged(object sender, EventArgs e)
        {
            if(CPU1Work.Checked)
            {
                vpatchValues["ProcessAffinityMask"] = "2";
            }
        }
        //CPU　0 と COU 1　で動作させる
        private void CPU0_CPU1Work_CheckedChanged(object sender, EventArgs e)
        {
            if(CPU0_CPU1Work.Checked)
            {
                vpatchValues["ProcessAffinityMask"] = "3";
            }
        }
        //DirectInput_DoubleSpoiler
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if(DirectInput_DoubleSpoiler.Checked )
            {
                vpatchValues["HookDirectInput"] = "1";
            }
            else
            {
                vpatchValues["HookDirectInput"] = "0";
            }
        }

        //D3DMultiTread(DS)
        private void D3DMultiThredforDubleSpoiler_CheckedChanged(object sender, EventArgs e)
        {
            //Direct3Dをマルチスレッドで動かす
            if (D3DMultiThredforDubleSpoiler.Checked )
            {
                vpatchValues["D3DMultiThread"] = "1";
            }
            else
            {
                vpatchValues["D3DMultiThread"] = "0";
            }
        }
        
        //D3DMultiTread(TendDesire)
        private void D3DMultiThredforDubleSpoilerforTenDesire_CheckedChanged(object sender, EventArgs e)
        {
            //Direct3Dをマルチスレッドで動かす
            if (D3DMultiThredforTenDesire.Checked )
            {
                vpatchValues["D3DMultiThread"] = "1";
            }
            else
            {
                vpatchValues["D3DMultiThread"] = "0";
            }
        }
        //th13Checksum
        private void checksum_CheckedChanged(object sender, EventArgs e)
        {
            //th13.exeのチェックサムを無効にする
            if (checksum.Checked)
            {
                vpatchValues["DisableChecksum"] = "1";
            }
            else
            {
                vpatchValues["DisableChecksum"] = "0";
            }
        }
        //Th13バイリニアフィルタリング
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (Bilinearfiltering.Checked)
            {
                vpatchValues["D3DMultiThread"] = "1";
            }
            else
            {
                vpatchValues["D3DMultiThread"] = "0";
            }
        }

        //LockBackBuffer
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if(VpatchDoesnotWork.Checked )
            {
                vpatchValues["LockBackBuffer"] = "1";
            }
            else
            {
                vpatchValues["LockBackBuffer"] = "0";
            }
        }
        
        //プロセス優先度の変更
        private void processPrimaryTrackBar_Scroll(object sender, EventArgs e)
        {
            //値の変更
            vpatchValues["ProcessPriority"] = processPrimaryTrackBar.Value.ToString();

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
        #endregion

        //決定ボタン
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("設定を変更致しますよろしいですか？", "お知らせ", MessageBoxButtons.OKCancel);
            if (DialogResult == DialogResult.OK)
            {
                //ここでVpatch.iniへの書き込み処理です
                algo.IniFileValueWrite.IniFileWriter(vpatchValues, _FilePath);
                this.Close();
            }
        }
        //キャンセルボタン
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("キャンセルしますか？", "お知らせ", MessageBoxButtons.OKCancel);
            if(DialogResult == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void VsyncRev7_Vsyncth128_HardcoreSettings_Enter(object sender, EventArgs e)
        {

        }
    }
}
