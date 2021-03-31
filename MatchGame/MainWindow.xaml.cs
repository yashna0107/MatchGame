/*This is a simple matching game developed using C# and XAML. 
 Author: Yashna Islam*/
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

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if(matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            //Create a list of eight pairs of emoji
            List<string> animalEmoji = new List<string>() 
            {
                "🐵", "🐵",
                "🐱",  "🐱",
                "🦁",  "🦁",
                "🦄",   "🦄",
                "🐇",  "🐇",
                "🐉",  "🐉",
                "🐳",  "🐳",
                "🐠",  "🐠",
            };

            //Create a new random number generator.
            Random random = new Random();

            /*Find every TextBlock in the mainGrid and repeat the following statements for each of them*/
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if(textBlock.Name != "timeTextBlock" && textBlock.Name != "matchFoundBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    //pick a random number between 0 and the number of emoji left in the list and call it a "index"
                    int index = random.Next(animalEmoji.Count);
                    //use the random number called "index" to get a random emoji from the list
                    string nextEmoji = animalEmoji[index];
                    //update the TextBlock with the random emoji from the list
                    textBlock.Text = nextEmoji;
                    //Remove the random emoji from the list.`
                    animalEmoji.RemoveAt(index);
                }
                
            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;

        }
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            /* if the player clicked the first animal in pair, it makes that animal invisible and keeps
             track of its TextBlock in case it needs to make it visibl;e again.*/
            if(findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            /*the player finds a match, so it makes the second animal pair invisible too, and resets findingMatch so that the next
             animal clicked on is the first animal in a pair again.*/
            else if(textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                matchFoundBlock.Text = "Matches Found: " + matchesFound.ToString();
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            /*the player clicked on the animal that doesn't match, so it makes the first animal that was 
             clicked visible again and resets findingMatch*/
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
