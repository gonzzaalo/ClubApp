using ClubApp.Models;
using ClubApp.ViewModels.Deportes;

namespace ClubApp.View.Deportes;

public partial class AddEditDeporteView : ContentPage
{
    private AddEditDeporteViewModel viewModel;
    public AddEditDeporteView()
    {
        InitializeComponent();
        viewModel = new AddEditDeporteViewModel();
        BindingContext = viewModel;
    }

    public AddEditDeporteView(Deporte deporte)
    {
       InitializeComponent();
        viewModel = new AddEditDeporteViewModel();
        BindingContext = viewModel;

        if (deporte != null)
        {
            viewModel.Deporte = deporte;
            Console.WriteLine($"Editando deporte con ID: {deporte.Id}");
        }
        else
        {
            Console.WriteLine("Creando un nuevo deporte.");
        }
    }
}
