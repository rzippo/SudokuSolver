using SudokuSolverLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SudokuSolverWPF
{
    public class SudokuCellViewModel : DependencyObject
    {
        public string TextValue
        {
            get { return (string)GetValue(TextValueProperty); }
            set { SetValue(TextValueProperty, value); }
        }

        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string), typeof(SudokuCellViewModel), new PropertyMetadata(""));

        public SudokuCell Cell { get; }

        public SudokuCellViewModel(SudokuCell observedCell)
        {
            Cell = observedCell;
            Cell.PropertyChanged += CellChangedHandler;
            UpdateTextValue();
        }

        private void CellChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => UpdateTextValue()));
        }

        private void UpdateTextValue()
        {
            if (Cell.IsDetermined)
                TextValue = $"{Cell.Value}";
            else
                TextValue = string.Empty;
        }
    }
}
