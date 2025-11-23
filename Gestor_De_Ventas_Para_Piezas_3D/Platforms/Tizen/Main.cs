using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Gestor_De_Ventas_Para_Piezas_3D
{
    internal class Program : MauiApplication
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
