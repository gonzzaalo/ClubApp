using ClubApp.Models;
using ClubApp.ViewModels.Cuotas;

namespace ClubApp.View.Cuotas;

public partial class AddEditCuotaView : ContentPage
{
    private AddEditCuotaViewModels viewModel;
    public AddEditCuotaView()
    {
        InitializeComponent();
        viewModel = new AddEditCuotaViewModels();
        BindingContext = viewModel;
    }

    public AddEditCuotaView(Cuota cuota)
    {
        InitializeComponent();
        viewModel = new AddEditCuotaViewModels();
        BindingContext = viewModel;
        if (cuota != null)
        {
            viewModel.Cuota = cuota;
        }

    }
}