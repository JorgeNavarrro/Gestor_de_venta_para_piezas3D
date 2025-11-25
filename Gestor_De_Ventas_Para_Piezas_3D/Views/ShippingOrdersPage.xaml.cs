using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class ShippingOrdersPage : ContentPage
{
    public ObservableCollection<Venta> ListaEnvios { get; set; }

    public ShippingOrdersPage()
    {
        InitializeComponent();
        ListaEnvios = new ObservableCollection<Venta>();
        cvEnvios.ItemsSource = ListaEnvios;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarPedidosCompletados();
    }

    private async Task CargarPedidosCompletados()
    {
        var db = new DatabaseService();
        var ventas = await db.ObtenerVentasAsync();

        ListaEnvios.Clear();

        // FILTRO: Solo mostramos los que están "Completado"
        // Puedes agregar "|| v.Estado == 'Entregado'" si también quieres ver los históricos
        var completados = ventas.Where(v => v.Estado == "Completado").OrderBy(v => v.Id);

        foreach (var venta in completados)
        {
            ListaEnvios.Add(venta);
        }

        if (ListaEnvios.Count == 0)
        {
            // Opcional: Mostrar mensaje si no hay nada listo para envío
            // await DisplayAlert("Info", "No hay pedidos completados listos para envío.", "OK");
        }
    }

    private async void BtnImprimir_Clicked(object sender, EventArgs e)
    {
        // Obtenemos el objeto Venta del botón presionado
        var button = sender as Button;
        if (button?.CommandParameter is Venta venta)
        {
            // Simulación de impresión
            bool respuesta = await DisplayAlert("Imprimir Orden",
                $"¿Deseas generar la orden de envío para el pedido #{venta.Id} de {venta.Cliente}?",
                "Sí, Imprimir", "Cancelar");

            if (respuesta)
            {
                await DisplayAlert("Éxito", "Orden de envío generada y enviada a la impresora.", "OK");

                // Opcional: Cambiar estado a "Entregado" o "Enviado" automáticamente
                // venta.Estado = "Entregado";
                // var db = new DatabaseService();
                // await db.GuardarVentaAsync(venta);
                // await CargarPedidosCompletados(); // Refrescar lista
            }
        }
    }
}