using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KGLab4
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private Canvas _figureView;

        public Canvas FigureView
        {
            get => _figureView;
            set
            {
                _figureView = value;
                OnPropertyChanged(nameof(FigureView));
            }
        }

        public ViewModel()
        {
            FigureView = new Canvas();
        }


        #region Notify

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
