using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class InventoryPage : ContentPage
{
    // Lista observable conectada al XAML
    public ObservableCollection<InventarioItem> ListaInventario { get; set; }
    // Almacena la lista completa para poder pasarla al reporte
    private List<InventarioItem> _todosLosItems;

    public InventoryPage()
    {
        InitializeComponent();
        CargarDatos();
    }

    private void CargarDatos()
    {
        // Datos de prueba basados en el PDF
        _todosLosItems = new List<InventarioItem>
        {
            new InventarioItem { Id = 1, NombreArticulo = "Filamento PLA Premium", Categoria = "Material", Detalles = "PLA / Rojo", StockActual = 12, Unidad = "Rollos (1kg)", Estado = "En Stock", PrecioCosto = 450.00m, TipoPrecio = "Costo" },
            new InventarioItem { Id = 2, NombreArticulo = "Filamento PETG", Categoria = "Material", Detalles = "PETG / Negro", StockActual = 3, Unidad = "Rollos (1kg)", Estado = "Bajo Stock", PrecioCosto = 520.00m, TipoPrecio = "Costo" },
            new InventarioItem { Id = 3, NombreArticulo = "Resina UV Estándar", Categoria = "Material", Detalles = "Resina / Gris", StockActual = 5, Unidad = "Botellas (1L)", Estado = "En Stock", PrecioCosto = 800.00m, TipoPrecio = "Costo" },
            new InventarioItem { Id = 4, NombreArticulo = "Baby Yoda (Grogu)", Categoria = "Producto Terminado", Detalles = "PLA / Verde (Pintado)", StockActual = 8, Unidad = "Piezas", Estado = "En Stock", PrecioCosto = 250.00m, TipoPrecio = "Venta" },
            new InventarioItem { Id = 5, NombreArticulo = "Stormtrooper Casco", Categoria = "Producto Terminado", Detalles = "Resina / Blanco", StockActual = 0, Unidad = "Piezas", Estado = "Agotado", PrecioCosto = 600.00m, TipoPrecio = "Venta" },
            new InventarioItem { Id = 6, NombreArticulo = "Boquilla 0.4mm (MK8)", Categoria = "Refacción", Detalles = "Latón", StockActual = 25, Unidad = "Piezas", Estado = "En Stock", PrecioCosto = 30.00m, TipoPrecio = "Costo" }
        };

        ListaInventario = new ObservableCollection<InventarioItem>(_todosLosItems);
        cvInventario.ItemsSource = ListaInventario;
    }

    // Maneja la selección de un ítem en la tabla principal
    private async void OnItemSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is InventarioItem item)
        {
            await DisplayAlert("Detalle", $"Artículo seleccionado: {item.NombreArticulo}.\nStock: {item.StockActual} {item.Unidad}", "OK");
            // Deseleccionar
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    // --- LÓGICA DE REPORTE (Opción B) ---
    // Abre la nueva página de reporte, enviando los datos (POO)
    private async void BtnGenerarReporte_Clicked(object sender, EventArgs e)
    {
        // Navegamos a la página de reporte y le pasamos la lista completa de inventario.
        // Asegúrate de que la clase InventoryReportPage exista.
        await Navigation.PushAsync(new InventoryReportPage(_todosLosItems));
    }
}