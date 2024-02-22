using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.IdentityModel.Tokens;

namespace LabBilling.Core.DataAccess
{
    public struct InsCoverage
    {
        private string value;

        public static IEnumerable<InsCoverage> AllInsCoverages
        {
            get
            {
                yield return Primary;
                yield return Secondary;
                yield return Tertiary;
                yield return Temporary;
            }
        }

        public static InsCoverage Primary { get; } = new InsCoverage("A");
        public static InsCoverage Secondary { get; } = new InsCoverage("B");
        public static InsCoverage Tertiary { get; } = new InsCoverage("C");
        public static InsCoverage Temporary { get; } = new InsCoverage("X");


        /// <summary>
        /// primary constructor
        /// </summary>
        /// <param name="value">The string value that this is a wrapper for</param>
        private InsCoverage(string value)
        {
            this.value = value;
        }

        public bool IsValid
        {
            get
            {
                var result = Parse(value);
                if (result != Primary && result != Secondary && result != Tertiary)
                    return false;
                else
                    return true;
            }
        }


        /// <summary>
        /// Compares the Group to another group, or to a string value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is InsCoverage coverage)
            {
                return this.value.Equals(coverage.value);
            }

            if (obj is string otherString)
            {
                return this.value.Equals(otherString);
            }

            throw new ArgumentException("obj is neither a InsCoverage nor a String");
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// returns the internal string that this is a wrapper for.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static implicit operator string(InsCoverage insCoverage)
        {
            return insCoverage.value;
        }

        /// <summary>
        /// Parses a string and returns an instance that corresponds to it.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static InsCoverage Parse(string input)
        {
            return AllInsCoverages.Where(item => item.value == input).FirstOrDefault();
        }

        /// <summary>
        /// Syntatic sugar for the Parse method.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static explicit operator InsCoverage(string other)
        {
            return Parse(other);
        }

        public override readonly string ToString()
        {
            return value;
        }

        public static bool operator ==(InsCoverage left, InsCoverage right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(InsCoverage left, InsCoverage right)
        {
            return !(left == right);
        }
    }
}
