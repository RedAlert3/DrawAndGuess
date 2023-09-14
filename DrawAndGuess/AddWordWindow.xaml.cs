using System;
using System.Windows;

namespace DrawAndGuess;

public partial class AddWordWindow : Window
{
    public AddWordWindow()
    {
        InitializeComponent();

        Loaded += (_, _) => RefreshWords();
    }

    private void RefreshWords()
    {
        ListBox_Words.Items.Clear();

        foreach (var guess in Guessings!.GuessingsList)
            ListBox_Words.Items.Add(guess);
    }

    public Guessings? Guessings { get; set; }

    public Action<Guessings>? OnSave { get; set; }

    protected override void OnClosed(EventArgs e)
    {
        OnSave?.Invoke(Guessings!);

        base.OnClosed(e);
    }

    private void Button_Delete_Click(object sender, RoutedEventArgs e)
    {
        var selectedIndex = ListBox_Words.SelectedIndex;

        if (selectedIndex != -1)
            Guessings!.GuessingsList.RemoveAt(selectedIndex);

        RefreshWords();
    }

    private void Button_Add_Click(object sender, RoutedEventArgs e)
    {
        var text = TextBox_Word.Text;

        if (!string.IsNullOrWhiteSpace(text))
        {
            Guessings!.GuessingsList.Add(text);
            RefreshWords();
        }
    }
}
