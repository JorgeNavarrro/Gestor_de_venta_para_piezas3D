using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services; // Necesario para la BD
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class SalesRegisterPage : ContentPage
{
    public ObservableCollection<Venta> ListaVentas { get; set; }

    // Almacenamos el ítem seleccionado para el botón Editar
    private Venta _selectedVenta;

    public SalesRegisterPage()
    {
        InitializeComponent();

        // Inicializamos la colección vacía
        ListaVentas = new ObservableCollection<Venta>();

        // Conectamos la vista
        BindingContext = this;
        cvVentas.ItemsSource = ListaVentas;
    }

    // Recargamos los datos cada vez que la pantalla aparece
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatosDesdeBD();
    }

    private async Task CargarDatosDesdeBD()
    {
        var db = new DatabaseService();
        var ventas = await db.ObtenerVentasAsync();

        ListaVentas.Clear();
        foreach (var venta in ventas)
        {
            ListaVentas.Add(venta);
        }
    }

    // --- FUNCIONALIDAD DE BÚSQUEDA ---
    private async void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var textoBusqueda = e.NewTextValue;
        var db = new DatabaseService();
        var todas = await db.ObtenerVentasAsync();

        if (string.IsNullOrWhiteSpace(textoBusqueda))
        {
            // Si está vacío, mostramos todas (recargando desde la lista local 'todas')
            ListaVentas.Clear();
            foreach (var v in todas) ListaVentas.Add(v);
        }
        else
        {
            // Filtramos
            var resultados = todas
                .Where(v => (v.NombreEmpleado?.ToLower().Contains(textoBusqueda.ToLower()) ?? false) ||
                            (v.Cliente?.ToLower().Contains(textoBusqueda.ToLower()) ?? false) ||
                            (v.Producto?.ToLower().Contains(textoBusqueda.ToLower()) ?? false) ||
                            v.Id.ToString().Contains(textoBusqueda))
                .ToList();

            ListaVentas.Clear();
            foreach (var r in resultados) ListaVentas.Add(r);
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

    // --- REGISTRAR NUEVA VENTA ---
    private async void BtnRegistrarVenta_Clicked(object sender, EventArgs e)
    {
        // Vamos al formulario vacío
        await Navigation.PushAsync(new SaleFormPage());
    }

    // --- EDITAR SOLICITUD ---
    private async void BtnEditarSolicitud_Clicked(object sender, EventArgs e)
    {
        if (_selectedVenta == null)
        {
            await DisplayAlert("Advertencia", "Por favor, selecciona una venta de la lista para editar.", "OK");
            return;
        }

        // Vamos al formulario pasando la venta seleccionada
        await Navigation.PushAsync(new SaleFormPage(_selectedVenta));

        // Limpiamos la selección
        cvVentas.SelectedItem = null;
        _selectedVenta = null;
    }
}