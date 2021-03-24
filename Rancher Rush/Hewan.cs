using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rancher_Rush
{
    public class Hewan
    {
        public PictureBox Animal { get; set; }
        public PictureBox Ki = new PictureBox();
        public PictureBox Ka = new PictureBox();
        public PictureBox JumpKi = new PictureBox();
        public PictureBox JumpKa = new PictureBox();
        public PictureBox TransKi = new PictureBox();
        public PictureBox TransKa = new PictureBox();
        public PictureBox TransBack = new PictureBox();

        public int Mark {set;get;}

        public void Decide(int Speed)
        {
            #region Domba Putih
            if (Speed == 3)
            {
                // domba putih
                Ki.Image = Properties.Resources.D2Ki;
                Ka.Image = Properties.Resources.D2Ka;
                JumpKa.Image = Properties.Resources.D2JumpKa;
                JumpKi.Image = Properties.Resources.D2JumpKi;
                TransKi.Image = Properties.Resources.D2TransKi;
                TransKa.Image = Properties.Resources.D2TransKa;
                TransBack.Image = Properties.Resources.D2Back;
            }
            #endregion

            #region Domba Hitam
            else if (Speed == 4)
            {
                // domba hitam
                Ki.Image = Properties.Resources.D1Ki;
                Ka.Image = Properties.Resources.D1Ka;
                JumpKa.Image = Properties.Resources.D1JumpKa;
                JumpKi.Image = Properties.Resources.D1JumpKi;
                TransKi.Image = Properties.Resources.D1TransKi;
                TransKa.Image = Properties.Resources.D1TransKa;
                TransBack.Image = Properties.Resources.D1Back;
            }
            #endregion

            #region Kambing
            else if (Speed == 5)
            {
                // kambing
                Ki.Image = Properties.Resources.D3Ki;
                Ka.Image = Properties.Resources.D3Ka;
                JumpKa.Image = Properties.Resources.D3JumpKa;
                TransKi.Image = Properties.Resources.D3TransKi;
                TransKa.Image = Properties.Resources.D3TransKa;
                JumpKi.Image = Properties.Resources.D3JumpKi;
                TransBack.Image = Properties.Resources.D3Back;
            }
            #endregion

            #region Kuda
            else if (Speed == 7)
            {
                // kuda
                Ki.Image = Properties.Resources.D4Ki;
                Ka.Image = Properties.Resources.D4Ka;
                JumpKa.Image = Properties.Resources.D4JumpKa;
                JumpKi.Image = Properties.Resources.D4JumpKi;
                TransKi.Image = Properties.Resources.D4TransKi;
                TransKa.Image = Properties.Resources.D4TransKa;
                TransBack.Image = Properties.Resources.D4Back;
            }
            #endregion

            #region Ayam Raksasa
            else if (Speed == 6)
            {
                // chocobo(kalo yang main FF) ato supupunya ayam lah wkwk
                Ki.Image = Properties.Resources.D5Ki;
                Ka.Image = Properties.Resources.D5Ka;
                JumpKa.Image = Properties.Resources.D5JumpKa;
                JumpKi.Image = Properties.Resources.D5JumpKi;
                TransKi.Image = Properties.Resources.D5TransKi;
                TransKa.Image = Properties.Resources.D5TransKa;
                TransBack.Image = Properties.Resources.D5Back;
            }
            #endregion
        }

        public void CheckMark(int Changes)
        {
            if (Mark != Changes)
            {
                Mark = Changes;
                if (Mark == 1)
                    Animal.Image = Ki.Image;

                else if (Mark == 2)
                    Animal.Image = Ka.Image;

                else if (Mark == 3)
                    Animal.Image = JumpKi.Image;

                else if (Mark == 4)
                    Animal.Image = JumpKa.Image;

                else if (Mark == 5)
                    Animal.Image = TransKi.Image;

                else if (Mark == 6)
                    Animal.Image = TransKa.Image;

                else if (Mark == 7)
                    Animal.Image = TransBack.Image;
            }
        }

        //public void CheckMark(int Changes)
        //{
        //    if (Mark != Changes)
        //    {
        //        Mark = Changes;
        //        switch (Mark)
        //        {
        //            else if (Mark == 1:
        //                Animal.Image = Ki.Image;
        //                break;
        //            else if (Mark == 2:
        //                Animal.Image = Ka.Image;
        //                break;
        //            case 3:
        //                Animal.Image = JumpKi.Image;
        //                break;
        //            case 4:
        //                Animal.Image = JumpKa.Image;
        //                break;
        //            case 5:
        //                Animal.Image = TransKi.Image;
        //                break;
        //            case 6:
        //                Animal.Image = TransKa.Image;
        //                break;
        //            case 7:
        //                Animal.Image = TransBack.Image;
        //                break;
        //            default: Animal.Image = Ki.Image;
        //                break;
        //        }
        //    }
        //}
    }
}
