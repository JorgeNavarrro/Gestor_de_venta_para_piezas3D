using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class InventoryReportPage : ContentPage
{
    // Constructor que RECIBE la lista de items
    public InventoryReportPage(List<InventarioItem> items)
    {
        InitializeComponent();

        // Asignar la lista recibida a la tabla visual
        cvReporteInventario.ItemsSource = new ObservableCollection<InventarioItem>(items);

        // Actualizar la fecha y hora de generación
        lblFechaReporte.Text = $"Generado el: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}";
    }

    private async void BtnCerrar_Clicked(object sender, EventArgs e)
    {
        // Cierra la página de reporte y vuelve al inventario
        await Navigation.PopAsync();
    }
}