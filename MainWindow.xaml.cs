using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FindSuffixApp
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, int> _wordFrequency = new Dictionary<string, int>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private new void AddText(string text)
        {
            var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (_wordFrequency.ContainsKey(word))
                {
                    _wordFrequency[word]++;
                }
                else
                {
                    _wordFrequency[word] = 1;
                }
            }
        }

        private string GetSuggestion(string prefix)
        {
            var candidates = _wordFrequency.Where(w => w.Key.StartsWith(prefix)).ToList();
            if (!candidates.Any())
                return "Нет подходящих слов";

            return candidates.OrderByDescending(w => w.Value)
                             .ThenBy(w => w.Key.Length)
                             .First().Key;
        }

        private void AddTextButton_Click(object sender, RoutedEventArgs e)
        {
            var text = AddTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                AddText(text);
                MessageBox.Show($"Текст '{text}' добавлен.");
                AddTextBox.Clear();
            }
        }

        private void PrefixTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var prefix = PrefixTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(prefix))
            {
                var suggestion = GetSuggestion(prefix);
                SuggestionLabel.Content = $"Подсказка: {suggestion}";
            }
            else
            {
                SuggestionLabel.Content = "Подсказка: ";
            }
        }
    }
}