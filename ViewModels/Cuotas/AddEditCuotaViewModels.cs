using ClubApp.Class;
using ClubApp.Models;
using ClubApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.ViewModels.Cuotas
{
    public class AddEditCuotaViewModels : ObservableObject
    {
        private readonly GenericService<Cuota> cuotaService = new GenericService<Cuota>();

        private Cuota cuota;
        public Cuota Cuota
        {
            get => cuota;
            set
            {
                cuota = value ?? new Cuota(); // Asigna una nueva cuota si el valor es null
                OnPropertyChanged();
            }
        }

        public AddEditCuotaViewModels()
        {
            Cuota = new Cuota(); // Asegura que la cuota esté inicializada al inicio
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        private async Task Guardar()
        {
            if (string.IsNullOrEmpty(Cuota.Nombre))
            {
                // Mostrar alerta de error
                return;
            }

            if (Cuota.Monto <= 0)
            {
                // Mostrar alerta de error
                return;
            }

            try
            {
                if (Cuota.Id == 0)
                {
                    await cuotaService.AddAsync(Cuota);
                }
                else
                {
                    await cuotaService.UpdateAsync(Cuota);
                }

                WeakReferenceMessenger.Default.Send(new MyMessage("VolverACuotas"));
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error al guardar la cuota: {ex.Message}");
            }
        }


        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverACuotas"));
        }
    }
}
