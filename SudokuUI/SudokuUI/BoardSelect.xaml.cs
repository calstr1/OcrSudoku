using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FrontEnd;
using System.Collections.ObjectModel;
using BackEnd;

namespace SudokuUI
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoardSelect : ContentPage
    {
        private List<Board> Boards => Board.RetrieveAll();
        private int Selection = 0;

        public Board Option;
        public BoardSelect()
        {
            
            InitializeComponent();
            //App.CurrentCarouselView = 0;

            Option = Boards[Selection];
            LoadBoard();

            //displayGrid.Bo
        }

        private async void SelectBoard(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(Option));
        }

        private void NavBoard(object sender, EventArgs e)
        {
            Image backgroundImage = new Image
            {
                Source = "blankgrid.png",
                Aspect = Aspect.AspectFill,
            };

            displayGrid.Children.Clear();
            displayGrid.Children.Add(backgroundImage);
            Grid.SetColumnSpan(backgroundImage, 9);
            Grid.SetRowSpan(backgroundImage, 9);


            string Direction = ((Button)sender).Text;
            if (((Button)sender) == next)
            {
                Selection++;
                if (Selection >= Boards.Count) Selection = 0;
            }
            if (((Button)sender) == prev)
            {
                Selection--;
                if (Selection <= 0) Selection = Boards.Count - 1;
            }

            Option = Boards[Selection];
            LoadBoard();
        }

        private void LoadBoard()
        {
            int row, col;
            string num;
            for (int i = 0; i < 81; i++)
            {
                col = i % 9;
                row = i / 9;
                num = "" + Option.GameBoard[i];
                if (num == "0") displayGrid.Children.Add(CreateButton(""), col, row);
                else displayGrid.Children.Add(CreateButton(num), col, row);
            }
        }

        private Button CreateButton(string i)
        {
            var b = new Button
            {
                Text = i,
                BackgroundColor = Color.Transparent,
                TranslationX = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            return b;
        }

        protected override void OnSizeAllocated(double width, double height)
        {

            base.OnSizeAllocated(width, height);

            displayGrid.HeightRequest = baseStack.Width - baseStack.Padding.Left * 2;
            displayGrid.WidthRequest = baseStack.Width - baseStack.Padding.Left * 2;
            navGrid.WidthRequest = baseStack.Width - baseStack.Padding.Left * 2;

            /*if (width != height)
            {
                if (width > height) displayGrid.HeightRequest = width;
                else displayGrid.WidthRequest = height;
            }*/
        }
    }
}