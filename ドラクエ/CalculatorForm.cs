// GUIプログラムに必要な部分
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Calculator
{
    class Calculator : Form
    {
        //フィールド
        TextBox display;
        //ボタンクラスのインスタンスを生成
        public int ButtonWidth = 80;
        public int ButtonHeight = 80;
        public Font buttonFont = new Font("Arial", 30);

        bool arithmeticFlg; //演算子をクリック前(false),クリック後(true)
        int bufferNumber;   //足される数を格納する。
        string bufferOpertator; //四則演算の演算子を格納する。

        //コンストラクタ
        public Calculator()
        {
            int formWidth = 400;
            int formHeight = 600;
            string formTitle = "電卓アプリ";

            //サイズ
            this.Width = formWidth;
            this.Height = formHeight;

            this.Text = formTitle;

            //ディスプレイの設定
            display = new TextBox();
            display.Width = 350;
            display.Height = 100;
            display.Location = new Point(20, 0);
            display.Text = "0";
            display.Font = new Font("Arial", 55);
            display.TextAlign = HorizontalAlignment.Right;
            display.BackColor = Color.White;
            display.ReadOnly = true;
            this.Controls.Add(display);

            //ボタンの配置
            MyButton button1 = new MyButton("1", buttonFont, ButtonWidth, ButtonHeight, new Point(20, 120));
            MyButton button2 = new MyButton("2", buttonFont, ButtonWidth, ButtonHeight, new Point(110, 120));
            MyButton button3 = new MyButton("3", buttonFont, ButtonWidth, ButtonHeight, new Point(200, 120));
            MyButton button4 = new MyButton("4", buttonFont, ButtonWidth, ButtonHeight, new Point(20, 210));
            MyButton button5 = new MyButton("5", buttonFont, ButtonWidth, ButtonHeight, new Point(110, 210));
            MyButton button6 = new MyButton("6", buttonFont, ButtonWidth, ButtonHeight, new Point(200, 210));
            MyButton button7 = new MyButton("7", buttonFont, ButtonWidth, ButtonHeight, new Point(20, 300));
            MyButton button8 = new MyButton("8", buttonFont, ButtonWidth, ButtonHeight, new Point(110, 300));
            MyButton button9 = new MyButton("9", buttonFont, ButtonWidth, ButtonHeight, new Point(200, 300));
            MyButton button0 = new MyButton("0", buttonFont, ButtonWidth, ButtonHeight, new Point(20, 390));
            //四則演算ボタン配置
            MyButton buttonPlus = new MyButton("+", buttonFont, ButtonWidth, ButtonHeight, new Point(290, 120));
            MyButton buttonMinus = new MyButton("-", buttonFont, ButtonWidth, ButtonHeight, new Point(290, 210));
            MyButton buttonMulti = new MyButton("×", buttonFont, ButtonWidth, ButtonHeight, new Point(290, 300));
            MyButton buttonDivision = new MyButton("÷", buttonFont, ButtonWidth, ButtonHeight, new Point(290, 390));
            MyButton buttonTotal = new MyButton("合計", buttonFont, 170, ButtonHeight, new Point(110, 390));

            //イベントハンドラを追加
            button1.Click += Number_Click;
            button2.Click += Number_Click;
            button3.Click += Number_Click;
            button4.Click += Number_Click;
            button5.Click += Number_Click;
            button6.Click += Number_Click;
            button7.Click += Number_Click;
            button8.Click += Number_Click;
            button9.Click += Number_Click;
            button0.Click += Number_Click;

            buttonPlus.Click += Arithmetic_click;
            buttonMinus.Click += Arithmetic_click;
            buttonMulti.Click += Arithmetic_click;
            buttonDivision.Click += Arithmetic_click;

            buttonTotal.Click += Total_click;

            arithmeticFlg = false;
            //ボタンをフォームに追加
            this.Controls.Add(button1);
            this.Controls.Add(button2);
            this.Controls.Add(button3);
            this.Controls.Add(button4);
            this.Controls.Add(button5);
            this.Controls.Add(button6);
            this.Controls.Add(button7);
            this.Controls.Add(button8);
            this.Controls.Add(button9);
            this.Controls.Add(button0);

            this.Controls.Add(buttonPlus);
            this.Controls.Add(buttonMinus);
            this.Controls.Add(buttonMulti);
            this.Controls.Add(buttonDivision);
            this.Controls.Add(buttonTotal);

        }

        //イベントハンドラ
        //数値ボタンを押されるとディスプレイに文字が表示される
        void Number_Click(object sender, EventArgs e)
        {
            //ディスプレイに8桁以上が表示されていたら何もせずに処理終了
            if (display.Text.Length >= 8)
            {
                return;
            }
            //クリックした数字を取得
            string clickNumber = ((Button)sender).Text;

            //表示されているディスプレイの数字が0かチェック
            if (display.Text == "0")
            {
                display.Text = clickNumber;
            }
            else
            {
                //右側に付与
                display.Text = display.Text + clickNumber;
            }
        }

        //四則演算ハンドラ
        void Arithmetic_click(object sender, EventArgs e)
        {
            if (arithmeticFlg == true)
            {
                return;
            }
            //クリックした演算子をバッファに格納
            bufferOpertator = ((Button)sender).Text;
            //ディスプレイの数値をバッファに格納
            bufferNumber = int.Parse(display.Text);
            display.Text = "0";
            arithmeticFlg = true;
        }
        //合計ボタン
        void Total_click(object sender, EventArgs e)
        {
            if (arithmeticFlg == false)
            {
                return;
            }
            display.Text = (bufferNumber + int.Parse(display.Text)).ToString();

            MessageBox.Show("合計が表示されました");

            display.Text = "0";

            arithmeticFlg = false;
        }
    }
}