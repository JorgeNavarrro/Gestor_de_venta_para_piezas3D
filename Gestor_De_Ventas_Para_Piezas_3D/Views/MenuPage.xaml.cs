using System;
using Microsoft.Maui.Controls;
// Necesario para poder usar DatabaseService en la exportación
using Gestor_De_Ventas_Para_Piezas_3D.Services;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class MenuPage : ContentPage
{
    public MenuPage()
    {
        InitializeComponent();
    }

    // ✅ 1. MÉTODO EXPORTAR BD (Faltaba este, por eso el error XC0002)
    public void BtnExportarBD_Clicked(object sender, EventArgs e)
    {
        var db = new DatabaseService();
        // Llamada síncrona al método de exportación
        string resultado = db.ExportarBaseDeDatos();

        DisplayAlert("Exportación de BD", resultado, "OK");
    }

    // ✅ 2. MÉTODO CERRAR SESIÓN (Corregido para reiniciar la app en Login)
    private void BtnCerrarSesion_Clicked(object sender, EventArgs e)
    {
        // Al cambiar MainPage, borramos el historial de navegación y volvemos al inicio
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }

    // --- 3. TUS NAVEGACIONES ORIGINALES (Las mantenemos intactas) ---

    private async void OnRegistrarVentaTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SalesRegisterPage());
    }

    private async void OnAgendaPedidosTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrderAgendaPage());
    }

    private async void OnPagosTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PaymentsPage());
    }

    private async void OnGenerarOrdenesTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ShippingOrdersPage());
    }

    private async void OnEstadoPedidoTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrderStatusPage());
    }

    private async void OnInventarioTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new InventoryPage());
    }

    private async void OnCatalogoTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new CatalogPage());
    }
}