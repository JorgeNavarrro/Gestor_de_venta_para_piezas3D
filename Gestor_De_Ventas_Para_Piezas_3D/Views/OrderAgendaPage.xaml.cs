using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class OrderAgendaPage : ContentPage
{
    public ObservableCollection<Venta> ListaPedidos { get; set; }

    public OrderAgendaPage()
    {
        InitializeComponent();
        ListaPedidos = new ObservableCollection<Venta>();
        cvPedidos.ItemsSource = ListaPedidos;
    }

    // Cargar datos cada vez que aparece la pantalla
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatosDesdeBD();
    }

    private async Task CargarDatosDesdeBD()
    {
        var db = new DatabaseService();
        var ventas = await db.ObtenerVentasAsync();

        ListaPedidos.Clear();
        foreach (var venta in ventas)
        {
            // Si el estado está vacío (ventas viejas), le ponemos "Pendiente" visualmente
            if (string.IsNullOrEmpty(venta.Estado)) venta.Estado = "Pendiente";

            ListaPedidos.Add(venta);
        }
    }

    // --- BÚSQUEDA ---
    private async void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var texto = e.NewTextValue;
        var db = new DatabaseService();
        var todas = await db.ObtenerVentasAsync();

        if (string.IsNullOrWhiteSpace(texto))
        {
            ListaPedidos.Clear();
            foreach (var v in todas) ListaPedidos.Add(v);
        }
        else
        {
            var filtradas = todas.Where(v =>
                (v.Cliente?.ToLower().Contains(texto.ToLower()) ?? false) ||
                (v.Producto?.ToLower().Contains(texto.ToLower()) ?? false)
            ).ToList();

            ListaPedidos.Clear();
            foreach (var v in filtradas) ListaPedidos.Add(v);
        }
    }

    // --- SELECCIÓN (EDITAR) ---
    private async void OnPedidoSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Venta ventaSeleccionada)
        {
            // Navegar al formulario de edición con los datos de la venta
            await Navigation.PushAsync(new SaleFormPage(ventaSeleccionada));

            // Limpiar selección
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}