using ClubApp.Class;
using ClubApp.Models;
using ClubApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace ClubApp.ViewModels.Socios
{
    public class AddEditSocioViewModel : ObservableObject
    {
        private readonly GenericService<Socio> socioService = new GenericService<Socio>();
        private readonly GenericService<Cuota> cuotaService = new GenericService<Cuota>();

        private Socio socio;
        private Cuota cuota;

        public Socio Socio
        {
            get => socio;
            set
            {
                socio = value ?? new Socio();
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

        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        public AddEditSocioViewModel()
        {
            Socio = new Socio();
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las cuotas: {ex.Message}");
            }
        }

        private async Task Guardar()
        {
            if (Socio == null)
            {
                Socio = new Socio();
            }

            // Validar campos obligatorios
            var validationErrors = ValidateFields();
            if (validationErrors.Any())
            {
                // Mostrar alertas de error
                await Application.Current.MainPage.DisplayAlert("Error", string.Join("\n", validationErrors), "OK");
                return;
            }

            try
            {
                // Asignar la cuota seleccionada al socio
                Socio.CuotaId = Cuota?.Id ?? 0;

                if (Socio.Id == 0)
                {
                    // Agregar nuevo socio
                    await socioService.AddAsync(Socio);
                }
                else
                {
                    // Editar socio existente
                    await socioService.UpdateAsync(Socio);
                }

                WeakReferenceMessenger.Default.Send(new MyMessage("VolverASocios"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar socio: {ex.Message}");
            }
        }

        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverASocios"));
        }

        private List<string> ValidateFields()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Socio.Nombre))
                errors.Add("El campo Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(Socio.Apellido))
                errors.Add("El campo Apellido es obligatorio.");
            if (!string.IsNullOrWhiteSpace(Socio.Email) && !new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(Socio.Email))
                errors.Add("El campo Email no es válido.");
            if (!string.IsNullOrWhiteSpace(Socio.Telefono) && !new System.ComponentModel.DataAnnotations.PhoneAttribute().IsValid(Socio.Telefono))
                errors.Add("El campo Teléfono no es válido.");
            if (Cuota == null || Cuota.Id == 0)
                errors.Add("Debe seleccionar una Cuota.");

            return errors;
        }
    }
}
