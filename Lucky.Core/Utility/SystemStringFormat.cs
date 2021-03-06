﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Utility
{
    public sealed class SystemStringFormat
    {
        private readonly IFormatProvider m_provider;
        private readonly string m_format;
        private readonly object[] m_args;

        #region Constructor

        /// <summary>
        /// Initialise the <see cref="SystemStringFormat"/>
        /// </summary>
        /// <param name="provider">An <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">A <see cref="System.String"/> containing zero or more format items.</param>
        /// <param name="args">An <see cref="System.Object"/> array containing zero or more objects to format.</param>
        public SystemStringFormat(IFormatProvider provider, string format, params object[] args)
        {
            m_provider = provider;
            m_format = format;
            m_args = args;
        }

        #endregion Constructor

        /// <summary>
        /// Format the string and arguments
        /// </summary>
        /// <returns>the formatted string</returns>
        public override string ToString()
        {
            return StringFormat(m_provider, m_format, m_args);
        }

        #region StringFormat

        /// <summary>
        /// Replaces the format item in a specified <see cref="System.String"/> with the text equivalent 
        /// of the value of a corresponding <see cref="System.Object"/> instance in a specified array.
        /// A specified parameter supplies culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">A <see cref="System.String"/> containing zero or more format items.</param>
        /// <param name="args">An <see cref="System.Object"/> array containing zero or more objects to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the <see cref="System.String"/> 
        /// equivalent of the corresponding instances of <see cref="System.Object"/> in args.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method does not throw exceptions. If an exception thrown while formatting the result the
        /// exception and arguments are returned in the result string.
        /// </para>
        /// </remarks>
        private static string StringFormat(IFormatProvider provider, string format, params object[] args)
        {
            try
            {
                // The format is missing, log null value
                if (format == null)
                {
                    return null;
                }

                // The args are missing - should not happen unless we are called explicitly with a null array
                if (args == null)
                {
                    return format;
                }

                // Try to format the string
                return String.Format(provider, format, args);
            }
            catch (Exception ex)
            {
                return StringFormatError(ex, format, args);
            }
        }

        /// <summary>
        /// Process an error during StringFormat
        /// </summary>
        private static string StringFormatError(Exception formatException, string format, object[] args)
        {
            try
            {
                StringBuilder buf = new StringBuilder("<log.Error>");

                if (formatException != null)
                {
                    buf.Append("Exception during StringFormat: ").Append(formatException.Message);
                }
                else
                {
                    buf.Append("Exception during StringFormat");
                }
                buf.Append(" <format>").Append(format).Append("</format>");
                buf.Append("<args>");
                RenderArray(args, buf);
                buf.Append("</args>");
                buf.Append("</log.Error>");

                return buf.ToString();
            }
            catch
            {
                
            }
            return "";
        }

        /// <summary>
        /// Dump the contents of an array into a string builder
        /// </summary>
        private static void RenderArray(Array array, StringBuilder buffer)
        {
            if (array == null)
            {
                buffer.Append("");
            }
            else
            {
                if (array.Rank != 1)
                {
                    buffer.Append(array.ToString());
                }
                else
                {
                    buffer.Append("{");
                    int len = array.Length;

                    if (len > 0)
                    {
                        RenderObject(array.GetValue(0), buffer);
                        for (int i = 1; i < len; i++)
                        {
                            buffer.Append(", ");
                            RenderObject(array.GetValue(i), buffer);
                        }
                    }
                    buffer.Append("}");
                }
            }
        }

        /// <summary>
        /// Dump an object to a string
        /// </summary>
        private static void RenderObject(Object obj, StringBuilder buffer)
        {
            if (obj == null)
            {
                buffer.Append("");
            }
            else
            {
                try
                {
                    buffer.Append(obj);
                }
                catch
                {
                    
                }
            }
        }

        #endregion StringFormat

        #region Private Static Fields

        /// <summary>
        /// The fully qualified type of the SystemStringFormat class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(SystemStringFormat);

        #endregion Private Static Fields
    }
}
