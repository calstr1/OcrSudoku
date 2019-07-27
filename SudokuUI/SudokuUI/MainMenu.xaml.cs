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

            string[] InitialBoards = new string[3] 
            {
                "5,3,8,0,1,6,0,7,9,0,0,0,3,8,0,5,4,1,2,4,1,5,0,0,0,0,0,0,6,0,9,0,0,0,0,0,0,0,0,0,3,5,0,9,0,0,9,0,0,0,4,0,0,2,6,0,0,2,0,0,9,3,0,1,2,9,0,4,0,0,5,0,0,5,4,6,9,0,0,0,8",
                "7,9,0,0,0,0,3,0,0,0,0,0,0,0,6,9,0,0,8,0,0,0,3,0,0,7,6,0,0,0,0,0,5,0,0,2,0,0,5,4,1,8,7,0,0,4,0,0,7,0,0,0,0,0,6,1,0,0,9,0,0,0,8,0,0,2,3,0,0,0,0,0,0,0,9,0,0,0,0,5,4",
                "0,3,0,9,0,0,0,2,0,8,0,0,0,0,2,0,0,7,0,0,1,4,0,0,6,0,0,0,9,0,0,4,0,5,0,2,0,0,0,6,0,3,0,0,0,7,0,6,0,1,0,0,8,0,0,0,9,0,0,4,1,0,0,2,0,0,8,0,0,0,0,3,0,7,0,0,0,9,0,5,0"
            };
            
            if (!IsBoards())
            {
                foreach (string BoardStr in InitialBoards)
                {
                    using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                    {
                        Board temp = new Board(BoardStr);
                        temp.SolvedBoard = Logic.Main((Board)temp.Clone());
                        temp.SaveToDB();
                    }
                }
            }
        }

        public bool IsBoards()
        {
            bool output = false;
            try
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    if (conn.Table<SaveBoard>().Count() > 0) output = true;
                }
            }
            catch { }
            return output;
        }

        private async void Continue(object sender, EventArgs e)
        {
            //Board board = new BackEnd.Board();
            //board.Fill("5,3,8,0,1,6,0,7,9,0,0,0,3,8,0,5,4,1,2,4,1,5,0,0,0,0,0,0,6,0,9,0,0,0,0,0,0,0,0,0,3,5,0,9,0,0,9,0,0,0,4,0,0,2,6,0,0,2,0,0,9,3,0,1,2,9,0,4,0,0,5,0,0,5,4,6,9,0,0,0,8");
            await Navigation.PushAsync(new MainPage(Board.Retrieve()));

        }
        private async void NewGame(object sender, EventArgs e)
        {
            //Board board = new BackEnd.Board();
            //board.Fill("5,3,8,0,1,6,0,7,9,0,0,0,3,8,0,5,4,1,2,4,1,5,0,0,0,0,0,0,6,0,9,0,0,0,0,0,0,0,0,0,3,5,0,9,0,0,9,0,0,0,4,0,0,2,6,0,0,2,0,0,9,3,0,1,2,9,0,4,0,0,5,0,0,5,4,6,9,0,0,0,8");
            await Navigation.PushAsync(new BoardSelect());

        }
    }
}