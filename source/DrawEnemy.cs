using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorKillerカラキラ
{
    class DrawEnemy
    {
        private System.Drawing.Bitmap inkballLimg;
        private System.Drawing.Bitmap inkballMimg;
        private System.Drawing.Bitmap inkballSimg;
        private System.Drawing.Bitmap inckholeblack1img;
        private System.Drawing.Bitmap inckholeblack2img;
        private System.Drawing.Bitmap inckholeblack3img;
        private System.Drawing.Bitmap inckholewhite1img;
        private System.Drawing.Bitmap inckholewhite2img;
        private System.Drawing.Bitmap inckholewhite3img;
        private System.Drawing.Bitmap inkballLLLimg;
        private System.Drawing.Bitmap inkballLLLbodyimg;
        private System.Drawing.Bitmap inkballLLLeyeimg;
        private System.Drawing.Bitmap inkvaderimg;

        public DrawEnemy()
        {
            inkballLimg = new System.Drawing.Bitmap("./img/InkBall/InkBallL.png");
            inkballMimg = new System.Drawing.Bitmap("./img/InkBall/InkBallM.png");
            inkballSimg = new System.Drawing.Bitmap("./img/InkBall/InkBallS.png");
            inckholeblack1img = new System.Drawing.Bitmap("./img/InckHole/InckHoleBlack1.png");
            inckholeblack2img = new System.Drawing.Bitmap("./img/InckHole/InckHoleBlack2.png");
            inckholeblack3img = new System.Drawing.Bitmap("./img/InckHole/InckHoleBlack3.png");
            inckholewhite1img = new System.Drawing.Bitmap("./img/InckHole/InckHoleWhite1.png");
            inckholewhite2img = new System.Drawing.Bitmap("./img/InckHole/InckHoleWhite2.png");
            inckholewhite3img = new System.Drawing.Bitmap("./img/InckHole/InckHoleWhite3.png");
            inkballLLLimg = new System.Drawing.Bitmap("./img/InkBallLLL/InkBallLLL.png");
            inkballLLLbodyimg = new System.Drawing.Bitmap("./img/InkBallLLL/InkBallLLLbody.png");
            inkballLLLeyeimg = new System.Drawing.Bitmap("./img/InkBallLLL/InkBallLLLeye.png");
            inkvaderimg = new System.Drawing.Bitmap("./img/InkVader/InkVader.png");
        }

        struct Enemyes
        {
            public int objectclass;     //敵番号
            public bool live;           //行動可能状態かどうか
            public double x;            //x座標
            public double y;            //y座標
            public int co1;             //以下5つ計算用領域
            public int co2;
            public int co3;
            public double dco1;
            public double dco2;
        }

        /*public void draws(Enemyes[] enemy, int enemynum, Graphics g)
        {
            //g.DrawImage(canvas, 0, 0, 1000, 700);
            for (int i = 1; i <= enemynum; ++i)
            {

                switch (enemy[i].objectclass)
                {
                    case 1:
                        g.DrawImage(inkballLimg, (int)enemy[i].x, (int)enemy[i].y, 100, 100);
                        break;
                    case 2:
                        g.DrawImage(inkballMimg, (int)enemy[i].x, (int)enemy[i].y, 50, 50);
                        break;
                    case 3:
                        g.DrawImage(inkballSimg, (int)enemy[i].x, (int)enemy[i].y, 25, 25);
                        break;
                    case 4:
                        g.DrawImage(inckholeblack1img, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                        break;
                    case 5:
                        g.DrawImage(inckholewhite1img, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                        break;
                    case 6:
                        g.DrawImage(inkballLLLimg, (int)enemy[i].x, (int)enemy[i].y, 200, 200);
                        break;
                    case 7:
                        g.DrawImage(inkvaderimg, (int)enemy[i].x, (int)enemy[i].y, 1000, 700);
                        break;
                    case 8:

                        break;
                }
            }
        }*/
    }
}
