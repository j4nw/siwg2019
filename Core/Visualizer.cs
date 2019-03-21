using System;
using System.Collections.Generic;
using System.Drawing;

namespace Core
{
    public static class Visualizer<TLayer>
    {
        public static Image BoardToImage(IBoard<TLayer> board, Dictionary<TLayer, (Color lowest, Color highest)> colorByLayer, List<TLayer> orderedLayers)
        {
            var image = new Bitmap(board.Width, board.Height);
            for (var x = 0; x < board.Width; x++)
            {
                for (var y = 0; y < board.Height; y++)
                {
                    var topLayer = default(TLayer);
                    var layerValue = default(byte);
                    var foundLayer = false;
                    foreach (var layer in orderedLayers)
                    {
                        layerValue = board.Layer(layer, x, y);
                        var layerExists = Convert.ToBoolean(layerValue);
                        if (layerExists)
                        {
                            topLayer = layer;
                            foundLayer = true;
                            break;
                        }
                    }

                    if (foundLayer)
                    {
                        var (lowest, highest) = colorByLayer[topLayer];
                        var normalizedValue = (float)layerValue / byte.MaxValue;
                        var color = LerpColor(lowest, highest, normalizedValue);

                        image.SetPixel(x, y, color);
                    }
                    else
                    {
                        image.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return image;
        }

        private static Color LerpColor(Color lowest, Color highest, float amount)
        {
            var r = Convert.ToByte(lowest.R + (highest.R - lowest.R) * amount);
            var g = Convert.ToByte(lowest.G + (highest.G - lowest.G) * amount);
            var b = Convert.ToByte(lowest.B + (highest.B - lowest.B) * amount);
            return Color.FromArgb(r, g, b);
        }
    }
}
