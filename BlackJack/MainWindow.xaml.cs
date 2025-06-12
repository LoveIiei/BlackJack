using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlackJack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer backgroundMusic = new MediaPlayer();
        private Game game; // Declare the 'game' field to fix CS0103
        public List<Image> p1Images;
        public List<Image> p2Images;
        public int fund = 100;

        public MainWindow()
        {
            InitializeComponent();
            PlayBackgroundMusic();
            game = new Game(); // Initialize the 'game' field
            game.GameStarted += UpdateUI;
            game.Start(); // now you have data to show
        }

        private void PlayBackgroundMusic()
        {
            backgroundMusic.Open(new Uri("Resources/background.mp3", UriKind.Relative));
            backgroundMusic.MediaEnded += (s, e) => {
                backgroundMusic.Position = TimeSpan.Zero; // Loop
                backgroundMusic.Play();
            };
            backgroundMusic.Play();
        }


        private void UpdateUI()
        {
            p1Fund.Text = $"P1 Credits: {game.Player1.money}";
            p2Fund.Text = $"P2 Credits: {game.Player2.money}";
            p1Value.Text = $"Value: {game.Player1.card_num}";
            p2Value.Text = $"Value: {game.Player2.card_num}";
            p1Images = new List<Image> { p1_1, p1_2, p1_3, p1_4, p1_5, p1_6 };
            p2Images = new List<Image> { p2_1, p2_2, p2_3, p2_4, p2_5, p2_6 };
            int i = 0, j = 0;
            while (i < game.Player1.cards.Count())
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri($"Resources/player1/{game.Player1.cards[i]}.png", UriKind.Relative);
                bmp.DecodePixelWidth = 100;
                bmp.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                p1Images[i].Source = bmp;
                i++;
            }
            while (j < game.Player2.cards.Count())
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri($"Resources/player2/{game.Player2.cards[j]}.png", UriKind.Relative);
                bmp.DecodePixelWidth = 100;
                bmp.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                p2Images[j].Source = bmp;
                j++;
            }
        }

        private void ResetUI()
        {
            // Enable buttons
            p1Hit.IsEnabled = true;
            p1Stay.IsEnabled = true;
            p1Double.IsEnabled = true;
            p2Hit.IsEnabled = true;
            p2Stay.IsEnabled = true;
            p2Double.IsEnabled = true;

            // Clear card images
            foreach (var img in p1Images) img.Source = null;
            foreach (var img in p2Images) img.Source = null;
        }

        public void FinishGame()
        {
            int win = 0, lose = 0;
            if (game.Player1.card_num > 21 && game.Player2.card_num <= 21)
            {
                MessageBox.Show("Player2 won!!");
                win = 2;
                lose = 1;
            }
            else if (game.Player2.card_num > 21 && game.Player1.card_num <= 21)
            {
                MessageBox.Show("Player1 won!!");
                win = 1;
                lose = 2;
            }
            else if (game.Player1.card_num > game.Player2.card_num)
            {
                MessageBox.Show("Player1 won!!");
                win = 1;
                lose = 2;
            }
            else if (game.Player2.card_num > game.Player1.card_num)
            {
                MessageBox.Show("Player2 won!!");
                win =  2;
                lose = 1;
            } else
            {
                MessageBox.Show("DRAW!");
            }
            if (win == 1)
            {
                game.Player1.money += fund;
                game.Player2.money -= fund;
            }
            else if (win == 2)
            {
                game.Player2.money += fund;
                game.Player1.money -= fund;
            }
            int p1Earn = 0, p2Earn = 0;
            fund = 100;
            p1Earn = game.Player1.money;
            p2Earn = game.Player2.money;
            game = new Game(); // <-- NEW Game object
            game.Start();      // Re-deal cards
            game.Player1.money = p1Earn;
            game.Player2.money = p2Earn;
            // Reset UI
            ResetUI();
            UpdateUI();
        }

        private void p2Stay_Click(object sender, RoutedEventArgs e)
        {
            game.Player2.stayed = true;
            MessageBox.Show("Player 2 stayed!");
            p2Hit.IsEnabled = false;
            p2Double.IsEnabled = false;
            p2Stay.IsEnabled = false;
            if (game.Player1.stayed)
            {
                FinishGame();
            }
        }

        private void p2Hit_Click(object sender, RoutedEventArgs e)
        {
            game.Hit(game.Player2);
            UpdateUI();
        }

        private void p2Double_Click(object sender, RoutedEventArgs e)
        {
            game.Hit(game.Player2);
            UpdateUI();
            game.Player2.stayed = true;
            MessageBox.Show("Player 2 stayed!");
            fund *= 2;
            pool.Text = $"{fund}";
            p2Hit.IsEnabled = false;
            p2Double.IsEnabled = false;
            p2Stay.IsEnabled = false;
            if (game.Player1.stayed)
            {
                FinishGame();
            }
        }

        private void p1Double_Click(object sender, RoutedEventArgs e)
        {
            game.Hit(game.Player1);
            UpdateUI();
            game.Player1.stayed = true;
            MessageBox.Show("Player 1 stayed!");
            fund *= 2;
            pool.Text = $"{fund}";
            p1Hit.IsEnabled = false;
            p1Double.IsEnabled = false;
            p1Stay.IsEnabled = false;
            if (game.Player2.stayed)
            {
                FinishGame();
            }
        }

        private void p1Stay_Click(object sender, RoutedEventArgs e)
        {
            game.Player1.stayed = true;
            MessageBox.Show("Player 1 stayed!");
            p1Hit.IsEnabled = false;
            p1Double.IsEnabled = false;
            p1Stay.IsEnabled = false;
            if (game.Player2.stayed)
            {
                FinishGame();
            }
        }

        private void p1Hit_Click(object sender, RoutedEventArgs e)
        {
            game.Hit(game.Player1);
            UpdateUI();
        }
    }
}