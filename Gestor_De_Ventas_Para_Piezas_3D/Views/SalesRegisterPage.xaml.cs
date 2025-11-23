using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class SalesRegisterPage : ContentPage
{
    public ObservableCollection<Venta> ListaVentas { get; set; }
    private List<Venta> _todasLasVentas;

    // Almacenamos el ítem seleccionado para el botón Editar
    private Venta _selectedVenta;

    public SalesRegisterPage()
    {
        InitializeComponent();
        CargarDatos();
    }

    // Es buena práctica recargar datos cada vez que la página aparece (por si se edita algo en el formulario)
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // CargarDatos(); // Descomentar si implementamos una base de datos real
    }

    private void CargarDatos()
    {
        // Datos iniciales
        _todasLasVentas = new List<Venta>
        {
            // Nota: Aquí se usa un cliente diferente al empleado para demostrar el nuevo formato
            new Venta { Id = 1, NombreEmpleado = "Ana Martinez", Cliente = "Luis García", Telefono = "982131888", FechaSolicitud = "17/10/2025", FechaEntrega = "20/10/2025", Duracion = "3 días", Producto = "Maceta", Cantidad = 21, Observaciones = "Requiere color verde", Costo = 5000.00m },
            new Venta { Id = 2, NombreEmpleado = "Luis Torres", Cliente = "Sofia Hernández", Telefono = "9821317777", FechaSolicitud = "18/10/2025", FechaEntrega = "22/10/2025", Duracion = "4 días", Producto = "Llavero", Cantidad = 2, Observaciones = "Personalizado 'S'", Costo = 1200.00m },
            new Venta { Id = 3, NombreEmpleado = "Jorge Navarro", Cliente = "Mariana Díaz", Telefono = "9821316666", FechaSolicitud = "22/10/2025", FechaEntrega = "26/10/2025", Duracion = "4 días", Producto = "Figura", Cantidad = 54, Observaciones = "Ninguna", Costo = 1750.00m }
        };

        ListaVentas = new ObservableCollection<Venta>(_todasLasVentas);
        cvVentas.ItemsSource = ListaVentas;
    }

    // --- FUNCIONALIDAD DE BÚSQUEDA ---
    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var textoBusqueda = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(textoBusqueda))
        {
            cvVentas.ItemsSource = new ObservableCollection<Venta>(_todasLasVentas);
        }
        else
        {
            var resultados = _todasLasVentas
                .Where(v => v.NombreEmpleado.ToLower().Contains(textoBusqueda.ToLower()) ||
                            v.Cliente.ToLower().Contains(textoBusqueda.ToLower()) ||
                            v.Producto.ToLower().Contains(textoBusqueda.ToLower()) ||
                            v.Id.ToString().Contains(textoBusqueda))
                .ToList();

            cvVentas.ItemsSource = new ObservableCollection<Venta>(resultados);
        }
    }

    // Almacena el ítem seleccionado para el botón Editar
    private void OnVentaSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Venta ventaSeleccionada)
        {
            _selectedVenta = ventaSeleccionada;
        }
    }

    // --- FUNCIONALIDAD: REGISTRAR NUEVA VENTA (Abre formulario sin datos) ---
    private async void BtnRegistrarVenta_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SaleFormPage());
    }

    // --- FUNCIONALIDAD: EDITAR SOLICITUD (Abre formulario con datos) ---
    private async void BtnEditarSolicitud_Clicked(object sender, EventArgs e)
    {
        if (_selectedVenta == null)
        {
            await DisplayAlert("Advertencia", "Por favor, selecciona una venta de la lista para editar.", "OK");
            return;
        }

        // Abre el formulario en modo Edición, pasando el objeto seleccionado
        await Navigation.PushAsync(new SaleFormPage(_selectedVenta));

        // Limpiamos la selección después de la acción
        cvVentas.SelectedItem = null;
        _selectedVenta = null;
    }
}