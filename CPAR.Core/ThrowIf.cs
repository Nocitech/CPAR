using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Core
{
    public static class ThrowIf
    {
        public static class Argument
        {
            public static void IsNull(object argumentValue, string argumentName)
            {
                if (argumentValue == null)
                {
                    throw new ArgumentNullException(argumentName);
                }
            }

            public static void OutOfBounds(int index, object[] array)
            {
                if (index >= array.Length)
                {
                    throw new IndexOutOfRangeException();
                }
            }

            public static void IsZero(int parameter, string argumentName)
            {
                if (parameter == 0)
                {
                    throw new ArgumentException(System.String.Format("{0} cannot be zero", argumentName));
                }
            }

            public static void IsZeroOrNegative(int parameter, string argumentName)
            {
                if (parameter < 1)
                {
                    throw new ArgumentException(System.String.Format("{0} cannot be zero", argumentName));
                }
            }

        }

        public static class Array
        {
            public static void IsIncorrectSize(object[] array, int length, string argumentName)
            {
                if (array.Length != length)
                {
                    throw new ArgumentOutOfRangeException(argumentName, System.String.Format("array {0} must have a length of {1}", argumentName, length));
                }
            }

            public static void IsEmpty(object[] array, string argumentName)
            {
                if (array.Length == 0)
                {
                    throw new ArgumentException("cannot be empty", argumentName);
                }
            }
        }

        public static class List
        {
            public static void IsEmpty<T>(List<T> list, string argumentName)
            {
                if (list.Count == 0)
                {
                    throw new ArgumentException("cannot be empty", argumentName);
                }
            }
        }

        public static class String
        {
            public static void IsEmpty(string str, string argumentName)
            {
                if (str.Length == 0)
                {
                    throw new ArgumentException("cannot be empty", argumentName);
                }
            }
        }

        public static class File
        {
            public static void DoesNotExists(string filename)
            {
                if (!System.IO.File.Exists(filename))
                {
                    throw new ArgumentException(System.String.Format("File {0} does not exists", filename));
                }
            }
        }
    }
}
