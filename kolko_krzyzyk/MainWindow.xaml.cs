using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace kolko_krzyzyk
{
    public partial class MainWindow : Window
    {
        #region Private Members

        //przechowuje obecny stan gry w komórkach
        private MarkType[] mResults;

        //prawda jeśli gracz wykonuje ruch ruch
        private bool mPlayer1Turn;

        //prawda jeśli gra skończona
        private bool mGameEnded;

        #endregion

        #region Constructor

        //konstruktor
        public MainWindow()
        {
            InitializeComponent();

            NewGame();
        }

        #endregion

        private void NewGame()
        {
            //tworzenie pustej planszy
            mResults = new MarkType[9];

            for (var i = 0; i < mResults.Length; i++)
                mResults[i] = MarkType.Free;

            //zmiana na true, ponieważ gracz 1 wykonuje ruch
            mPlayer1Turn = true;

            // tworzenie interaktywnych przycisków
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                // Change background, foreground and content to default values
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;
            });

            mGameEnded = false;
        }

        /// uchwyty klikanych przycisków
        /// <param name="sender">przycisk został kliknięty</param>
        /// <param name="e">kliknięcie przycisku</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // jeżeli gra się skończyła, to zacznij nową kliknięciem
            if (mGameEnded)
            {
                NewGame();
                return;
            }

            // przekazanie przycisku
            var button = (Button)sender;

            // lokalizacja przycisku w tablicy
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            var index = column + (row * 3);

            // zablokowanie klikniętych przycisków
            if (mResults[index] != MarkType.Free)
                return;

            //ustaw wartość komórki na podstawie tego, który gracz ją obrócił
            mResults[index] = mPlayer1Turn ? MarkType.Cross : MarkType.Nought;

            //wpisanie tekstu w przycisk
            button.Content = mPlayer1Turn ? "X" : "O";

            //zmiana koloru kółka
            if (!mPlayer1Turn)
                button.Foreground = Brushes.Red;

            //przełączanie tur graczy
            mPlayer1Turn ^= true;

            //sprawdzanie zwycięzcy
            CheckForWinner();
        }

        //sprawdzanie zwyciezcy
        private void CheckForWinner()
        {
            #region Horizontal Wins

            // sprawdzanie poziomych lini
            // row 0
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[1] & mResults[2]) == mResults[0])
            {
                mGameEnded = true;
                //podkreślenie zwycięzkich komórek
                Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
            }
            // row 1
            if (mResults[3] != MarkType.Free && (mResults[3] & mResults[4] & mResults[5]) == mResults[3])
            {
                mGameEnded = true;
                Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
            }
            //  row 2
            if (mResults[6] != MarkType.Free && (mResults[6] & mResults[7] & mResults[8]) == mResults[6])
            {
                mGameEnded = true;
                Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Green;
            }

            #endregion

            #region Vertical Wins

            // sprawdzanie pionowych lini
            //  column 0
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[3] & mResults[6]) == mResults[0])
            {
                mGameEnded = true;
                Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;
            }
            //  column 1
            if (mResults[1] != MarkType.Free && (mResults[1] & mResults[4] & mResults[7]) == mResults[1])
            {
                mGameEnded = true;
                Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Green;
            }
            //  column 2
            if (mResults[2] != MarkType.Free && (mResults[2] & mResults[5] & mResults[8]) == mResults[2])
            {
                mGameEnded = true;
                Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Green;
            }

            #endregion

            #region Diagonal Wins

            // sprawdzanie przekątnych lini
            // lewa góra - prawy dół
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[4] & mResults[8]) == mResults[0])
            {
                mGameEnded = true;
                Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;
            }
            //  prawa góra - lewy dół
            if (mResults[2] != MarkType.Free && (mResults[2] & mResults[4] & mResults[6]) == mResults[2])
            {
                mGameEnded = true;
                Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.Green;
            }

            // jeżeli wszystkie komórki są pełne i nie ma zwycięzcy
            if (!mResults.Any(f => f == MarkType.Free))
            {
                mGameEnded = true;
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    button.Background = Brushes.Orange;
                });
            }

            #endregion
        }
    }
}

