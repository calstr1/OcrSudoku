using SudokuUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    public class Logic
        /*Contains all the methods to solve a sudoku, uses data structures in Board.
         * Has methods to: receive a user's input, return all legal possibilities for a box, and apply logic to solve the puzzle, and call on the user to solve the puzzle through Game.
         */
    {
        //public static Board Unsolved = new Board();
        public static Board Solved = new Board();
        public static int[] Main(Board Input)//(string input)
        {
            //Console.WriteLine("Enter a sudoku board line by line with comma seperated numbers, and a zero for an empty square: ");
            Solved = Input;
            //Solved = (Board)Input.Clone();
            //Solved.Fill(input);
            //MainPage.Unsolved.Fill(input);
            //Solved.PrintBoard();
            
            while (Solved.Zeroes.Count() != 0)
            {
                int initial = Solved.Zeroes.Count();
                for (int i = 0; i < Solved.Zeroes.Count(); i++)
                {
                    int boardIndex = Solved.Zeroes.ElementAt(i);
                    List<int> currPoss = PossList(boardIndex, Solved);
                    if (currPoss.Count() == 1 && Solved.Zeroes.Contains(boardIndex))//if there is only one possible number for that cell, enter it
                    {
                        int num = currPoss.ElementAt(0);
                        Solved.Reconstruct(boardIndex, num);
                    }
                    else
                    {
                        /*return from zero list matching row, column, and square find unique possibility, implrement, repeat for squ and col
                        *In other words, if a row needs 3 numbers, and one of those numbers only has one place it could go,
                        * regardless of whether you record it as the only possible option or not, it must be the solution and is entered
                        */
                        int row = boardIndex / 9;
                        int col = boardIndex % 9;
                        int squ = (3 * (row / 3)) + (col / 3);
                        int[] rowIndx = new int[9];
                        int[] colIndx = new int[9];
                        int[] squIndx = new int[9];
                        for (int a = 0; a < 9; a++)//populates the index lists for the current row and column
                        {
                            rowIndx[a] = (row * 9) + a;
                            colIndx[a] = col + (9 * a);
                        }
                        for (int c = 0; c < 3; c++)//populates the index lists for the current square
                        {
                            for (int d = 0; d < 3; d++)
                            {
                                squIndx[(c * 3) + (d)] = (((squ - squ % 3) + c) * 9) + ((squ % 3) * 3) + d;
                            }
                        }
                        List<int> zRow = new List<int>();//lists that will hold the indexes of all the empty or 'zero' values in the row column or square
                        List<int> zCol = new List<int>();
                        List<int> zSqu = new List<int>();
                        for (int iter = 0; iter < 9; iter++)//populates zero lists
                        {
                            int r1 = rowIndx[iter];
                            int c = colIndx[iter];
                            int s = squIndx[iter];
                            if (Solved.Zeroes.Contains(r1)) zRow.Add(r1);
                            if (Solved.Zeroes.Contains(c)) zCol.Add(c);
                            if (Solved.Zeroes.Contains(s)) zSqu.Add(s);
                        }
                        IndexLogic(zRow);//applies aforementioned logic through the IndexLogic function to the applicable row, column, and square
                        IndexLogic(zCol);
                        IndexLogic(zSqu);
                    }
                }
                if (initial == Solved.Zeroes.Count()) { /*Console.WriteLine("true");*/ break; }
            }
            return Solved.GameBoard;
        }

        public static List<int> PossList(int i, Board board)//returns a list with all the legal number options
        {
            int row = i / 9;
            int col = i % 9;
            int squ = (3 * (row / 3)) + (col / 3);
            List<int> rcs = new List<int>();//a list which contains all conflicting numbers for that index
            rcs.AddRange(board.Rows.GetArr(row));
            rcs.AddRange(board.Columns.GetArr(col));
            rcs.AddRange(board.Squares.GetArr(squ));
            List<int> poss = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            poss.RemoveAll(item => rcs.Contains(item));//removes allconflicting numbers from the possibility list and returns it
            return poss;
        }

        public static void IndexLogic(List<int> z)/* 'z' is for zero
            for any row column or square if for all of the missing numbers in the array, any of them only occurs as a possibility once,
            it gets implemented and inserted into the board
            */
        {
            List<List<int>> indexesPoss = new List<List<int>>();
            List<int> remove = new List<int>();
            List<int> unique = new List<int>();
            List<int> temp = new List<int>();
            List<int> currPoss = new List<int>();
            List<int> noCheck = new List<int>();
            for (int i = 0; i < z.Count(); i++)//if there is only one possible number for that cell, enter it
            {
                int boardIndex = z.ElementAt(i);
                currPoss = PossList(boardIndex, Solved);//curr poss contains the current list of possibilities for a specific index
                if (currPoss.Count() == 1 && Solved.Zeroes.Contains(boardIndex))//checks if there is only one possibility for that index and double checks if index is empty
                {
                    int num = currPoss.ElementAt(0);
                    Solved.Reconstruct(boardIndex, num);
                    temp.Add(num);
                    noCheck.Add(indexesPoss.Count());//populates a list of indexes which are no longer empty since zero check
                    indexesPoss.Add(new List<int>());
                }
                else indexesPoss.Add(currPoss);

            }
            for (int j = 0; j < indexesPoss.Count(); j++)//populates unique with each possible number for a cell in the array
            {
                if (!noCheck.Contains(j))//makes sure the cell hasnt been filled in since zero check
                {
                    for (int k = 0; k < indexesPoss.ElementAt(j).Count(); k++)
                    {
                        int val = indexesPoss.ElementAt(j).ElementAt(k);
                        if (!unique.Contains(val)) unique.Add(val);
                        else if (!temp.Contains(val)) temp.Add(val);
                    }
                }
            }
            unique.RemoveAll(item => temp.Contains(item));//removes all numbers from unique that occurred multiple times
            for (int j = 0; j < indexesPoss.Count(); j++)//fills in all cells that have a possibility in the unique list with that number
            {
                if (!noCheck.Contains(j))
                {
                    for (int k = 0; k < indexesPoss.ElementAt(j).Count(); k++)
                    {
                        int val = indexesPoss.ElementAt(j).ElementAt(k);
                        if (unique.Contains(val) && Solved.Zeroes.Contains(z.ElementAt(j)))
                        {
                            Solved.Reconstruct(z.ElementAt(j), val);
                        }
                    }
                }
            }
        }
    }
}
