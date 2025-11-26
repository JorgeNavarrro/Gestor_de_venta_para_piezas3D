using System.Collections.ObjectModel;
using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class CatalogPage : ContentPage
{
    // Colección observable para la lista de productos
    public ObservableCollection<ModeloReference> CatalogList { get; set; }

    public CatalogPage()
    {
        InitializeComponent();
        CatalogList = new ObservableCollection<ModeloReference>();
        BindingContext = this;
    }

    // Cargar productos cada vez que aparece la página
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarProductosDesdeBD();
    }

    // Botón Verde: Agregar Nuevo Producto
    public async void OnAgregarNuevoClicked(object sender, EventArgs e)
    {
        // Asegúrate de que AgregarProductoPage exista en el namespace Vistas
        await Navigation.PushAsync(new AgregarProductoPage());
    }

    // Botón Borrar (X)
    private async void BtnBorrar_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        // Verificamos que el CommandParameter sea un producto válido
        if (button?.CommandParameter is ModeloReference productoABorrar)
        {
            bool confirmar = await DisplayAlert("Eliminar Producto",
                $"¿Estás seguro de que deseas eliminar '{productoABorrar.Nombre}' del catálogo?",
                "Sí, Eliminar", "Cancelar");

            if (confirmar)
            {
                var dbService = new DatabaseService();
                await dbService.BorrarProductoAsync(productoABorrar);

                // Refrescamos la lista para que desaparezca el elemento borrado
                await CargarProductosDesdeBD();
            }
        }
    }

    // Carga los productos desde la base de datos SQLite
    private async Task CargarProductosDesdeBD()
    {
        var dbService = new DatabaseService();

        // Aseguramos que la BD esté lista y con datos iniciales si es nueva
        await dbService.InicializarBaseDeDatosAsync();

        // Opcional: Solo carga datos iniciales si la base está vacía
        // await dbService.CargarDatosInicialesAsync(); 

        var productos = await dbService.ObtenerProductosAsync();

        CatalogList.Clear();
        foreach (var prod in productos)
        {
            CatalogList.Add(prod);
        }
    }
}