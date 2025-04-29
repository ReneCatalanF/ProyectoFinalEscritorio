using Restaurant_SAP.DB;
using Restaurant_SAP.ViewModels;
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

namespace Restaurant_SAP.Views
{
    /// <summary>
    /// Lógica de interacción para HistorialEconomicoView.xaml
    /// </summary>
    public partial class HistorialEconomicoView : UserControl
    {
        public HistorialEconomicoView()
        {
            InitializeComponent();
        }

        private void HistorialEconomicoView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue) // Si el UserControl se vuelve visible
            {
                if (DataContext is HistorialEconomicoViewModel viewModel)
                {
                    viewModel.CargarMenusYMesas();
                }
            }
        }
    }
}
