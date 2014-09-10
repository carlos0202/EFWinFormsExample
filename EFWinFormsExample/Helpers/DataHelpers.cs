using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace EFWinFormsExample.Helpers
{
    public static class DataHelpers
    {
        public static string phoneRegex = @"(\d{3})(\d{3})(\d{4})";
        public static string phoneMask = "($1) $2-$3";
        public static string dniRegex = @"(\d{3})(\d{7})(\d{1})";
        public static string dniMask = "$1-$2-$3";
        public static string rncRegex = @"(\d{3})(\d{5})(\d{1})";
        public static string rncMask = "$1-$2-$3";

        public static string PhoneFormat(this string phone)
        {
            try
            {
                return Regex.Replace(phone, phoneRegex, phoneMask);
            }
            catch (Exception)
            {
                return phone;
            }
        }

        public static string DniFormat(this string dni)
        {
            try
            {
                return Regex.Replace(dni, dniRegex, dniMask);
            }
            catch (Exception)
            {
                return dni;
            }
        }

        /// <summary>
        /// Version mejorada de la funcion para formatear cadenas en formatos específicos
        /// </summary>
        /// <param name="rawValue">Cadena con los datos a ser formateados</param>
        /// <param name="formatType">Tipo de formato a utilizar (phone,dni,rnc,date,datetime)</param>
        /// <returns></returns>
        public static string CFormat(this string rawValue, string formatType)
        {
            try
            {
                switch (formatType.ToLower())
                {
                    case "phone": return Regex.Replace(rawValue, phoneRegex, phoneMask);
                    case "dni": return Regex.Replace(rawValue, dniRegex, dniMask);
                    case "rnc": return Regex.Replace(rawValue, rncRegex, rncMask);
                    default: return rawValue;
                }
            }
            catch (Exception)
            {
                return rawValue;
            }
        }

        /// <summary>
        /// Metodo utilitario privado para traspasar los valores
        /// de las propiedades con igual nombre desde un objeto
        /// a otro.
        /// </summary>
        /// <param name="source">Instancia del objeto del cual se obtendrán los datos.</param>
        /// <param name="target">Instancia del objeto que recibirá los datos.</param>
        static void Transfer(object source, object target, List<string> toSkip = null)
        {
            var sourceType = source.GetType(); //tipo de objeto de instancia fuente
            var targetType = target.GetType(); //tipo de objeto de instancia destino

            //creación de parámetros para la expresión lambda
            var sourceParameter = Expression.Parameter(typeof(object), "source");
            var targetParameter = Expression.Parameter(typeof(object), "target");

            //creación de variables para la expresión lambda
            var sourceVariable = Expression.Variable(sourceType, "castedSource");
            var targetVariable = Expression.Variable(targetType, "castedTarget");

            var expressions = new List<Expression>();
            //agregar variables y parámetros a las expresiones lambda a ejecutar
            expressions.Add(Expression.Assign(sourceVariable, Expression.Convert(sourceParameter, sourceType)));
            expressions.Add(Expression.Assign(targetVariable, Expression.Convert(targetParameter, targetType)));

            foreach (var property in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead) // verificar si la propiedad fuente admite lectura.
                    continue;

                if (toSkip != null)
                    if (toSkip.Contains(property.Name))
                        continue;

                var targetProperty = targetType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                if (targetProperty != null
                        && targetProperty.CanWrite //se puede escribir en la propiedad de destino?
                        && targetProperty.PropertyType.IsAssignableFrom(property.PropertyType))
                {
                    expressions.Add(
                        Expression.Assign( //expresión para la asignación de las propiedades de los objetos.
                            Expression.Property(targetVariable, targetProperty),
                            Expression.Convert(
                                Expression.Property(sourceVariable, property), targetProperty.PropertyType)));
                }
            }

            // creación formal de la expresión lambda a ejecutar.
            var lambda =
                Expression.Lambda<Action<object, object>>(
                    Expression.Block(new[] { sourceVariable, targetVariable }, expressions),
                    new[] { sourceParameter, targetParameter });

            var del = lambda.Compile(); //compilar expresión lambda y obtener el delegado.

            del(source, target); //ejectuar la expresión lambda utilizando el delegado obtenido.
        }

        /// <summary>
        /// Metodo para copiar los datos de propiedades con igual nombre
        /// desde una instancia de una clase hacia otra.
        /// </summary>
        /// <typeparam name="SourceType">Tipo de datos del objeto fuente (proveedor de datos)</typeparam>
        /// <typeparam name="TargetType">Tipo de datos del objeto destino (receptor de datos)</typeparam>
        /// <param name="source">Instancia del objeto fuente de los datos.</param>
        /// <param name="targetObj">Instancia opcional del objeto recibidor de los datos</param>
        /// <returns></returns>
        public static TargetType Transfer<SourceType, TargetType>(SourceType source, object targetObj = null, string toSkip = null)
            where TargetType : class, new()
        {
            TargetType target = new TargetType();
            if (targetObj != null)
            {
                target = (TargetType)targetObj;
            }
            if (toSkip != null)
            {
                List<string> skipList = toSkip.Split(',').ToList();
                Transfer(source, target, skipList);
            }
            else
            {
                Transfer(source, target);
            }
            return target;
        }

    }
}