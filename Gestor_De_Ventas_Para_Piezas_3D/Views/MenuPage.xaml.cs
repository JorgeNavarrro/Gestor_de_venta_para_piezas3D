namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class MenuPage : ContentPage
{
    public MenuPage()
    {
        InitializeComponent();
    }

    // Botón Cerrar Sesión (Vuelve al Login)
    private async void BtnCerrarSesion_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // Opción 1: Registrar nueva venta
    private async void OnRegistrarVentaTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SalesRegisterPage());
    }

    // Opción 2: Actualizar agenda de pedidos
    private async void OnAgendaPedidosTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrderAgendaPage());
    }

    // Opción 3: Registrar pago de pedido
    private async void OnPagosTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PaymentsPage());
    }

    // Opción 4: Generar órdenes de envío
    private async void OnGenerarOrdenesTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ShippingOrdersPage());
    }

    // Opción 5 (CENTRAL): Consultar Estado de Pedido / Producción
    // Este conecta con la nueva pantalla OrderStatusPage
    private async void OnEstadoPedidoTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrderStatusPage());
    }

    // Opción 6 (DERECHA): Verificar / Solicitar inventario
    private async void OnInventarioTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new InventoryPage());
    }

    // Opción 7 (INFERIOR CENTRAL): Catálogo de Referencias (NUEVO)
    private async void OnCatalogoTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new CatalogPage());
    }
}