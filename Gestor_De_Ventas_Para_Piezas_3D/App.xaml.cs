namespace Gestor_De_Ventas_Para_Piezas_3D;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();  //Falla por que quiere


        // Ahora sí encontrará la carpeta Vistas dentro de tu proyecto
        MainPage = new NavigationPage(new Vistas.LoginPage());
    }
}