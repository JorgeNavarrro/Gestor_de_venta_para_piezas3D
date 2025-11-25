using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class InventoryItemFormPage : ContentPage
{
    private InventarioItem _item;

    // Nuevo
    public InventoryItemFormPage()
    {
        InitializeComponent();
        _item = new InventarioItem();
    }

    // Editar
    public InventoryItemFormPage(InventarioItem itemEditar)
    {
        InitializeComponent();
        _item = itemEditar;
        CargarDatos();
    }

    private void CargarDatos()
    {
        txtNombre.Text = _item.NombreArticulo;
        pkrCategoria.SelectedItem = _item.Categoria;
        txtDetalles.Text = _item.Detalles;
        txtStock.Text = _item.StockActual.ToString();
        pkrUnidad.SelectedItem = _item.Unidad;
        txtCosto.Text = _item.PrecioCosto.ToString();
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNombre.Text))
        {
            await DisplayAlert("Error", "El nombre es obligatorio", "OK");
            return;
        }

        _item.NombreArticulo = txtNombre.Text;
        _item.Categoria = pkrCategoria.SelectedItem?.ToString() ?? "Material";
        _item.Detalles = txtDetalles.Text;
        _item.Unidad = pkrUnidad.SelectedItem?.ToString() ?? "Piezas";

        if (int.TryParse(txtStock.Text, out int s)) _item.StockActual = s;
        if (decimal.TryParse(txtCosto.Text, out decimal c)) _item.PrecioCosto = c;

        var db = new DatabaseService();
        await db.GuardarItemInventarioAsync(_item);

        await Navigation.PopAsync();
    }
}