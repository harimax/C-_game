using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dragon_Quest
{
    class Program
    {
        public static bool Retry=false;
        [STAThread]
        public static void Main(string[] args)
        {
            //Rettyがtrueならゲーム続行
            do
            {
                Retry=false;
                GameUI Game=new GameUI();
                Application.Run(Game);
                ContinueUI continueUI = new ContinueUI();
                Application.Run(continueUI);
            } while(Retry!=false);
        }
    }
}