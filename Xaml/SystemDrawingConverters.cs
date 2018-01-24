using System;
using System.Drawing;
using Spencen.Mobile.Converters;

namespace Spencen.Mobile.UI.Markup.Converters
{
    public class ColorConverter : Converter<Color>
    {
        public override Color ConvertFromString(string input, Type toType)
        {
            if (string.IsNullOrEmpty(input))
                return Color.Transparent;

            // TODO: Well known names?

            var rgb = input.Split(',');
            return Color.FromArgb(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
        }
    }
    public class SizeConverter : Converter<Size>
    {
        public override Size ConvertFromString(string input, Type toType)
        {
            if (string.IsNullOrEmpty(input))
                return Size.Empty;

            var xy = input.Split(',');
            if (xy.Length != 2)
                throw new ArgumentOutOfRangeException("input");
            return new Size(int.Parse(xy[0]), int.Parse(xy[1]));
        }
    }
    public class PointConverter : Converter<Point>
    {
        public override Point ConvertFromString(string input, Type toType)
        {
            if (string.IsNullOrEmpty(input))
                return Point.Empty;

            var xy = input.Split(',');
            if (xy.Length != 2)
                throw new ArgumentOutOfRangeException("input");
            return new Point(int.Parse(xy[0]), int.Parse(xy[1]));
        }
    }
}

