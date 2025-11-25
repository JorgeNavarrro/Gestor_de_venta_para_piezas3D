using System;
using Microsoft.Maui.Controls;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class MenuPage : ContentPage
{
    public MenuPage()
    {
        InitializeComponent();
    }

    // ✅ CORREGIDO: Botón Cerrar Sesión
    // Usamos Application.Current.MainPage para reiniciar la app en el Login
    private void BtnCerrarSesion_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }

    // --- TUS NAVEGACIONES ORIGINALES (INTACTAS) ---

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
    private async void OnEstadoPedidoTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrderStatusPage());
    }

    // Opción 6 (DERECHA): Verificar / Solicitar inventario
    private async void OnInventarioTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new InventoryPage());
    }

    // Opción 7 (INFERIOR CENTRAL): Catálogo de Referencias
    private async void OnCatalogoTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new CatalogPage());
    }
}