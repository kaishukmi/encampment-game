using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.SmallBasic.Library;

namespace ColorKillerカラキラ
{
    class MusicPlayerExam
    {
        string fileName = "./bgm/c1.mp3";
        private const int SongQuan = 5;         //総曲数

        public int GetSongQuan()
        {
            return SongQuan;
        }

        public void MusicPlay(int musicnum)
        {
            
            switch (musicnum)
            {
                case 0:
                    fileName = "./bgm/c1.mp3";
                    break;
                case 1:
                    fileName = "./bgm/c2.mp3";
                    break;
                case 2:
                    fileName = "./bgm/c3.mp3";
                    break;
                case 3:
                    fileName = "./bgm/c4.mp3";
                    break;
                case 4:
                    fileName = "./bgm/c5.mp3";
                    break;
            }

            Sound.Play(fileName);
            //ファイルを開く
            
        }

        public void MusicStop()
        {
            Sound.Stop(fileName);
        }
    }
}
