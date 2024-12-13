using ClubApp.Class;
using ClubApp.Models;
using ClubApp.Services;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.ViewModels.Deportes
{
    public class DeportesViewModel : NotificationObject
    {
        GenericService<Deporte> deporteService = new GenericService<Deporte>();

        private bool activityStart;

    public bool ActivityStart
    {
        get { return activityStart; }
        set
        {
            activityStart = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Deporte> deportes;

    public ObservableCollection<Deporte> Deportes
    {
        get { return deportes; }
        set
        {
            deportes = value;
            OnPropertyChanged();
        }
    }

    private Deporte deporteCurrent;

    public Deporte DeporteCurrent
    {
        get { return deporteCurrent; }
        set
        {
            deporteCurrent = value;
            OnPropertyChanged();
            EditarCommand.ChangeCanExecute();
            EliminarCommand.ChangeCanExecute();
        }
    }

    private bool isRefreshing;

    public bool IsRefreshing
    {
        get => isRefreshing;
        set
        {
            isRefreshing = value;
            OnPropertyChanged();
        }
    }

    public Command AgregarCommand { get; }
    public Command EditarCommand { get; }
    public Command EliminarCommand { get; }

    public DeportesViewModel()
    {
        AgregarCommand = new Command(Agregar);
        EditarCommand = new Command(Editar, PermitirEditar);
        EliminarCommand = new Command(Eliminar, PermitirEliminar);

        Task.Run(async () => await ObtenerDeportes());

        WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
        {
            AlRecibirMensaje(m);
        });
    }

    private async void AlRecibirMensaje(MyMessage m)
    {
        if (m.Value == "VolverADeportes")
        {
            await RefreshDeportistas(this);
        }
    }

    private async Task RefreshDeportistas(object obj)
    {
        IsRefreshing = true;
        await ObtenerDeportes();
        IsRefreshing = false;
    }

    private bool PermitirEliminar(object arg)
    {
        return DeporteCurrent != null;
    }

    private async void Eliminar(object obj)
    {
        bool respuesta = await Application.Current.MainPage.DisplayAlert("Eliminar", $"¿Estás seguro de eliminar este deporte {DeporteCurrent.Nombre}?", "Si", "No");
        if (respuesta)
        {
            ActivityStart = true;
            await deporteService.DeleteAsync(DeporteCurrent.Id);
            await ObtenerDeportes();

        }
    }

    private bool PermitirEditar(object arg)
    {
        return DeporteCurrent != null;
    }

    private void Editar(object obj)
    {
        WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditDeporteView") { Deporte = DeporteCurrent });
    }

    private void Agregar(object obj)
    {
        WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditDeporteView"));
    }

    public async Task ObtenerDeportes()
    {
        ActivityStart = true;
        var deportes = await deporteService.GetAllAsync();
        Deportes = new ObservableCollection<Deporte>(deportes);
        ActivityStart = false;




    }
}
}
