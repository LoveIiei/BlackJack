using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlackJack
{
    internal class Player
    {
        public int money, card_num;
        public List<int> cards = new List<int>();
        public bool stayed = false;
        public Player(int money)
        {
            this.money = money;
            card_num = 0;
        }
    }
    internal class Game
    {
        public event Action GameStarted;
        private void OnGameStarted()
        {
            GameStarted?.Invoke();
        }
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public const int CHECKS = 500;
        public void Start()
        {
            
            Player1 = new Player(CHECKS);
            Player2 = new Player(CHECKS);
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                int num = rand.Next(1, 14);
                if (i < 2)
                {
                    if (!Player1.cards.Contains(num))
                    {
                        Player1.cards.Add(num);
                        Player1.card_num += num;
                    }
                }
                else
                {
                    if (!Player2.cards.Contains(num))
                    {
                        Player2.cards.Add(num);
                        Player2.card_num += num;
                    }
                }
            }
            OnGameStarted();
        }

        public void Hit(Player player)
        {
            if (player.stayed){
                return;
            }
            Random rand = new Random();
            int num = rand.Next(1, 14);
            if (!player.cards.Contains(num) && player.cards.Count() < 6)
            {
                player.cards.Add(num);
                player.card_num += num;
            }
        }
    }
}
