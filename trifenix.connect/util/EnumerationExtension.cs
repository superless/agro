using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace trifenix.connect.util
{
    // https://stackoverflow.com/questions/37305985/enum-description-attribute-in-dotnet-core
    public static class EnumerationExtension
    {
        public static string Description(this Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return description
            return displayAttribute?.Description ?? "Description Not Found";
        }

        public static bool IsPrimitive(Type t)
        {
            // TODO: put any type here that you consider as primitive as I didn't
            // quite understand what your definition of primitive type is
            return new[] {
            typeof(string),
            typeof(char),
            typeof(byte),
            typeof(sbyte),
            typeof(ushort),
            typeof(short),
            typeof(uint),
            typeof(int),
            typeof(ulong),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            typeof(bool),
            typeof(byte?),
            typeof(sbyte?),
            typeof(ushort?),
            typeof(short?),
            typeof(uint?),
            typeof(int?),
            typeof(ulong?),
            typeof(long?),
            typeof(float?),
            typeof(double?),
            typeof(decimal?),
            typeof(DateTime?),
            typeof(bool?),
        }.Contains(t);
        }

        public static bool IsPrimitiveAndCollection(Type t)
        {
            if (IsPrimitive(t)) return true;
            return IsPrimitiveCollection(t);
        }

        public static bool IsPrimitiveCollection(Type t)
        {  
            return new[] {
            typeof(string[]),
            typeof(bool[]),
            typeof(char[]),
            typeof(byte[]),
            typeof(sbyte[]),
            typeof(ushort[]),
            typeof(short[]),
            typeof(uint[]),
            typeof(int[]),
            typeof(ulong[]),
            typeof(long[]),
            typeof(float[]),
            typeof(double[]),
            typeof(decimal[]),
            typeof(DateTime[]),
            typeof(IEnumerable<string>),
            typeof(IEnumerable<char>),
            typeof(IEnumerable<byte>),
            typeof(IEnumerable<sbyte>),
            typeof(IEnumerable<ushort>),
            typeof(IEnumerable<short>),
            typeof(IEnumerable<uint>),
            typeof(IEnumerable<int>),
            typeof(IEnumerable<ulong>),
            typeof(IEnumerable<long>),
            typeof(IEnumerable<float>),
            typeof(IEnumerable<double>),
            typeof(IEnumerable<decimal>),
            typeof(IEnumerable<DateTime>),
            typeof(List<string>),
            typeof(List<char>),
            typeof(List<byte>),
            typeof(List<sbyte>),
            typeof(List<ushort>),
            typeof(List<short>),
            typeof(List<uint>),
            typeof(List<int>),
            typeof(List<ulong>),
            typeof(List<long>),
            typeof(List<float>),
            typeof(List<double>),
            typeof(List<decimal>),
            typeof(List<DateTime>),
        }.Contains(t);
        }
    }

    
}
