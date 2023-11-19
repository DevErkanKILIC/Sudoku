using System;
using System.Runtime.CompilerServices;

namespace Sudoku.WinForm
{
    public partial class Game : Form
    {
        private const int ROW_COLUMN_SIZE = 9;
        private readonly int[,] _sudokuTable;
        private Random _random;
        private TextBox[,] _txtSudokuTable;
        private readonly Size _txtSize = new(30, 30);

        private int _txtLocationBetweenSize = 35;
        private int _txtLocationX = 0;
        private int _txtLocationY = 0;

        private readonly int _firstColumnIndex;
        private readonly int _firstRowIndex;

        public Game()
        {
            InitializeComponent();

            _sudokuTable = new int[ROW_COLUMN_SIZE, ROW_COLUMN_SIZE];
            _random = new Random();
            _txtSudokuTable = new TextBox[9, 9];
            _firstColumnIndex = _random.Next(0, 9);
            _firstRowIndex = _random.Next(0, 9);
        }


        private void Game_Load(object sender, EventArgs e)
        {
            CreateSudokuTable(_sudokuTable);
            CreateSudokuTableComponents();
            PrintSolvedSudokuTable(_sudokuTable);
            //PrintSudokuTable(_sudokuTable);
        }

        private int[,] SetFirstValue(int[,] sudokuTable)
        {
            sudokuTable[_firstColumnIndex, _firstRowIndex] = _random.Next(1, 10);
            return sudokuTable;
        }

        private int[,] SetFirstValues(int[,] sudokuTable, int firstValueLength)
        {
            int value = 0;
            int randomColumnIndex = 0;
            int randomRowIndex = 0;
            for (int i = 0; i < firstValueLength; i++)
            {
                do
                {
                    randomRowIndex = _random.Next(0, 9);
                    randomColumnIndex = _random.Next(0, 9);
                    value = _random.Next(1, 10);
                } while (sudokuTable[randomRowIndex, randomColumnIndex] != 0 || !IsAppropriateValue(sudokuTable, randomRowIndex, randomColumnIndex, value));

                sudokuTable[randomRowIndex, randomColumnIndex] = value;
            }
            return sudokuTable;
        }

        private int[,] CreateSudokuTable(int[,] sudokuTable)
        {
            return SetFirstValues(sudokuTable,40);
        }

        private bool IsAppropriateValue(int[,] sudokuTable, int column, int row, int value)
        {
            for (int index = 0; index < ROW_COLUMN_SIZE; index++)
            {
                if (sudokuTable[column, index] == value || sudokuTable[index, row] == value) return false;
            }

            int mainRow = row - row % 3;
            int mainColumn = column - column % 3;

            for (int columnIndex = mainColumn; columnIndex < mainColumn + 3; columnIndex++)
            {
                for (int rowIndex = mainRow; rowIndex < mainRow + 3; rowIndex++)
                {
                    if (sudokuTable[columnIndex, rowIndex] == value) return false;
                }
            }

            return true;
        }

        private bool SolveSudokuTable(int[,] sudokuTable)
        {
            for (int column = 0; column < ROW_COLUMN_SIZE; column++)
            {
                for (int row = 0; row < ROW_COLUMN_SIZE; row++)
                {
                    if (sudokuTable[column, row] == 0)
                    {
                        for (int value = 1; value <= ROW_COLUMN_SIZE; value++)
                        {
                            if (IsAppropriateValue(sudokuTable, column, row, value))
                            {
                                sudokuTable[column, row] = value;
                                if (SolveSudokuTable(sudokuTable)) return true;
                                sudokuTable[column, row] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        private void CreateSudokuTableComponents()
        {
            for (int column = 0; column < ROW_COLUMN_SIZE; column++)
            {
                for (int row = 0; row < ROW_COLUMN_SIZE; row++)
                {
                    _txtSudokuTable[column, row] = new TextBox();
                    _txtSudokuTable[column, row].Name = $"CR-{column}{row}";
                    _txtSudokuTable[column, row].Location = new Point(_txtLocationX, _txtLocationY);
                    _txtSudokuTable[column, row].Size = _txtSize;
                    Controls.Add(_txtSudokuTable[column, row]);
                    _txtLocationX += _txtLocationBetweenSize;
                }
                _txtLocationX = 0;
                _txtLocationY += _txtLocationBetweenSize;
            }
        }

        private void PrintSudokuTable(int[,] sudokuTable)
        {
            for (int column = 0; column < ROW_COLUMN_SIZE; column++)
            {
                for (int row = 0; row < ROW_COLUMN_SIZE; row++)
                {
                    _txtSudokuTable[column, row].Text = sudokuTable[column, row].ToString();
                }
            }
        }

        private void PrintSolvedSudokuTable(int[,] sudokuTable)
        {
            if (!SolveSudokuTable(sudokuTable)) { MessageBox.Show("Sudoku cannot be solve"); }

            for (int column = 0; column < ROW_COLUMN_SIZE; column++)
            {
                for (int row = 0; row < ROW_COLUMN_SIZE; row++)
                {
                    _txtSudokuTable[column, row].Text = sudokuTable[column, row].ToString();
                }
            }
        }




        private void Game_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar)) e.Handled = true;
            else e.Handled = false;
        }
    }
}