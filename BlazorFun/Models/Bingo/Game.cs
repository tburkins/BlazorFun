using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFun.Models.Bingo
{
    public class Game
    {
        private readonly Random rng = new Random((int)DateTime.Now.Ticks);
        private List<List<Square>> PotentialWinningCombos;

        public Game()
        {
            Squares = new Square[5, 5];
        }

        public Square[,] Squares { get; private set; }

        public bool IsInitialized { get; private set; }

        public bool IsWinner { get; private set; }

        public void SelectSquare(Square square)
        {
            if (square.IsWinner || !square.CanCheck)
            {
                return;
            }

            square.IsChecked = !square.IsChecked;
            CheckForWinner();
        }

        private void CheckForWinner()
        {
            foreach (var s in PotentialWinningCombos)
            {
                if (s.All(sq => sq.IsChecked))
                {
                    IsWinner = true;
                    s.ForEach(sq => sq.IsWinner = true);
                }
            }
        }

        public void SetCategories(List<string> categories)
        {
            var squareList = new List<string>(categories);
            while (squareList.Count < 25)
            {
                Shuffle(categories);
                squareList.AddRange(categories);
            }

            squareList = squareList.Take(25).ToList();
            Shuffle(squareList);

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    if (x == 2 && y == 2)
                    {
                        Squares[x, y] = new Square
                        {
                            Name = "Free",
                            IsChecked = true,
                            CanCheck = false
                        };
                    }
                    else
                    {
                        Squares[x, y] = new Square
                        {
                            Name = squareList[(5 * x) + y],
                            IsChecked = false,
                            CanCheck = true
                        };
                    }
                }
            }

            BuildPotentialWinningCombos();
            IsInitialized = true;
        }

        private void BuildPotentialWinningCombos()
        {
            PotentialWinningCombos = new List<List<Square>>();
            for (int row = 0; row < 5; row++)
            {
                PotentialWinningCombos.Add(SelectRow(row));
            }

            for (int col = 0; col < 5; col++)
            {
                PotentialWinningCombos.Add(SelectColumn(col));
            }

            var diagonal = new List<Square>();
            for (int n = 0; n < 5; n++)
            {
                diagonal.Add(Squares[n, n]);
            }
            PotentialWinningCombos.Add(diagonal);

            PotentialWinningCombos.Add(new List<Square>
            {
                Squares[4,0],Squares[3,1],Squares[2,2],Squares[1,3],Squares[0,4]
            });

            PotentialWinningCombos.Add(new List<Square>
            {
                Squares[0,0],Squares[0,4],Squares[2,2],Squares[4,0],Squares[4,4]
            });
        }

        private List<Square> SelectRow(int row)
        {
            var result = new List<Square>();

            for (int col = 0; col < 5; col++)
            {
                result.Add(Squares[row, col]);
            }

            return result;
        }

        private List<Square> SelectColumn(int col)
        {
            var result = new List<Square>();
            for (int row = 0; row < 5; row++)
            {
                result.Add(Squares[row, col]);
            }
            return result;
        }

        private void Shuffle(List<string> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


    }
}
