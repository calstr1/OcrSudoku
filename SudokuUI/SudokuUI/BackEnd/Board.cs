using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SudokuUI;
using SQLite;

namespace BackEnd
{
    public class Board
        /*Contains all the sudoku data in terms of a full board, rows, columns, and squares.
         * Has methods to: initialise boards with an input provided, apply updates, and output the board.
         */
    {
        [PrimaryKey, AutoIncrement]
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
            this.Zeroes = new List<int>();
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
        }

        public void Reconstruct(int i, int num)//finalises changes and updates board
        {
            this.GameBoard[i] = num;
            this.Zeroes.Remove(i);
            this.Populate(i, num);
        }

        public void Populate(int i, int num)//Populates appropriate row, column, and square array with the value
        {
            int row = i / 9;
            int col = i % 9;
            int squ = (3 * (row / 3)) + (col / 3);
            this.Columns[col, row] = num;
            this.Rows[row, col] = num;
            this.Squares[squ, col % 3 + (3 * (row % 3))] = num;
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
