using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class InventoryPage : ContentPage
{
    public ObservableCollection<InventarioItem> ListaInventario { get; set; }

    // Para pasar al reporte
    private List<InventarioItem> _todosLosItems;

    public InventoryPage()
    {
        InitializeComponent();
        ListaInventario = new ObservableCollection<InventarioItem>();
        cvInventario.ItemsSource = ListaInventario;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatosBD();
    }

    private async Task CargarDatosBD()
    {
        var db = new DatabaseService();
        _todosLosItems = await db.ObtenerInventarioAsync();

        // Si está vacío, metemos datos de ejemplo para que no se vea triste
        if (_todosLosItems.Count == 0)
        {
            await db.GuardarItemInventarioAsync(new InventarioItem { NombreArticulo = "Filamento PLA", Categoria = "Material", Detalles = "Rojo", StockActual = 12, Unidad = "Rollos", PrecioCosto = 450 });
            await db.GuardarItemInventarioAsync(new InventarioItem { NombreArticulo = "Filamento PETG", Categoria = "Material", Detalles = "Negro", StockActual = 3, Unidad = "Rollos", PrecioCosto = 520 });
            _todosLosItems = await db.ObtenerInventarioAsync();
        }

        ListaInventario.Clear();
        foreach (var item in _todosLosItems)
        {
            ListaInventario.Add(item);
        }
    }

    // Ir a formulario para AGREGAR
    private async void BtnAgregar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryItemFormPage());
    }

    // Ir al REPORTE
    private async void BtnGenerarReporte_Clicked(object sender, EventArgs e)
    {
        // Pasamos la lista actual al reporte
        await Navigation.PushAsync(new InventoryReportPage(_todosLosItems));
    }

    // BORRAR ITEM
    private async void BtnBorrar_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button; // O ImageButton según uses
        if (button?.CommandParameter is InventarioItem itemABorrar)
        {
            bool confirmar = await DisplayAlert("Eliminar", $"¿Seguro que deseas eliminar '{itemABorrar.NombreArticulo}'?", "Sí", "No");
            if (confirmar)
            {
                var db = new DatabaseService();
                await db.BorrarItemInventarioAsync(itemABorrar);
                await CargarDatosBD(); // Recargar lista
            }
        }
    }

    // EDITAR (Al tocar la fila)
    private async void OnItemSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is InventarioItem item)
        {
            // Navegar al formulario en modo edición
            await Navigation.PushAsync(new InventoryItemFormPage(item));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}