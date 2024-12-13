using ClubApp.Class;
using ClubApp.Models;
using ClubApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ClubApp.ViewModels.Deportes;

//public class AddEditDeporteViewModel : ObservableObject
//{
//    private readonly GenericService<Deporte> deporteService = new GenericService<Deporte>();

//    private Deporte deporte;

//    public Deporte Deporte
//    {
//        get => deporte;
//        set
//        {
//            deporte = value ?? new Deporte();
//            OnPropertyChanged();
//        }
//    }

//    public AddEditDeporteViewModel()
//    {
//        Deporte = new Deporte();
//        GuardarCommand = new AsyncRelayCommand(Guardar);
//        CancelarCommand = new RelayCommand(Cancelar);
//    }

//    public IAsyncRelayCommand GuardarCommand { get; }
//    public IRelayCommand CancelarCommand { get; }

//    private async Task Guardar()
//    {
//        if (string.IsNullOrWhiteSpace(Deporte.Nombre))
//        {
//            Console.WriteLine("El nombre del deporte es obligatorio.");
//            return;
//        }

//        try
//        {
//            if (Deporte.Id == 0)
//            {
//                await deporteService.AddAsync(Deporte);
//            }
//            else
//            {
//                await deporteService.UpdateAsync(Deporte);
//            }

//            WeakReferenceMessenger.Default.Send(new MyMessage("VolverADeportes"));
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error al guardar deporte: {ex.Message}");
//        }
//    }

//    private void Cancelar()
//    {
//        WeakReferenceMessenger.Default.Send(new MyMessage("VolverADeportes"));
//    }
//}
public class AddEditDeporteViewModel : ObservableObject
{
    private readonly GenericService<Deporte> deporteService = new GenericService<Deporte>();
    private readonly GenericService<Profesor> profesorService = new GenericService<Profesor>();

    private Deporte deporte;
    private Profesor profesor;
    public Deporte Deporte
    {
        get => deporte;
        set
        {
            deporte = value ?? new Deporte();
            OnPropertyChanged();
            OnPropertyChanged(nameof(HoraString)); // Actualiza también la propiedad auxiliar
        }
    }

    public Profesor Profesor
    {
        get => profesor;
        set
        {
            profesor = value ?? new Profesor();
            OnPropertyChanged();
        }
    }

    public string HoraString
    {
        get => Deporte.Hora.ToString(@"hh\:mm"); // Formato legible para la hora
        set
        {
            if (TimeSpan.TryParse(value, out var parsedTime))
            {
                Deporte.Hora = parsedTime;
            }
            else
            {
                Deporte.Hora = TimeSpan.Zero; // Valor predeterminado si falla
            }
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Profesor> profesores;
    public ObservableCollection<Profesor> Profesores
    {
        get => profesores;
        set
        {
            profesores = value;
            OnPropertyChanged();
        }
    }
    public IAsyncRelayCommand GuardarCommand { get; }
    public IRelayCommand CancelarCommand { get; }
    //public AddEditDeporteViewModel()
    //{
    //    Deporte = new Deporte();
    //    Profesor = new Profesor();
    //    GuardarCommand = new AsyncRelayCommand(Guardar);
    //    CancelarCommand = new RelayCommand(Cancelar);
    //}
    public AddEditDeporteViewModel()
    {
        Deporte = new Deporte();
        Profesor = new Profesor();
        Profesores = new ObservableCollection<Profesor>();
        GuardarCommand = new AsyncRelayCommand(Guardar);
        CancelarCommand = new RelayCommand(Cancelar);

        _ = ObtenerListas(); // Inicializa la lista de profesores al crear el ViewModel
    }

    public async Task ObtenerListas()
    {
        try
        {
            var profesores = await profesorService.GetAllAsync();
            Profesores = new ObservableCollection<Profesor>(profesores);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener profesores: {ex.Message}");
        }
    }



    private async Task Guardar()
    {
        if ( Deporte == null)
        {
            Deporte = new Deporte();
        }

        try
        {
            //Asigar el profesor seleccionado al deporte
            Deporte.ProfesorId = Profesor?.Id ?? 0;

            if (Deporte.Id == 0)
            {
                await deporteService.AddAsync(Deporte);
            }
            else
            {
                await deporteService.UpdateAsync(Deporte);
            }

            WeakReferenceMessenger.Default.Send(new MyMessage("VolverADeportes"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar deporte: {ex.Message}");
        }
    }

    private void Cancelar()
    {
        WeakReferenceMessenger.Default.Send(new MyMessage("VolverADeportes"));
    }
}

