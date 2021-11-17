using System;
using System.Collections.Generic;
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

namespace KGLab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_OnKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = (ViewModel)this.DataContext;
            if (e.Key == Key.A && viewModel.LeftTransformCommand.CanExecute(e)) 
            {
                if (viewModel.LeftButtonIsChecked ||
                    viewModel.RightButtonIsChecked ||
                    viewModel.UpButtonIsChecked ||
                    viewModel.DownButtonIsChecked)
                {
                    return;
                }
                viewModel.LeftButtonIsChecked = true;
                viewModel.LeftTransformCommand.Execute(e);
            }
            else if(e.Key == Key.D && viewModel.RightTransformCommand.CanExecute(e))
            {
                if (viewModel.LeftButtonIsChecked ||
                    viewModel.RightButtonIsChecked ||
                    viewModel.UpButtonIsChecked ||
                    viewModel.DownButtonIsChecked)
                {
                    return;
                }
                viewModel.RightButtonIsChecked = true;
                viewModel.RightTransformCommand.Execute(e);
            }
            else if (e.Key == Key.W && viewModel.UpTransformCommand.CanExecute(e))
            {
                if (viewModel.LeftButtonIsChecked ||
                    viewModel.RightButtonIsChecked ||
                    viewModel.UpButtonIsChecked ||
                    viewModel.DownButtonIsChecked)
                {
                    return;
                }
                viewModel.UpButtonIsChecked = true;
                viewModel.UpTransformCommand.Execute(e);
            }
            else if (e.Key == Key.S && viewModel.DownTransformCommand.CanExecute(e))
            {
                if (viewModel.LeftButtonIsChecked ||
                    viewModel.RightButtonIsChecked ||
                    viewModel.UpButtonIsChecked ||
                    viewModel.DownButtonIsChecked)
                {
                    return;
                }
                viewModel.DownButtonIsChecked = true;
                viewModel.DownTransformCommand.Execute(e);
            }
        }

        private void Window_OnKeyUp(object sender, KeyEventArgs e)
        {
            var viewModel = (ViewModel)this.DataContext;
            if (e.Key == Key.A)
            {
                viewModel.LeftButtonIsChecked = false;
            }
            else if (e.Key == Key.D)
            {
                viewModel.RightButtonIsChecked = false;
            }
            else if (e.Key == Key.W)
            {
                viewModel.UpButtonIsChecked = false;
            }
            else if (e.Key == Key.S)
            {
                viewModel.DownButtonIsChecked = false;
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            //var viewModel = (ViewModel.ViewModel)this.DataContext;
            //if (viewModel.OnMouseDownCommand.CanExecute(e.GetPosition((IInputElement)sender)))
            //{
            //    viewModel.OnMouseDownCommand.Execute(e.GetPosition((IInputElement)sender));
            //}
        }

        private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            //var viewModel = (ViewModel.ViewModel)this.DataContext;
            //if (viewModel.OnMouseUpCommand.CanExecute(e.GetPosition((IInputElement)sender)))
            //{
            //    viewModel.OnMouseUpCommand.Execute(e.GetPosition((IInputElement)sender));
            //}
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            tt.Placement = System.Windows.Controls.Primitives.PlacementMode.Relative;
            tt.HorizontalOffset = e.GetPosition((IInputElement)sender).X + 10;
            tt.VerticalOffset = e.GetPosition((IInputElement)sender).Y + 10;
            tt.Content = "X-Coordinate: " + e.GetPosition((IInputElement)sender).X + "\n" + "Y-Coordinate: " + e.GetPosition((IInputElement)sender).Y;
        }
    }
}
