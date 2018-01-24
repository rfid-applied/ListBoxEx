using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Spencen.Mobile.Converters
{
    public interface IConverter
    {
        object ConvertFromString(string input, Type toType);
        string ConvertToString(object input);
        bool CanConvertFromString(string input, Type toType);
    }

    public interface IConverter<T>
    {
        T ConvertFromString(string input, Type toType);
        string ConvertToString(T input);
        bool CanConvertFromString(string input, Type toType);
    }

    public abstract class Converter<T> : IConverter<T>, IConverter
    {
        #region IConverter<T> Members

        public virtual T ConvertFromString(string input, Type toType)
        {
            throw new NotImplementedException();
        }

        public virtual string ConvertToString(T input)
        {
            return input.ToString();
        }

        public virtual bool CanConvertFromString(string input, Type toType)
        {
            return true;
        }

        #endregion

        #region IConverter Members

        object IConverter.ConvertFromString(string input, Type toType)
        {
            return ConvertFromString(input, toType);
        }

        string IConverter.ConvertToString(object input)
        {
            return ConvertToString((T)input);
        }

        bool IConverter.CanConvertFromString(string input, Type toType)
        {
            return CanConvertFromString(input, toType);
        }

        #endregion
    }
}
