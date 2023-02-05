using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Oligopoly.DirectX.Windows;

partial class GameWindow : Window
{
    public GameWindow()
    {
        InitializeComponent();

        int squares = 40;
        int squaresPerSide = squares / 4;

        for (int i = 0; i <= squaresPerSide; i++)
        {
            _grid.ColumnDefinitions.Add(new ColumnDefinition());
            _grid.RowDefinitions.Add(new RowDefinition());
        }

        Brush brush = new SolidColorBrush(Colors.Red);

        for (int i = 0; i < squares; i++)
        {
            if (i % squaresPerSide is 0)
            {
                continue;
            }

            TextBlock b = new TextBlock()
            {
                Background = brush
            };

            int quotient = i % squaresPerSide;

            switch (i / squaresPerSide)
            {
                case 0:
                    Grid.SetRow(b, squaresPerSide);
                    Grid.SetColumn(b, squaresPerSide - quotient);
                    break;

                case 1:
                    Grid.SetRow(b, squaresPerSide - quotient);
                    break;

                case 2:
                    Grid.SetColumn(b, quotient);
                    break;

                case 3:
                    Grid.SetRow(b, quotient);
                    Grid.SetColumn(b, squaresPerSide);
                    break;
            }

            _grid.Children.Add(b);
        }
    }
}
