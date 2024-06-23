using Avalonia.Controls;
using Finance.AvaloniaClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.AvaloniaClient.Views
{
    public partial class FinanceView : UserControl
    {
        public FinanceView()
        {
            InitializeComponent();
            DataContext = ((App)Avalonia.Application.Current).Services.GetRequiredService<FinanceViewModel>();
        }
    }
}
