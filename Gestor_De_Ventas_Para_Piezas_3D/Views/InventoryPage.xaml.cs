using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class InventoryPage : ContentPage
{
    public ObservableCollection<InventarioItem> ListaInventario { get; set; }
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

        // Aseguramos que la BD esté creada
        await db.InicializarBaseDeDatosAsync();

        _todosLosItems = await db.ObtenerInventarioAsync();

        // Si no hay nada, ponemos un ejemplo para probar la alerta de stock
        if (_todosLosItems.Count == 0)
        {
            // Ejemplo de BAJO STOCK (<50)
            await db.GuardarItemInventarioAsync(new InventarioItem { NombreArticulo = "Filamento PLA", Categoria = "Material", Detalles = "Rojo", StockActual = 10, Unidad = "Rollos", PrecioCosto = 450 });

            // Ejemplo de EXCESO (>80)
            await db.GuardarItemInventarioAsync(new InventarioItem { NombreArticulo = "Tornillos M3", Categoria = "Refacción", Detalles = "Acero", StockActual = 200, Unidad = "Piezas", PrecioCosto = 2 });

            // Ejemplo NORMAL
            await db.GuardarItemInventarioAsync(new InventarioItem { NombreArticulo = "Resina Gris", Categoria = "Material", Detalles = "Standard", StockActual = 60, Unidad = "Litros", PrecioCosto = 800 });

            _todosLosItems = await db.ObtenerInventarioAsync();
        }

        ListaInventario.Clear();
        bool alertaBajoStock = false;

        foreach (var item in _todosLosItems)
        {
            ListaInventario.Add(item);
            if (item.EsBajoStock)
            {
                alertaBajoStock = true;
            }
        }

        if (alertaBajoStock)
        {
            // await DisplayAlert("Alerta de Stock", "Hay productos con bajo stock (< 50). Por favor revise el inventario.", "OK");
        }
    }

    // Botón "+ Nuevo" (Navega al formulario de agregar)
    private async void BtnAgregar_Clicked(object sender, EventArgs e)
    {
        // AQUÍ ES DONDE SE USA LA CLASE QUE CREASTE ANTES
        await Navigation.PushAsync(new InventoryItemFormPage());
    }

    // Botón "Generar Reporte"
    private async void BtnGenerarReporte_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryReportPage(_todosLosItems));
    }

    // Botón Borrar ("X")
    private async void BtnBorrar_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button?.CommandParameter is InventarioItem itemABorrar)
        {
            bool confirmar = await DisplayAlert("Eliminar", $"¿Eliminar '{itemABorrar.NombreArticulo}'?", "Sí", "No");
            if (confirmar)
            {
                var db = new DatabaseService();
                await db.BorrarItemInventarioAsync(itemABorrar);
                await CargarDatosBD();
            }
        }
    }

    // Al tocar un ítem de la lista -> Editar
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