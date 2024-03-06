// GUIプログラムに必要な部分
using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows;
using System.Media;

namespace Dragon_Quest
{
    class GameUI : Form
    {
        //フィールド
        TextDisplay textdisplay;
        TextDisplay ParaDisplay;
        Chara_Para Player;
        Chara_Para Enemy;
        MyButton Attckbutton;
        MyButton Magicbutton;
        MyButton Defensebutton;
        MyButton Curebutton;
        SoundPlayer MissSound;
        SoundPlayer DamegeSound;
        SoundPlayer MagicSound;
        SoundPlayer WinSound;
        SoundPlayer GAMEOVERSound;
        int monsterNum;
        int PlayerHitrate;
        int EnemyHitRate;
        int CurePoint;
        int CureMP;
        int EnemyDamege;
        int PlayerDamege; 
        int MaxHP;
        int MaxMP;
        //モンスター
        string [] monsoters={"png/ryuou.png","png/show.png","png/bowenny.png","png/dragonMachine.png","png/DiamondSlime.png","png/Drakey.png"};
        string [] monsName={"りゅうおう","スライム","ブラウニー","ドラゴンマシン","ダイヤモンドスライム","ドラキー"};
        bool DefenseState;
        System.Random sr =new System.Random();
        // MediaPlayer mediaPlayer = new MediaPlayer();
        // string[] musicPath={};
        
        //コンストラクタ
        public GameUI()
        {
            //画面
            int formWidth = 1000;
            int formHeight = 900;
            string formTitle = "ドラクエ";
            this.Width = formWidth;
            this.Height = formHeight;
            this.Text = formTitle;
            //音楽のインスタンス
            MissSound= new SoundPlayer("music/maou_se_8bit09.wav");
            DamegeSound= new SoundPlayer("music/maou_se_battle16.wav");
            MagicSound= new SoundPlayer("music/maou_se_magical09.wav");
            WinSound=new SoundPlayer("music/maou_se_chime14.wav");
            GAMEOVERSound=new SoundPlayer("music/maou_se_onepoint31.wav");
            
            //パラメータ
            string HP="HP:";
            string MP="MP:";
            Enemy  =new Chara_Para(140,0);
            Player =new Chara_Para(120,100);
            MaxHP=Player.Chara_HP;
            MaxMP=Player.Chara_MP;
            PlayerHitrate=25;
            EnemyHitRate=25;//後で変える
            string [] Para={HP+MaxHP.ToString(),MP+MaxMP.ToString()};

            monsterNum=sr.Next(0,monsoters.Length-1);
 
            //画像の配置----------------------------------------------------------------
            string imagePath = monsoters[monsterNum];
            // 画像が存在する場合
            if (System.IO.File.Exists(imagePath))
            {
                //pictureboxの設定
                PictureBox pictureBox = new PictureBox();
                Console.WriteLine(monsterNum);
                pictureBox.Image = Image.FromFile(imagePath);
                // PictureBoxのサイズ調整方法を指定
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                // 画像のサイズと位置を設定
                pictureBox.Size = new Size(400, 400); // 幅200、高さ100
                pictureBox.Location = new Point(300, 90);

                // PictureBoxをフォームに追加
                this.Controls.Add(pictureBox);
            }
            else
            {
                // 画像が存在しない場合の処理
                MessageBox.Show("画像ファイルが見つかりませんでした。");
            }
            //-----------------------------------------------------------------------------

            //テキストディスプレイの設定
            textdisplay =new TextDisplay(monsName[monsterNum]+"があらわれた\r\nどうする？", new Font("Arial", 20), 500, 300, new Point(250, 500));
            this.Controls.Add(textdisplay);

            //パラメータディスプレイの設定
            ParaDisplay = new TextDisplay("プレイヤー\r\n"+Para[0]+"\r\n"+Para[1]+"\r\n", 
            new Font("Arial", 23), 200, 200, new Point(10, 120));
            this.Controls.Add(ParaDisplay);

        //ボタンの配置--------------------------------------------------------------
            int ButtonWidth = 140;
            int ButtonHeight = 60;
            Font buttonFont = new Font("Arial", 20);
            Attckbutton = new MyButton("こうげき", buttonFont, ButtonWidth, ButtonHeight, new Point(800, 120));
            Magicbutton = new MyButton("じゅもん", buttonFont, ButtonWidth, ButtonHeight, new Point(800, 190));
            Defensebutton = new MyButton("ぼうぎょ", buttonFont, ButtonWidth, ButtonHeight, new Point(800, 260));
            Curebutton = new MyButton("かいふく", buttonFont, ButtonWidth, ButtonHeight, new Point(800, 330));

            Attckbutton.Click+=Attck_Click;
            Magicbutton.Click+=Magic_Click;
            Defensebutton.Click+=Defence_Click;
            Curebutton.Click+=Cure_Click;

            this.Controls.Add(Attckbutton);
            this.Controls.Add(Magicbutton);
            this.Controls.Add(Defensebutton);
            this.Controls.Add(Curebutton);
        //------------------------------------------------------------------------- 
        }

        //イベントハンドラ--------------------------------------------------------------
        //攻撃メソッド
        void Attck_Click(object sender, EventArgs e)
        {
            Attckbutton.Enabled=false;
            Magicbutton.Enabled=false;
            Defensebutton.Enabled=false;
            Curebutton.Enabled=false;
            //攻撃が外れる
            if(MissCheck(PlayerHitrate)==false)
            {
                MissSound.Play();
                textdisplay.Text="プレイヤーのこうげき!!!!!\r\nしかしこうげきがはずれた";
                textdisplay.Text+="\r\n";
            }
            //攻撃が当たる
            else
            {
                DamegeSound.Play();
                EnemyDamege=sr.Next(30,51);
                Enemy.Chara_HP=Enemy.Chara_HP-EnemyDamege;
                textdisplay.Text="プレイヤーのこうげき!!!!!\r\n"+monsName[monsterNum]+"に"+EnemyDamege+"のダメージ!!!!!";
                textdisplay.Text+="\r\n"; 
            }
            
            Task.Run(async () =>
            {
                await Task.Delay(2000); // 1000ミリ秒（1秒）の遅延
                Console.WriteLine("プレイヤーの攻撃終了");
                if(Enemy.Chara_HP<=0)
                {
                    End_game(Player.Chara_HP,Enemy.Chara_HP);
                }
                else
                {
                    Enemy_Attack();
                }
            });
        }
        //敵の攻撃
        void Enemy_Attack()
        {
            PlayerDamege=EnemyDamege=sr.Next(20,56);; //仮
            //防御状態だとダメージ半減
            if(DefenseState==true)
            {
                DamegeSound.Play();
                PlayerDamege=PlayerDamege/2;
                Player.Chara_HP=Player.Chara_HP-PlayerDamege;
                textdisplay.Text=monsName[monsterNum]+"のこうげき!!!!!\r\nプレイヤーは"+PlayerDamege+"のダメージをうけた!!!!!!"; 
            }
            else if(MissCheck(EnemyHitRate)==false)
            {
                MissSound.Play();
                textdisplay.Text=monsName[monsterNum]+"のこうげき!!!!!\r\nしかしこうげきがはずれた";
            }
            else
            {
                DamegeSound.Play();
                Player.Chara_HP=Player.Chara_HP-PlayerDamege;
                textdisplay.Text=monsName[monsterNum]+"のこうげき!!!!!\r\nプレイヤーは"+PlayerDamege+"のダメージをうけた!!!!!!"; 
            }
            
            ParaDisplay.Text="プレイヤー\r\n"+"HP:"+Player.Chara_HP+"\r\n"+"MP:"+Player.Chara_MP+"\r\n";
            Console.WriteLine("プレイヤー:"+Player.Chara_HP.ToString()+"\r\n"+"敵:"+Enemy.Chara_HP.ToString());
            if(Player.Chara_HP<=0)
            {
                End_game(Player.Chara_HP,Enemy.Chara_HP);
            }
            else
            {
                Attckbutton.Enabled=true;
                Defensebutton.Enabled=true;
                if(Player.Chara_MP>10)
                {
                    Magicbutton.Enabled=true;
                }
                if(Player.Chara_MP>15)
                {
                    Curebutton.Enabled=true;
                }
                DefenseState=false;
            }
        }
        //防御メソッド
        void Defence_Click(object sender, EventArgs e)
        {
            Attckbutton.Enabled=false;
            Magicbutton.Enabled=false;
            Defensebutton.Enabled=false;
            Curebutton.Enabled=false;
            textdisplay.Text="プレイヤーはぼうぎょをかためた";
            DefenseState=true;
            CureMP=sr.Next(10,51);
            if(Player.Chara_HP+CureMP>MaxMP) Player.Chara_MP=100;
            else Player.Chara_MP+=CureMP;

            Task.Run(async () =>
            {
                await Task.Delay(2000); // 1000ミリ秒（1秒）の遅延
                Console.WriteLine("プレイヤーの攻撃終了");
                if(Enemy.Chara_HP<=0 || Player.Chara_HP<=0)
                {
                    End_game(Player.Chara_HP,Enemy.Chara_HP);
                }
                else
                {
                    Enemy_Attack();
                }
            });
        }
        //魔法メソッド(攻撃よりもひくいけど必ず命中)
        void Magic_Click(object sender, EventArgs e)
        {
            MagicSound.Play();
            Attckbutton.Enabled=false;
            Magicbutton.Enabled=false;
            Defensebutton.Enabled=false;
            Curebutton.Enabled=false;
            EnemyDamege=sr.Next(20,51);
            Enemy.Chara_HP=Enemy.Chara_HP-EnemyDamege;
            Player.Chara_MP-=10;
            textdisplay.Text="プレイヤーのまほうこうげき!!!!!\r\n"+monsName[monsterNum]+"に"+EnemyDamege+"のダメージ!!!!!";
            ParaDisplay.Text="プレイヤー\r\n"+"HP:"+Player.Chara_HP+"\r\n"+"MP:"+Player.Chara_MP+"\r\n";
            Task.Run(async () =>
            {
                await Task.Delay(2000); // 1000ミリ秒（1秒）の遅延
                Console.WriteLine("プレイヤーの攻撃終了");
                if(Enemy.Chara_HP<=0 || Player.Chara_HP<=0)
                {
                    End_game(Player.Chara_HP,Enemy.Chara_HP);
                }
                else
                {
                    Enemy_Attack();
                }
            });
        }
        //回復メソッド
        void Cure_Click(object sender, EventArgs e)
        {
            MagicSound.Play();
            Attckbutton.Enabled=false;
            Magicbutton.Enabled=false;
            Defensebutton.Enabled=false;
            Curebutton.Enabled=false;
            CurePoint=sr.Next(20,81);
            Player.Chara_MP-=30;
            if(Player.Chara_HP+CurePoint>MaxHP) Player.Chara_HP=100;
            else Player.Chara_HP+=CurePoint;
            textdisplay.Text="プレイヤーは回復した";
            ParaDisplay.Text="プレイヤー\r\n"+"HP:"+Player.Chara_HP+"\r\n"+"MP:"+Player.Chara_MP+"\r\n";

            Task.Run(async () =>
            {
                await Task.Delay(2000); // 1000ミリ秒（1秒）の遅延
                Console.WriteLine("プレイヤーの攻撃終了");
                if(Enemy.Chara_HP<=0 || Player.Chara_HP<=0)
                {
                    End_game(Player.Chara_HP,Enemy.Chara_HP);
                }
                else
                {
                    Enemy_Attack();
                }
            });
            
        }
        //ゲーム終了メソッド
        void End_game(int Player_HP, int Enemy_HP)
        {
            if(Player_HP>Enemy_HP) 
            {
                WinSound.Play();
                textdisplay.Text="てきをたおした!!!!!!!!!!\r\nあと3秒でゲームが閉じられます";
                Task.Run(async () =>
                {
                    await Task.Delay(3000); // 1000ミリ秒（1秒）の遅延
                    this.Close();   //画面を閉じる
                });
            }
            else 
            {    
                GAMEOVERSound.Play();
                textdisplay.Text="ぜんめつした!!!!!!!!\r\nあと3秒でゲームが閉じられます";
                Task.Run(async () =>
                {
                    await Task.Delay(3000); // 1000ミリ秒（1秒）の遅延
                    this.Close();   //画面を閉じる
                });
            }
        }
        //命中判定
        bool MissCheck(int Hitrate)
        {
            System.Random sr2 =new System.Random();
            int probability=sr2.Next(101);
            Console.WriteLine(probability);
            if(probability>=Hitrate)
            return true;
            else
            return false;
        }
    }
    //ボタンクラス-----------------------------------------------------------------------
    class MyButton : Button
    {
        /* 1ボタンの設置初期化 */
        public MyButton(string name, Font font, int width, int height, Point point)
        {
            Text = name;
            Font = font;
            Width = width;
            Height = height;
            Location = point;
        }
    }
    //------------------------------------------------------------------------------------

    //ウインドウクラス-----------------------------------------------------------------------
    class TextDisplay : TextBox
    {
        /* 1ボタンの設置初期化 */
        public TextDisplay(string name, Font font, int width, int height, Point point)
        {
            Text = name;     //文字列
            Font = font;
            Width = width;   //横サイズ
            Height = height; //縦サイズ
            Location = point;//配置場所
            Multiline = true;
            BackColor = Color.Black;
            TextAlign = HorizontalAlignment.Left;
            ReadOnly = true;
            ForeColor = Color.White;
        }
    }

    //------------------------------------------------------------------------------------
    
    //キャラクタークラス-------------------------------------------------------------------
    class Chara_Para
    {
        public int Chara_HP;
        public int Chara_MP;
        public Chara_Para(int HP,int MP)
        {
            Chara_HP=HP;
            Chara_MP=MP;
        }
    }
    //------------------------------------------------------------------------------------

    //コンティニューウインドウ-----------------------------------------------------------
    class ContinueUI : Form
    {
        //フィールド
        MyButton Continuebutton;
        //コンストラクタ
        public ContinueUI()
        {
            //画面
            int formWidth = 500;
            int formHeight = 450;
            string formTitle = "ウインドウ";
            this.Width = formWidth;
            this.Height = formHeight;
            this.Text = formTitle;
            //ボタンの配置--------------------------------------------------------------
            int ButtonWidth = 250;
            int ButtonHeight = 80;
            Font buttonFont = new Font("Arial", 20);
            Continuebutton = new MyButton("もういっかいやる？", buttonFont, ButtonWidth, ButtonHeight, new Point(100, 140));

            Continuebutton.Click+=Continue_Click;
            this.Controls.Add(Continuebutton);
        }
        void Continue_Click(object sender, EventArgs e)
        {
            Program.Retry=true;
            this.Close();
        }
    }
    //------------------------------------------------------------------------------------
}