using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SudokuUI
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            //BackgroundImage = "blankgrid.png";//new Image { Source = ImageSource.FromResource("SudokuUI.Resources.blankgrid.png") };

            int row, col;
            for (int i = 0; i < 81; i++)
            {
                col = i % 9;
                row = i / 9;
                numGrid.Children.Add(CreateButton(i)/*new Button
                {
                    Text = "" + i,
                    StyleId = "" + i,
                    BackgroundColor = Color.Transparent,
                    TranslationX = 0,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                }*/
                , col, row);
            }

            for (int i = 0; i < 9; i++)
            {
                col = i%3;
                row = i/3;
                butGrid.Children.Add(CreateNumButton(i)/*new Button
                {
                    Text = "" + (i + 1),
                    StyleId = "numButton" + i,
                    BackgroundColor = Color.LightBlue,
                    TranslationX = 0,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                }*/
                , col, row);
                butGrid.Children.Add(CreateNumCancelButton(),3,1,0,3);
            }
        }

        private Button CreateButton(int i)
        {
            var b = new Button
            {
                Text = "" + i,
                StyleId = "" + i,
                BackgroundColor = Color.Transparent,
                TranslationX = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //b.Clicked += ButtonClicked;

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
            //b.Clicked += ButtonClicked;

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

        /*async void ButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if ()
            {
                await numGrid
            }
        }*/

        public void NumButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
        }

        public void CancelButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (height > width)
            {
                numGrid.HeightRequest = width;
                butGrid.HeightRequest = width / 3;
            }
            else
            {
                numGrid.WidthRequest = height;
                butGrid.WidthRequest = (height * 4) / 3;
            }
        }
    }
}
