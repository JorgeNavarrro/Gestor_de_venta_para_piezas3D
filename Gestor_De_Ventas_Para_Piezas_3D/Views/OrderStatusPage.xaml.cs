using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class OrderStatusPage : ContentPage
{
    public ObservableCollection<Venta> ListaProduccion { get; set; }

    public OrderStatusPage()
    {
        InitializeComponent();
        ListaProduccion = new ObservableCollection<Venta>();
        cvProduccion.ItemsSource = ListaProduccion;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatosDesdeBD();
    }

    private async Task CargarDatosDesdeBD()
    {
        var db = new DatabaseService();
        // Usamos ObtenerVentasAsync porque ahí están guardados los pedidos
        var ventas = await db.ObtenerVentasAsync();

        ListaProduccion.Clear();
        // Ordenamos por ID ascendente para que salga 01, 02, 03...
        foreach (var v in ventas.OrderBy(x => x.Id))
        {
            // Aseguramos que tenga un estado válido por defecto
            if (string.IsNullOrEmpty(v.Estado)) v.Estado = "Venta";
            ListaProduccion.Add(v);
        }
    }

    // Evento al tocar el ID: Muestra detalles
    private async void OnIdTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Venta venta)
        {
            await DisplayAlert($"Detalles Pedido #{venta.Id:D2}",
                $"Cliente: {venta.Cliente}\n" +
                $"Producto: {venta.Producto}\n" +
                $"Fecha Entrega: {venta.FechaEntrega}\n" +
                $"Estado Actual: {venta.Estado}\n\n" +
                $"Obs: {venta.Observaciones}",
                "Cerrar");
        }
    }

    // Evento al tocar una celda de color: Cambia el estado
    private async void OnEstadoTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Venta venta)
        {
            string nuevoEstado = await DisplayActionSheet($"Mover Pedido {venta.Id:D2} a:", "Cancelar", null,
                "Venta", "En producción", "Completado", "Entregado");

            if (!string.IsNullOrEmpty(nuevoEstado) && nuevoEstado != "Cancelar")
            {
                // 1. Actualizamos el objeto localmente
                venta.Estado = nuevoEstado;

                // 2. Guardamos en la Base de Datos
                var db = new DatabaseService();
                await db.GuardarVentaAsync(venta);

                // 3. Refrescamos la lista para que la "X" se mueva
                await CargarDatosDesdeBD();
            }
        }

    }

    //NUVEO
    // En Gestor_De_Ventas_Para_Piezas_3D/Views/OrderStatusPage.xaml.cs

    private async void BtnLimpiarEntregados_Clicked(object sender, EventArgs e)
    {
        // 1. Verificar si hay algo que borrar (opcional, pero buena práctica visual)
        var hayEntregados = ListaProduccion.Any(v => v.Estado == "Entregado");
        if (!hayEntregados)
        {
            await DisplayAlert("Aviso", "No hay pedidos marcados como 'Entregado' para eliminar.", "OK");
            return;
        }

        // 2. Pedir confirmación
        bool confirmar = await DisplayAlert("Confirmar Eliminación",
            "¿Estás seguro de que deseas eliminar TODOS los pedidos que ya han sido entregados? Esta acción no se puede deshacer.",
            "Sí, Eliminar",
            "Cancelar");

        if (confirmar)
        {
            var db = new DatabaseService();

            // 3. Llamar al método de borrado masivo
            int eliminados = await db.EliminarVentasEntregadasAsync();

            // 4. Mostrar confirmación y recargar la lista
            await DisplayAlert("Éxito", $"Se han eliminado {eliminados} pedidos del historial.", "OK");
            await CargarDatosDesdeBD();
        }
    }


}