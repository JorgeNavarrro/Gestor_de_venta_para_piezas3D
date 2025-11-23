using Gestor_De_Ventas_Para_Piezas_3D.Modelos;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class OrderDetailPage : ContentPage
{
    // Constructor que RECIBE el objeto Pedido seleccionado
    public OrderDetailPage(Pedido pedidoSeleccionado)
    {
        InitializeComponent();

        // Llenamos los campos con los datos que recibimos
        if (pedidoSeleccionado != null)
        {
            txtId.Text = pedidoSeleccionado.Id.ToString();
            txtCliente.Text = pedidoSeleccionado.Cliente;
            txtProducto.Text = pedidoSeleccionado.Producto;
            txtFecha.Text = pedidoSeleccionado.FechaEntrega;
            // txtObservaciones.Text = pedidoSeleccionado.Observaciones; // (Si agregamos ese campo al modelo)
        }
    }

    private async void BtnVolver_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
        // Aquí iría el código para guardar en base de datos
        await DisplayAlert("Éxito", "¡Cambios guardados correctamente!", "Aceptar");
        await Navigation.PopAsync();
    }
}