using System.Collections.ObjectModel;
using Gestor_De_Ventas_Para_Piezas_3D.Modelos;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class CatalogPage : ContentPage
{
    // La lista de tus productos que se verá en la pantalla
    public ObservableCollection<ModeloReference> CatalogList { get; set; }

    public CatalogPage()
    {
        InitializeComponent();

        // 1. Inicializar la lista vacía
        CatalogList = new ObservableCollection<ModeloReference>();

        // 2. Llenar la lista con los datos (Nombres coinciden con los Triggers del XAML)
        CargarDatos();

        // 3. CONEXIÓN CLAVE: Le decimos a la pantalla que busque los datos aquí mismo
        BindingContext = this;
    }

    private void CargarDatos()
    {
        // NOTA: Los nombres aquí ("Mini Figura Astro", etc.) deben ser IDÉNTICOS
        // a los que pusiste en los <DataTrigger> del archivo XAML para que cambie la foto.

        CatalogList.Add(new ModeloReference
        {
            Nombre = "Mini Figura Astro",
            Categoria = "Coleccionables",
            Material = "Resina",
            Descripcion = "Figura detallada de astronauta."
        });

        CatalogList.Add(new ModeloReference
        {
            Nombre = "Soporte Auriculares",
            Categoria = "Accesorios",
            Material = "PLA",
            Descripcion = "Soporte resistente para mesa."
        });

        CatalogList.Add(new ModeloReference
        {
            Nombre = "Engranes Mecánicos",
            Categoria = "Repuestos",
            Material = "PETG",
            Descripcion = "Set de engranajes funcionales."
        });

        CatalogList.Add(new ModeloReference
        {
            Nombre = "Llaveros Personalizados",
            Categoria = "Merch",
            Material = "PLA",
            Descripcion = "Llaveros con logo corporativo."
        });

        CatalogList.Add(new ModeloReference
        {
            Nombre = "Figura Dragon",
            Categoria = "Arte",
            Material = "Resina",
            Descripcion = "Figura decorativa pintada a mano."
        });
    }
}