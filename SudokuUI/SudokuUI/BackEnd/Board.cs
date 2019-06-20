using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SudokuUI;
using SQLite;
using System.Diagnostics;

namespace BackEnd
{
    public class Board
        /*Contains all the sudoku data in terms of a full board, rows, columns, and squares.
         * Has methods to: initialise boards with an input provided, apply updates, and output the board.
         */
    {
        public int Id { get; set; }
        public int[,] Rows { get; set; }// = new int[9, 9];
        public int[,] Columns { get; set; }// = new int[9, 9];
        public int[,] Squares { get; set; }// = new int[9, 9];
        public int[] GameBoard { get; set; }// = new int[81];
        public int[] InitialBoard { get; set; }// = new int[81];
        public int[] SolvedBoard { get; set; }// = new int[81];
        public List<int> Zeroes { get; set; }// = new List<int>();
        public static List<int> options  = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

        public Board()
        {
            this.Rows = new int[9, 9];
            this.Columns = new int[9, 9];
            this.Squares = new int[9, 9];
            this.GameBoard = new int[81];
            this.InitialBoard = new int[81];
            this.SolvedBoard = new int[81];
            this.Zeroes = new List<int>();
        }

        public Board(string Input)
        {
            this.Rows = new int[9, 9];
            this.Columns = new int[9, 9];
            this.Squares = new int[9, 9];
            this.GameBoard = new int[81];
            this.InitialBoard = new int[81];
            this.SolvedBoard = new int[81];
            this.Zeroes = new List<int>();

            Fill(Input);
        }

        public Board(int id, string rows, string columns, string squares, string gameBoard, string initialBoard, string solvedBoard, string zeroes)
        {
            this.Id = id;
            this.Rows = StringToArray2d(rows);
            this.Columns = StringToArray2d(columns);
            this.Squares = StringToArray2d(squares);
            this.GameBoard = gameBoard.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            this.InitialBoard = initialBoard.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            //this.SolvedBoard = solvedBoard.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            this.SolvedBoard = Logic.Main(this.MemberwiseClone() as Board);
            this.Zeroes = zeroes.Split(',').Select(n => Convert.ToInt32(n)).ToList<int>();
        }

        public void Fill(string strIn)//initialises/fills out the board
        {
            int[] input = strIn.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            GameBoard = input;
            InitialBoard = input;
            int num1 = input[0];
            if (num1 == 0) Zeroes.Add(0);
            Columns[0, 0] = input[0];
            Rows[0, 0] = input[0];
            Squares[0, 0] = input[0];
            for (int i = 1; i < 81; i++)
            {
                int num = input[i];
                if (num == 0) Zeroes.Add(i);
                Populate(i, num);
            }
            SolvedBoard = Logic.Main(this.MemberwiseClone() as Board);
        }

        public void Reconstruct(int index, int num)//finalises changes and updates board
        {
            this.GameBoard[index] = num;
            this.Zeroes.Remove(index);
            this.Populate(index, num);
        }

        public void Populate(int index, int num)//Populates appropriate row, column, and square array with the value
        {
            int row = index / 9;
            int col = index % 9;
            int squ = (3 * (row / 3)) + (col / 3);
            this.Columns[col, row] = num;
            this.Rows[row, col] = num;
            this.Squares[squ, col % 3 + (3 * (row % 3))] = num;
        }

        public int[,] StringToArray2d(string input)
        {
            int[,] output = new int[9, 9];
            int[] temp = input.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                output[row, col] = temp[i];
            }
            return output;
        }

        public void UpdateDB()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.Update(new SaveBoard(this.Id, this.Rows, this.Columns, this.Squares, this.GameBoard, this.InitialBoard, this.SolvedBoard, this.Zeroes));
                conn.CreateTable<LatestId>();
                conn.Update(new LatestId(this.Id));
            }
        }

        public void SaveToDB()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<SaveBoard>();
                conn.Insert(new SaveBoard(this.Id, this.Rows, this.Columns, this.Squares, this.GameBoard, this.InitialBoard, this.SolvedBoard, this.Zeroes));
                conn.CreateTable<LatestId>() ;
                conn.Update(new LatestId(this.Id));
            }
        }

        public static Board Retrieve(int id)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                //conn.CreateTable<SaveBoard>();
                SaveBoard sBoard = conn.Find<SaveBoard>(id);

                if (sBoard != null)
                {
                    return new Board(
                        sBoard.Id,
                        sBoard.Rows, 
                        sBoard.Columns, 
                        sBoard.Squares, 
                        sBoard.GameBoard, 
                        sBoard.InitialBoard, 
                        sBoard.SolvedBoard, 
                        sBoard.Zeroes
                        );
                }
                else
                {
                    return new Board();
                }
            }
        }

        public static Board Retrieve()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                //conn.CreateTable<SaveBoard>();
                SaveBoard sBoard = conn.Find<SaveBoard>(conn.Find<LatestId>(1).LatestID);

                if (sBoard != null)
                {
                    return new Board(
                        sBoard.Id,
                        sBoard.Rows,
                        sBoard.Columns,
                        sBoard.Squares,
                        sBoard.GameBoard,
                        sBoard.InitialBoard,
                        sBoard.SolvedBoard,
                        sBoard.Zeroes
                        );
                }
                else
                {
                    return new Board();
                }
            }
        }

        public void PrintBoard()//outputs the board in its current state
        {
            for (int i = 0; i < 9; i++)
            {
                String line = "";
                for (int j = 0; j < 9; j++)
                {
                    String num = Rows[i,j].ToString();
                    if (num == "") num = " ";
                    line += num + " ";
                }
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
    }

    public class LatestId
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int LatestID { get; set; }

        public LatestId()
        {

        }
        public LatestId(int LatestId)
        {
            this.Id = 1;
            this.LatestID = LatestId;
        }
    }

    public class SaveBoard
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Rows { get; set; }// = new int[9, 9];
        public string Columns { get; set; }// = new int[9, 9];
        public string Squares { get; set; }// = new int[9, 9];
        public string GameBoard { get; set; }// = new int[81];
        public string InitialBoard { get; set; }// = new int[81];
        public string SolvedBoard { get; set; }// = new int[81];
        public string Zeroes { get; set; }// = new List<int>();

        public SaveBoard()
        {

        }

        public SaveBoard(int id, int[,] rows, int[,] columns, int[,] squares, int[] gameBoard, int[] initialBoard, int[] solvedBoard, List<int> zeroes)
        {
            this.Id = id;
            this.Rows = Array2dToString(rows);
            this.Columns = Array2dToString(columns);
            this.Squares = Array2dToString(squares);
            this.GameBoard = string.Join(",",gameBoard);
            this.InitialBoard = string.Join(",", initialBoard);
            this.SolvedBoard = string.Join(",", solvedBoard);
            this.Zeroes = string.Join(",", zeroes);
        }
        public string Array2dToString(int[,] input)
        {
            string output = "";
            for (int i = 0; i < 9; i++)
            {
                var tempArray = input.GetArr(i);
                for (int j = 0; j < 9; j++)
                {
                    output += (tempArray[j] + ",");
                }
            }
            Debug.WriteLine(output.Remove(output.Length - 1));
            return output.Remove(output.Length-1);
        }
    }
    public static class BoardExtensions
    {
        public static Board Clone(this Board Original)
        {
            Board Copy = new Board();
            Copy.Id = Original.Id;
            Copy.Rows = Original.Rows;
            Copy.Columns = Original.Columns;
            Copy.Squares = Original.Squares;
            Copy.GameBoard = Original.GameBoard;
            Copy.InitialBoard = Original.InitialBoard;
            Copy.SolvedBoard = Original.SolvedBoard;
            Copy.Zeroes = Original.Zeroes;
            return Copy;
        }
    }

    public static class ArrayExt//provides a way to return a whole array at once from an array of arrays
    {
        public static T[] GetArr<T>(this T[,] array, int row)
        {
            if (!typeof(T).IsPrimitive)
                throw new InvalidOperationException("Not supported for managed types.");

            if (array == null)
                throw new ArgumentNullException("array");

            int cols = array.GetUpperBound(1) + 1;
            T[] result = new T[cols];
            int size = Marshal.SizeOf<T>();

            Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

            return result;
        }
    }
}
