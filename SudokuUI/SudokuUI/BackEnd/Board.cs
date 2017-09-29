using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BackEnd
{
    public class Board
        /*Contains all the sudoku data in terms of a full board, rows, columns, and squares.
         * Has methods to: initialise boards with an input provided, apply updates, and output the board.
         */
    {
        public int[,] rows = new int[9, 9];
        public int[,] columns = new int[9, 9];
        public int[,] squares = new int[9, 9];
        public int[] board = new int[81];
        public int[] initialBoard = new int[81];
        public int[] solvedBoard = new int[81];
        public List<int> zeroes = new List<int>();
        public List<int> options = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

        public Board()
        {
            this.rows = new int[9, 9];
            this.columns = new int[9, 9];
            this.squares = new int[9, 9];
            this.board = new int[81];
            this.initialBoard = new int[81];
            this.zeroes = new List<int>();
        }

        public void Fill(string strIn)//initialises/fills out the board
        {
            int[] input = strIn.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            board = input;
            initialBoard = input;
            int num1 = input[0];
            if (num1 == 0) zeroes.Add(0);
            columns[0, 0] = input[0];
            rows[0, 0] = input[0];
            squares[0, 0] = input[0];
            for (int i = 1; i < 81; i++)
            {
                int num = input[i];
                if (num == 0) zeroes.Add(i);
                Populate(i, num);
            }
        }

        public void Reconstruct(int i, int num)//finalises changes and updates board
        {
            this.board[i] = num;
            this.zeroes.Remove(i);
            this.Populate(i, num);
        }

        public void Populate(int i, int num)//Populates appropriate row, column, and square array with the value
        {
            int row = i / 9;
            int col = i % 9;
            int squ = (3 * (row / 3)) + (col / 3);
            this.columns[col, row] = num;
            this.rows[row, col] = num;
            this.squares[squ, col % 3 + (3 * (row % 3))] = num;
        }

        public void PrintBoard()//outputs the board in its current state
        {
            for (int i = 0; i < 9; i++)
            {
                String line = "";
                for (int j = 0; j < 9; j++)
                {
                    String num = rows[i,j].ToString();
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
