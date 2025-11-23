using Gestor_De_Ventas_Para_Piezas_3D.Modelos;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class SalesNotePage : ContentPage
{
    private Pago _pagoActual;

    // Constructor que recibe el Pago seleccionado
    public SalesNotePage(Pago pagoSeleccionado)
    {
        InitializeComponent();
        _pagoActual = pagoSeleccionado;
        CargarDatos();
    }

    private void CargarDatos()
    {
        if (_pagoActual != null)
        {
            lblNoNota.Text = _pagoActual.NumeroNota;
            lblFecha.Text = _pagoActual.FechaEmision;
            lblCliente.Text = _pagoActual.NombreCliente;
            lblTelefono.Text = _pagoActual.TelefonoCliente;
            lblMonto.Text = _pagoActual.CostoTotalFormateado;
            lblMetodo.Text = _pagoActual.MedioPago;
            lblObservaciones.Text = _pagoActual.Observaciones;
        }
    }

    private async void BtnGenerar_Clicked(object sender, EventArgs e)
    {
        // Aviso solicitado al momento de generar la nota
        await DisplayAlert("Nota Generada", $"La nota de venta {_pagoActual.NumeroNota} ha sido generada y guardada correctamente.", "Aceptar");

        // Opcional: Regresar a la lista
        await Navigation.PopAsync();
    }
}