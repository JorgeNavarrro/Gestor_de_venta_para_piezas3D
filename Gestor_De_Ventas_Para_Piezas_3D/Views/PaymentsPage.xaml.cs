using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class PaymentsPage : ContentPage
{
    public PaymentsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatosDesdeBD();
    }

    private async Task CargarDatosDesdeBD()
    {
        var db = new DatabaseService();
        var pagos = await db.ObtenerPagosAsync();
        cvPagos.ItemsSource = new ObservableCollection<Pago>(pagos);
    }

    private async void OnFiltroChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        var seleccion = picker.SelectedItem as string;

        var db = new DatabaseService();
        var todos = await db.ObtenerPagosAsync();

        if (string.IsNullOrEmpty(seleccion) || seleccion == "Todos")
        {
            cvPagos.ItemsSource = new ObservableCollection<Pago>(todos);
        }
        else
        {
            var filtrados = todos.Where(p => p.MedioPago == seleccion).ToList();
            cvPagos.ItemsSource = new ObservableCollection<Pago>(filtrados);
        }
    }

    private async void OnPagoSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Si se selecciona una nota, vamos a Editarla
        if (e.CurrentSelection.FirstOrDefault() is Pago pagoSeleccionado)
        {
            // Pasamos el pago seleccionado al constructor de GenerateNotePage
            await Navigation.PushAsync(new GenerateNotePage(pagoSeleccionado));
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void BtnNuevaNota_Clicked(object sender, EventArgs e)
    {
        // Nueva nota (constructor vacío)
        await Navigation.PushAsync(new GenerateNotePage());
    }
}