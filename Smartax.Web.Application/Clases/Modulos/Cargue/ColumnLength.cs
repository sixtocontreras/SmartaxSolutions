using System;

namespace Smartax.Web.Application.Clases.Modulos.Cargue
{
    public struct ColumnLength : IEquatable<ColumnLength>
    {
        public int StartIndex { get; }

        public int Length { get; }

        public ColumnLength(int startIndex, int length)
        {
            if (startIndex < 0)
            {
                throw new Exception("start index cannot be less than zero");
            }

            StartIndex = startIndex;
            Length = length;
        }

        public bool Equals(ColumnLength other)
        {
            return StartIndex == other.StartIndex && Length == other.Length;
        }

        public override int GetHashCode()
        {
            return StartIndex.GetHashCode() + Length.GetHashCode();
        }
    }
}