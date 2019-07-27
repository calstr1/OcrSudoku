
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using BackEnd;

namespace SudokuUI
{
	public partial class MainPage : ContentPage
	{
        public static Board Unsolved;
        public int selectedIndex = 100;
        public MainPage(Board unsolved)
		{
            Unsolved = unsolved;
            InitializeComponent();

            if (Unsolved.SolvedBoard.Contains(0))
            {
                Unsolved.SolvedBoard = Logic.Main((Board)Unsolved.Clone());
            }

            int row, col;
            string num;
            for (int i = 0; i < 81; i++)
            {
                col = i % 9;
                row = i / 9;
                num = "" + Unsolved.GameBoard[i];
                if (num == "0") numGrid.Children.Add(CreateButton("", i+1), col, row);
                else numGrid.Children.Add(CreateButton(num, i+1), col, row);
            }
            for (int i = 0; i < 9; i++)
            {
                col = i%3;
                row = i/3;
                numpad.Children.Add(CreateNumButton(i), col, row);
                butGrid.Children.Add(numpad, 0, 0);
                numGrid.Children.ElementAt(80);
            }
        }

        public void ReloadBoard(int index)
        {
            numGrid.Children.RemoveAt(index);
        }

        private Button CreateButton(string i, int id)
        {
            var b = new Button
            {
                Text =  i,
                StyleId = "" + id,
                BackgroundColor = Color.Transparent,
                TranslationX = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            if (i == "" || Unsolved.InitialBoard[id-1] != Unsolved.GameBoard[id-1]) b.Clicked += ButtonClicked;
            else b.FontAttributes = FontAttributes.Bold;
            return b;
        }

        private Button CreateNumButton(int i)
        {
            var b = new Button {
                Text = "" + (i + 1),
                StyleId = "numButton" + i,
                BackgroundColor = Color.LightBlue,
                TranslationX = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            b.Clicked += NumButtonClicked;

            return b;
        }

        private Button CreateNumCancelButton()
        {
            var b = new Button
            {
                Text = "Cancel",
                StyleId = "numCancelButton",
                BackgroundColor = Color.LightBlue,
                TranslationX = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //b.Clicked += ButtonClicked;

            return b;
        }

        private void SelectReset(int index = -1)
        {
            if (selectedIndex != 100)
            {
                if (selectedIndex != index)
                {
                    ((Button)numGrid.Children.ElementAt(selectedIndex)).BackgroundColor = Color.Transparent;
                }
            }
            selectedIndex = 100;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int index = Convert.ToInt32(button.StyleId);
            SelectReset(index);
            selectedIndex = index;
            button.BackgroundColor = Color.Yellow;
        }

        public void NumButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (selectedIndex != 100)
            {
                int zcount = Unsolved.Zeroes.Count();
                //((Button)numGrid.Children.ElementAt(selectedIndex)).Text = button.Text;
                string msg = Legallity(selectedIndex-1, Convert.ToInt32(button.Text));
                if (msg == "That value is correct")
                {
                    ((Button)numGrid.Children.ElementAt(selectedIndex)).Text = button.Text;
                }
                else if (Unsolved.Zeroes.Count() == zcount)
                {
                    DisplayAlert("Warning", msg, "OK");
                }
                SelectReset();
                if (Unsolved.Zeroes.Count() == 0)
                {
                    DisplayAlert("Game Over","The Sudoku is Complete","Finish");
                }
            }
        }

        public void CancelButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            SelectReset();
        }

        public string Legallity(int index, int value)//Checks whether the value is correct and if so implements
        {
                if (Logic.PossList(index, Unsolved).Contains(value))//checks if there is a clash between a number in a row, column, or square, and the selected value
                {
                    if (value == Unsolved.SolvedBoard[index])//checks if value is correct
                    {
                        Unsolved.Reconstruct(index, value);
                        //Unsolved.PrintBoard();
                        return "That value is correct";
                    }
                    else return "" + value + " is not the correct value.";
                }
                else return "There is a " + value + " in the same square, column, or row.";
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Unsolved.UpdateDB();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);


            if (height > width)
            {
                numGrid.HeightRequest = width;
                //numpad.HeightRequest = width / 3;
                //butGrid.HeightRequest = buttonStack.HeightRequest;
                double numWidth = (base.Height - (base.Width)) * 0.75;

                //butGrid.HeightRequest = numWidth;// butGrid.Height;// 200;// (height - (width + 25)) * (3 / 4);
                butGrid.WidthRequest = numWidth;//.75 * Convert.ToDouble((height - (width)) * (3 / 4) - 6);
            }
            else
            {
                numGrid.WidthRequest = height;
                //numpad.HeightRequest = width / 3;
                //butGrid.HeightRequest = height / 3;
                //butGrid.WidthRequest = (height / 3) * (3 / 4);
            }
        }
    }
}
