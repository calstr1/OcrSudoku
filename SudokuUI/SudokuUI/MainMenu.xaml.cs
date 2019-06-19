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
    public partial class MainMenu : ContentPage
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private async void Continue(object sender, EventArgs e)
        {
            Board board = new BackEnd.Board();
            board.Fill("5,3,8,0,1,6,0,7,9,0,0,0,3,8,0,5,4,1,2,4,1,5,0,0,0,0,0,0,6,0,9,0,0,0,0,0,0,0,0,0,3,5,0,9,0,0,9,0,0,0,4,0,0,2,6,0,0,2,0,0,9,3,0,1,2,9,0,4,0,0,5,0,0,5,4,6,9,0,0,0,8");
            await Navigation.PushAsync(new MainPage(board));
        }
    }
}