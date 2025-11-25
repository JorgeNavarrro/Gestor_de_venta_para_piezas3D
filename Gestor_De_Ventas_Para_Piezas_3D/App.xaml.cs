using Gestor_De_Ventas_Para_Piezas_3D.Vistas; // Necesario para ver LoginPage

namespace Gestor_De_Ventas_Para_Piezas_3D; // <--- ¡ESTA LÍNEA ES CRUCIAL!

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Configuración del flujo de inicio:
        // Arrancamos en el Login dentro de una página de navegación
        MainPage = new NavigationPage(new LoginPage());
    }
}