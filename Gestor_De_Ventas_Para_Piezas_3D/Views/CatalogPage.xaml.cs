using System.Collections.ObjectModel;
using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class CatalogPage : ContentPage
{
    // La lista de productos que se ve en la pantalla
    public ObservableCollection<ModeloReference> CatalogList { get; set; }

    public CatalogPage()
    {
        InitializeComponent();

        // 1. Inicializamos la lista vacía
        CatalogList = new ObservableCollection<ModeloReference>();

        // 3. Conectamos la vista con el código
        BindingContext = this;
    }

    // Se ejecuta automáticamente cada vez que la pantalla aparece
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Intentamos cargar lo que haya en la base de datos al entrar
        await CargarProductosDesdeBD();
    }

    // ✅ LÓGICA DEL BOTÓN NARANJA (Prueba y Datos Iniciales)
    // Lo cambiamos a 'public' para asegurar que el XAML lo encuentre
    public async void OnProbarConexionClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            button.IsEnabled = false;
            button.Text = "⏳ Configurando...";
        }

        var dbService = new DatabaseService();

        try
        {
            string mensajeCarga = await dbService.CargarDatosInicialesAsync();
            string mensajeEstado = await dbService.ProbarConexionAsync();

            await DisplayAlert("Resultado", $"{mensajeCarga}\n\n{mensajeEstado}", "OK");
            await CargarProductosDesdeBD();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error Inesperado", ex.Message, "OK");
        }
        finally
        {
            if (button != null)
            {
                button.IsEnabled = true;
                button.Text = "🔄 Probar Conexión";
            }
        }
    }

    // ✅ LÓGICA DEL BOTÓN VERDE (¡ESTA ES LA QUE FALTABA!)
    // IMPORTANTE: Cambiado a 'public' para corregir el error XC0002
    public async void OnAgregarNuevoClicked(object sender, EventArgs e)
    {
        // Navegar a la pantalla de agregar producto
        await Navigation.PushAsync(new AgregarProductoPage());
    }

    // Método auxiliar para leer de la BD y actualizar la interfaz
    private async Task CargarProductosDesdeBD()
    {
        var dbService = new DatabaseService();
        var productos = await dbService.ObtenerProductosAsync();

        CatalogList.Clear();
        foreach (var prod in productos)
        {
            CatalogList.Add(prod);
        }
    }
}