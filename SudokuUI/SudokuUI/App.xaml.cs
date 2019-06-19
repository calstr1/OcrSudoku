using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using BackEnd;

namespace SudokuUI
{
	public partial class App : Application
	{
        public static string DB_PATH = string.Empty;

		public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new MainMenu())
            {
                Title = "OcrSudoku",
                BarBackgroundColor = Color.Red
            };
            //Board board = new BackEnd.Board();
            //board.Fill("5,3,8,0,1,6,0,7,9,0,0,0,3,8,0,5,4,1,2,4,1,5,0,0,0,0,0,0,6,0,9,0,0,0,0,0,0,0,0,0,3,5,0,9,0,0,9,0,0,0,4,0,0,2,6,0,0,2,0,0,9,3,0,1,2,9,0,4,0,0,5,0,0,5,4,6,9,0,0,0,8");
            //MainPage = new NavigationPage(new MainPage(board));
        }

        public App(string DB_Path)
        {
            InitializeComponent();

            DB_PATH = DB_Path;

            MainPage = new NavigationPage(new MainMenu())
            {
                Title = "OcrSudoku"
            };
            //Board board = new BackEnd.Board();
            //board.Fill("5,3,8,0,1,6,0,7,9,0,0,0,3,8,0,5,4,1,2,4,1,5,0,0,0,0,0,0,6,0,9,0,0,0,0,0,0,0,0,0,3,5,0,9,0,0,9,0,0,0,4,0,0,2,6,0,0,2,0,0,9,3,0,1,2,9,0,4,0,0,5,0,0,5,4,6,9,0,0,0,8");
            //MainPage = new NavigationPage(new MainPage(board));
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
            SudokuUI.MainPage.Unsolved.UpdateDB();
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
