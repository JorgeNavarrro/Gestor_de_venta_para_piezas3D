using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using System.Collections.ObjectModel;
using System.Linq; // Necesario para usar LINQ (Where, ToList)

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class PaymentsPage : ContentPage
{
    public PaymentsPage()
    {
        InitializeComponent();
    }

    // Usamos OnAppearing para recargar la lista cada vez que entramos a la pantalla
    // (útil cuando regresamos de crear una nueva nota)
    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarDatos();
    }

    private void CargarDatos()
    {
        // Cargamos los datos directamente desde el Repositorio compartido
        // Esto asegura que las nuevas notas creadas aparezcan aquí.
        cvPagos.ItemsSource = PaymentRepository.Pagos;
    }

    private void OnFiltroChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        var seleccion = picker.SelectedItem as string;

        // Si no hay selección o es "Todos", mostramos la lista completa
        if (string.IsNullOrEmpty(seleccion) || seleccion == "Todos")
        {
            cvPagos.ItemsSource = PaymentRepository.Pagos;
        }
        else
        {
            // Filtramos sobre la colección del repositorio según el medio de pago
            var filtrados = PaymentRepository.Pagos.Where(p => p.MedioPago == seleccion).ToList();
            cvPagos.ItemsSource = new ObservableCollection<Pago>(filtrados);
        }
    }

    private async void OnPagoSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Si se seleccionó un pago, navegamos al detalle (Nota de Venta)
        if (e.CurrentSelection.FirstOrDefault() is Pago pagoSeleccionado)
        {
            await Navigation.PushAsync(new SalesNotePage(pagoSeleccionado));

            // Limpiamos la selección para que se pueda volver a tocar el mismo elemento
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    // Método para ir a la pantalla de Generar Nota (botón "+ Nueva Nota")
    private async void BtnNuevaNota_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GenerateNotePage());
    }
}