using BackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SudokuUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoardSelect : ContentPage
    {
        public Board Option;
        public BoardSelect()
        {
            InitializeComponent();

            Option = Board.Retrieve(1);

            int row, col;
            string num;
            for (int i = 0; i < 81; i++)
            {
                col = i % 9;
                row = i / 9;
                num = "" + Option.InitialBoard[i];
                if (num == "0") displayGrid.Children.Add(CreateLabel(""), col, row);
                else displayGrid.Children.Add(CreateLabel(num), col, row);
            }

            //displayGrid.Bo
        }

        private Label CreateLabel(string i)
        {
            return new Label
            {
                Text = i,
                BackgroundColor = Color.Transparent,
                TranslationX = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width != height)
            {
                if (width > height) displayGrid.HeightRequest = width;
                else displayGrid.WidthRequest = height;
            }
        }
    }
}