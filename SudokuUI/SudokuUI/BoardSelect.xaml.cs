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

        async void NavBoard(object sender, EventArgs e)
        {
            Image backgroundImage = new Image
            {
                Source = "blankgrid.png",
                Aspect = Aspect.AspectFill,
            };

            displayGrid.Children.Clear();
            displayGrid.Children.Add(backgroundImage);
            string Direction = "next";
            if (Direction == "next")
            {
                Selection++;
                if (Selection >= Boards.Count) Selection = 0;
            }
            if (Direction == "prev")
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
                num = "" + Option.InitialBoard[i];
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
            //width -= 10;
            //height -= 10;

            base.OnSizeAllocated(width, height);

            displayGrid.HeightRequest = baseStack.Width - baseStack.Padding.Left * 2;
            displayGrid.WidthRequest = baseStack.Width - baseStack.Padding.Left * 2;

            /*if (width != height)
            {
                if (width > height) displayGrid.HeightRequest = width;
                else displayGrid.WidthRequest = height;
            }*/
        }
    }
}