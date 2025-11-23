using Gestor_De_Ventas_Para_Piezas_3D.Modelos;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class GenerateNotePage : ContentPage
{
	public GenerateNotePage()
	{
		InitializeComponent();
        
        // Datos automáticos
        lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
        // Simular siguiente ID
        int nextId = PaymentRepository.Pagos.Count + 1;
        lblNoNota.Text = $"NOTA-{nextId:D6}";
	}

    private async void BtnGenerar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtCliente.Text) || string.IsNullOrEmpty(txtMonto.Text))
        {
            await DisplayAlert("Error", "Por favor ingrese Cliente y Monto", "OK");
            return;
        }

        // 1. Crear el nuevo pago
        var nuevoPago = new Pago
        {
            Id = PaymentRepository.Pagos.Count + 1,
            IdPedido = new Random().Next(1000, 9999), // Simulado
            NombreCliente = txtCliente.Text,
            TelefonoCliente = txtTelefono.Text,
            Productos = "Venta Mostrador", // Valor por defecto
            MedioPago = pkrMetodo.SelectedItem?.ToString() ?? "Efectivo",
            EstadoPago = "Pagado",
            CostoTotal = decimal.TryParse(txtMonto.Text, out decimal m) ? m : 0,
            FechaEmision = lblFecha.Text,
            Observaciones = txtObservaciones.Text
        };

        // 2. Guardar en el repositorio compartido
        PaymentRepository.AgregarPago(nuevoPago);

        // 3. Avisar y Salir
        await DisplayAlert("Éxito", "Nota de venta generada y guardada en el historial.", "Aceptar");
        await Navigation.PopAsync();
    }
}