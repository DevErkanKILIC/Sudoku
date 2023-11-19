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

        public Game()
        {
            InitializeComponent();

            _sudokuTable = new int[ROW_COLUMN_SIZE, ROW_COLUMN_SIZE];
            _random = new Random();
            _txtSudokuTable = new TextBox[9, 9];
        }


        private void Game_Load(object sender, EventArgs e)
        {
            CreateSudokuTable();
            PrintSudokuTable();
        }

        private int[,] CreateSudokuTable()
        {
            for (int column = 0; column < ROW_COLUMN_SIZE; column++)
            {
                for (int row = 0; row < ROW_COLUMN_SIZE; row++)
                {
                    _sudokuTable[column, row] = _random.Next(0, 9);
                }
            }
            return _sudokuTable;
        }

        private void PrintSudokuTable()
        {
            for (int column = 0; column < ROW_COLUMN_SIZE; column++)
            {
                for (int row = 0; row < ROW_COLUMN_SIZE; row++)
                {
                    _txtSudokuTable[column, row] = new TextBox();
                    _txtSudokuTable[column, row].Name = $"CR-{column - row}";
                    _txtSudokuTable[column, row].Size = _txtSize;
                    _txtSudokuTable[column, row].Location = new Point(_txtLocationX, _txtLocationY);
                    _txtSudokuTable[column, row].MaxLength = 1;
                    _txtSudokuTable[column, row].KeyPress += Game_KeyPress;
                    _txtLocationX += _txtLocationBetweenSize;
                    Controls.Add(_txtSudokuTable[column, row]);
                }
                _txtLocationX = 0;
                _txtLocationY += _txtLocationBetweenSize;
            }
        }

        private void Game_KeyPress(object? sender, KeyPressEventArgs e)
        {
           if(!char.IsDigit(e.KeyChar)) e.Handled = true;
           else e.Handled = false;
        }
    }
}