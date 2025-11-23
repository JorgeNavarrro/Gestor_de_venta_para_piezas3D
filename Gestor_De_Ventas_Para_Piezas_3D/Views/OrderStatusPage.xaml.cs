using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class OrderStatusPage : ContentPage
{
    public ObservableCollection<Pedido> ListaProduccion { get; set; }

    public OrderStatusPage()
    {
        InitializeComponent();
        CargarDatos();
    }

    private void CargarDatos()
    {
        // Datos de prueba para la tabla de producción
        var pedidos = new List<Pedido>
        {
            new Pedido { Id = 1, Cliente = "Ana Martinez", Producto = "Maceta", Estado = "Venta", FechaEntrega = "20/10/2025" },
            new Pedido { Id = 2, Cliente = "Luis Torres", Producto = "Llavero", Estado = "En producción", FechaEntrega = "22/10/2025" },
            new Pedido { Id = 3, Cliente = "Jorge Navarro", Producto = "Figura", Estado = "Entregado", FechaEntrega = "25/10/2025" },
            new Pedido { Id = 4, Cliente = "Sofia H.", Producto = "Engrane", Estado = "Completado", FechaEntrega = "27/10/2025" },
            new Pedido { Id = 5, Cliente = "Carlos R.", Producto = "Soporte", Estado = "Venta", FechaEntrega = "30/10/2025" }
        };

        ListaProduccion = new ObservableCollection<Pedido>(pedidos);
        cvProduccion.ItemsSource = ListaProduccion;
    }

    // Evento al tocar el número de ID: Muestra detalles
    private async void OnIdTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Pedido pedido)
        {
            await DisplayAlert($"Detalles Pedido #{pedido.Id:D2}",
                $"Cliente: {pedido.Cliente}\n" +
                $"Producto: {pedido.Producto}\n" +
                $"Fecha Entrega: {pedido.FechaEntrega}\n" +
                $"Estado Actual: {pedido.Estado}",
                "Cerrar");
        }
    }

    // Evento al tocar una celda de estado: Permite cambiar el estado
    private async void OnEstadoTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Pedido pedido)
        {
            string nuevoEstado = await DisplayActionSheet($"Mover Pedido {pedido.Id:D2} a:", "Cancelar", null,
                "Venta", "En producción", "Completado", "Entregado");

            if (!string.IsNullOrEmpty(nuevoEstado) && nuevoEstado != "Cancelar")
            {
                pedido.Estado = nuevoEstado;
                // Gracias a INotifyPropertyChanged en el modelo, la "X" se moverá sola en la pantalla
            }
        }
    }
}