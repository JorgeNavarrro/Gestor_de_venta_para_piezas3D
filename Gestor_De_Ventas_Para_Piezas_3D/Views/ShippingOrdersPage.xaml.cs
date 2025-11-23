using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class ShippingOrdersPage : ContentPage
{
    public ObservableCollection<Pedido> ListaPedidos { get; set; }

    public ShippingOrdersPage()
    {
        InitializeComponent();
        CargarDatos();
    }

    private void CargarDatos()
    {
        // Simulamos la lista de todos los pedidos
        var todosLosPedidos = new List<Pedido>
        {
            // Pedidos de ejemplo del PDF
            new Pedido { Id = 1, Cliente = "Ana Martinez", Producto = "Maceta", FechaEntrega = "20/10/2025", Estado = "En producción", FechaInicio = "17/10/2025", Duracion = "" },
            new Pedido { Id = 2, Cliente = "Luis Torres", Producto = "Llavero", FechaEntrega = "22/10/2025", Estado = "Pendiente", FechaInicio = "18/10/2025", Duracion = "" },
            new Pedido { Id = 3, Cliente = "Jorge Navarro", Producto = "Figura", FechaEntrega = "25/10/2025", Estado = "Completado", FechaInicio = "22/10/2025", Duracion = "3 días" },
            // Agregamos más para simular pedidos listos
            new Pedido { Id = 4, Cliente = "Carlos Ruiz", Producto = "Logo", FechaEntrega = "27/10/2025", Estado = "Completado", FechaInicio = "20/10/2025", Duracion = "7 días" },
            new Pedido { Id = 5, Cliente = "Marta Sol", Producto = "Engrane", FechaEntrega = "30/10/2025", Estado = "En producción", FechaInicio = "25/10/2025", Duracion = "" }
        };

        // Filtramos para mostrar solo los que están "Completado" o "Pendiente"
        var pedidosFinalizados = todosLosPedidos
            .Where(p => p.Estado == "Completado" || p.Estado == "Pendiente")
            .ToList();

        ListaPedidos = new ObservableCollection<Pedido>(pedidosFinalizados);
        cvPedidosFinalizados.ItemsSource = ListaPedidos;
    }

    private async void BtnImprimir_Clicked(object sender, EventArgs e)
    {
        // En una aplicación real, aquí se llamaría a un servicio de impresión o PDF

        // Obtenemos el pedido de la fila donde se hizo clic
        var button = sender as Button;
        if (button?.BindingContext is Pedido pedido)
        {
            await DisplayAlert("Impresión", $"Generando orden de envío para el pedido ID {pedido.Id} de {pedido.Cliente}.", "Aceptar");
        }
    }
}