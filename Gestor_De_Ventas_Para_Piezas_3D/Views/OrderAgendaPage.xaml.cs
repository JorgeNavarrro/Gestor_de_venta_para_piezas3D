using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class OrderAgendaPage : ContentPage
{
    // Lista que se muestra en pantalla
    public ObservableCollection<Pedido> ListaPedidos { get; set; }

    // Lista auxiliar para guardar todos los datos y poder buscar
    private List<Pedido> _todosLosPedidos;

    public OrderAgendaPage()
    {
        InitializeComponent();
        CargarDatos();
    }

    private void CargarDatos()
    {
        // Creamos los datos iniciales
        _todosLosPedidos = new List<Pedido>
        {
            new Pedido { Id = 1, Cliente = "Ana Martinez", Producto = "Maceta", FechaEntrega = "20/10/2025", Estado = "En producción", FechaInicio = "17/10/2025", Duracion = "" },
            new Pedido { Id = 2, Cliente = "Luis Torres", Producto = "Llavero", FechaEntrega = "22/10/2025", Estado = "Pendiente", FechaInicio = "18/10/2025", Duracion = "" },
            new Pedido { Id = 3, Cliente = "Jorge Navarro", Producto = "Figura", FechaEntrega = "25/10/2025", Estado = "Completado", FechaInicio = "22/10/2025", Duracion = "3 días" }
        };

        // Inicializamos la lista visual con todos los datos
        ListaPedidos = new ObservableCollection<Pedido>(_todosLosPedidos);
        cvPedidos.ItemsSource = ListaPedidos;
    }

    // --- MÉTODO DE BÚSQUEDA ---
    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var textoBusqueda = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(textoBusqueda))
        {
            // Si no hay texto, mostramos todo de nuevo
            cvPedidos.ItemsSource = new ObservableCollection<Pedido>(_todosLosPedidos);
        }
        else
        {
            // Filtramos por Cliente o Producto (ignorando mayúsculas/minúsculas)
            var resultados = _todosLosPedidos
                .Where(p => p.Cliente.ToLower().Contains(textoBusqueda.ToLower()) ||
                            p.Producto.ToLower().Contains(textoBusqueda.ToLower()))
                .ToList();

            cvPedidos.ItemsSource = new ObservableCollection<Pedido>(resultados);
        }
    }

    // --- MÉTODO DE NAVEGACIÓN ---
    private async void OnPedidoSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Pedido pedidoSeleccionado)
        {
            await Navigation.PushAsync(new OrderDetailPage(pedidoSeleccionado));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}