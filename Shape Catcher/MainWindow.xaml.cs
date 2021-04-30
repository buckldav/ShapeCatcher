using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Shape_Catcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        int score = 0;
        int seconds = 2000;
        int interval = 400;
        bool gameover = false;

        int cX = 0;
        const int FAST = 3;
        const int SLOW = 2;
        const int CWIDTH = 49;
        const int CHEIGHT = 30;
        const int WINDOWH = 360;
        const int BOX = 10;
        

        Rectangle[] rects = new Rectangle[1000];
        int[] xPos = new int[1000];
        int[] yVel = new int[1000];
        int[] dt = new int[1000];
        int index = 0;

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            // Updating the Label which displays the current second           
            timerDisplay.Text = "Time Remaining: " + (seconds / 100);
            catcher.Focus();
            if (seconds == 0 && gameover == false)
            {
                //end of game, display score in a message.
                string highscore = "Final Score: " + score;
                MessageBox.Show(highscore, "Game Over!");
                gameover = true;
            }
            if (seconds > 0)
            {
                //move all the rectangles
                MoveRects();

                //make rectangles
                if (seconds / 5 == interval)
                {
                    MakeRect();
                    interval -= 5;
                }

                //decrease the time every millisecond
                seconds--;
            }

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        private void MoveRects()
        {
            int i = 0;

            //move all the rectangles
            while (i < index)
            {
                TranslateTransform transform = new TranslateTransform();
                transform.X = xPos[i];
                int yPos = yVel[i] * dt[i];
                transform.Y = yPos;
                rects[i].RenderTransform = transform;
                dt[i] = dt[i] + 1;
                i++;

                //check pos of shape, if "caught", increment score
                //if (yPos <= WINDOWH - CHEIGHT + 5 && yPos >= WINDOWH - CHEIGHT && cX < xPos[i] && cX + CWIDTH - BOX > xPos[i])
                if (yPos == WINDOWH - CHEIGHT && cX < xPos[i] && cX + CWIDTH - BOX > xPos[i])
                {
                    if (yVel[i] == FAST)
                    {
                        score += 25;
                    }
                    if (yVel[i] == SLOW)
                    {
                        score += 10;    
                    }
                    scoreDisplay.Text = "Score: " + score;
                    rects[i].Stroke = System.Windows.Media.Brushes.White;
                    rects[i].Fill = System.Windows.Media.Brushes.White;
                }
            }
        }

        private void catcher_KeyDown(object sender, KeyEventArgs e)
        {

            TranslateTransform transform = new TranslateTransform();
            
            if (e.Key == Key.Left)
            {
                cX -= 4;
                transform.X = cX;
                catcher.RenderTransform = transform;                
            }
            if (e.Key == Key.Right)
            {
                cX += 4;
                transform.X = cX;
                catcher.RenderTransform = transform;                
            }
        }

        private void MakeRect()
        {
            // Add a Rectangle Element
            Rectangle myRect = new System.Windows.Shapes.Rectangle();

            // Coloring
            Random random = new Random();
            int colorRand = random.Next(1, 5);
            if (colorRand == 1)
            {
                myRect.Stroke = System.Windows.Media.Brushes.Blue;
                myRect.Fill = System.Windows.Media.Brushes.Blue;
            }
            if (colorRand == 2)
            {
                myRect.Stroke = System.Windows.Media.Brushes.Red;
                myRect.Fill = System.Windows.Media.Brushes.Red;
            }
            if (colorRand == 3)
            {
                myRect.Stroke = System.Windows.Media.Brushes.Yellow;
                myRect.Fill = System.Windows.Media.Brushes.Yellow;
            }
            if (colorRand == 4)
            {
                myRect.Stroke = System.Windows.Media.Brushes.Green;
                myRect.Fill = System.Windows.Media.Brushes.Green;
            }


            // Position
            int gridWidth = 525;
            int xRangeMin = 10;
            int xRangeMax = gridWidth - 10;
            int xTrans = random.Next(xRangeMin, xRangeMax);
            myRect.HorizontalAlignment = HorizontalAlignment.Left;
            myRect.VerticalAlignment = VerticalAlignment.Top;
            TranslateTransform transform = new TranslateTransform();
            transform.X = xTrans;
            myRect.RenderTransform = transform;

            // Size
            myRect.Height = 10;
            myRect.Width = 10;
            myGrid.Children.Add(myRect);

            // Add to array of rectangles
            rects[index] = myRect;
            xPos[index] = xTrans;

            // Speed
            //int speed = random.Next(SLOW, FAST + 1);
            int speed = SLOW;
            yVel[index] = speed;
            dt[index] = 1;
            index++;
        }


        public MainWindow()
        {
            InitializeComponent();
            //initialize scoreDisplay as zero
            scoreDisplay.Text = "Score: " + score;

            //DispatcherTimer setup
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();

            //move catcher
            catcher.KeyDown += new KeyEventHandler(catcher_KeyDown);
            catcher.Focusable = true;
            catcher.IsEnabled = true;
            catcher.Focus();
            Canvas.SetZIndex(catcher, 10000);
        }

    }


}