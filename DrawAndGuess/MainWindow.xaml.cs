using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrawAndGuess;

public partial class MainWindow : Window
{
    private const int TOTAL_SECONDS = 45;

    private int secondsLeft = TOTAL_SECONDS;
    private System.Timers.Timer timer;
    private SoundPlayer dingSoundPlayer;
    private SoundPlayer dongSoundPlayer;

    private readonly SolidColorBrush scbrush_red = new(Colors.Red);
    private readonly SolidColorBrush scbrush_white = new(Colors.White);
    private readonly SolidColorBrush scbrush_black = new(Colors.Black);
    private readonly Random random = new((int)DateTime.Now.TimeOfDay.TotalSeconds);

    public MainWindow()
    {
        InitializeComponent();

        timer = new();
        dingSoundPlayer = new("./Sources/Medias/ding.wav");
        dongSoundPlayer = new("./Sources/Medias/dong.wav");

        Container_Gaming.Visibility = Visibility.Hidden;
    }

    private void ResetTimer()
    {
        timer.Stop();
        timer.Dispose();

        secondsLeft = TOTAL_SECONDS;
        UpdateSecondsLeftView();

        timer = new()
        {
            Interval = 1 * 1000,
        };
        timer.Elapsed += (_, _) =>
        {
            secondsLeft -= 1;
            Dispatcher.Invoke(new(() => UpdateSecondsLeftView()));
        };
        timer.Start();
    }

    private void UpdateSecondsLeftView()
    {
        if (secondsLeft <= 0)
        {
            timer.Stop();
            secondsLeft = 0;
        }

        TextBlock_TimeLeft.Text = secondsLeft.ToString();

        if (secondsLeft <= 5)
        {
            TextBlock_TimeLeft.Foreground = scbrush_red;
            TextBlock_TimeLeft.FontSize = 124;

            if (secondsLeft <= 3 && secondsLeft != 0)
                dingSoundPlayer.Play();

            if (secondsLeft == 0)
                dongSoundPlayer.Play();
        }
        else
        {
            TextBlock_TimeLeft.Foreground = scbrush_black;
            TextBlock_TimeLeft.FontSize = 84;
        }
    }

    private void BeginOneTurn()
    {
        Container_Timer.Width = 0;
        Button_BeginRound.Width = 200;
        GenerateNextGuessing();
    }

    private void GenerateNextGuessing()
    {
        var total = App.Guessings!.GuessingsList.Count;
        var target = random.Next(0, total);

        new Thread(() =>
        {
            for (var i = 0; i <= 10; ++i)
            {
                Thread.Sleep(100);

                var current = random.Next(0, total);

                if (i == 10) current = target;

                Dispatcher.Invoke(new(() =>
                {
                    TextBlock_Guessing.Text = App.Guessings!.GuessingsList[current];
                }));
            }
        }).Start();
    }

    private void Button_BeginGame_Click(object sender, RoutedEventArgs e)
    {
        Container_Welcome.Visibility = Visibility.Hidden;

        Container_Gaming.Visibility = Visibility.Visible;

        BeginOneTurn();
    }

    private void Button_NextOne_Click(object sender, RoutedEventArgs e) => BeginOneTurn();

    private void Button_Settings_Click(object sender, RoutedEventArgs e)
    {
        new AddWordWindow()
        {
            Guessings = App.Guessings,
            OnSave = (x) =>
            {
                App.Guessings = x;
                App.Guessings.SaveTo("./guessings.json");
            },
        }.ShowDialog();
    }

    private void Button_Quit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void Button_BeginRound_Click(object sender, RoutedEventArgs e)
    {
        Container_Timer.Width = 450;
        Button_BeginRound.Width = 0;
        ResetTimer();
    }
}
