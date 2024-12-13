using ClubApp.Models;
using ClubApp.ViewModels.Deportistas;

namespace ClubApp.View.Deportistas;

public partial class AddEditDeportistaView : ContentPage
{
    private AddEditDeportistaViewModel viewModel;

    public AddEditDeportistaView()
    {
        InitializeComponent();
        viewModel = new AddEditDeportistaViewModel();
        BindingContext = viewModel;
    }

    public AddEditDeportistaView(Deportista deportista)
    {
        InitializeComponent();
        viewModel = new AddEditDeportistaViewModel();
        BindingContext = viewModel;

        if (deportista != null)
        {
            viewModel.Deportista = deportista;
            Console.WriteLine($"Editando deportista con ID: {deportista.Id}");
        }
        else
        {
            Console.WriteLine("Creando un nuevo deportista.");
        }
    }
}
