using ClubApp.Class;
using ClubApp.Models;
using ClubApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.ViewModels.Deportistas
{
     public class AddEditDeportistaViewModel : ObservableObject 
    {
        private readonly GenericService<Deportista> deportistaService = new GenericService<Deportista>();
        private readonly GenericService<Cuota> cuotaService = new GenericService<Cuota>();
        private readonly GenericService<Deporte> deporteService = new GenericService<Deporte>();

        private Deportista deportista;
        private Cuota cuota;
        private Deporte deporte;

        public  Deportista Deportista
		{
			get => deportista;
			set {
				deportista = value ?? new Deportista();
                OnPropertyChanged();
            }
		}
        public Cuota Cuota
        {
            get => cuota;
            set
            {
                cuota = value;
                OnPropertyChanged();
            }
        }

        public Deporte Deporte
        {
            get => deporte;
            set
            {
                deporte = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Cuota> cuotas;
        public ObservableCollection<Cuota> Cuotas
        {
            get => cuotas;
            set
            {
                cuotas = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Deporte> deportes;
        public ObservableCollection<Deporte> Deportes
        {
            get => deportes;
            set
            {
                deportes = value;
                OnPropertyChanged();
            }
        }

        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        public AddEditDeportistaViewModel()
        {
            Deportista = new Deportista();
            Deporte = new Deporte();
            Cuota = new Cuota();
            ObtenerListas();
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }
        private async Task ObtenerListas()
        {
            try
            {
                var cuotas = await cuotaService.GetAllAsync();
                Cuotas = new ObservableCollection<Cuota>(cuotas);
                var deportes = await deporteService.GetAllAsync();
                Deportes = new ObservableCollection<Deporte>(deportes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las cuotas: {ex.Message}");
                Console.WriteLine($"Error al obtener los deportes: {ex.Message}");
            }
        }


        private async Task Guardar()
        {
            if (Deportista == null)
            {
                Deportista = new Deportista();
            }
            try
            {
                //Asignar la cuota y deporte seleccionado al deportista
                Deportista.CuotaId = Cuota?.Id ?? 0;
                Deportista.DeporteId = Deporte?.Id ?? 0;

                if (Deportista.Id == 0)
                {
                    await deportistaService.AddAsync(Deportista);
                }
                else
                {
                    await deportistaService.UpdateAsync(Deportista);
                }
                
                WeakReferenceMessenger.Default.Send(new MyMessage("VolverADeportistas"));
            }
            catch (Exception ex)
            {
                // Manejo de errores, puedes también mostrar un mensaje al usuario si es necesario
                Console.WriteLine($"Error al guardar deportista: {ex.Message}");
            }
        }

        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverADeportistas"));
        }

    }
}
