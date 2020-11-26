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
    class ck : Form
    {
        
        #region VariableDeclaration
        //プログラム制御
        private string stCurrentDir = System.IO.Directory.GetCurrentDirectory();
        private System.Drawing.Text.PrivateFontCollection pfc = new System.Drawing.Text.PrivateFontCollection();
        private System.Drawing.Font HPdisp;
        private System.Drawing.Font TIMEdisp;
        private System.Drawing.Font HITdisp;
        private int FontNum = 0;                        //現在選択されているフォントの添字
        //以下描画andゲーム画面制御
        private System.Windows.Forms.PictureBox paintbox;       //以下2行描画空間
        Bitmap canvas; //= new Bitmap(paintbox.Width, paintbox.Height);
        Bitmap canvasl2; //= new Bitmap(paintbox.Width, paintbox.Height);
        private bool bStart;                            //マウスがクリックされている状態かそうではないか
        private int mousex;                             //マウスのX座標
        private int mousey;                             //マウスのY座標
        private bool move = false;                      //マウスポインターを前回の描画から動かしたかどうか
        private int csize = 1;                          //現在描画される円の直径
        private int ctiming = 0;                        //次の描画までの移動フレーム数
        private Color colorname;                        //プレイヤーの現在の描画色
        private Color ebackcolor = Color.White;         //背景の描画色
        private Random rnd = new Random();              //乱数発生用             例えばrnd.Next(100)なら0-99までの乱数が発生する
        private bool[,] disp = new bool[1001, 701];     //どれだけ画面内が埋まったかをピクセルごとに
        private int fill = 0;                           //どれだけ画面内が埋まったか最高で700000
        //以下計算用領域
        private int k;
        private int kk;
        private bool bout;                              //ループ抜け判定
        private double[] usercos = new double[361];     //コサインの計算軽量化
        private double[] usersin = new double[361];     //サインの計算軽量化
        //angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballLsize / 2, mousex - enemy[i].x - enemyesdata.inkballLsize / 2);
        private double HUDsizecosR = Math.Cos(Math.Atan2(33,-24));
        private double HUDsizesinR = Math.Sin(Math.Atan2(33,-24));
        private double HUDsizecosL = Math.Cos(Math.Atan2(24,-33));
        private double HUDsizesinL = Math.Sin(Math.Atan2(24,-33));
        //以下ゲーム全体制御
        private int r = 0;
        private int g = 0;
        private int b = 0;
        private int dispr = 0;
        private int dispg = 0;
        private int dispb = 0;
        private int colormode = 0;                      //ゲーム全体の色初期値   getrandcolor5の引数と合わせる
        private byte gamemode = 96;                       //現在のゲームモード 96=オープニング 97=ステージセレクト 98=最初のステージの命令文 99=ゲーム画面 100=ステージリザルト&次のステージの導入
        private int openingcounter = 0;
        private int selectedstage = 29;                 //選択したステージ      29が選択されていない状態とする
        private string[] stageoption = new string[29];
        private int[] stagecleartarget = new int[29];
        private string[] stagename = new string[29];
        //オプション＆カラーチェンジ画面準備制御
        private int scr = 0;
        //カラーチェンジ画面制御
        private System.Windows.Forms.ComboBox enumcolor;
        private System.Windows.Forms.Panel changepanel;
        //オプション制御 + オプション画面制御
        private System.Windows.Forms.Panel optionpanel;
        private bool circlespritinkable = false;          //プレイヤーの描画するインクが跳ねるかどうか
        private bool circleantialiasing = true;        //プレイヤーの描画するインクにアンチエイリアスをかけるかどうか
        //ステージセレクト画面制御         ↓この辺の変数は全部構造体にしときゃよかったと後で後悔
        private System.Windows.Forms.Label[] selection = new Label[28];
        private const int selectsize = 90;              //仮     選択するボタンのサイズ
        private const int rex = 0;                      //x軸に対する補正
        private const int rey = 125;                    //y軸に対する補正
        private double movex;                           //選択完了時の移動X座標
        private double movey;                           //選択完了時の移動y座標
        private double movingx;                         //移動中のラベルのx座標の小数点以下計算用
        private double movingy;                         //移動中のラベルのy座標の小数点以下計算用
        private Point backing;                          //選択動作終了後に元の位置に戻す用
        private const int movetime = 20;                //移動にかかるフレーム数
        private int selectcount = 0;                    //ステージセレクトのインターフェース制御用
        private int enteredstage = 29;                  //現在マウスをかざしているステージ      29が範囲外を選択している状態とする
        private bool entering = false;                  //マウスをかざしているステージがあるかどうか
        private int enteringcounter = 0;                //マウスをかざしているステージの点滅用
        private Color enteringstrcolor;                 //点滅用文字色
        private Color enteringbackcolor;                //点滅用背景色
        //ゲーム前準備画面制御                98
        private bool throughselect;                     //ステージセレクト画面を通ったかどうか
        private int expcounter = 0;                     //説明のインタフェース制御用
        private System.Windows.Forms.Label white;       //ホワイトアウト用
        //ステージ情報表示画面制御            101
        private int dispcounter = 0;
        //ゲーム中制御
        int DrawLevel = 99999;                          //プレイヤーのHP
        int hitcounter = 0;
        bool drawing = false;
        int ENEMY_MY_CD;
        int ENEMY_MY_SIZE;
        int enemyenum;                                  //敵の数
        bool blackout = false;                          //ブラックアウト
        bool whiteout = false;                          //ホワイトアウト
        double angle = 0;
        double decision = 0;
        DateTime starttime = DateTime.Now;
        TimeSpan nowtime;
        TimeSpan cleartime;
        //音楽制御
        MusicPlayerExam mpe = new MusicPlayerExam();
        MusicPlayerExam2 mpe2 = new MusicPlayerExam2();
        //描画制御
        //DrawEnemy drawenemy = new DrawEnemy();
        //敵情報制御
        private const int enemysoat = 8;                      //敵の種類総数        敵番号 = 1.ballL 2.ballM 3.ballS 4.black 5.white 6.ballLLL 7.vader 8.killer
        private const int HittingDamageDisp = 200;        //ヒットしたときの画面を消す半径
        struct Enemyes
        {
            public int objectclass;     //敵番号
            public bool live;           //行動可能状態かどうか
            public int lifespan;        //寿命    0で不死 1以上で寿命有り
            public double x;            //x座標
            public double y;            //y座標
            public int co1;             //以下6つ計算用領域
            public int co2;
            public int co3;
            public int co4;
            public double dco1;
            public double dco2;
        }
        struct EnemyesData
        {
            public int inkballLsize;
            public int inkballMsize;
            public int inkballSsize;
            public int inckholesize;
            public int inkballLLLsize;
            public Size inkvadersize;
            public int inkballLcd;           //cd = 	collision detection当たり判定半径
            public int inkballMcd;
            public int inkballScd;
            public int inckholecd;
            public int inkballLLLcd;
            public int inkballLspeed;
            public int inkballMspeed;
            public int inkballSspeed;
            public int inckholespeedLow;
            public int inckholespeedMid;
            public int inckholespeedHigh;
            public int inkballLLLspeed;
            public int colorkillerblink;
        }

        private Enemyes[] enemy = new Enemyes[101];              //new Enemyes[n];   n-1が最大の敵表示数 可用性はあるはず...    とりま100にしておく
        private EnemyesData enemyesdata = new EnemyesData();

        //画像制御
        private System.Drawing.Bitmap Logoimg;
        private System.Drawing.Bitmap KMHOTFimg;
        private System.Drawing.Bitmap MPPimg;
        private System.Drawing.Bitmap HUDimg;
        private System.Drawing.Bitmap inkballLimg;
        private System.Drawing.Bitmap inkballMimg;
        private System.Drawing.Bitmap inkballSimg;
        private System.Drawing.Bitmap inckholeblack1img;
        private System.Drawing.Bitmap inckholeblack2img;
        private System.Drawing.Bitmap inckholeblack3img;
        private System.Drawing.Bitmap inckholeblackphantomimg;
        private System.Drawing.Bitmap inckholewhite1img;
        private System.Drawing.Bitmap inckholewhite2img;
        private System.Drawing.Bitmap inckholewhite3img;
        private System.Drawing.Bitmap inckholewhitephantomimg;
        private System.Drawing.Bitmap inkballLLLimg;
        private System.Drawing.Bitmap inkballLLLbodyimg;
        private System.Drawing.Bitmap inkballLLLeyeimg;
        private System.Drawing.Bitmap inkvaderimg;
        private System.Drawing.Bitmap colorkillerimg1;
        private System.Drawing.Bitmap colorkillerimg2;
        private System.Drawing.Bitmap colorkillerimg3;
        private System.Drawing.Bitmap colorkillerimg4;
        private System.Drawing.Bitmap colorkillerimg5;
        private System.Drawing.Bitmap colorkillerimg6;
        private System.Drawing.Bitmap colorkillerimg7;
        private System.Drawing.Bitmap colorkillerimg8;
        private System.Drawing.Bitmap colorkillerimg9;
        private System.Drawing.Bitmap colorkillerimg10;
        private System.Drawing.Bitmap colorkillerphantomimg;

        //フォームコントロール
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();//タイマー
        private System.Windows.Forms.Label label1;      //デバッグ用インターフェイス
        private System.Windows.Forms.PictureBox title;
        private System.Windows.Forms.Label option;
        private System.Windows.Forms.Label change;
        private System.Windows.Forms.Label clear;
        private System.Windows.Forms.Label menu;
        private System.Windows.Forms.Label replay;
        private System.Windows.Forms.Label next;
        private System.Windows.Forms.Label stagetitleout;
        private System.Windows.Forms.Label stagetitle;
        private System.Windows.Forms.Label target;
        private System.Windows.Forms.Label cleartimedisp;
        private System.Windows.Forms.ComboBox colorselectbox;

        #endregion

        public static void Main()
        {
            Application.Run(new ck());
        }

        public ck()
        {
            #region CalculateInit
            bStart = false;
            for (int i = 0; i < 360; i++)
            {
                usercos[i] = Math.Cos(i);
                usersin[i] = Math.Sin(i);
            }
            #endregion

            #region FontLoadInit
            pfc.AddFontFile("./img/kimberley bl.ttf");
            #endregion

            #region PictureLoadInit
            //画像読み込み
            Logoimg = new System.Drawing.Bitmap("./img/Logo/Logo1.png");
            KMHOTFimg = new System.Drawing.Bitmap("./img/Logo/KMHOTF.png");
            MPPimg = new System.Drawing.Bitmap("./img/Logo/MPP.png");
            HUDimg = new System.Drawing.Bitmap("./img/HUD.png");
            inkballLimg = new System.Drawing.Bitmap("./img/InkBall/InkBallL.png");
            inkballMimg = new System.Drawing.Bitmap("./img/InkBall/InkBallM.png");
            inkballSimg = new System.Drawing.Bitmap("./img/InkBall/InkBallS.png");
            inckholeblack1img = new System.Drawing.Bitmap("./img/InckHole/InckHoleBlack1.png");
            inckholeblack2img = new System.Drawing.Bitmap("./img/InckHole/InckHoleBlack2.png");
            inckholeblack3img = new System.Drawing.Bitmap("./img/InckHole/InckHoleBlack3.png");
            inckholeblackphantomimg = new System.Drawing.Bitmap("./img/InckHole/InckHoleBlackPhantom.png");
            inckholewhite1img = new System.Drawing.Bitmap("./img/InckHole/InckHoleWhite1.png");
            inckholewhite2img = new System.Drawing.Bitmap("./img/InckHole/InckHoleWhite2.png");
            inckholewhite3img = new System.Drawing.Bitmap("./img/InckHole/InckHoleWhite3.png");
            inckholewhitephantomimg = new System.Drawing.Bitmap("./img/InckHole/InckHoleWhitePhantom.png");
            inkballLLLimg = new System.Drawing.Bitmap("./img/InkBallLLL/InkBallLLL.png");
            inkballLLLbodyimg = new System.Drawing.Bitmap("./img/InkBallLLL/InkBallLLLbody.png");
            inkballLLLeyeimg = new System.Drawing.Bitmap("./img/InkBallLLL/InkBallLLLeye.png");
            inkvaderimg = new System.Drawing.Bitmap("./img/InkVader/InkVader.png");
            colorkillerimg1 = new System.Drawing.Bitmap("./img/ColorKiller/ColorKiller1.png");
            colorkillerimg2 = new System.Drawing.Bitmap("./img/ColorKiller/ColorKiller2.png");
            colorkillerimg3 = new System.Drawing.Bitmap("./img/ColorKiller/ColorKiller3.png");
            colorkillerimg4 = new System.Drawing.Bitmap("./img/ColorKiller/ColorKiller4.png");
            colorkillerimg5 = new System.Drawing.Bitmap("./img/ColorKiller/ColorKiller5.png");
            colorkillerimg6 = new System.Drawing.Bitmap("./img/ColorKiller/ColorKiller6.png");
            colorkillerimg7 = new System.Drawing.Bitmap("./img/ColorKiller/ColorKiller7.png");
            colorkillerimg8 = new System.Drawing.Bitmap("./img/ColorKiller/ColorKiller8.png");
            colorkillerimg9 = new System.Drawing.Bitmap("./img/ColorKiller/ColorKiller9.png");
            colorkillerimg10 = new System.Drawing.Bitmap("./img/ColorKiller/ColorKiller10.png");
            colorkillerphantomimg = new System.Drawing.Bitmap("./img/ColorKiller/ColorKillerPhantom.png");
            #endregion

            #region FormControlProperties
            //フォーム制御
            Text = "ColorKillers -カラキラ！-";
            Size = new Size(1001, 701);
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            FormBorderStyle = FormBorderStyle.FixedSingle;//通常はFixedSingle  デバッグモード時はNone
            BackColor = Color.White;
            MaximizeBox = false;
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(keypress);

            this.paintbox = new PictureBox();
            this.paintbox.Size = new Size(this.Width, this.Height);
            this.paintbox.Location = new Point(0, 0);
            this.paintbox.BackColor = ebackcolor;
            this.paintbox.MouseDown += new MouseEventHandler(this.paintbox_MouseDown);
            this.paintbox.MouseUp += new MouseEventHandler(this.paintbox_MouseUp);
            this.paintbox.MouseMove += new MouseEventHandler(this.paintbox_MouseMove);
            this.Controls.Add(paintbox);

            canvas = new Bitmap(paintbox.Width, paintbox.Height);
            canvasl2 = new Bitmap(paintbox.Width, paintbox.Height);

            for (int i = 0; i < this.selection.Length; i++)                 //ステージセレクト画面インターフィス初期化
            {
                this.selection[i] = new Label();
                this.selection[i].Size = new Size(selectsize, selectsize);
                if (i <= 5)
                {
                    this.selection[i].Left = i * selectsize * 2 + rex;
                    this.selection[i].Top = rey;
                }
                else if (i <= 10)
                {
                    this.selection[i].Left = (i - 6) * selectsize * 2 + selectsize + rex;
                    this.selection[i].Top = selectsize + rey;
                }
                else if (i <= 16)
                {
                    this.selection[i].Left = (i - 11) * selectsize * 2 + rex;
                    this.selection[i].Top = selectsize * 2 + rey;

                }
                else if (i <= 21)
                {
                    this.selection[i].Left = (i - 17) * selectsize * 2 + selectsize + rex;
                    this.selection[i].Top = selectsize * 3 + rey;
                }
                else
                {
                    this.selection[i].Left = (i - 22) * selectsize * 2 + rex;
                    this.selection[i].Top = selectsize * 4 + rey;
                }
                int ioi = i + 1;
                this.selection[i].Name = i.ToString();
                this.selection[i].Text = ioi.ToString();
                this.selection[i].Font = new Font(pfc.Families[FontNum], 37);
                this.selection[i].TextAlign = ContentAlignment.MiddleCenter;
                this.selection[i].Visible = false;
                this.selection[i].Click += new EventHandler(this.selections_Click);
                this.selection[i].MouseEnter += new EventHandler(this.selections_MouseEnter);
                this.selection[i].MouseLeave += new EventHandler(this.selections_MouseLeave);
                this.Controls.Add(this.selection[i]);
            }
            this.ResumeLayout(false);

            this.change = new Label();
            this.change.Text = "ColorChange";
            this.change.Font = new Font(pfc.Families[FontNum], 30);
            this.change.TextAlign = ContentAlignment.MiddleCenter;
            this.change.ForeColor = ebackcolor;
            this.change.BackColor = getrandcolor5(colormode);
            this.change.Location = new Point(360, 50);
            this.change.Size = new Size(270, 50);
            this.change.Visible = false;
            this.change.Click += new EventHandler(this.change_Click);
            this.Controls.Add(this.change);

            this.option = new Label();
            this.option.Text = "OPTION";
            this.option.Font = new Font(pfc.Families[FontNum], 30);
            this.option.TextAlign = ContentAlignment.MiddleCenter;
            this.option.ForeColor = ebackcolor;
            this.option.BackColor = getrandcolor5(colormode);
            this.option.Location = new Point(400, 600);
            this.option.Size = new Size(200, 50);
            this.option.Visible = false;
            this.option.Click += new EventHandler(this.option_Click);
            this.Controls.Add(this.option);

            this.clear = new Label();
            this.clear.Text = "STAGE CLEAR";
            this.clear.Font = new Font(pfc.Families[FontNum], 100);
            this.clear.TextAlign = ContentAlignment.MiddleCenter;
            this.clear.ForeColor = getrandcolor5(colormode);
            this.clear.BackColor = Color.Transparent;
            this.clear.Location = new Point(0, 170);
            this.clear.AutoSize = false;
            this.clear.Size = new Size(1000, 100);
            this.clear.Visible = false;
            this.Controls.Add(this.clear);
            this.clear.Parent = paintbox;

            this.menu = new Label();
            this.menu.Text = "BACK MENU";
            this.menu.Font = new Font(pfc.Families[FontNum], 30);
            this.menu.TextAlign = ContentAlignment.MiddleCenter;
            this.menu.ForeColor = ebackcolor;
            this.menu.BackColor = getrandcolor5(colormode);
            this.menu.Location = new Point(100, 600);
            this.menu.Size = new Size(260, 50);
            this.menu.Visible = false;
            this.menu.Click += new EventHandler(this.menu_Click);
            this.Controls.Add(this.menu);

            this.replay = new Label();
            this.replay.Text = "REPLAY";
            this.replay.Font = new Font(pfc.Families[FontNum], 30);
            this.replay.TextAlign = ContentAlignment.MiddleCenter;
            this.replay.ForeColor = ebackcolor;
            this.replay.BackColor = getrandcolor5(colormode);
            this.replay.Location = new Point(370, 600);
            this.replay.Size = new Size(260, 50);
            this.replay.Visible = false;
            this.replay.Click += new EventHandler(this.replay_Click);
            this.Controls.Add(this.replay);

            this.next = new Label();
            this.next.Text = "NEXT STAGE";
            this.next.Font = new Font(pfc.Families[FontNum], 30);
            this.next.TextAlign = ContentAlignment.MiddleCenter;
            this.next.ForeColor = ebackcolor;
            this.next.BackColor = getrandcolor5(colormode);
            this.next.Location = new Point(640, 600);
            this.next.Size = new Size(260, 50);
            this.next.Visible = false;
            this.next.Click += new EventHandler(this.next_Click);
            this.Controls.Add(this.next);

            this.stagetitleout = new Label();
            this.stagetitleout.Text = "";
            this.stagetitleout.BackColor = Color.Transparent;
            this.stagetitleout.Location = new Point(0, -300);
            this.stagetitleout.Size = new Size(1000, 0);
            this.stagetitleout.Visible = false;
            this.Controls.Add(this.stagetitleout);
            this.stagetitleout.Parent = paintbox;

            this.stagetitle = new Label();
            this.stagetitle.Text = "THIS IS ERROR MESSAGE OK?";
            this.stagetitle.Font = new Font(pfc.Families[FontNum], 80);
            this.stagetitle.TextAlign = ContentAlignment.MiddleCenter;
            this.stagetitle.ForeColor = getrandcolor5(colormode);
            this.stagetitle.BackColor = Color.Transparent;
            this.stagetitle.Location = new Point(0, 0);
            this.stagetitle.Size = new Size(1000, 200);
            this.stagetitle.Visible = false;
            this.Controls.Add(this.stagetitle);
            this.stagetitle.Parent = paintbox;

            this.target = new Label();
            this.target.Text = "DAUB ??%";
            this.target.Font = new Font(pfc.Families[FontNum], 120);
            this.target.TextAlign = ContentAlignment.TopCenter;
            this.target.ForeColor = getrandcolor5(colormode);
            this.target.BackColor = Color.Transparent;
            this.target.Location = new Point(0, 300);
            this.target.Size = new Size(1000, 200);
            this.target.Visible = false;
            this.Controls.Add(this.target);
            this.target.Parent = paintbox;

            this.cleartimedisp = new Label();
            this.cleartimedisp.Text = "??:??:??";
            this.cleartimedisp.Font = new Font(pfc.Families[FontNum], 70);
            this.cleartimedisp.TextAlign = ContentAlignment.TopCenter;
            this.cleartimedisp.ForeColor = getrandcolor5(colormode);
            this.cleartimedisp.BackColor = Color.Transparent;
            this.cleartimedisp.Location = new Point(0, 330);
            this.cleartimedisp.Size = new Size(1000, 200);
            this.cleartimedisp.Visible = false;
            this.Controls.Add(this.cleartimedisp);
            this.cleartimedisp.Parent = paintbox;

            this.changepanel = new Panel();                             //カラーチェンジ画面初期化
            this.changepanel.Size = new Size(this.Width, 200);
            this.changepanel.Location = new Point(0, -400);
            this.changepanel.BackColor = Color.Transparent;//getrandcolor5(colormode);
            this.changepanel.Visible = true;
            this.Controls.Add(this.changepanel);
            //this.changepanel.Parent = paintbox;

            this.optionpanel = new Panel();                             //オプション画面初期化
            this.optionpanel.Size = new Size(this.Width, this.Height);
            this.optionpanel.Location = new Point(0, 700);
            this.optionpanel.BackColor = Color.Transparent;//getrandcolor5(colormode);
            this.Controls.Add(this.optionpanel);

            this.white = new System.Windows.Forms.Label();                 //説明画面インターフェイス初期化
            this.white.Text = "";
            this.white.Location = new Point(this.Width / 2, this.Height / 2);
            this.white.Size = new Size(0, 0);
            this.white.BackColor = Color.White;
            this.white.Visible = false;
            this.Controls.Add(this.white);

            this.colorselectbox = new System.Windows.Forms.ComboBox();
            this.colorselectbox.Size = new Size(150, 20);
            this.colorselectbox.Location = new Point(425,90);
            this.colorselectbox.Visible = true;
            this.colorselectbox.DropDownStyle = ComboBoxStyle.DropDownList;
            /*mode =    0:All           1:Vivid         2:PerfectVivit      3:Pale          4:VeryPale          5:Deep              6:Light 
             *          7:Dark          8:Soft          9:Strong            10:Dull         11:Grayish          12:LightGrayish     13:DarkGrayish
             *          
             *          100:B&W         101:Sepia       102:ColorBlind      103:Fall        104:Spring          105:Sumer           106:Winter
             *          107:Red         108:Orange      109:Yellow          110:Green       111:Cyan            112:Blue            113:Purple
             *          114:Pink        115:Bloody      116:Gore        
             */
            this.colorselectbox.Items.Add("All");
            this.colorselectbox.Items.Add("Vivid");
            this.colorselectbox.Items.Add("PerfectVivit");
            this.colorselectbox.Items.Add("Pale");
            this.colorselectbox.Items.Add("VeryPale");
            this.colorselectbox.Items.Add("Deep");
            this.colorselectbox.Items.Add("Light");
            this.colorselectbox.Items.Add("Dark");
            this.colorselectbox.Items.Add("Soft");
            this.colorselectbox.Items.Add("Strong");
            this.colorselectbox.Items.Add("Dull");
            this.colorselectbox.Items.Add("Grayish");
            this.colorselectbox.Items.Add("LightGrayish");
            this.colorselectbox.Items.Add("DarkGrayish");
            this.colorselectbox.Items.Add("Black & White");
            this.colorselectbox.Items.Add("Sepia");
            this.colorselectbox.Items.Add("ColorBlind");
            this.colorselectbox.Items.Add("Fall");
            this.colorselectbox.Items.Add("Spring");
            this.colorselectbox.Items.Add("Sumer");
            this.colorselectbox.Items.Add("Winter");
            this.colorselectbox.Items.Add("Red");
            this.colorselectbox.Items.Add("Orange");
            this.colorselectbox.Items.Add("Yellow");
            this.colorselectbox.Items.Add("Green");
            this.colorselectbox.Items.Add("Cyan");
            this.colorselectbox.Items.Add("Blue");
            this.colorselectbox.Items.Add("Purple");
            this.colorselectbox.Items.Add("Pink");
            this.colorselectbox.Items.Add("Bloody");
            this.colorselectbox.Items.Add("Gore");
            this.colorselectbox.SelectedIndexChanged += new EventHandler(colorselectbox_SelectedIndexChanged);
            this.Controls.Add(this.colorselectbox);
            this.colorselectbox.Parent = changepanel;

            this.paintbox.SendToBack();                                 //コントロールの順番の調整
            //this.changepanel.SendToBack();
            //this.optionpanel.SendToBack();

            #endregion

            #region EnemyDataInit
            enemyesdata.inkballLsize = 100;
            enemyesdata.inkballMsize = 50;
            enemyesdata.inkballSsize = 25;
            enemyesdata.inckholesize = 130;
            enemyesdata.inkballLLLsize = 200;
            enemyesdata.inkvadersize = new Size(1000, 700);
            enemyesdata.inkballLcd = 40;           //cd = 	collision detection当たり判定半径
            enemyesdata.inkballMcd = 20;
            enemyesdata.inkballScd = 10;
            enemyesdata.inckholecd = 50;
            enemyesdata.inkballLLLcd = 90;
            enemyesdata.inkballLspeed = 10;
            enemyesdata.inkballMspeed = 20;
            enemyesdata.inkballSspeed = 30;
            enemyesdata.inckholespeedLow = 20;
            enemyesdata.inckholespeedMid = 30;
            enemyesdata.inckholespeedHigh = 40;
            enemyesdata.inkballLLLspeed = 10;
            enemyesdata.colorkillerblink = 100;
            #endregion

            //ゲーム制御
            colorname = getrandcolor5(colormode);

            #region DebugControl
            stagestart();                                               //以下後で消すこと※デバッグ用
            this.label1 = new System.Windows.Forms.Label();
            this.label1.Text = fill.ToString();
            this.label1.Location = new Point(10, 10);
            this.label1.Size = new System.Drawing.Size(604, 15);//44
            this.label1.BackColor = Color.Transparent;
            this.Controls.Add(this.label1);                             //以上後で消すこと
            #endregion


            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            StageOptionInit();
            gamemode = 95;
            openingcounter = 0;
            colormode = 2;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            #region GraphicProcessing
            //基本的に実行するもの
            if (gamemode == 99)
            {
                paintbox.Image = canvasl2;
            }
            else
            {
                paintbox.Image = canvas;
            }
            //paintbox.Image = canvas;
            if (gamemode == 99)
            {
                nowtime = DateTime.Now - starttime;
                this.Text = "ColorKillers -カラキラ！-" + "   " + "Stage" + (selectedstage + 1) + "   " + ((int)decision).ToString("00") + "%" + "   " + nowtime.ToString(@"mm\:ss\:ff") + "   " + stagename[selectedstage + 1] + "   " + "Escボタンでメニューに戻る";
            }
            else if (gamemode == 100)
            {
                this.Text = "ColorKillers -カラキラ！-" + "   " + "End" + "  " + cleartime.ToString(@"mm\:ss\:ff") + "   " + stagename[selectedstage + 1];
            }
            else
            {
                this.Text = "ColorKillers -カラキラ！-" + "   " + "ゲームプレイ中はここにインフォメーションが表示されるよ！";
            }
            //if (enteredstage != 29) label1.Text = selection[enteredstage].ForeColor.ToString() + selection[enteredstage].BackColor.ToString();//色確認用
            //if (enteredstage != 29) label1.Text = System.Drawing.ColorTranslator.ToOle(selection[enteredstage].ForeColor).ToString() + "      " + System.Drawing.ColorTranslator.ToOle(selection[enteredstage].BackColor).ToString() + selection[enteredstage].ForeColor.ToString() + selection[enteredstage].BackColor.ToString();
            //label1.Text = fill.ToString();//mousex.ToString() + "  "+mousey.ToString() ;
            //label1.Parent = paintbox;
            //以上
            #endregion

            switch (gamemode)
            {

                #region EachOptionDisplayProperties
                case 50:                                                //カラーチェンジ画面準備
                    for (int i = 0; i < this.selection.Length; i++)
                    {
                        this.selection[i].Top += 10;
                    }
                    option.Top += 10;
                    change.Top += 10;
                    changepanel.Top += 10;
                    optionpanel.Top += 10;
                    scr++;
                    if (scr == 55)
                    {
                        gamemode = 51;
                    }
                    break;
                case 51:                                                //カラーチェンジ画面

                    break;
                case 52:                                                //カラーチェンジ終了準備
                    for (int i = 0; i < this.selection.Length; i++)
                    {
                        this.selection[i].Top -= 10;
                    }
                    option.Top -= 10;
                    change.Top -= 10;
                    changepanel.Top -= 10;
                    optionpanel.Top -= 10;
                    scr++;
                    if (scr == 55)
                    {
                        gamemode = 97;
                    }
                    break;
                case 53:                                                //オプション画面準備
                    for (int i = 0; i < this.selection.Length; i++)
                    {
                        this.selection[i].Top -= 10;
                    }
                    option.Top -= 10;
                    change.Top -= 10;
                    changepanel.Top -= 10;
                    optionpanel.Top -= 10;
                    scr++;
                    if (scr == 58)
                    {
                        gamemode = 54;
                    }
                    break;
                case 54:                                                //オプション画面

                    break;
                case 55:                                                //オプション終了準備  
                    for (int i = 0; i < this.selection.Length; i++)
                    {
                        this.selection[i].Top += 10;
                    }
                    option.Top += 10;
                    change.Top += 10;
                    changepanel.Top += 10;
                    optionpanel.Top += 10;
                    scr++;
                    if (scr == 58)
                    {
                        gamemode = 97;
                    }
                    break;
                #endregion

                #region Title&GamePrepareDisplayPropaties
                case 95:                                                //トップ画面
                    openingcounter++;
                    Graphics g10 = Graphics.FromImage(canvas);
                    g10.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    if (openingcounter == 1)
                    {
                        g10.Clear(Color.FromArgb(255, 255, 255, 255));
                    }
                    else if (openingcounter == 100)
                    {
                        g10.DrawImage(MPPimg, 378, 300, 245, 101);
                    }
                    else if (openingcounter == 200)
                    {
                        g10.Clear(Color.FromArgb(255, 255, 255, 255));
                        g10.DrawImage(KMHOTFimg, 378, 260, 265, 163);
                    }
                    else if (openingcounter == 385)
                    {
                        g10.Clear(Color.FromArgb(255, 255, 255, 255));
                        g10.DrawImage(Logoimg, 175, 25, 650, 650);
                    }
                    else if (openingcounter == 1000)
                    {
                        g10.Clear(Color.FromArgb(255, 255, 255, 255));
                    }
                    else if (openingcounter >= 1050)
                    {
                        g10.Clear(Color.FromArgb(255, 255, 255, 255));
                        gamemode = 96;
                    }
                    else if ((openingcounter >= 975) && (openingcounter < 1000))
                    {
                        if (openingcounter % 1 == 0) spritink(0, 0, 0, 0, getrandcolor5(colormode));
                    }
                    else if ((openingcounter >= 950) && (openingcounter < 975))
                    {
                        if (openingcounter % 2 == 0) spritink(0, 0, 0, 0, getrandcolor5(colormode));
                    }
                    else if ((openingcounter >= 925) && (openingcounter < 950))
                    {
                        if (openingcounter % 3 == 0) spritink(0, 0, 0, 0, getrandcolor5(colormode));
                    }
                    else if ((openingcounter >= 900) && (openingcounter < 925))
                    {
                        if (openingcounter % 5 == 0) spritink(0, 0, 0, 0, getrandcolor5(colormode));
                    }
                    else if ((openingcounter >= 800) && (openingcounter < 900))
                    {
                        if (openingcounter % 10 == 0) spritink(0, 0, 0, 0, getrandcolor5(colormode));
                    }
                    else if ((openingcounter >= 700) && (openingcounter < 800))
                    {
                        if (openingcounter % 20 == 0) spritink(0, 0, 0, 0, getrandcolor5(colormode));
                    }
                    else if ((openingcounter >= 600) && (openingcounter < 700))
                    {
                        if (openingcounter % 40 == 0) spritink(0, 0, 0, 0, getrandcolor5(colormode));
                    }
                    else if ((openingcounter >= 300) && (openingcounter < 385))
                    {
                        g10.Clear(Color.FromArgb(255, 255, 255, 255));
                        g10.DrawImage(Logoimg, 175, 25, 650, 650);
                        g10.FillRectangle(new SolidBrush(Color.FromArgb(255-(openingcounter-300)*3, 255, 255, 255)), 0, 0, this.Width, this.Height);
                    }
                    else if ((openingcounter >= 250) && (openingcounter < 300))
                    {
                        g10.FillRectangle(new SolidBrush(Color.FromArgb(30, 255, 255, 255)), 0, 0, this.Width, this.Height);
                    }
                    else if ((openingcounter >= 150) && (openingcounter < 200))
                    {
                        g10.FillRectangle(new SolidBrush(Color.FromArgb(30, 255, 255, 255)), 0, 0, this.Width, this.Height);
                    }
                    g10.Dispose();
                    break;
                case 96:                                                //トップ画面直後(セレクト画面インターフェイス初期化用)
                    for (int i = 0; i < this.selection.Length; i++)
                    {
                        this.selection[i].BackColor = getrandcolor5(colormode);
                        this.selection[i].ForeColor = BackColor;
                        this.selection[i].Left += selectsize / 2;
                        this.selection[i].Width = 0;
                        this.selection[i].Font = new Font(pfc.Families[FontNum], 7);
                        this.selection[i].Visible = true;
                    }
                    this.option.Visible = true;
                    this.change.Visible = true;
                    selectcount = 0;
                    selectedstage = 29;
                    gamemode = 97;
                    mpe.MusicPlay(rnd.Next(mpe.GetSongQuan()));
                    break;
                case 97:                                                //セレクト画面※絶対にゲームモード96から実行すること(初期化の手間を省く)
                    if ((selectcount <= 155) && (selectedstage == 29))
                    {
                        for (int i = 0; i < this.selection.Length; i++)
                        {
                            if ((selectcount > i * 5) && (this.selection[i].Width != selectsize))
                            {
                                this.selection[i].Left -= 3;
                                this.selection[i].Width += 6;
                                this.selection[i].Font = new Font(pfc.Families[FontNum], this.selection[i].Font.Size + 2);
                            }
                        }
                        if (selectcount % 30 == 0)      //30フレームごとにインクはね
                        {
                            spritink(0, 0, 0, 0, getrandcolor5(colormode));
                        }
                        selectcount++;
                    }
                    else if (selectedstage == 29)
                    {
                        for (int i = 0; i < this.selection.Length; i++)
                        {
                            if ((!entering) && (this.selection[i].BackColor == Color.Transparent) || (i != enteredstage) && (this.selection[i].BackColor == Color.Transparent))
                            {
                                this.selection[i].ForeColor = enteringstrcolor;
                                this.selection[i].BackColor = enteringbackcolor;
                                enteringcounter = 0;
                            }
                        }
                        if (entering)
                        {
                            if (enteringcounter > 5)
                            {
                                this.selection[enteredstage].ForeColor = Color.Transparent;
                                this.selection[enteredstage].BackColor = Color.Transparent;
                            }
                            else
                            {
                                this.selection[enteredstage].ForeColor = enteringstrcolor;
                                this.selection[enteredstage].BackColor = enteringbackcolor;
                            }
                            enteringcounter++;
                            if (enteringcounter == 12) enteringcounter = 0;
                            //entering = false;
                        }
                    }
                    else
                    {
                        if (selectcount < 156)
                        {
                            for (int i = 0; i < this.selection.Length; i++)
                            {
                                if ((selectcount >= i * 5) && (this.selection[i].Width != 0) && (selectedstage != i))
                                {
                                    this.selection[i].Width -= 6;
                                    this.selection[i].Left += 3;
                                    this.selection[i].Font = new Font(pfc.Families[FontNum], this.selection[i].Font.Size - 2);
                                }
                            }
                            selectcount++;
                        }
                        else if (selectcount < 156 + movetime)
                        {
                            movingx += movex;
                            movingy += movey;
                            this.selection[selectedstage].Left = (int)Math.Floor(movingx + 0.5);
                            this.selection[selectedstage].Top = (int)Math.Floor(movingy + 0.5);
                            selectcount++;
                        }
                        else if (selectcount < 156 + movetime + 121)
                        {
                            this.selection[selectedstage].Left -= 4;
                            this.selection[selectedstage].Top -= 4;
                            this.selection[selectedstage].Width += 8;
                            this.selection[selectedstage].Height += 8;
                            if (this.selection[selectedstage].Font.Size != 1) this.selection[selectedstage].Font = new Font(this.selection[selectedstage].Font.Name, this.selection[selectedstage].Font.Size - 4);
                            if (this.selection[selectedstage].Font.Size == 1) this.selection[selectedstage].ForeColor = this.selection[selectedstage].BackColor;
                            selectcount++;
                        }
                        else
                        {
                            throughselect = true;                   //セレクト画面を通ったフラグ
                            this.white.Location = new Point(this.Width / 2, this.Height / 2);      //説明画面用初期化↓2
                            this.white.Size = new Size(0, 0);
                            this.white.Visible = true;
                            this.white.BringToFront();
                            expcounter = 0;
                            gamemode = 98;
                            mpe.MusicStop();
                            mpe.MusicPlay(rnd.Next(mpe.GetSongQuan()));
                        }
                    }
                    break;
                case 98:                                                //説明画面ゲーム開始前初期化用
                    if (expcounter <= 50)
                    {
                        this.white.Left -= 10;
                        this.white.Top -= 10;
                        this.white.Width += 20;
                        this.white.Height += 20;
                        expcounter++;
                    }
                    else if (throughselect)
                    {
                        this.selection[selectedstage].Location = backing;
                        for (int i = 0; i < this.selection.Length; i++)
                        {
                            this.selection[i].Left -= selectsize / 2;
                            this.selection[i].Width = 0;
                            this.selection[i].Height = selectsize;
                            this.selection[i].Visible = false;
                        }
                        option.Visible = false;
                        change.Visible = false;
                        throughselect = false;
                    }
                    else
                    {                                                                               //ゲーム開始前最終直前準備
                        Graphics g = Graphics.FromImage(canvas);
                        //g.FillRectangle(new SolidBrush(ebackcolor), 0, 0, this.Width, this.Height);
                        g.Clear(ebackcolor);
                        g.Dispose();
                        this.white.Visible = false;
                        //this.paintboxenemy.Visible = true;
                        stagestart();
                        starttime = DateTime.Now;
                        dispcounter = 0;
                        stagetitle.Height = 0;
                        target.Top = 200;
                        stagetitleout.Height = 0;
                        stagetitle.Visible = true;
                        target.Visible = true;
                        stagetitleout.Visible = true;
                        stagetitle.Text = stagename[selectedstage + 1];
                        target.Text = "DAUB " + stagecleartarget[selectedstage + 1].ToString() + "%";
                        DrawLevel = 99999;
                        hitcounter = 0;
                        blackout = false;
                        whiteout = false;
                        gamemode = 101;

                        enemy_data_call();                          //敵情報呼び出し
                        control_color_shuffle();                    //フォームコントロールの色をシャッフル
                        //this.paintbox.Visible = false;
                    }
                    break;
                #endregion

                #region GamingPropaties
                case 99:
                    if (true)
                    {
                        #region PlayerProcessing
                        Graphics g = Graphics.FromImage(canvas);
                        if (circleantialiasing) g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        drawing = false;
                        if (move)
                        {
                            if ((bStart) && (mousex > 0) && (mousex < 1000) && (mousey > 0) && (mousey < 700))
                            {
                                if ((bStart == true) && (ctiming == 0))
                                {
                                    drawing = true;
                                    csize += 4;
                                    if (csize > 120) csize = 120;
                                    if ((circlespritinkable) && (csize >= 80))
                                    {
                                        spritinklight(2, mousex, mousey, (int)csize / 2, colorname);
                                    }
                                    else
                                    {
                                        g.FillEllipse(new SolidBrush(colorname), mousex - ((csize - 1) / 2), mousey - ((csize - 1) / 2), csize, csize);
                                    }
                                    //以下塗りつぶされた範囲の計算
                                    FillCalcuration(true, mousex, mousey, (csize - 1) / 2);
                                    ctiming = 2;
                                    decision = (double)fill * 100 / (double)700000;
                                    if (decision >= stagecleartarget[selectedstage + 1])
                                    {
                                        cleartime = DateTime.Now - starttime;
                                        stagestart();
                                        clear.Visible = true;
                                        menu.Visible = true;
                                        replay.Visible = true;
                                        if (selectedstage != 27) next.Visible = true;
                                        cleartimedisp.Text = cleartime.ToString(@"mm\:ss\:ff");
                                        cleartimedisp.Visible = true;
                                        clear.Text = "STAGE CLEAR";
                                        gamemode = 100;
                                        mpe.MusicStop();
                                        timer.Interval = 10;
                                        Cursor.Show();
                                    }
                                }
                                else
                                {
                                    ctiming -= 1;
                                }

                            }
                            move = false;
                        }
                        DrawLevel += 123;
                        if (DrawLevel > 99999) DrawLevel = 99999;
                        if (hitcounter != 0) hitcounter -= 1;
                        #endregion

                        #region EnemyMoveAI
                        for (int i = 1; i <= enemyenum; ++i)
                        {
                            //寿命処理
                            #region LifeSpanProsessing
                            if (enemy[i].lifespan == 1)
                            {
                                if (i != enemyenum)
                                {
                                    NaturalDeath(i, enemyenum);
                                }
                                enemyenum -= 1;
                            }
                            else if (enemy[i].lifespan != 0)
                            {
                                enemy[i].lifespan -= 1;
                            }
                            #endregion
                            switch (enemy[i].objectclass)
                            {
                                case 1:         //InkBallL          半完成
                                    #region InkBallL
                                    enemy[i].x += enemy[i].dco1;
                                    enemy[i].y += enemy[i].dco2;
                                    angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballLsize / 2, mousex - enemy[i].x - enemyesdata.inkballLsize / 2);
                                    enemy[i].dco1 += Math.Cos(angle) * 2;
                                    enemy[i].dco2 += Math.Sin(angle) * 2;
                                    if (enemy[i].x < 0)
                                    {
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballLsize / 2, mousex - enemy[i].x - enemyesdata.inkballLsize / 2);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballLspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballLspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco1 = enemy[i].dco1 * (-1);
                                        }
                                    }
                                    else if (enemy[i].x > 1000 - enemyesdata.inkballLsize)
                                    {
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballLsize / 2, mousex - enemy[i].x - enemyesdata.inkballLsize / 2);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballLspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballLspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco1 = enemy[i].dco1 * (-1);
                                        }
                                    }
                                    else if (enemy[i].y < 0)
                                    {
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballLsize / 2, mousex - enemy[i].x - enemyesdata.inkballLsize / 2);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballLspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballLspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco2 = enemy[i].dco2 * (-1);
                                        }
                                    }
                                    else if (enemy[i].y > 700 - enemyesdata.inkballLsize)
                                    {
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballLsize / 2, mousex - enemy[i].x - enemyesdata.inkballLsize / 2);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballLspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballLspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco2 = enemy[i].dco2 * (-1);
                                        }
                                    }
#endregion
                                    #region HitCheck
                                    ENEMY_MY_CD = enemyesdata.inkballLcd;
                                    ENEMY_MY_SIZE = enemyesdata.inkballLsize/2; //大体半径だから先に/2
                                    if ((mousex - enemy[i].x - ENEMY_MY_SIZE) * (mousex - enemy[i].x - 50 - ENEMY_MY_SIZE)
                                        + (mousey - enemy[i].y - ENEMY_MY_SIZE) * (mousey - enemy[i].y - ENEMY_MY_SIZE)
                                        <= ((csize - 1) / 2 + ENEMY_MY_CD) * ((csize - 1) / 2 + ENEMY_MY_CD) && (bStart)&&(drawing))
                                    {
                                        DrawLevel -= 33333;
                                        hitcounter = 60;
                                        bStart = false;
                                        spritink(2, (int)enemy[i].x + ENEMY_MY_SIZE, (int)enemy[i].y + ENEMY_MY_SIZE, HittingDamageDisp, ebackcolor);
                                        FillCalcuration(false, (int)enemy[i].x + ENEMY_MY_SIZE, (int)enemy[i].y + ENEMY_MY_SIZE, HittingDamageDisp);
                                    }
                                    #endregion
                                    break;
                                case 2:         //InkBallM          半完成
                                    #region InkBallM
                                    enemy[i].x += enemy[i].dco1;
                                    enemy[i].y += enemy[i].dco2;
                                    if (enemy[i].x < 0)
                                    {
                                        angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballMsize / 2, mousex - enemy[i].x - enemyesdata.inkballMsize / 2);
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballMsize / 2, mousex - enemy[i].x - enemyesdata.inkballMsize / 2);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballMspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballMspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco1 = enemy[i].dco1 * (-1);
                                        }
                                    }
                                    else if (enemy[i].x > 1000 - enemyesdata.inkballMsize)
                                    {
                                        angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballMsize / 2, mousex - enemy[i].x - enemyesdata.inkballMsize / 2);
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballMsize / 2, mousex - enemy[i].x - enemyesdata.inkballMsize / 2);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballMspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballMspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco1 = enemy[i].dco1 * (-1);
                                        }
                                    }
                                    else if (enemy[i].y < 0)
                                    {
                                        angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballMsize / 2, mousex - enemy[i].x - enemyesdata.inkballMsize / 2);
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballMsize / 2, mousex - enemy[i].x - enemyesdata.inkballMsize / 2);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballMspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballMspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco1 = enemy[i].dco1 * (-1);
                                        }
                                    }
                                    else if (enemy[i].y > 700 - enemyesdata.inkballMsize)
                                    {
                                        angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballMsize / 2, mousex - enemy[i].x - enemyesdata.inkballMsize / 2);
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - enemyesdata.inkballMsize / 2, mousex - enemy[i].x - enemyesdata.inkballMsize / 2);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballMspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballMspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco1 = enemy[i].dco1 * (-1);
                                        }
                                    }
                                    #endregion 
                                    #region HitCheck
                                    ENEMY_MY_CD = enemyesdata.inkballMcd;
                                    ENEMY_MY_SIZE = enemyesdata.inkballMsize/2; //大体半径だから先に/2
                                    if ((mousex - enemy[i].x - ENEMY_MY_SIZE) * (mousex - enemy[i].x - 50 - ENEMY_MY_SIZE)
                                        + (mousey - enemy[i].y - ENEMY_MY_SIZE) * (mousey - enemy[i].y - ENEMY_MY_SIZE)
                                        <= ((csize - 1) / 2 + ENEMY_MY_CD) * ((csize - 1) / 2 + ENEMY_MY_CD) && (bStart) && (drawing))
                                    {
                                        DrawLevel -= 33333;
                                        hitcounter = 60;
                                        bStart = false;
                                        spritink(2, (int)enemy[i].x + ENEMY_MY_SIZE, (int)enemy[i].y + ENEMY_MY_SIZE, HittingDamageDisp, ebackcolor);
                                    }
                                    #endregion
                                    break;
                                case 3:         //InkBallS          半完成
                                    #region InkBallS
                                    enemy[i].x += enemy[i].dco1;
                                    enemy[i].y += enemy[i].dco2;
                                    if (enemy[i].x < 0)
                                    {
                                        enemy[i].dco1 = enemy[i].dco1 * (-1);
                                    }
                                    else if (enemy[i].x > 1000 - enemyesdata.inkballSsize)
                                    {
                                        enemy[i].dco1 = enemy[i].dco1 * (-1);
                                    }
                                    else if (enemy[i].y < 0)
                                    {
                                        enemy[i].dco2 = enemy[i].dco2 * (-1);
                                    }
                                    else if (enemy[i].y > 700 - enemyesdata.inkballSsize)
                                    {
                                        enemy[i].dco2 = enemy[i].dco2 * (-1);
                                    }
                                    #endregion
                                    #region HitCheck
                                    ENEMY_MY_CD = enemyesdata.inkballScd;
                                    ENEMY_MY_SIZE = enemyesdata.inkballSsize/2; //大体半径だから先に/2
                                    if ((mousex - enemy[i].x - ENEMY_MY_SIZE) * (mousex - enemy[i].x - 50 - ENEMY_MY_SIZE)
                                        + (mousey - enemy[i].y - ENEMY_MY_SIZE) * (mousey - enemy[i].y - ENEMY_MY_SIZE)
                                        <= ((csize - 1) / 2 + ENEMY_MY_CD) * ((csize - 1) / 2 + ENEMY_MY_CD) && (bStart) && (drawing))
                                    {
                                        DrawLevel -= 33333;
                                        hitcounter = 60;
                                        bStart = false;
                                        spritink(2, (int)enemy[i].x + ENEMY_MY_SIZE, (int)enemy[i].y + ENEMY_MY_SIZE, HittingDamageDisp, ebackcolor);
                                    }
                                    #endregion
                                    break;
                                case 4:         //InckHoleBlack     完成！
                                    #region InckHoleBlack
                                    enemy[i].co1 = (enemy[i].co1 + 1) % 3;
                                    switch (enemy[i].co2)
                                    {
                                        case 0:                                 //抽選モード
                                            int Lot = rnd.Next(100);    //攻撃パターンの抽選
                                            //Lot = 75;
                                            if (Lot < 35)
                                            {   
                                                enemy[i].co2 = 1;       //パターン決定
                                                enemy[i].co3 = 60;     //攻撃時間
                                                enemy[i].co4 = 0;
                                            }
                                            else if (Lot < 55)
                                            {
                                                enemy[i].co2 = 2;
                                                enemy[i].co3 = 90;
                                            }
                                            else if (Lot < 80)
                                            {
                                                enemy[i].co2 = 3;
                                                enemy[i].co3 = 90;
                                                enemy[i].co4 = 0;
                                            }
                                            else
                                            {
                                                enemy[i].co2 = 4;
                                                enemy[i].co3 = 60;
                                            }
                                            break;
                                        case 1:                                 //searchモード 1lw 2tw 3rw 4bw
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) enemy[i].co2 = 0;
                                            if (enemy[i].co4 == 0)
                                            {
                                                int lw = (int)enemy[i].x;
                                                int tw = (int)enemy[i].y;
                                                int rw = 1000 - (int)enemy[i].x - 130;
                                                int bw = 700 - (int)enemy[i].y - 130;
                                                if (lw < tw)
                                                {
                                                    if (lw < rw)
                                                    {
                                                        if (lw < bw)        //左短し
                                                        {
                                                            enemy[i].x -= enemyesdata.inckholespeedMid;
                                                            if (enemy[i].x <= 0)
                                                            {
                                                                enemy[i].x = 0;
                                                                enemy[i].co4 = 1;
                                                            }
                                                        }
                                                        else                //下短し
                                                        {
                                                            enemy[i].y += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].y >= 555)
                                                            {
                                                                enemy[i].y = 555;
                                                                enemy[i].co4 = 4;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (rw < bw)        //右短し
                                                        {
                                                            enemy[i].x += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].x >= 870)
                                                            {
                                                                enemy[i].x = 870;
                                                                enemy[i].co4 = 3;
                                                            }
                                                        }
                                                        else                //下短し
                                                        {
                                                            enemy[i].y += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].y >= 555)
                                                            {
                                                                enemy[i].y = 555;
                                                                enemy[i].co4 = 4;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (tw < rw)
                                                    {
                                                        if (tw < bw)        //上短し
                                                        {
                                                            enemy[i].y -= enemyesdata.inckholespeedMid;
                                                            if (enemy[i].y <= 0)
                                                            {
                                                                enemy[i].y = 0;
                                                                enemy[i].co4 = 2;
                                                            }
                                                        }
                                                        else                //下短し
                                                        {
                                                            enemy[i].y += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].y >= 555)
                                                            {
                                                                enemy[i].y = 555;
                                                                enemy[i].co4 = 4;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (rw < bw)        //右短し
                                                        {
                                                            enemy[i].x += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].x >= 870)
                                                            {
                                                                enemy[i].x = 870;
                                                                enemy[i].co4 = 3;
                                                            }
                                                        }
                                                        else                //下短し
                                                        {
                                                            enemy[i].y += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].y >= 555)
                                                            {
                                                                enemy[i].y = 555;
                                                                enemy[i].co4 = 4;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                switch (enemy[i].co4)
                                                {
                                                    case 1:         //l
                                                        enemy[i].y -= enemyesdata.inckholespeedHigh;
                                                        if (enemy[i].y <= 0)
                                                        {
                                                            enemy[i].y = 0;
                                                            enemy[i].co4 = 2;
                                                        }
                                                        break;
                                                    case 2:         //t
                                                        enemy[i].x += enemyesdata.inckholespeedHigh;
                                                        if (enemy[i].x >= 870)
                                                        {
                                                            enemy[i].x = 870;
                                                            enemy[i].co4 = 3;
                                                        }
                                                        break;
                                                    case 3:         //r
                                                        enemy[i].y += enemyesdata.inckholespeedHigh;
                                                        if (enemy[i].y >= 555)
                                                        {
                                                            enemy[i].y = 555;
                                                            enemy[i].co4 = 4;
                                                        }
                                                        break;
                                                    case 4:         //b
                                                        enemy[i].x -= enemyesdata.inckholespeedHigh;
                                                        if (enemy[i].x <= 0)
                                                        {
                                                            enemy[i].x = 0;
                                                            enemy[i].co4 = 1;
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 2:                                 //sneakモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) enemy[i].co2 = 0;
                                            if (true)
                                            {
                                                angle = Math.Atan2(mousey-enemy[i].y-65,mousex - enemy[i].x-65);
                                                enemy[i].x += Math.Cos(angle) * enemyesdata.inckholespeedLow;
                                                enemy[i].y += Math.Sin(angle) * enemyesdata.inckholespeedLow;
                                            }
                                            break;
                                        case 3:                                 //boundモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) enemy[i].co2 = 0;
                                            if (enemy[i].co4 == 0)
                                            {
                                                angle = rnd.Next(360);
                                                enemy[i].dco1 = enemyesdata.inckholespeedMid * usercos[(int)angle];
                                                enemy[i].dco2 = enemyesdata.inckholespeedMid * usersin[(int)angle];
                                                enemy[i].co4 = 1;
                                            }
                                            else
                                            {
                                                enemy[i].x += enemy[i].dco1;
                                                enemy[i].y += enemy[i].dco2;
                                                if (enemy[i].x < 0)
                                                {
                                                    enemy[i].dco1 = enemy[i].dco1 * (-1);
                                                }
                                                else if (enemy[i].x > 1000 - enemyesdata.inckholesize)
                                                {
                                                    enemy[i].dco1 = enemy[i].dco1 * (-1);
                                                }
                                                else if (enemy[i].y < 0)
                                                {
                                                    enemy[i].dco2 = enemy[i].dco2 * (-1);
                                                }
                                                else if (enemy[i].y > 700 - enemyesdata.inckholesize)
                                                {
                                                    enemy[i].dco2 = enemy[i].dco2 * (-1);
                                                }
                                                break;
                                            }
                                            break;
                                        case 4:                                 //chargeモード     下の3モードに派生
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0)
                                            {
                                                enemy[i].co2 = 0;
                                                int Lota = rnd.Next(100);    //攻撃パターンの抽選
                                                //Lota = 90;
                                                if (Lota < 30)
                                                {
                                                    enemy[i].co2 = 5;       //パターン決定
                                                    enemy[i].co3 = 45;     //攻撃時間
                                                }
                                                else if (Lota < 50)
                                                {
                                                    enemy[i].co2 = 6;
                                                    enemy[i].co3 = 90;
                                                    enemy[i].co4 = 0;
                                                }
                                                else if (Lota < 80)
                                                {
                                                    enemy[i].co2 = 7;
                                                    enemy[i].co3 = 60;
                                                    enemy[i].co4 = 64;
                                                }
                                                else
                                                {
                                                    enemy[i].co2 = 8;
                                                    enemy[i].co3 = 120;
                                                }
                                            }
                                            break;
                                        case 5:                                 //blackoutモード
                                            enemy[i].co3 -= 1;
                                            blackout = true;
                                            if (enemy[i].co3 <= 0) { enemy[i].co2 = 0; blackout = false; }
                                            break;
                                        case 6:                                 //blackboundモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) enemy[i].co2 = 0;
                                            if (enemy[i].co4 == 0)
                                            {
                                                angle = rnd.Next(360);
                                                enemy[i].dco1 = enemyesdata.inckholespeedLow * usercos[(int)angle];
                                                enemy[i].dco2 = enemyesdata.inckholespeedLow * usersin[(int)angle];
                                                enemy[i].co4 = 1;
                                            }
                                            else
                                            {
                                                enemy[i].x += enemy[i].dco1;
                                                enemy[i].y += enemy[i].dco2;
                                                g.FillEllipse(new SolidBrush(Color.FromArgb(255,0,0,0)), (int)enemy[i].x + 15,(int)enemy[i].y + 15, 100, 100);
                                                FillCalcuration(false, (int)enemy[i].x + 65, (int)enemy[i].y + 65, 50);
                                                if (enemy[i].x < 0)
                                                {
                                                    enemy[i].dco1 = enemy[i].dco1 * (-1);
                                                }
                                                else if (enemy[i].x > 1000 - enemyesdata.inckholesize)
                                                {
                                                    enemy[i].dco1 = enemy[i].dco1 * (-1);
                                                }
                                                else if (enemy[i].y < 0)
                                                {
                                                    enemy[i].dco2 = enemy[i].dco2 * (-1);
                                                }
                                                else if (enemy[i].y > 700 - enemyesdata.inckholesize)
                                                {
                                                    enemy[i].dco2 = enemy[i].dco2 * (-1);
                                                }
                                                break;
                                            }
                                            break;
                                        case 7:                                 //blackworldモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) 
                                            { 
                                                enemy[i].co2 = 0; 
                                            }
                                            enemy[i].co4 += 8;
                                            if (enemy[i].co4 % 80 == 0) FillCalcuration(false, (int)enemy[i].x + 65, (int)enemy[i].y + 65, (int)enemy[i].co4/2);
                                            g.FillEllipse(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), (int)enemy[i].x + 65 - (enemy[i].co4 / 2), (int)enemy[i].y + 65 - (enemy[i].co4 / 2), enemy[i].co4, enemy[i].co4);
                                            break;
                                        case 8:                                 //blackholeモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) enemy[i].co2 = 0;
                                            angle = Math.Atan2(enemy[i].y + 65 -mousey,enemy[i].x + 65-mousex);
                                            System.Drawing.Point sp = System.Windows.Forms.Cursor.Position;
                                            System.Drawing.Point cp = this.PointToClient(sp);
                                            System.Windows.Forms.Cursor.Position = this.PointToScreen(new System.Drawing.Point((int)(cp.X + Math.Cos(angle)*15), (int)(cp.Y + Math.Sin(angle)*15)));
                                            break;
                                    }
#endregion
                                    #region HitCheck
                                    ENEMY_MY_CD = enemyesdata.inckholecd;
                                    ENEMY_MY_SIZE = enemyesdata.inckholesize/2; //大体半径だから先に/2
                                    if ((mousex - enemy[i].x - ENEMY_MY_SIZE) * (mousex - enemy[i].x - 50 - ENEMY_MY_SIZE)
                                        + (mousey - enemy[i].y - ENEMY_MY_SIZE) * (mousey - enemy[i].y - ENEMY_MY_SIZE)
                                        <= ((csize - 1) / 2 + ENEMY_MY_CD) * ((csize - 1) / 2 + ENEMY_MY_CD) && (bStart) && (drawing))
                                    {
                                        DrawLevel -= 33333;
                                        hitcounter = 60;
                                        bStart = false;
                                        spritink(2, (int)enemy[i].x + ENEMY_MY_SIZE, (int)enemy[i].y + ENEMY_MY_SIZE, HittingDamageDisp, ebackcolor);
                                    }
                                    #endregion
                                    break;
                                case 5:         //InckHoleWhite
                                    #region InckHoleWhite
                                    enemy[i].co1 = (enemy[i].co1 + 1) % 3;
                                    switch (enemy[i].co2)
                                    {
                                        case 0:                                 //抽選モード
                                            int Lot = rnd.Next(100);    //攻撃パターンの抽選
                                            //Lot = 70;
                                            if (Lot < 60)
                                            {
                                                enemy[i].co2 = 1;       //パターン決定
                                                enemy[i].co3 = 40;     //攻撃時間
                                                enemy[i].co4 = 0;
                                            }
                                            else if (Lot < 90)
                                            {
                                                enemy[i].co2 = 2;
                                                enemy[i].co3 = 5;
                                            }
                                            /*else if (Lot < 60)
                                            {
                                                enemy[i].co2 = 3;
                                                enemy[i].co3 = 60;
                                                enemy[i].co4 = 5;
                                            }
                                            else if (Lot < 80)
                                            {
                                                enemy[i].co2 = 4;
                                                enemy[i].co3 = 60;
                                                enemy[i].co4 = 5;
                                            }*/
                                            else
                                            {
                                                enemy[i].co2 = 5;
                                                enemy[i].co3 = 60;
                                            }
                                            break;
                                        case 1:                                 //searchモード 1lw 2tw 3rw 4bw
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) { enemy[i].co2 = 2; enemy[i].co3 = 20; }
                                            if (enemy[i].co4 == 0)
                                            {
                                                int lw = (int)enemy[i].x;
                                                int tw = (int)enemy[i].y;
                                                int rw = 1000 - (int)enemy[i].x - 130;
                                                int bw = 700 - (int)enemy[i].y - 130;
                                                if (lw < tw)
                                                {
                                                    if (lw < rw)
                                                    {
                                                        if (lw < bw)        //左短し
                                                        {
                                                            enemy[i].x -= enemyesdata.inckholespeedMid;
                                                            if (enemy[i].x <= 0)
                                                            {
                                                                enemy[i].x = 0;
                                                                enemy[i].co4 = 1;
                                                            }
                                                        }
                                                        else                //下短し
                                                        {
                                                            enemy[i].y += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].y >= 555)
                                                            {
                                                                enemy[i].y = 555;
                                                                enemy[i].co4 = 4;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (rw < bw)        //右短し
                                                        {
                                                            enemy[i].x += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].x >= 870)
                                                            {
                                                                enemy[i].x = 870;
                                                                enemy[i].co4 = 3;
                                                            }
                                                        }
                                                        else                //下短し
                                                        {
                                                            enemy[i].y += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].y >= 555)
                                                            {
                                                                enemy[i].y = 555;
                                                                enemy[i].co4 = 4;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (tw < rw)
                                                    {
                                                        if (tw < bw)        //上短し
                                                        {
                                                            enemy[i].y -= enemyesdata.inckholespeedMid;
                                                            if (enemy[i].y <= 0)
                                                            {
                                                                enemy[i].y = 0;
                                                                enemy[i].co4 = 2;
                                                            }
                                                        }
                                                        else                //下短し
                                                        {
                                                            enemy[i].y += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].y >= 555)
                                                            {
                                                                enemy[i].y = 555;
                                                                enemy[i].co4 = 4;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (rw < bw)        //右短し
                                                        {
                                                            enemy[i].x += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].x >= 870)
                                                            {
                                                                enemy[i].x = 870;
                                                                enemy[i].co4 = 3;
                                                            }
                                                        }
                                                        else                //下短し
                                                        {
                                                            enemy[i].y += enemyesdata.inckholespeedMid;
                                                            if (enemy[i].y >= 555)
                                                            {
                                                                enemy[i].y = 555;
                                                                enemy[i].co4 = 4;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                switch (enemy[i].co4)
                                                {
                                                    case 1:         //l
                                                        enemy[i].y += enemyesdata.inckholespeedHigh;
                                                        if (enemy[i].y >= 555)
                                                        {
                                                            enemy[i].y = 555;
                                                            enemy[i].co4 = 4;
                                                        }
                                                        break;
                                                    case 2:         //t
                                                        enemy[i].x -= enemyesdata.inckholespeedHigh;
                                                        if (enemy[i].x <= 0)
                                                        {
                                                            enemy[i].x = 0;
                                                            enemy[i].co4 = 1;
                                                        }
                                                        break;
                                                    case 3:         //r
                                                        enemy[i].y -= enemyesdata.inckholespeedHigh;
                                                        if (enemy[i].y <= 0)
                                                        {
                                                            enemy[i].y = 0;
                                                            enemy[i].co4 = 2;
                                                        }
                                                        break;
                                                    case 4:         //b
                                                        enemy[i].x += enemyesdata.inckholespeedHigh;
                                                        if (enemy[i].x >= 870)
                                                        {
                                                            enemy[i].x = 870;
                                                            enemy[i].co4 = 3;
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 2:                                 //Warpモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0)
                                            {
                                                enemy[i].x = rnd.Next(1000 - enemyesdata.inckholesize);
                                                enemy[i].y = rnd.Next(700 - enemyesdata.inckholesize);
                                                Lot = rnd.Next(100);    //攻撃パターンの抽選
                                                //Lot = 70;
                                                if (Lot < 60)
                                                {
                                                    enemy[i].co2 = 2;   
                                                    enemy[i].co3 = 20;  
                                                }
                                                else if (Lot < 75)
                                                {
                                                    enemy[i].co2 = 3;
                                                    enemy[i].co3 = 60;
                                                    enemy[i].co4 = 5;
                                                }
                                                else if (Lot < 90)
                                                {
                                                    enemy[i].co2 = 4;
                                                    enemy[i].co3 = 60;
                                                    enemy[i].co4 = 5;
                                                }
                                                else
                                                {
                                                    enemy[i].co2 = 5;
                                                    enemy[i].co3 = 60;
                                                }
                                            }
                                            if (enemy[i].co3 % 2 == 0)
                                            {
                                                enemy[i].x += 2;
                                                enemy[i].y += 2;
                                            }
                                            else
                                            {
                                                enemy[i].x -= 2;
                                                enemy[i].y -= 2;
                                            }
                                            break;
                                        case 3:                                 //HorizontalEraseモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) enemy[i].co2 = 0;
                                            if (enemy[i].co3 > 45)
                                            {
                                                enemy[i].x += enemy[i].co4; 
                                                if(enemy[i].co3 % 3 == 0) enemy[i].co4 *= -1;
                                            }
                                            else
                                            {
                                                g.FillEllipse(new SolidBrush(ebackcolor), (int)enemy[i].x + 20 + (int)enemy[i].co3, 0, (45-(int)enemy[i].co3) * 2, 700);
                                                for (int i2 = (int)enemy[i].x + 20 + enemy[i].co3; i2 < (int)enemy[i].x + 20 + enemy[i].co3+((45 - enemy[i].co3) * 2); ++i2)
                                                {
                                                    for (int j2 = 1; j2 < 1000; ++j2)
                                                    {
                                                        if ((j2 >= 1) && (j2 <= 1000) && (i2 >= 1) && (i2 <= 700))
                                                        {
                                                            if (disp[j2, i2])
                                                            {
                                                                disp[j2, i2] = false;
                                                                fill--;
                                                            }
                                                        }
                                                    }
                                                }
                                                decision = (double)fill * 100 / (double)700000;
                                            }
                                            break;
                                        case 4:                                 //VerticalEraseモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) enemy[i].co2 = 0;
                                            if (enemy[i].co3 > 45)
                                            {
                                                enemy[i].y += enemy[i].co4;
                                                if (enemy[i].co3 % 3 == 0) enemy[i].co4 *= -1;
                                            }
                                            else
                                            {
                                                g.FillEllipse(new SolidBrush(ebackcolor),0, (int)enemy[i].y + 20 + (int)enemy[i].co3,1000, (45 - (int)enemy[i].co3) * 2);
                                                for (int i2 = 1; i2 < 700; ++i2)
                                                {
                                                    for (int j2 = (int)enemy[i].y + 20 + enemy[i].co3; j2 < (int)enemy[i].y + 20 + enemy[i].co3+((45 - enemy[i].co3) * 2); ++j2)
                                                    {
                                                        if ((j2 >= 1) && (j2 <= 1000) && (i2 >= 1) && (i2 <= 700))
                                                        {
                                                            if (disp[j2, i2])
                                                            {
                                                                disp[j2, i2] = false;
                                                                fill--;
                                                            }
                                                        }
                                                    }
                                                }
                                                decision = (double)fill * 100 / (double)700000;
                                            }
                                            break;
                                        case 5:                                 //Chargeモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0)
                                            {
                                                enemy[i].co2 = 0;
                                                int Lota = rnd.Next(100);    //攻撃パターンの抽選
                                                //Lota = 90;
                                                if (Lota < 50)
                                                {
                                                    enemy[i].co2 = 6;       //パターン決定
                                                    enemy[i].co3 = 45;     //攻撃時間
                                                }
                                                else if (Lota < 100)
                                                {
                                                    enemy[i].co2 = 7;
                                                    enemy[i].co3 = 120;
                                                }
                                                else
                                                {
                                                    enemy[i].co2 = 8;
                                                    enemy[i].co3 = 120;
                                                    enemy[i].co4 = 64;
                                                }
                                            }
                                            break;
                                        case 6:                                 //WhiteOutモード
                                            enemy[i].co3 -= 1;
                                            whiteout = true;
                                            if (enemy[i].co3 <= 0) { enemy[i].co2 = 0; whiteout = false; }
                                            break;
                                        case 7:                                 //WhiteHoleモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) enemy[i].co2 = 0;
                                            angle = Math.Atan2(enemy[i].y + 65 -mousey,enemy[i].x + 65-mousex);
                                            System.Drawing.Point sp = System.Windows.Forms.Cursor.Position;
                                            System.Drawing.Point cp = this.PointToClient(sp);
                                            System.Windows.Forms.Cursor.Position = this.PointToScreen(new System.Drawing.Point((int)(cp.X - Math.Cos(angle)*15), (int)(cp.Y - Math.Sin(angle)*15)));
                                            break;
                                        case 8:                                 //WhiteSummonモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) enemy[i].co2 = 0;
                                            break;
                                        case 101:                               //Phantom用攻撃コマンド   直進モード
                                            enemy[i].x += enemy[i].dco1;
                                            enemy[i].y += enemy[i].dco2;
                                            break;
                                        case 102:                               //Phantom用攻撃コマンド   追跡モード
                                            if (true)
                                            {
                                                angle = Math.Atan2(mousey - enemy[i].y - 65, mousex - enemy[i].x - 65);
                                                enemy[i].x += Math.Cos(angle) * enemyesdata.inckholespeedLow;
                                                enemy[i].y += Math.Sin(angle) * enemyesdata.inckholespeedLow;
                                            }
                                            break;
                                    }
                                    #endregion
                                    #region HitCheck
                                    ENEMY_MY_CD = enemyesdata.inckholecd;
                                    ENEMY_MY_SIZE = enemyesdata.inckholesize/2; //大体半径だから先に/2
                                    if ((mousex - enemy[i].x - ENEMY_MY_SIZE) * (mousex - enemy[i].x - 50 - ENEMY_MY_SIZE)
                                        + (mousey - enemy[i].y - ENEMY_MY_SIZE) * (mousey - enemy[i].y - ENEMY_MY_SIZE)
                                        <= ((csize - 1) / 2 + ENEMY_MY_CD) * ((csize - 1) / 2 + ENEMY_MY_CD) && (bStart) && (drawing))
                                    {
                                        DrawLevel -= 33333;
                                        hitcounter = 60;
                                        bStart = false;
                                        spritink(2, (int)enemy[i].x + ENEMY_MY_SIZE, (int)enemy[i].y + ENEMY_MY_SIZE, HittingDamageDisp, ebackcolor);
                                    }
                                    #endregion
                                    break;
                                case 6:
                                    #region InkBallLLL
                                    enemy[i].x += enemy[i].dco1;
                                    enemy[i].y += enemy[i].dco2;
                                    angle = Math.Atan2(mousey - enemy[i].y - 100, mousex - enemy[i].x - 100);
                                    enemy[i].dco1 += Math.Cos(angle) * 1;
                                    enemy[i].dco2 += Math.Sin(angle) * 1;
                                    if (enemy[i].x < 0)
                                    {
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - 100, mousex - enemy[i].x - 100);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballLLLspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballLLLspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco1 = enemy[i].dco1 * (-1);
                                        }
                                    }
                                    else if (enemy[i].x > 1000 - enemyesdata.inkballLLLsize)
                                    {
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - 100, mousex - enemy[i].x - 100);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballLLLspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballLLLspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco1 = enemy[i].dco1 * (-1);
                                        }
                                    }
                                    else if (enemy[i].y < 0)
                                    {
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - 100, mousex - enemy[i].x - 100);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballLLLspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballLLLspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco2 = enemy[i].dco2 * (-1);
                                        }
                                    }
                                    else if (enemy[i].y > 700 - enemyesdata.inkballLLLsize)
                                    {
                                        if ((mousex >= 0) && (mousex <= 1000) && (mousey >= 0) && (mousey <= 700))
                                        {
                                            angle = Math.Atan2(mousey - enemy[i].y - 100, mousex - enemy[i].x - 100);
                                            enemy[i].dco1 = Math.Cos(angle) * enemyesdata.inkballLLLspeed;
                                            enemy[i].dco2 = Math.Sin(angle) * enemyesdata.inkballLLLspeed;
                                        }
                                        else
                                        {
                                            enemy[i].dco2 = enemy[i].dco2 * (-1);
                                        }
                                    }
                                    switch (enemy[i].co2)
                                    {
                                        case 0:
                                            enemy[i].co3 -= 1;
                                            //if (enemy[i].co3 <= 0) enemy[i].co2 = 0;
                                            if (enemy[i].co3 <= 0)
                                            {
                                                int Lot = rnd.Next(100);    //攻撃パターンの抽選
                                                //Lot = 90;
                                                if (Lot < 50)
                                                {
                                                    enemy[i].co2 = 1;       //パターン決定
                                                    enemy[i].co3 = 90;     //攻撃時間
                                                }
                                                else if (Lot < 100)
                                                {
                                                    enemy[i].co2 = 2;
                                                    enemy[i].co3 = 90;
                                                }
                                                else
                                                {
                                                    enemy[i].co2 = 3;
                                                    enemy[i].co3 = 60;
                                                }
                                            }
                                            break;
                                        case 1:                             //嫉妬ゲロモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) { enemy[i].co2 = 0; enemy[i].co3 = rnd.Next(60)+30; }
                                            if ((enemy[i].co3 % 1 == 0)&&(enemyenum <98))
                                            {
                                                pre_enemy(enemyenum + 1, 2, true, 10, (int)enemy[i].x + 100, (int)enemy[i].y + 100, 0, 0, 0, 0, 0, 0, (90 - enemy[i].co3)*5 % 360,20/*enemyesdata.inkballMspeed*/);
                                                pre_enemy(enemyenum + 2, 2, true, 10, (int)enemy[i].x + 100, (int)enemy[i].y + 100, 0, 0, 0, 0, 0, 0, 360 - ((90 - enemy[i].co3) * 5 % 360), 20/*enemyesdata.inkballMspeed*/);
                                            }
                                            break;
                                        case 2:                             //追跡ウンコモード
                                            enemy[i].co3 -= 1;
                                            if (enemy[i].co3 <= 0) { enemy[i].co2 = 0; enemy[i].co3 = rnd.Next(60)+30; }
                                            if ((enemy[i].co3 % 6 == 0)&&(enemyenum <98))
                                            {
                                                pre_enemy(enemyenum + 1, 5, true, 30, (int)enemy[i].x + 100, (int)enemy[i].y + 100, 0, 102, 0, 0, 0, 0);
                                                //pre_enemy(enemyenum + 2, 2, true, 10, (int)enemy[i].x + 100, (int)enemy[i].y + 100, 0, 0, 0, 0, 0, 0, 360 - ((90 - enemy[i].co3) * 5 % 360), 20/*enemyesdata.inkballMspeed*/);
                                            }
                                            break;
                                    }
                                    #endregion
                                    #region HitCheck
                                    ENEMY_MY_CD = enemyesdata.inkballLLLcd;
                                    ENEMY_MY_SIZE = enemyesdata.inkballLLLsize/2; //大体半径だから先に/2
                                    if ((mousex - enemy[i].x - ENEMY_MY_SIZE) * (mousex - enemy[i].x - 50 - ENEMY_MY_SIZE)
                                        + (mousey - enemy[i].y - ENEMY_MY_SIZE) * (mousey - enemy[i].y - ENEMY_MY_SIZE)
                                        <= ((csize - 1) / 2 + ENEMY_MY_CD) * ((csize - 1) / 2 + ENEMY_MY_CD) && (bStart) && (drawing))
                                    {
                                        DrawLevel -= 33333;
                                        hitcounter = 60;
                                        bStart = false;
                                        spritink(2, (int)enemy[i].x + ENEMY_MY_SIZE, (int)enemy[i].y + ENEMY_MY_SIZE, HittingDamageDisp, ebackcolor);
                                    }
                                    #endregion
                                    break;
                                case 7:
                                    break;
                                case 8:
                                    #region ColorKiller
                                    enemy[i].co1--;
                                    if(enemy[i].co1 == -1) enemy[i].co1 = enemyesdata.colorkillerblink;
                                    if (fill > 500000)  //通常モード
                                    {
                                        if (enemy[i].co1 % 8 == 0)
                                        {
                                            int colorkillerx = rnd.Next(1000);
                                            int colorkillery = rnd.Next(700);
                                            int colorkillerrad = rnd.Next(25) + 25;
                                            spritink(2, colorkillerx, colorkillery, colorkillerrad, ebackcolor);
                                            FillCalcuration(false, colorkillerx, colorkillery, colorkillerrad);
                                        }
                                    }
                                    else                //発狂モード
                                    {
                                        if (enemy[i].co1 % 4 == 0)
                                        {
                                            int colorkillerx = rnd.Next(1000);
                                            int colorkillery = rnd.Next(700);
                                            int colorkillerrad = rnd.Next(40) + 40;
                                            spritink(2, colorkillerx, colorkillery, colorkillerrad, ebackcolor);
                                            FillCalcuration(false, colorkillerx, colorkillery, colorkillerrad);
                                        }
                                    }
                                    
                                    #endregion
                                    break;
                            }
                            #region GameoverProsessing
                            if (DrawLevel <= 0)
                            {
                                cleartime = DateTime.Now - starttime;
                                gamemode = 100;
                                timer.Interval = 10;
                                stagestart();
                                clear.Visible = true;
                                menu.Visible = true;
                                replay.Visible = true;
                                if (selectedstage != 27) next.Visible = true;
                                cleartimedisp.Text = "All Hope Is Gone";
                                cleartimedisp.Visible = true;
                                clear.Text = "GAME OVER";
                                Cursor.Show();
                            }
                            #endregion
                        }
                        #endregion
                        //drawenemy.draws(ref enemy, enemyenum, g);
                        draws();
                        g.Dispose();
                    }
                    //gamemode = 96;//確認作業用
                    break;
                case 100:                   //クリア後画面

                    break;
                case 101:                   //ステージ情報表示画面
                    if (dispcounter < 150)
                    {
                        dispcounter++;
                        stagetitle.Height += 4;
                        if (stagetitle.Height < 400) stagetitle.Height += 4;
                        target.Top += 2;
                        if (stagetitle.Height < 400) target.Top += 2;
                        stagetitleout.Height += 5;
                    }
                    /*else if (dispcounter == 150)
                    {
                        dispcounter++;
                        target.Visible = false;
                    }*/
                    /*else if (stagetitle.Height != 0)
                    {
                        stagetitle.Height -=
                    }*/
                    else
                    {
                        target.Visible = false;
                        stagetitle.Visible = false;
                        stagetitleout.Visible = false;
                        starttime = DateTime.Now;
                        gamemode = 99;

                        timer.Interval = 45;
                        Cursor.Hide();//このタイミングで非表示にしてみる
                    }
                    break;
                #endregion

            }
        }

        #region FillCalculation
        private void FillCalcuration(bool mode,int cx,int cy,int radius)//mode = Trueで塗りつぶしモード　Falseで消去モード  cx=中心のX座標  cy=中心のY座標  radius=半径
        {
            for (int j = cy - radius + 1; j < cy; ++j)
            {
                bout = false;
                for (int i = cx - radius; i < cx + radius; ++i)
                {
                    int csizep = radius * radius;
                    if ((i - cx) * (i - cx) + (j - cy) * (j - cy) <= csizep)
                    {
                        k = 2 * cx - i;
                        kk = 2 * cy - j;
                        for (int l = i; l < k + 1; ++l)
                        {
                            if ((l >= 1) && (l <= 1000) && (j >= 1) && (j <= 700))
                            {
                                if (mode)
                                {
                                    if (!disp[l, j])
                                    {
                                        disp[l, j] = true;
                                        fill++;
                                    }
                                }
                                else
                                {
                                    if (disp[l, j])
                                    {
                                        disp[l, j] = false;
                                        fill--;
                                    }
                                }
                            }
                            if ((l >= 1) && (l <= 1000) && (kk >= 1) && (kk <= 700))
                            {
                                if (mode)
                                {
                                    if (!disp[l, kk])
                                    {
                                        disp[l, kk] = true;
                                        fill++;
                                    }
                                }
                                else
                                {
                                    if (disp[l, kk])
                                    {
                                        disp[l, kk] = false;
                                        fill--;
                                    }
                                }
                            }
                        }
                        bout = true;
                        break;
                    }
                }
                if (bout)
                {
                    continue;
                }
            }
            decision = (double)fill * 100 / (double)700000;
            /*for (int j = mousey - ((csize - 1) / 2) + 1; j < mousey; ++j)
            {
                bout = false;
                for (int i = mousex - ((csize - 1) / 2); i < mousex + ((csize - 1) / 2); ++i)
                {
                    int csizep = (csize / 2) * (csize / 2);
                    if ((i - mousex) * (i - mousex) + (j - mousey) * (j - mousey) <= csizep)
                    {
                        k = 2 * mousex - i;
                        kk = 2 * mousey - j;
                        for (int l = i; l < k + 1; ++l)
                        {
                            if ((l >= 1) && (l <= 1000) && (j >= 1) && (j <= 700))
                            {
                                if (!disp[l, j])
                                {
                                    disp[l, j] = true;
                                    fill++;
                                }
                            }
                            if ((l >= 1) && (l <= 1000) && (kk >= 1) && (kk <= 700))
                            {
                                if (!disp[l, kk])
                                {
                                    disp[l, kk] = true;
                                    fill++;
                                }
                            }
                        }
                        bout = true;
                        break;
                    }
                }
                if (bout)
                {
                    continue;
                }
            }*/
        }
        #endregion

        #region EnemyProcessing
        private void enemy_data_call()
        {
            enemyenum = 0;
            for (int i = 1; i <= enemysoat; ++i)
            {
                if (int.Parse(Mid(stageoption[selectedstage + 1], i, 1)) != 0)
                {
                    for (int j = 1; j <= int.Parse(Mid(stageoption[selectedstage + 1], i, 1)); ++j)
                    {
                        enemyenum += 1;
                        switch (i)
                        {
                            case 1:
                                pre_inkballL(enemyenum);
                                break;
                            case 2:
                                pre_inkballM(enemyenum);
                                break;
                            case 3:
                                pre_inkballS(enemyenum);
                                break;
                            case 4:
                                pre_inckholeblack(enemyenum);
                                break;
                            case 5:
                                pre_inckholewhite(enemyenum);
                                break;
                            case 6:
                                pre_inkballLLL(enemyenum);
                                break;
                            case 7:
                                pre_inkvader(enemyenum);
                                break;
                            case 8:
                                pre_colorkiller(enemyenum);
                                break;
                        }
                    }
                }
            }
        }

        private void pre_enemy(int objectnum, int objectclass, bool live, int lifespan, double x, double y, int co1, int co2, int co3, int co4, double dco1,double dco2,int angle=361,int speed=0)
        {
            //objectnumには(enemyenum+1)を入れること  この関数以外でenemyenumの値を変更しないこと！
            //ステージのロードでこの関数を使わないこと
            enemy[objectnum].objectclass = objectclass;
            enemy[objectnum].live = live;
            enemy[objectnum].lifespan = lifespan;
            enemy[objectnum].x = x;
            enemy[objectnum].y = y;
            enemy[objectnum].co1 = co1;
            enemy[objectnum].co2 = co2;
            enemy[objectnum].co3 = co3;
            enemy[objectnum].co4 = co4;
            //enemy[objectnum].co5 = co5;
            enemy[objectnum].dco1 = dco1;
            enemy[objectnum].dco2 = dco2;
            if (angle < 361)
            {
                enemy[objectnum].dco1 = speed  * usercos[angle];
                enemy[objectnum].dco2 = speed * usersin[angle];
            }
            enemyenum += 1;
        }

        private void summon_enemyes(int summonNum, int startobjectnum, int objectclass, bool live, int lifespan, double[] x, double[] y, int co1, int co2, int co3, int co4, double dco1, double dco2, int[] angle, int[] speed)
        {
            for (int i = startobjectnum; i <= startobjectnum + summonNum - 1; ++i)
            {
                if (angle[i - startobjectnum] > 360)
                {
                    pre_enemy(i, objectclass, live, lifespan, x[i - startobjectnum], y[i - startobjectnum], co1, co2, co3, co4, dco1, dco2);
                }
                else
                {
                    pre_enemy(i, objectclass, live, lifespan, x[i - startobjectnum], y[i - startobjectnum], co1, co2, co3, co4, dco1, dco2, angle[i - startobjectnum], speed[i - startobjectnum]);
                }
            }
        }

        private void pre_inkballL(int objectnum)
        {
            enemy[objectnum].objectclass = 1;
            enemy[objectnum].live = true;
            enemy[objectnum].lifespan = 0;
            enemy[objectnum].x = rnd.Next(1000 - enemyesdata.inkballLsize);
            enemy[objectnum].y = rnd.Next(700 - enemyesdata.inkballLsize);
            int angle = rnd.Next(360);
            while ((angle < 15) || (angle > 75) && (angle < 105) || (angle > 165) && (angle < 195) || (angle > 255) && (angle < 285) || (angle > 345))
            {
                angle = rnd.Next(360);
            }
            enemy[objectnum].dco1 = enemyesdata.inkballLspeed * usercos[angle];
            enemy[objectnum].dco2 = enemyesdata.inkballLspeed * usersin[angle];
        }

        private void pre_inkballM(int objectnum)
        {
            enemy[objectnum].objectclass = 2;
            enemy[objectnum].live = true;
            enemy[objectnum].lifespan = 0;
            enemy[objectnum].x = rnd.Next(1000 - enemyesdata.inkballMsize);
            enemy[objectnum].y = rnd.Next(700 - enemyesdata.inkballMsize);
            int angle = rnd.Next(360);
            while ((angle < 15) || (angle > 75) && (angle < 105) || (angle > 165) && (angle < 195) || (angle > 255) && (angle < 285) || (angle > 345))
            {
                angle = rnd.Next(360);
            }
            enemy[objectnum].dco1 = enemyesdata.inkballMspeed * usercos[angle];
            enemy[objectnum].dco2 = enemyesdata.inkballMspeed * usersin[angle];
        }

        private void pre_inkballS(int objectnum)
        {
            enemy[objectnum].objectclass = 3;
            enemy[objectnum].live = true;
            enemy[objectnum].lifespan = 0;
            enemy[objectnum].x = rnd.Next(1000 - enemyesdata.inkballSsize);
            enemy[objectnum].y = rnd.Next(700 - enemyesdata.inkballSsize);
            int angle = rnd.Next(360);
            while ((angle < 15) || (angle > 75) && (angle < 105) || (angle > 165) && (angle < 195) || (angle > 255) && (angle < 285) || (angle > 345))
            {
                angle = rnd.Next(360);
            }
            enemy[objectnum].dco1 = enemyesdata.inkballSspeed * usercos[angle];
            enemy[objectnum].dco2 = enemyesdata.inkballSspeed * usersin[angle];
        }

        private void pre_inckholeblack(int objectnum)
        {
            enemy[objectnum].objectclass = 4;
            enemy[objectnum].live = true;
            enemy[objectnum].lifespan = 0;
            enemy[objectnum].x = rnd.Next(1000 - enemyesdata.inckholesize);
            enemy[objectnum].y = rnd.Next(700 - enemyesdata.inckholesize);
            enemy[objectnum].co1 = 0;
            enemy[objectnum].co2 = 0;
        }

        private void pre_inckholewhite(int objectnum)
        {
            enemy[objectnum].objectclass = 5;
            enemy[objectnum].live = true;
            enemy[objectnum].lifespan = 0;
            enemy[objectnum].x = rnd.Next(1000 - enemyesdata.inckholesize);
            enemy[objectnum].y = rnd.Next(700 - enemyesdata.inckholesize);
            enemy[objectnum].co1 = 0;
            enemy[objectnum].co2 = 0;
        }

        private void pre_inkballLLL(int objectnum)
        {
            enemy[objectnum].objectclass = 6;
            enemy[objectnum].live = true;
            enemy[objectnum].lifespan = 0;
            enemy[objectnum].x = rnd.Next(1000 - enemyesdata.inkballLLLsize);
            enemy[objectnum].y = rnd.Next(700 - enemyesdata.inkballLLLsize);
            int angle = rnd.Next(360);
            while ((angle < 15) || (angle > 75) && (angle < 105) || (angle > 165) && (angle < 195) || (angle > 255) && (angle < 285) || (angle > 345))
            {
                angle = rnd.Next(360);
            }
            enemy[objectnum].dco1 = enemyesdata.inkballLLLspeed * usercos[angle];
            enemy[objectnum].dco2 = enemyesdata.inkballLLLspeed * usersin[angle];
            enemy[objectnum].co2 = 0;
            enemy[objectnum].co3 = 90;
        }

        private void pre_inkvader(int objectnum)
        {
            enemy[objectnum].objectclass = 7;
            enemy[objectnum].live = true;
            enemy[objectnum].lifespan = 0;
            enemy[objectnum].x = 0;
            enemy[objectnum].y = 0;
        }

        private void pre_colorkiller(int objectnum)
        {
            enemy[objectnum].objectclass = 8;
            enemy[objectnum].live = true;
            enemy[objectnum].lifespan = 0;
            enemy[objectnum].x = 0;
            enemy[objectnum].y = 0;
            enemy[objectnum].co1 = enemyesdata.colorkillerblink;
            enemy[objectnum].co2 = 1;
        }

        private void NaturalDeath(int dead, int survivor)       //deadが死んだ敵の敵番号 suvivorが生きている一番最後の敵番号
        {
            if (dead != survivor)
            {
                enemy[dead].objectclass = enemy[survivor].objectclass;
                enemy[dead].live = enemy[survivor].live;
                enemy[dead].lifespan = enemy[survivor].lifespan;
                enemy[dead].x = enemy[survivor].x;
                enemy[dead].y = enemy[survivor].y;
                enemy[dead].co1 = enemy[survivor].co1;
                enemy[dead].co2 = enemy[survivor].co2;
                enemy[dead].co3 = enemy[survivor].co3;
                enemy[dead].co4 = enemy[survivor].co4;
                enemy[dead].dco1 = enemy[survivor].dco1;
                enemy[dead].dco2 = enemy[survivor].dco2;
            }
        }
#endregion

        #region Draws
        private void draws()
        {
            Graphics g = Graphics.FromImage(canvasl2);
            if (circleantialiasing) g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(ebackcolor);
            g.DrawImage(canvas, 0, 0, 1000, 700);
            if (blackout) 
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), 0, 0, this.Width, this.Height);
                g.DrawString("You Cannot See Campus", new Font(pfc.Families[FontNum], 50), new SolidBrush(Color.FromArgb(255, 128, 128, 128)), 90, 80);
            }
            if (whiteout)
            {
                g.DrawString("You Cannot See Enemyes", new Font(pfc.Families[FontNum], 50), new SolidBrush(Color.FromArgb(255, 128, 128, 128)), 80, 550);
            }
            else
            {
                for (int i = 1; i <= enemyenum; ++i)
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
                            if (enemy[i].lifespan != 0)
                            {
                                g.DrawImage(inckholeblackphantomimg, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                            }
                            else
                            {
                                switch (enemy[i].co1)
                                {
                                    case 0:
                                        g.DrawImage(inckholeblack1img, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                                        break;
                                    case 1:
                                        g.DrawImage(inckholeblack2img, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                                        break;
                                    case 2:
                                        g.DrawImage(inckholeblack3img, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                                        break;
                                }
                                if (enemy[i].co2 >= 4)
                                {
                                    g.DrawImage(inckholeblackphantomimg, (int)enemy[i].x - 4 + rnd.Next(9), (int)enemy[i].y - 4 + rnd.Next(9), 130, 130);
                                }
                            }
                            //g.DrawImage(inckholeblack1img, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                            break;
                        case 5:
                            if (enemy[i].lifespan != 0)
                            {
                                g.DrawImage(inckholewhitephantomimg, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                            }
                            else
                            {
                                switch (enemy[i].co1)
                                {
                                    case 0:
                                        g.DrawImage(inckholewhite1img, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                                        break;
                                    case 1:
                                        g.DrawImage(inckholewhite2img, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                                        break;
                                    case 2:
                                        g.DrawImage(inckholewhite3img, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                                        break;
                                }
                                if ((enemy[i].co2 == 5) || (enemy[i].co2 == 7) || (enemy[i].co2 == 8))
                                {
                                    g.DrawImage(inckholewhitephantomimg, (int)enemy[i].x - 4 + rnd.Next(9), (int)enemy[i].y - 4 + rnd.Next(9), 130, 130);
                                }
                            }
                            //g.DrawImage(inckholewhite1img, (int)enemy[i].x, (int)enemy[i].y, 130, 130);
                            break;
                        case 6:
                            g.DrawImage(inkballLLLbodyimg, (int)enemy[i].x, (int)enemy[i].y, 200, 200);
                            if (mousex <= enemy[i].x+80)
                            {
                                g.DrawImage(inkballLLLeyeimg, (int)enemy[i].x-20, (int)enemy[i].y, 200, 200);
                            }
                            else if ((mousex > enemy[i].x+80) && (mousex < enemy[i].x + 120))
                            {
                                g.DrawImage(inkballLLLeyeimg, mousex - 100, (int)enemy[i].y, 200, 200);
                            }
                            else
                            {
                                g.DrawImage(inkballLLLeyeimg, (int)enemy[i].x + 20, (int)enemy[i].y, 200, 200);
                            }
                            break;
                        case 7:
                            g.DrawImage(inkvaderimg, (int)enemy[i].x, (int)enemy[i].y, 1000, 700);
                            break;
                        case 8:
                            if ((enemy[i].co1 == 9) || (enemy[i].co1 == 29))
                            {
                                enemy[i].co2 = 10;
                            }
                            else if ((enemy[i].co1 == 8) || (enemy[i].co1 == 30))
                            {
                                enemy[i].co2 = 9;
                            }
                            else if ((enemy[i].co1 == 7) || (enemy[i].co1 == 31))
                            {
                                enemy[i].co2 = 8;
                            }
                            else if ((enemy[i].co1 == 6) || (enemy[i].co1 == 32))
                            {
                                enemy[i].co2 = 7;
                            }
                            else if ((enemy[i].co1 == 5) || (enemy[i].co1 == 33))
                            {
                                enemy[i].co2 = 6;
                            }
                            else if ((enemy[i].co1 == 4) || (enemy[i].co1 == 34))
                            {
                                enemy[i].co2 = 5;
                            }
                            else if ((enemy[i].co1 == 3) || (enemy[i].co1 == 35))
                            {
                                enemy[i].co2 = 4;
                            }
                            else if ((enemy[i].co1 == 2) || (enemy[i].co1 == 36))
                            {
                                enemy[i].co2 = 3;
                            }
                            else if ((enemy[i].co1 == 1) || (enemy[i].co1 == 37))
                            {
                                enemy[i].co2 = 2;
                            }
                            else if (enemy[i].co1 > 53)
                            {
                                enemy[i].co2 = 1;
                            }
                            if (fill >= 350000)
                            {
                                switch (enemy[i].co2)
                                {
                                    case 1:
                                        g.DrawImage(colorkillerimg1, 300 - 4 + rnd.Next(9), 150 - 4 + rnd.Next(9), 400, 400);
                                        break;
                                    case 2:
                                        g.DrawImage(colorkillerimg2, 300 - 4 + rnd.Next(9), 150 - 4 + rnd.Next(9), 400, 400);
                                        break;
                                    case 3:
                                        g.DrawImage(colorkillerimg3, 300 - 4 + rnd.Next(9), 150 - 4 + rnd.Next(9), 400, 400);
                                        break;
                                    case 4:
                                        g.DrawImage(colorkillerimg4, 300 - 4 + rnd.Next(9), 150 - 4 + rnd.Next(9), 400, 400);
                                        break;
                                    case 5:
                                        g.DrawImage(colorkillerimg5, 300 - 4 + rnd.Next(9), 150 - 4 + rnd.Next(9), 400, 400);
                                        break;
                                    case 6:
                                        g.DrawImage(colorkillerimg6, 300 - 4 + rnd.Next(9), 150 - 4 + rnd.Next(9), 400, 400);
                                        break;
                                    case 7:
                                        g.DrawImage(colorkillerimg7, 300 - 4 + rnd.Next(9), 150 - 4 + rnd.Next(9), 400, 400);
                                        break;
                                    case 8:
                                        g.DrawImage(colorkillerimg8, 300 - 4 + rnd.Next(9), 150 - 4 + rnd.Next(9), 400, 400);
                                        break;
                                    case 9:
                                        g.DrawImage(colorkillerimg9, 300 - 4 + rnd.Next(9), 150 - 4 + rnd.Next(9), 400, 400);
                                        break;
                                    case 10:
                                        g.DrawImage(colorkillerimg10, 300 - 4 + rnd.Next(9), 150 - 4 + rnd.Next(9), 400, 400);
                                        break;
                                }
                                g.DrawImage(colorkillerphantomimg, 300 - 16 + rnd.Next(33), 150 - 16 + rnd.Next(33), 400, 400);
                            }
                            else
                            {
                                switch (enemy[i].co2)
                                {
                                    case 1:
                                        g.DrawImage(colorkillerimg1, 300, 150, 400, 400);
                                        break;
                                    case 2:
                                        g.DrawImage(colorkillerimg2, 300, 150, 400, 400);
                                        break;
                                    case 3:
                                        g.DrawImage(colorkillerimg3, 300, 150, 400, 400);
                                        break;
                                    case 4:
                                        g.DrawImage(colorkillerimg4, 300, 150, 400, 400);
                                        break;
                                    case 5:
                                        g.DrawImage(colorkillerimg5, 300, 150, 400, 400);
                                        break;
                                    case 6:
                                        g.DrawImage(colorkillerimg6, 300, 150, 400, 400);
                                        break;
                                    case 7:
                                        g.DrawImage(colorkillerimg7, 300, 150, 400, 400);
                                        break;
                                    case 8:
                                        g.DrawImage(colorkillerimg8, 300, 150, 400, 400);
                                        break;
                                    case 9:
                                        g.DrawImage(colorkillerimg9, 300, 150, 400, 400);
                                        break;
                                    case 10:
                                        g.DrawImage(colorkillerimg10, 300, 150, 400, 400);
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
            #region DrawPlayerHUD
            if (dispr < r)
            {
                dispr += 3;
                if (dispr > r) dispr = r;
            }
            else if(dispr > r)
            {
                dispr -= 3;
                if (dispr < r) dispr = r;
            }
            if (dispg < this.g)
            {
                dispg += 3;
                if (dispg > this.g) dispg = this.g;
            }
            else if (dispg > this.g)
            {
                dispg -= 3;
                if (dispg < this.g) dispg = this.g;
            }
            if (dispb < b)
            {
                dispb += 3;
                if (dispb > b) dispb = b;
            }
            else if (dispb > b)
            {
                dispb -= 3;
                if (dispb < b) dispb = b;
            }
            double DDrawLevel = ((double)DrawLevel / 99999)*130;
            double Ddispr = ((double)dispr / 255) * 210;
            double Ddispg = ((double)dispg / 255) * 210;
            double Ddispb = ((double)dispb / 255) * 210;
            double DHUDsizecosR = ((Double)csize/120)*41*HUDsizecosR;//左線の長さは41
            double DHUDsizesinR = ((Double)csize/120)*41*HUDsizesinR;//左線の長さは41
            double DHUDsizecosL = ((Double)csize / 120) * 41 * HUDsizecosL;//右線も41
            double DHUDsizesinL = ((Double)csize / 120) * 41 * HUDsizesinL;//右線も41
            //Point[] points = { new Point(mousex - 26, mousey + 19), new Point(mousex - 20, mousey + 25), new Point(mousex - 44, mousey + 58), new Point(mousex - 59, mousey + 43)};
            Point[] points = { new Point(mousex - 26, mousey + 19), new Point(mousex - 20, mousey + 25), new Point(mousex - 20 + (int)DHUDsizecosR, mousey + 25 + (int)DHUDsizesinR), new Point(mousex - 26 + (int)DHUDsizecosL, mousey + 19+(int)DHUDsizesinL)};
            //g.DrawArc(new Pen(Color.FromArgb(255, 128, 128, 128),8), new Rectangle(new Point(mousex - 50, mousey - 50), new Size(100, 100)), 0, 90);
            g.DrawArc(new Pen(Color.FromArgb(178, 255, 0, 0), 2), new Rectangle(new Point(mousex - 53, mousey - 53), new Size(106, 106)), 145,(int)Ddispr);     //R
            g.DrawArc(new Pen(Color.FromArgb(178, 0, 255, 0), 2), new Rectangle(new Point(mousex - 47, mousey - 47), new Size(94, 94)), 145, (int)Ddispg);      //G
            g.DrawArc(new Pen(Color.FromArgb(178, 0, 0, 255), 2), new Rectangle(new Point(mousex - 41, mousey - 41), new Size(82, 82)), 145, (int)Ddispb);      //B
            g.DrawArc(new Pen(Color.FromArgb(178, 255, 191, 0), 6), new Rectangle(new Point(mousex - 40, mousey - 40), new Size(80, 80)), -3, (int)DDrawLevel);             //HP
            //g.DrawArc(new Pen(Color.FromArgb(178, 255, 191, 0), 60), new Rectangle(new Point(mousex - 50, mousey - 50), new Size(100, 100)), 130, 12);          //Size
            g.FillPolygon(new SolidBrush(Color.FromArgb(178, 255, 191, 0)), points);
            g.DrawString(String.Format("{0:00000}", DrawLevel), new System.Drawing.Font(pfc.Families[FontNum], 8), new SolidBrush(Color.FromArgb(178, 50, 181, 229)), mousex - 19, mousey + 8);
            g.DrawString(nowtime.ToString(@"mm\:ss\:ff"), new System.Drawing.Font(pfc.Families[FontNum], 8), new SolidBrush(Color.FromArgb(178, 255, 191, 0)), mousex - 25, mousey - 17);
            //g.DrawString("1000",)
            g.DrawImage(HUDimg, mousex - 70, mousey - 70, 140, 140);
            #endregion
            if (hitcounter % 4 != 0) g.DrawString("DAMAGE", new System.Drawing.Font(pfc.Families[FontNum], 160), new SolidBrush(Color.FromArgb(80, 255, 0, 0)), 30, 200);
            g.Dispose();
        }
        #endregion

        #region SpritInk
        private void spritink(byte mode, int icx, int icy, int irad, Color spritcolor)//インクがはねたような視覚効果を出す関数　かなり乱雑...
        //引数の解説 左から モード(0で座標とサイズのみランダム{この場合他の変数は0で入力する},1でサイズのみ指定でほかはランダム,2で座標とサイズ指定),中心円の中心点のx座標,y座標,中心円の半径,
        {
            Graphics g = Graphics.FromImage(canvas);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Color ccolor;       //中心円の色　モード1の場合飛沫の色もコレになる
            int ccx;            //中心円のx座標 モード0-1で使用
            int ccy;            //中心円のy座標 モード0-1で使用
            int centersize;     //中心円の半径  モード0-1で使用
            int lrad;           //飛沫の中心を中心点から見た半径
            double lang;        //飛沫を中心点から見た角度
            int lsize;          //飛沫の半径
            int lcx;            //飛沫の中心のx座標
            int lcy;            //飛沫の中心のy座標
            switch (mode)
            {
                case 0:
                    ccolor = spritcolor;
                    ccx = rnd.Next(1001);
                    ccy = rnd.Next(701);
                    centersize = rnd.Next(100) + 25;//ここは引数を使って新しいモードを作ってもいいかもしれない... とりあえず汎用性を考えていない
                    g.FillEllipse(new SolidBrush(ccolor), ccx - centersize, ccy - centersize, centersize * 2, centersize * 2);
                    for (int i = 0; i < 20; i++)//第一飛沫円         数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize / 2) + centersize;       //テキトー
                        lsize = rnd.Next(centersize / 5) + centersize / 10;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    for (int i = 0; i < 50; i++)//第二飛沫円         数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize) + centersize;       //テキトー
                        lsize = rnd.Next(centersize / 8) + centersize / 20;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    for (int i = 0; i < 100; i++)//第三飛沫円        数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize + centersize / 2) + centersize;       //テキトー
                        lsize = rnd.Next(3) + 1;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    break;
                case 1:
                    break;
                case 2:
                    ccolor = spritcolor;
                    ccx = icx;
                    ccy = icy;
                    centersize = irad;//ここは引数を使って新しいモードを作ってもいいかもしれない... とりあえず汎用性を考えていない
                    g.FillEllipse(new SolidBrush(ccolor), ccx - centersize, ccy - centersize, centersize * 2, centersize * 2);
                    for (int i = 0; i < 20; i++)//第一飛沫円         数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize / 2) + centersize;       //テキトー
                        lsize = rnd.Next(centersize / 5) + centersize / 10;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    for (int i = 0; i < 50; i++)//第二飛沫円         数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize) + centersize;       //テキトー
                        lsize = rnd.Next(centersize / 8) + centersize / 20;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    for (int i = 0; i < 100; i++)//第三飛沫円        数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize + centersize / 2) + centersize;       //テキトー
                        lsize = rnd.Next(3) + 1;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    break;
                    g.Dispose();
            }
        }

        private void spritinklight(byte mode, int icx, int icy, int irad, Color spritcolor)//インクがはねたような視覚効果を出す関数　かなり乱雑...
        //引数の解説 左から モード(0で座標とサイズのみランダム{この場合他の変数は0で入力する},1でサイズのみ指定でほかはランダム,2で座標とサイズ指定),中心円の中心点のx座標,y座標,中心円の半径,
        {
            Graphics g = Graphics.FromImage(canvas);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Color ccolor;       //中心円の色　モード1の場合飛沫の色もコレになる
            int ccx;            //中心円のx座標 モード0-1で使用
            int ccy;            //中心円のy座標 モード0-1で使用
            int centersize;     //中心円の半径  モード0-1で使用
            int lrad;           //飛沫の中心を中心点から見た半径
            double lang;        //飛沫を中心点から見た角度
            int lsize;          //飛沫の半径
            int lcx;            //飛沫の中心のx座標
            int lcy;            //飛沫の中心のy座標
            switch (mode)
            {
                case 0:
                    ccolor = spritcolor;
                    ccx = rnd.Next(1001);
                    ccy = rnd.Next(701);
                    centersize = rnd.Next(100) + 25;//ここは引数を使って新しいモードを作ってもいいかもしれない... とりあえず汎用性を考えていない
                    g.FillEllipse(new SolidBrush(ccolor), ccx - centersize, ccy - centersize, centersize * 2, centersize * 2);
                    for (int i = 0; i < 20; i++)//第一飛沫円         数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize / 2) + centersize;       //テキトー
                        lsize = rnd.Next(centersize / 5) + centersize / 10;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    for (int i = 0; i < 50; i++)//第二飛沫円         数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize) + centersize;       //テキトー
                        lsize = rnd.Next(centersize / 8) + centersize / 20;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    for (int i = 0; i < 100; i++)//第三飛沫円        数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize + centersize / 2) + centersize;       //テキトー
                        lsize = rnd.Next(3) + 1;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    break;
                case 1:
                    break;
                case 2:
                    ccolor = spritcolor;
                    ccx = icx;
                    ccy = icy;
                    centersize = irad;//ここは引数を使って新しいモードを作ってもいいかもしれない... とりあえず汎用性を考えていない
                    g.FillEllipse(new SolidBrush(ccolor), ccx - centersize, ccy - centersize, centersize * 2, centersize * 2);
                    for (int i = 0; i < 20; i++)//第一飛沫円         数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize / 2) + centersize;       //テキトー
                        lsize = rnd.Next(centersize / 5) + centersize / 10;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    for (int i = 0; i < 5; i++)//第二飛沫円         数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize) + centersize;       //テキトー
                        lsize = rnd.Next(centersize / 8) + centersize / 20;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    for (int i = 0; i < 20; i++)//第三飛沫円        数値はいい感じになるようにテキトーに変更しましょう
                    {
                        lrad = rnd.Next(centersize + centersize / 2) + centersize;       //テキトー
                        lsize = rnd.Next(3) + 1;//ここもテキトー
                        lang = rnd.Next(360);
                        lcx = ccx + (int)(usercos[(int)lang] * lrad);
                        lcy = ccy - (int)(usersin[(int)lang] * lrad);
                        g.FillEllipse(new SolidBrush(ccolor), lcx - lsize, lcy - lsize, lsize * 2, lsize * 2);
                    }
                    break;
                    g.Dispose();
            }
        }

        #endregion

        //＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿
        //◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇以下にはゲームの制御関係の関数をなるべく置かないこと◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆
        //￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣


        #region MouseEvent

        private void paintbox_MouseDown(object sender, MouseEventArgs e)
        {
            bStart = true;
            colorname = getrandcolor5(colormode);
        }

        private void paintbox_MouseUp(object sender, MouseEventArgs e)
        {
            bStart = false;
            csize = 1;
        }

        private void paintbox_MouseMove(object sender, MouseEventArgs e)
        {
            //label1.Text = e.X.ToString() + "  " + e.Y.ToString();
            if ((mousex != e.X) && (mousey != e.Y))
            {
                move = true;
                mousex = e.X;
                mousey = e.Y;
            }
        }

        #endregion

        #region GetRandColorProto(1,2)
        /*private Color getrandcolor1()              //実験用ランダムカラーコード発生関数            ※色がめっちゃ偏る
        {
            string r;                               //赤
            string g;                               //緑
            string b;                               //青
            string conr;
            string cong;
            string conb;
            r = Convert.ToString(rnd.Next(255), 16);
            conr = Convert.ToString(256 - int.Parse(r, System.Globalization.NumberStyles.HexNumber),16);
            if (r.Length == 0)
            {
                r = ("00");
            }
            else if (r.Length == 1)
            {
                r = ("0" + r);
            }
            if (conr.Length == 0)
            {
                conr = ("00");
            }
            else if (conr.Length == 1)
            {
                conr = ("0" + conr);
            }
            g = Convert.ToString(rnd.Next(255), 16);
            cong = Convert.ToString(256 - int.Parse(g,System.Globalization.NumberStyles.HexNumber),16);
            if (g.Length == 0)
            {
                g = ("00");
            }
            else if (g.Length == 1)
            {
                g = ("0" + g);
            }
            if (cong.Length == 0)
            {
                cong = ("00");
            }
            else if (cong.Length == 1)
            {
                cong = ("0" + cong);
            }
            b = Convert.ToString(rnd.Next(255), 16);
            conb = Convert.ToString(256 - int.Parse(b, System.Globalization.NumberStyles.HexNumber),16);
            if (b.Length == 0)
            {
                b = ("00");
            }
            else if (b.Length == 1)
            {
                b = ("0" + b);
            }
            if (conb.Length == 0)
            {
                conb = ("00");
            }
            else if (conb.Length == 1)
            {
                conb = ("0" + conb);
            }
            concolorname = ColorTranslator.FromHtml("0x" + conr + cong + conb);     
            return ColorTranslator.FromHtml("0x" + r + g + b);
        }*/

            /*private Color getrandcolor2()               //改善版ランダムカラーコード発生関数(16777216通り)逆色(Color concolorname)も同時生成      ※未だ高確率(1/30程度)で色が範囲外になるバグ有り。。。原因不明
            {
                string returnB;
                string conreturnB;
                returnB =  Convert.ToString(rnd.Next(16777216), 16);
                conreturnB = Convert.ToString(16777216 - int.Parse(returnB, System.Globalization.NumberStyles.HexNumber), 16);
                switch (returnB.Length)
                {
                    case 0:
                        returnB = ("000000");
                        break;
                    case 1:
                        returnB = ("00000" + returnB);
                        break;
                    case 2:
                        returnB = ("0000" + returnB);
                        break;
                    case 3:
                        returnB = ("000" + returnB);
                        break;
                    case 4:
                        returnB = ("00" + returnB);
                        break;
                    case 5:
                        returnB = ("0" + returnB);
                        break;

                }
                switch (conreturnB.Length)
                {
                    case 0:
                        returnB = ("000000");
                        break;
                    case 1:
                        returnB = ("00000" + conreturnB);
                        break;
                    case 2:
                        returnB = ("0000" + conreturnB);
                        break;
                    case 3:
                        returnB = ("000" + conreturnB);
                        break;
                    case 4:
                        returnB = ("00" + conreturnB);
                        break;
                    case 5:
                        returnB = ("0" + conreturnB);
                        break;

                }
                concolorname = ColorTranslator.FromHtml("0x" + conreturnB);
                return ColorTranslator.FromHtml("0x" + returnB);
            }*/
        #endregion

        private Color getrandcolor3()                       //完成版ランダムカラーコード発生関数     理論値では完全にランダムな数値を返してくれる  メモリ消費最小　実行速度最短
        {
            r = rnd.Next(256);
            g = rnd.Next(256);
            b = rnd.Next(256);
            return Color.FromArgb(255, r, g, b);      //他クラスで使う場合にはこの一行でおk(rndを宣言しておく必要あり)
        }

        #region GetRandColorProto(4)
        /*private Color getrandcolor4(int mode)//拡張版ランダムカラーコード発生関数　明るい系の色や暗い系の色を指定できる           ※汎用性が低いからやめた
        //本来の色の定義より可変域を広めに設定すること
        //mode = 0:All 1:Color 2:Pale 3:Deep 4:RandomLight 5: 6: 50:B&W 51:Sepia 52:ColorBlind
        //最小の値~最大の値+増やした可変域   or   最小の値-増やした可変域~最大の値   or   最小の値-増やした可変域~最大の値+新たに増やした可変域
        {
            int r = rnd.Next(256);            //赤
            int g = rnd.Next(256);            //緑
            int b = rnd.Next(256);            //青
            switch (mode)
            {
                case 2://165~225+20
                    r = rnd.Next(80) + 165;
                    g = rnd.Next(80) + 165;
                    b = rnd.Next(80) + 165;
                    break;
                case 3://11~86
                    r = rnd.Next(128);
                    g = rnd.Next(128);
                    b = rnd.Next(128);
                    break;
                case 4:
                    switch (rnd.Next(6))
                    {
                        case 0:
                            g = 0;b = 0;
                            break;
                        case 1:
                            r = 0;b = 0;
                            break;
                        case 2:
                            r = 0;g = 0;
                            break;
                        case 3:
                            r = 0;
                            break;
                        case 4:
                            g = 0;
                            break;
                        case 5:
                            b = 0;
                            break;
                    }
                    break;
                case 99:
                    switch (rnd.Next(6))
                    {
                        case 0:
                            r = rnd.Next(128)+128;g = 0;b = 0;
                            break;
                        case 1:
                            g = rnd.Next(128)+128;r = 0;b = 0;
                            break;
                        case 2:
                            b = rnd.Next(128)+128;r = 0;g = 0;
                            break;
                        case 3:
                            g = rnd.Next(128)+128;b = rnd.Next(128)+128;r = 0;
                            break;
                        case 4:
                            r = rnd.Next(128)+128;b = rnd.Next(128)+128;g = 0;
                            break;
                        case 5:
                            r = rnd.Next(128)+128;g = rnd.Next(128)+128;b = 0;
                            break;
                    }
                    break;
                case 100:
                    switch (rnd.Next(6))
                    {
                        case 0:
                            r = rnd.Next(64) + 192; g = 0; b = 0;
                            break;
                        case 1:
                            g = rnd.Next(64) + 192; r = 0; b = 0;
                            break;
                        case 2:
                            b = rnd.Next(64) + 192; r = 0; g = 0;
                            break;
                        case 3:
                            g = rnd.Next(64) + 192; b = rnd.Next(64) + 192; r = 0;
                            break;
                        case 4:
                            r = rnd.Next(64) + 192; b = rnd.Next(64) + 192; g = 0;
                            break;
                        case 5:
                            r = rnd.Next(64) + 192; g = rnd.Next(64) + 192; b = 0;
                            break;
                    }
                    break;
                case 50:
                    g = r;
                    b = r;
                    break;
                case 51:
                    r = (int)(r * 0.9);
                    g = (int)(g * 0.7);
                    b = (int)(b * 0.4);
                    break;
                case 52:
                    g = r;
                    break;
            }
            return Color.FromArgb(255, r, g, b);
        }*/
        #endregion

        #region GetRandColor5
        private Color getrandcolor5(int mode)//明輝彩分離型ランダムカラーコード発生関数      色の定義に完全に遵守  汎用性も完璧で新しい色の種類の追加も容易
        {
            //本来の色の定義より可変域を広めに設定すること
            /*mode =    0:All           1:Vivid         2:PerfectVivit      3:Pale          4:VeryPale          5:Deep              6:Light 
             *          7:Dark          8:Soft          9:Strong            10:Dull         11:Grayish          12:LightGrayish     13:DarkGrayish
             *          
             *          100:B&W         101:Sepia       102:ColorBlind      103:Fall        104:Spring          105:Sumer           106:Winter
             *          107:Red         108:Orange      109:Yellow          110:Green       111:Cyan            112:Blue            113:Purple
             *          114:Pink        115:Bloody      116:Gore        
             */
            //指定した種類の色の輝度が最小の場合の色のしきい値を｛最小の値~最大の値+増やした可変域   or   最小の値-増やした可変域~最大の値   or   最小の値-増やした可変域~最大の値+新たに増やした可変域｝と表現する
            //ここで色を増やしたらすること→→→→→→→→1.コンボボックスに項目を追加  2.コンボボックスのイベントに追加
            int imax = 0;               //彩度の最大値
            int imin = 0;               //彩度の最小値
            double ibright = 0;         //輝度の最小値(最大値はibright+0.05)
            double ibrightwaight = 0;   //輝度のしきい値を増加させるただし明るい方向のみの増加
            if ((mode < 100) && (mode != 0))
            {
                switch (mode)           //生成に必要な数値を代入
                {
                    case 1://8~169  *35
                        imax = 169; imin = 8; ibright = 35;
                        break;
                    case 2://0~255  *0
                        imax = 255; imin = 0; ibright = 0;
                        break;
                    case 3://165~216  *75
                        imax = 216; imin = 165; ibright = 75;
                        break;
                    case 4://198~209  *80
                        imax = 209; imin = 198; ibright = 80;
                        break;
                    case 5://65~11  *15
                        imax = 65; imin = 11; ibright = 15;
                        break;
                    case 6://103~228  *65
                        imax = 228; imin = 103; ibright = 65;
                        break;
                    case 7://22~53  *15
                        imax = 53; imin = 22; ibright = 15;
                        break;
                    case 8://94~186  *55
                        imax = 186; imin = 94; ibright = 55;
                        break;
                    case 9://35~142  *35
                        imax = 142; imin = 35; ibright = 35;
                        break;
                    case 10://53~124  *35
                        imax = 124; imin = 53; ibright = 35;
                        break;
                    case 11://91~112  *40
                        imax = 112; imin = 91; ibright = 40;
                        break;
                    case 12://142~163  *60
                        imax = 163; imin = 142; ibright = 60;
                        break;
                    case 13://30~45  *15
                        imax = 45; imin = 30; ibright = 15;
                        break;
                }
                switch (rnd.Next(6))    //色の生成
                {
                    case 0:
                        r = imax; g = imin; b = rnd.Next(imax - imin) + imin;
                        break;
                    case 1:
                        r = imax; b = imin; g = rnd.Next(imax - imin) + imin;
                        break;
                    case 2:
                        g = imax; r = imin; b = rnd.Next(imax - imin) + imin;
                        break;
                    case 3:
                        g = imax; b = imin; r = rnd.Next(imax - imin) + imin;
                        break;
                    case 4:
                        b = imax; r = imin; g = rnd.Next(imax - imin) + imin;
                        break;
                    case 5:
                        b = imax; g = imin; r = rnd.Next(imax - imin) + imin;
                        break;
                }
                if (ibright != 0)       //輝度補正
                {
                    double weight = rnd.Next(6 + (int)ibrightwaight);
                    if (weight != 0)
                    {
                        r = (int)((double)r / ibright * (ibright + ((weight) / 100)));
                        g = (int)((double)g / ibright * (ibright + ((weight) / 100)));
                        b = (int)((double)b / ibright * (ibright + ((weight) / 100)));
                    }
                }
            }
            else
            {
                r = rnd.Next(255); g = rnd.Next(255); b = rnd.Next(255);
                switch (mode)
                {
                    case 100:
                        g = r;
                        b = r;
                        break;
                    case 101:
                        r = rnd.Next(192) + 64;
                        r = (int)(r * 0.9);
                        g = (int)(r * 0.7);
                        b = (int)(r * 0.4);
                        break;
                    case 102:
                        g = r;
                        break;
                    case 103:
                        r = (int)(r * 0.9); g = (int)(g * 0.7); b = (int)(b * 0.4);
                        break;
                    case 107:
                        r = rnd.Next(34) + 222;
                        g = rnd.Next(r - 94);
                        b = g;
                        break;
                    case 110:
                        g = rnd.Next(162) + 94;
                        r = rnd.Next(g - 94);
                        b = r;
                        break;
                    case 115:
                        g = 0;
                        b = 0;
                        break;
                    case 116:
                        r = rnd.Next(64) + 168;
                        if (rnd.Next(5) != 1) r = 0;
                        g = 0;
                        b = 0;
                        break;
                }
            }
            return Color.FromArgb(255, r, g, b);
        }
        #endregion

        private void stagestart()
        {
            for (int i = 0; i < disp.GetLength(0); ++i)
            {
                for (int j = 0; j < disp.GetLength(1); ++j)
                {
                    disp[i, j] = false;
                }
            }
            fill = 0;
            decision = 0;
        }

        #region ClickEvent
        private void selections_Click(object sender, EventArgs e)       //ステージセレクトのクリックイベント
        {
            if ((gamemode == 97)&& (selectedstage == 29))
            {
                selectedstage = int.Parse(((System.Windows.Forms.Label)sender).Name);
                selectcount = 0;
                this.selection[selectedstage].ForeColor = enteringstrcolor;         //点滅対策
                this.selection[selectedstage].BackColor = enteringbackcolor;
                movex = ((double)(this.Width / 2) - (double)this.selection[int.Parse(((System.Windows.Forms.Label)sender).Name)].Left - (double)selectsize / 2) / (double)movetime;//以下5行選択完了後の移動のための演算
                movey = ((double)(this.Height / 2) - (double)this.selection[int.Parse(((System.Windows.Forms.Label)sender).Name)].Top - (double)selectsize / 2) / (double)movetime;
                movingx = this.selection[int.Parse(((System.Windows.Forms.Label)sender).Name)].Left;
                movingy = this.selection[int.Parse(((System.Windows.Forms.Label)sender).Name)].Top;
                backing = new Point(this.selection[int.Parse(((System.Windows.Forms.Label)sender).Name)].Left + selectsize / 2, this.selection[int.Parse(((System.Windows.Forms.Label)sender).Name)].Top);
            }
        }

        private void selections_MouseEnter(object sender, EventArgs e)
        {
            entering = true;
            enteredstage = int.Parse(((System.Windows.Forms.Label)sender).Name);
            enteringstrcolor = this.selection[enteredstage].ForeColor;
            enteringbackcolor = this.selection[enteredstage].BackColor;

        }
        private void selections_MouseLeave(object sender, EventArgs e)
        {
            entering = false;
        }

        private void keypress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (gamemode == 99)
                {
                    cleartime = DateTime.Now - starttime;
                    gamemode = 100;
                    timer.Interval = 10;
                    stagestart();
                    clear.Visible = true;
                    menu.Visible = true;
                    replay.Visible = true;
                    if (selectedstage != 27) next.Visible = true;
                    cleartimedisp.Text = "Color Is Gone";
                    cleartimedisp.Visible = true;
                    clear.Text = "GIVE UP";
                    Cursor.Show();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void option_Click(object sender, EventArgs e)
        {
            if ((gamemode == 53) && (selectedstage == 29) || (gamemode == 54) && (selectedstage == 29))
            {
                gamemode = 55;
            }
            else if (selectedstage == 29)
            {
                gamemode = 53;
            }
            scr = 0;
        }

        private void change_Click(object sender, EventArgs e)
        {
            if ((gamemode == 50) && (selectedstage == 29) || (gamemode == 51)&& (selectedstage == 29))
            {
                gamemode = 52;
            }
            else if (selectedstage == 29)
            {
                gamemode = 50;
            }
            scr = 0;
        }

        private void menu_Click(object sender, EventArgs e)
        {
            clear.Visible = false;
            menu.Visible = false;
            replay.Visible = false;
            next.Visible = false;
            cleartimedisp.Visible = false;
            selectedstage = 0;
            gamemode = 96;
        }

        private void replay_Click(object sender, EventArgs e)
        {
            clear.Visible = false;
            menu.Visible = false;
            replay.Visible = false;
            next.Visible = false;
            cleartimedisp.Visible = false;
            this.white.Location = new Point(this.Width / 2, this.Height / 2);      //説明画面用初期化↓2
            this.white.Size = new Size(0, 0);
            this.white.Visible = true;
            this.white.BringToFront();
            expcounter = 0;
            gamemode = 98;
        }

        private void next_Click(object sender, EventArgs e)
        {
            clear.Visible = false;
            menu.Visible = false;
            replay.Visible = false;
            next.Visible = false;
            cleartimedisp.Visible = false;
            selectedstage++;
            this.white.Location = new Point(this.Width / 2, this.Height / 2);      //説明画面用初期化↓2
            this.white.Size = new Size(0, 0);
            this.white.Visible = true;
            this.white.BringToFront();
            expcounter = 0;
            gamemode = 98;
        }
        #endregion

        #region ColorSelectBox_SelectIndexChanged
        private void colorselectbox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            /*mode =    0:All           1:Vivid         2:PerfectVivit      3:Pale          4:VeryPale          5:Deep              6:Light 
             *          7:Dark          8:Soft          9:Strong            10:Dull         11:Grayish          12:LightGrayish     13:DarkGrayish
             *          
             *          100:B&W         101:Sepia       102:ColorBlind      103:Fall        104:Spring          105:Sumer           106:Winter
             *          107:Red         108:Orange      109:Yellow          110:Green       111:Cyan            112:Blue            113:Purple
             *          114:Pink        115:Bloody      116:Gore        
             */
            switch (colorselectbox.Text)
            {
                case "All":
                    colormode = 0;
                    break;
                case "Vivid":
                    colormode = 1;
                    break;
                case "PerfectVivit":
                    colormode = 2;
                    break;
                case "Pale":
                    colormode = 3;
                    break;
                case "VeryPale":
                    colormode = 4;
                    break;
                case "Deep":
                    colormode = 5;
                    break;
                case "Light":
                    colormode = 6;
                    break;
                case "Dark":
                    colormode = 7;
                    break;
                case "Soft":
                    colormode = 8;
                    break;
                case "Strong":
                    colormode = 9;
                    break;
                case "Dull":
                    colormode = 10;
                    break;
                case "Grayish":
                    colormode = 11;
                    break;
                case "LightGrayish":
                    colormode = 12;
                    break;
                case "DarkGrayish":
                    colormode = 13;
                    break;
                case "Black & White":
                    colormode = 100;
                    break;
                case "Sepia":
                    colormode = 101;
                    break;
                case "ColorBlind":
                    colormode = 102;
                    break;
                case "Fall":
                    colormode = 103;
                    break;
                case "Spring":
                    colormode = 104;
                    break;
                case "Sumer":
                    colormode = 105;
                    break;
                case "Winter":
                    colormode = 106;
                    break;
                case "Red":
                    colormode = 107;
                    break;
                case "Orange":
                    colormode = 108;
                    break;
                case "Yellow":
                    colormode = 109;
                    break;
                case "Green":
                    colormode = 110;
                    break;
                case "Cyan":
                    colormode = 111;
                    break;
                case "Blue":
                    colormode = 112;
                    break;
                case "Purple":
                    colormode = 113;
                    break;
                case "Pink":
                    colormode = 114;
                    break;
                case "Bloody":
                    colormode = 115;
                    break;
                case "Gore":
                    colormode = 116;
                    break;
            }
            control_color_shuffle();
            if (gamemode != 99)
            {
                for (int i = 1; i <= 10; ++i)
                {
                    spritink(0, 0, 0, 0, getrandcolor5(colormode));
                }
            }
        }
        #endregion 

        private void control_color_shuffle()
        {
            option.BackColor = getrandcolor5(colormode);
            change.BackColor = getrandcolor5(colormode);
            stagetitle.ForeColor = getrandcolor5(colormode);
            target.ForeColor = getrandcolor5(colormode);
            clear.ForeColor = getrandcolor5(colormode);
            cleartimedisp.ForeColor = getrandcolor5(colormode);
            menu.BackColor = getrandcolor5(colormode);
            replay.BackColor = getrandcolor5(colormode);
            next.BackColor = getrandcolor5(colormode);
            for (int i = 0; i < this.selection.Length; i++)
            {
                selection[i].BackColor = getrandcolor5(colormode);
            }
        }

        #region StageOptionInit
        private void StageOptionInit()
        {
            for (int i = 0; i < this.stagecleartarget.Length; i++)
            {
                stagecleartarget[i] = 80;
            }
            stagecleartarget[1] = 90;
            //1.ballL 2.ballM 3.ballS 4.black 5.white 6.ballLLL 7.vader 8.killer
            stageoption[1] = "00000000";
            stagename[1] = "Up To You";
            stageoption[2] = "01000000";
            stagename[2] = "Just Do It";
            stageoption[3] = "03000000";
            stagename[3] = "Roll Out The Red Carpet";
            stageoption[4] = "10000000";
            stagename[4] = "Out Of The Blue";
            stageoption[5] = "14000000";
            stagename[5] = "She Is Green-Eyed Monster";
            stageoption[6] = "26000000";
            stagename[6] = "Don't Show\n"+"The White Feather";
            stageoption[7] = "00300000";
            stagename[7] = "Coll Me \"Greenhorn\"";
            stageoption[8] = "12300000";
            stagename[8] = "You Looks A Blue Murder";
            stageoption[9] = "00010000";
            stagename[9] = "You Can See\n" + "Pink Elephants";
            stageoption[10] = "20010000";
            stagename[10] = "It Was Born In\n" + "The Purple";
            stageoption[11] = "12110000";
            stagename[11] = "I Was Totally\n" + "Black Out Then";
            stageoption[12] = "00020000";
            stagename[12] = "She Named Me\n" + "Black Sheep";
            stageoption[13] = "00001000";
            stagename[13] = "They Gave False Color\n" + "To I'm A Liar";
            stageoption[14] = "02301000";
            stagename[14] = "Please Color Me Green";
            stageoption[15] = "00011000";
            stagename[15] = "Paint A Person Black";
            stageoption[16] = "32600000";
            stagename[16] = "Paint The Town Red";
            stageoption[17] = "00311000";
            stagename[17] = "Is He In The Pink?\n" + "I Don't Think So";
            stageoption[18] = "00000100";
            stagename[18] = "All My Friends Are\n" + "Yellow Belly";
            stageoption[19] = "30200100";
            stagename[19] = "Till You Are\n" + "Blue In The Face";
            stageoption[20] = "00200200";
            stagename[20] = "I'll Bleed White Your Mind";
            stageoption[21] = "00030000";                                           //↓こっからボスラッシュ↓
            stagename[21] = "Apple Of Your Eye";
            stageoption[22] = "00011000";                                           //
            stagename[22] = "Don't See Red";
            stageoption[23] = "99901000";                                           //
            stagename[23] = "Try Not To Make Waves";
            stageoption[24] = "00010300";                                           //
            stagename[24] = "I'm Standing\n" + "Back Against The Wall";
            stageoption[25] = "22200800";                                           //
            stagename[25] = "Do You Feel Good?";
            stageoption[26] = "00022000";                                           //
            stagename[26] = "The Sky Is The Limit";
            stageoption[27] = "00033100";                                           //
            stagename[27] = "No Way";
            stageoption[28] = "00000001";                                           //28カラーキラー
            stagename[28] = "Not At All";
        }
        #endregion

        #region MidMethod
        /// -----------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の指定された位置から、指定された文字数分の文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。</param>
        /// <param name="iStart">
        ///     取り出しを開始する位置。</param>
        /// <param name="iLength">
        ///     取り出す文字数。</param>
        /// <returns>
        ///     指定された位置から指定された文字数分の文字列。
        ///     文字数を超えた場合は、指定された位置からすべての文字列が返されます。</returns>
        /// -----------------------------------------------------------------------------------
        public static string Mid(string stTarget, int iStart, int iLength)
        {
            if (iStart <= stTarget.Length)
            {
                if (iStart + iLength - 1 <= stTarget.Length)
                {
                    return stTarget.Substring(iStart - 1, iLength);
                }

                return stTarget.Substring(iStart - 1);
            }

            return string.Empty;
        }
        #endregion

    }
}