using System.Windows;

namespace DrawAndGuess;

public partial class App : Application
{
    public static Guessings? Guessings { get; set; }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var guessings_loaded = "./guessings.json".Load();

        Guessings = guessings_loaded ?? new();
        Guessings.SaveTo("./guessings.json");
    }
}

