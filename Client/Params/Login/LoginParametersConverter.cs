using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Client.Params.Login
{
    /// <summary>
    /// A converter class which converts the login parameters (username/password)
    /// to a custom data type.
    /// </summary>
    public class LoginParametersConverter : IMultiValueConverter
    {

        /// <summary>
        /// Converts an array of string parameters to complex data type.
        /// </summary>
        /// <param name="values">The array storing login parameter strings.</param>
        /// <param name="targetType">The complex data type to convert to.</param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            LoginParameters parameters = new LoginParameters();

            // Assign the first and second values of the array to corresponding data type attributes
            if (values[0] is string) parameters.Username = (string)values[0];
            if (values[1] is string) parameters.Password = (string)values[1];

            return parameters;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
