
using Microsoft.Extensions.DependencyInjection;
using NotasRapidasSQLite.Views;

namespace NotasRapidasSQLite;

public partial class AppShell : Shell
{
    public AppShell(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        Routing.RegisterRoute(
            nameof(DetalleNotaPage),
            typeof(DetalleNotaPage));

        var tabBar = new TabBar();

        var pestañaNotas = new ShellContent
        {
            Title = "Notas",
            Icon = "dotnet_bot.png",
            ContentTemplate = new DataTemplate(
                () => serviceProvider.GetRequiredService<NotasPage>())
        };

        var pestañaHistorial = new ShellContent
        {
            Title = "Historial",
            ContentTemplate = new DataTemplate(
                () => serviceProvider.GetRequiredService<HistorialPage>())
        };

        tabBar.Items.Add(new Tab
        {
            Title = "Notas",
            Items = { pestañaNotas }
        });

        tabBar.Items.Add(new Tab
        {
            Title = "Historial",
            Items = { pestañaHistorial }
        });

        Items.Add(tabBar);
    }
}

