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
        public int selectedIndex = 100;
        public int[] board = new int[81];
        public MainPage()
		{
			InitializeComponent();
            //BackgroundImage = "blankgrid.png";//new Image { Source = ImageSource.FromResource("SudokuUI.Resources.blankgrid.png") };

            
            for (int i = 0; i < 81; i++)
            {
                board[i] = i + 1;
            }
            int row, col;
            for (int i = 0; i < 81; i++)
            {
                col = i % 9;
                row = i / 9;
                numGrid.Children.Add(CreateButton(board[i], board[i]), col, row);
            }
            for (int i = 0; i < 9; i++)
            {
                col = i%3;
                row = i/3;
                numpad.Children.Add(CreateNumButton(i), col, row);
                butGrid.Children.Add(numpad, 0, 0);
                //butGrid.Children.Add(CreateNumCancelButton(), 0, 1);
                numGrid.Children.ElementAt(80);
            }
        }

        public void ReloadBoard(int index)
        {
            numGrid.Children.RemoveAt(index);
        }

        private Button CreateButton(int i, int id)
        {
            var b = new Button
            {
                Text =  "" + i,
                StyleId = "" + id,
                BackgroundColor = Color.Transparent,
                TranslationX = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            b.Clicked += ButtonClicked;
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
            //button.BackgroundColor = Color.White;
            //numGrid.Children.RemoveAt(index);
        }

        public void NumButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (selectedIndex != 100)
            {
                ((Button)numGrid.Children.ElementAt(selectedIndex)).Text = button.Text;
                board[selectedIndex] = Convert.ToInt32(button.Text);
                SelectReset();
            }
        }

        public void CancelButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            SelectReset();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);


            if (height > width)
            {
                numGrid.HeightRequest = width;
                //numpad.HeightRequest = width / 3;
                //butGrid.HeightRequest = buttonStack.HeightRequest;
                int numWidth = Convert.ToInt32(height - (width + 25)) * (3 / 4);

                butGrid.WidthRequest = butGrid.Height;// 200;// (height - (width + 25)) * (3 / 4);
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
