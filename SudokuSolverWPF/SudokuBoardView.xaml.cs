using SudokuSolverLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuSolverWPF
{
    /// <summary>
    /// Interaction logic for SudokuBoardView.xaml
    /// </summary>
    public partial class SudokuBoardView : UserControl
    {

        public SudokuBoard Board
        {
            get { return (SudokuBoard)GetValue(BoardProperty); }
            set
            {
                SetValue(BoardProperty, value);
                ReloadCellsView();
            }
        }

        public static readonly DependencyProperty BoardProperty =
            DependencyProperty.Register(
                "Board",
                typeof(SudokuBoard),
                typeof(SudokuBoardView),
                new PropertyMetadata(
                    new SudokuBoard()
                )
            );

        public ObservableCollection<SudokuCellViewModel> CellsView
        {
            get { return (ObservableCollection<SudokuCellViewModel>)GetValue(CellsViewProperty); }
            set { SetValue(CellsViewProperty, value); }
        }

        public static readonly DependencyProperty CellsViewProperty =
            DependencyProperty.Register(
                "CellsView",
                typeof(ObservableCollection<SudokuCellViewModel>),
                typeof(SudokuBoardView),
                new PropertyMetadata(new ObservableCollection<SudokuCellViewModel>())
                );

        private SudokuPuzzle puzzle;
        public SudokuPuzzle Puzzle
        {
            get => puzzle;
            set
            {
                puzzle = value;
                Board = new SudokuBoard(puzzle);
            }
        }

        public SudokuBoardView()
        {
            InitializeComponent();
            ReloadCellsView();
        }

        private void ReloadCellsView()
        {
            CellsView.Clear();
            for (int rowIndex = 0; rowIndex < Board.RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Board.ColumnCount; columnIndex++)
                {
                    CellsView.Add(new SudokuCellViewModel(Board.Matrix[rowIndex, columnIndex]));
                }
            }
        }
    }
}
