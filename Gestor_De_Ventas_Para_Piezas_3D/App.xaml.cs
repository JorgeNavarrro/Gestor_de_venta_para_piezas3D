using Gestor_De_Ventas_Para_Piezas_3D.Services; // Necesario
using Gestor_De_Ventas_Para_Piezas_3D.Vistas;

namespace Gestor_De_Ventas_Para_Piezas_3D;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // ✅ INICIALIZAR BD AL ARRCANAR
        // Esto crea el archivo .db3 si no existe, para que siempre haya algo que exportar
        var db = new DatabaseService();
        Task.Run(async () => await db.InicializarBaseDeDatosAsync());

        // Arrancar en el Login
        MainPage = new NavigationPage(new LoginPage());
    }
}