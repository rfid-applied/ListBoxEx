using System;
using Spencen.Mobile.Converters;

namespace ListBoxExSample.Xaml
{
    public class ValueTypeConverter : Converter<ValueType>
    {
        public override ValueType ConvertFromString(string input, Type toType)
        {
            if (toType == typeof(bool))
                return bool.Parse(input);
            else if (toType == typeof(int))
                return int.Parse(input);
            else if (toType == typeof(decimal))
                return decimal.Parse(input);
            else if (toType == typeof(float))
                return float.Parse(input);
            else if (toType == typeof(double))
                return double.Parse(input);
            else if (toType == typeof(byte))
                return double.Parse(input);
            else if (toType.IsEnum)
                return (ValueType)Enum.Parse(toType, input, false);
            else if (toType == typeof(TimeSpan))
                return TimeSpan.Parse(input);

            throw new ArgumentOutOfRangeException("input");
        }

        public override bool CanConvertFromString(string input, Type toType)
        {
            return toType == typeof(bool) ||
                toType == typeof(int) ||
                toType == typeof(decimal) ||
                toType == typeof(float) ||
                toType == typeof(double) ||
                toType == typeof(byte) ||
                toType.IsEnum ||
                toType == typeof(TimeSpan);
        }
    }
}
