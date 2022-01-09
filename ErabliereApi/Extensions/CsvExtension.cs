using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ErabliereApi.Extensions
{
    /// <summary>
    /// Classe d'extension pour les traitements relié au type de donnée csv.
    /// </summary>
    public static class CsvExtension
    {
        /// <summary>
        /// Transforme une liste en fichier csv représenté par le type byte[]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="separator">Séparateur des données dans le fichier csv</param>
        /// <returns></returns>
        public static byte[] AsCsvInByteArray<T>(this IEnumerable<T> list, char separator = ';')
        {
            var sb = new StringBuilder();

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                sb.Append(prop.DisplayName);
                sb.Append(separator);
            }
            sb.AppendLine();

            foreach (T item in list)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    sb.Append(prop.Converter.ConvertToString(prop.GetValue(item)));
                    sb.Append(separator);
                }
                sb.AppendLine();
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
