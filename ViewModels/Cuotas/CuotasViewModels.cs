using ClubApp.Class;
using ClubApp.Models;
using ClubApp.Services;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.ViewModels.Cuotas
{
    public class CuotasViewModels : NotificationObject
    {
        GenericService<Cuota> cuotaService = new GenericService<Cuota>();

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

        private ObservableCollection<Cuota> cuotas;
        public ObservableCollection<Cuota> Cuotas
        {
            get { return cuotas; }
            set
            {
                cuotas = value;
                OnPropertyChanged();
            }
        }


        private Cuota cuotaCurrent;
        public Cuota CuotaCurrent
        {
            get { return cuotaCurrent; }
            set
            {
                cuotaCurrent = value;
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

        public CuotasViewModels()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerCuotas());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverACuotas")
            {
                await RefreshCuotas(this);
            }
        }

        private async Task RefreshCuotas(object obj)
        {
            IsRefreshing = true;
            await ObtenerCuotas();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return CuotaCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Eliminar la cuota",
                $"¿Está seguro que desea eliminar la cuota {CuotaCurrent.Nombre}?",
                "Sí",
                "No"
            );
            if (respuesta)
            {
                ActivityStart = true;
                await cuotaService.DeleteAsync(CuotaCurrent.Id);
                await ObtenerCuotas();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return CuotaCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditCuotaView") { Cuota = CuotaCurrent });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditCuotaView"));
        }

        public async Task ObtenerCuotas()
        {
            ActivityStart = true;
            var cuotas = await cuotaService.GetAllAsync();
            Cuotas = new ObservableCollection<Cuota>(cuotas);
            ActivityStart = false;
        }
    }
}
