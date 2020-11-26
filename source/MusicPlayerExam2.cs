using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorKillerカラキラ
{
    class MusicPlayerExam2
    {
        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern int mciSendString(String command,
          StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        private string aliasName = "MediaFile";

        // ボタン(演奏)がクリックされた時の処理
        public void MusicPlay()
        {
            // 再生するファイル名
            string fileName = "./bgm/c1.mp3";

            string cmd;

            // ファイルを開く
            cmd = "open \"" + fileName + "\" type mpegvideo alias " + aliasName;
            if (mciSendString(cmd, null, 0, IntPtr.Zero) != 0)
                return;

            // 再生する
            cmd = "play " + aliasName;
            mciSendString(cmd, null, 0, IntPtr.Zero);
        }

        // ボタン(停止)がクリックされた時の処理
        public void MusicStop()
        {
            string cmd;

            // 再生して居るWAVEを停止する
            cmd = "stop " + aliasName;
            mciSendString(cmd, null, 0, IntPtr.Zero);

            // 閉じる
            cmd = "close " + aliasName;
            mciSendString(cmd, null, 0, IntPtr.Zero);
        }
    }
}
