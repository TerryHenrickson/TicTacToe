using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TicTacToe.Model;

namespace TicTacToe.ViewModels
{
    public class ColouredBoardViewModel : BindableBase
    {
        private Dictionary<GridPoint, double> availableMoveScores;
        private static Dictionary<int, Color> ColourDictionary;

        static ColouredBoardViewModel()
        {
            var colourGradient = CreateColourGradient();

            ColourDictionary = new Dictionary<int, Color>();
            for (int i = 0; i < 101; i++)
            {
                ColourDictionary[i] = GetRelativeColor(colourGradient, (double)i / 100);
            }
        }

        public ColouredBoardViewModel(Dictionary<GridPoint, double> availableMoveScores)
        {
            this.availableMoveScores = availableMoveScores;
        }

        private static GradientStopCollection CreateColourGradient()
        {
            Color red = Color.FromRgb(248, 105, 107);
            Color yellow = Color.FromRgb(255, 235, 132);
            Color green = Color.FromRgb(99, 190, 123);

            return new GradientStopCollection(3)
            {
                new GradientStop(red, 0),
                new GradientStop(yellow, 0.501),
                new GradientStop(green, 1)
            };
        }


        private Color GridPointToColour(GridPoint gridpoint)
        {
            if (this.availableMoveScores != null && this.availableMoveScores.ContainsKey(gridpoint))
            {
                return ColourDictionary[ Convert.ToInt32(100 * this.availableMoveScores[gridpoint])];
            }

            return Colors.Ivory;
        }

        public Color TopLeft { get { return GridPointToColour(new GridPoint(0, 0)); } }
        public Color TopMiddle { get { return GridPointToColour(new GridPoint(1, 0)); } }
        public Color TopRight { get { return GridPointToColour(new GridPoint(2, 0)); } }
        public Color MiddleLeft { get { return GridPointToColour(new GridPoint(0, 1)); } }
        public Color MiddleMiddle { get { return GridPointToColour(new GridPoint(1, 1)); } }
        public Color MiddleRight { get { return GridPointToColour(new GridPoint(2, 1)); } }
        public Color BottomLeft { get { return GridPointToColour(new GridPoint(0, 2)); } }
        public Color BottomMiddle { get { return GridPointToColour(new GridPoint(1, 2)); } }
        public Color BottomRight { get { return GridPointToColour(new GridPoint(2, 2)); } }


        private static Color GetRelativeColor(GradientStopCollection colourGradient, double offset)
        {
            GradientStop before = colourGradient.Where(w => w.Offset == colourGradient.Min(m => m.Offset)).First();
            GradientStop after = colourGradient.Where(w => w.Offset == colourGradient.Max(m => m.Offset)).First();

            foreach (var gs in colourGradient)
            {
                if (gs.Offset < offset && gs.Offset > before.Offset)
                {
                    before = gs;
                }

                if (gs.Offset > offset && gs.Offset < after.Offset)
                {
                    after = gs;
                }
            }

            var color = new Color();

            color.ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA);
            color.ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR);
            color.ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG);
            color.ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB);

            return color;
        }

    }
}
