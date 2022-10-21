﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DooGame
{
    internal class GoblinEnemy
    {
        public Rectangle goblin, goblinLabal;
        public Rect goblinRect;
        public TextBlock textBlockGoblinKill = new TextBlock();

        public ImageBrush goblinImg, goblinLabalImg;
        public ProgressBar goblinLife = new ProgressBar();
        public string goblinAct;
        bool goblinCanAttak;
        public double goblinCounterForChangeImgWalk = 0, goblinCounterForChangeImgAttack = 0;
        int goblinSpeed = 5; 
        public GoblinEnemy()
        {
            goblin = new Rectangle();
            goblinImg = new ImageBrush();
            goblinLabal = new Rectangle();
            goblinLabalImg = new ImageBrush();  
        }

        public void InitialGoblin(Canvas mainCanvas, Player player)
        {
            textBlockGoblinKill.Text = $"0/{player.goblinKillCound}";

            goblin.Width = 50; goblin.Height = 100; goblin.Fill = goblinImg;
            goblinLabal.Width = 40; goblinLabal.Height = 60;
            goblinLabal.Fill = goblinLabalImg;
            goblinImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/goblin(enemy)/goblin_walk_R1.png"));
            goblinLabalImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/goblin(enemy)/goblin_walk_R3.png"));
            goblinLife.Value = 100; goblinLife.Width = 50; goblinLife.Height = 10; goblinLife.Foreground = new SolidColorBrush(Colors.Purple);
            
            Canvas.SetLeft(goblin, 2000); Canvas.SetTop(goblin, 0);

            Canvas.SetTop(goblinLife, Canvas.GetTop(goblin) - goblin.Height); Canvas.SetLeft(goblinLife, Canvas.GetLeft(goblin) - (goblin.Width / 2));
          
            mainCanvas.Children.Add(goblin);
            mainCanvas.Children.Add(goblinLabal);
            mainCanvas.Children.Add(goblinLife);
            mainCanvas.Children.Add(textBlockGoblinKill);

        }
        public void SetGoblinRect()
        {
            goblinRect = new Rect(Canvas.GetLeft(goblin), Canvas.GetTop(goblin), goblin.Width, goblin.Height);
        }

        public void GoblinController(Player player, PhysicsDecor decor, DispatcherTimer GameTimer)
        {
            Canvas.SetTop(goblinLife, Canvas.GetTop(goblin) - 10); Canvas.SetLeft(goblinLife, Canvas.GetLeft(goblin) - (goblin.Width / 2));

            Canvas.SetTop(goblin, Canvas.GetTop(goblin) + goblinSpeed);
            if (goblinRect.IntersectsWith(decor.flor1Rect) || goblinRect.IntersectsWith(decor.flor2Rect))
            {
                Canvas.SetTop(goblin, Canvas.GetTop(goblin) - goblinSpeed);

            }
            if (Canvas.GetLeft(goblin) < Canvas.GetLeft(player.player) && !goblinCanAttak)
            {
                Canvas.SetLeft(goblin, Canvas.GetLeft(goblin) + goblinSpeed);
                goblinAct = "Rigth";
            }
            else if (Canvas.GetLeft(goblin) > Canvas.GetLeft(player.player) && !goblinCanAttak)
            {
                goblinAct = "Left";
                Canvas.SetLeft(goblin, Canvas.GetLeft(goblin) - goblinSpeed);
            }

            if (goblinRect.IntersectsWith(player.playerHitBox))
            {
                goblinCanAttak = true;
            }
            else 
            {
                goblinCanAttak = false;
            }

            if(!goblinCanAttak)
            {
                if (goblinCounterForChangeImgWalk > 8)
                {
                    goblinCounterForChangeImgWalk = 1;
                } 
                goblinCounterForChangeImgWalk += 0.5;
                ChangeGoblinWalkAnimation(goblinCounterForChangeImgWalk);
            }
            else if (goblinCanAttak)
            {
                if (goblinCounterForChangeImgAttack > 8)
                {
                    goblinCounterForChangeImgAttack = 1;
                } 
                goblinCounterForChangeImgAttack += 0.5;
                ChangeGoblinAttackAnimation(goblinCounterForChangeImgAttack);
            }

            if(player.playerHitBox.IntersectsWith(goblinRect) && player.playerAttack == true)
            {
                goblinLife.Value -= 10;

                if(goblinLife.Value == 0)
                {
                    Canvas.SetLeft(goblin, 1500);
                    goblinLife.Value = 100;
                    player.actualGoblinKillCound += 1;
                    textBlockGoblinKill.Text = $"{player.actualGoblinKillCound}/{player.goblinKillCound}";
                }
            }
             if (player.playerHitBox.IntersectsWith(goblinRect) && goblinCanAttak == true)
             {
                player.lifePlayer.Value -= 0.5;
                if(player.lifePlayer.Value == 0)
                {
                    GameTimer.Stop();
                }
             }
             if (Canvas.GetLeft(goblin) < -50)
             {
                Canvas.SetLeft(goblin, 1500);
             }
        }

        public void ChangeGoblinWalkAnimation(double x)
        {
            if(goblinAct == "Left")
            {
                for (int i = 1; i <= 8; i++)
                {
                    if(x == i)
                    {
                        goblinImg.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/images/goblin(enemy)/goblin_walk_L{i}.png"));
                    }
                }
            }
            else if (goblinAct == "Rigth")
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (x == i)
                    {
                        goblinImg.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/images/goblin(enemy)/goblin_walk_R{i}.png"));
                    }
                }
            }
                goblin.Fill = goblinImg;
        }

        public void ChangeGoblinAttackAnimation(double x)
        {
            if (goblinAct == "Left")
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (x == i)
                    {
                        goblinImg.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/images/goblin(enemy)/goblin_attack_L{i}.png"));
                    }
                }
            }
            else if (goblinAct == "Rigth")
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (x == i)
                    {
                        goblinImg.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/images/goblin(enemy)/goblin_attack_R{i}.png"));
                    }
                }
            }
            goblin.Fill = goblinImg;
        }
    }
}
