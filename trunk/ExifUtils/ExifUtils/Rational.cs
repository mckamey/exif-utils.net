#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2005-2009 Stephen M. McKamey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
	THE SOFTWARE.

\*---------------------------------------------------------------------------------*/
#endregion License

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace ExifUtils
{
	/// <summary>
	/// Represents a rational number
	/// </summary>
	[Serializable]
	public struct Rational<T> :
		IConvertible,
		IComparable,
		IComparable<T>
		where T : IConvertible
	{
		#region Delegate Types

		private delegate T ParseDelegate(string value);
		private delegate bool TryParseDelegate(string value, out T rational);

		#endregion Delegate Types

		#region Constants

		private const char Delim = '/';
		private static readonly char[] DelimSet = new char[] { Delim };

		#endregion Constants

		#region Fields

		private T numerator;
		private T denominator;
		private static ParseDelegate Parser;
		private static TryParseDelegate TryParser;
		private static decimal maxValue;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="numerator">The numerator of the rational number.</param>
		/// <param name="denominator">The denominator of the rational number.</param>
		/// <remarks>reduces by default</remarks>
		public Rational(T numerator, T denominator)
			: this(numerator, denominator, false)
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="numerator">The numerator of the rational number.</param>
		/// <param name="denominator">The denominator of the rational number.</param>
		/// <param name="reduce">determines if should reduce by greatest common divisor</param>
		public Rational(T numerator, T denominator, bool reduce)
		{
			this.numerator = numerator;
			this.denominator = denominator;

			if (reduce)
			{
				this.Reduce();
			}
		}

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets the numerator of the rational number
		/// </summary>
		public T Numerator
		{
			get { return this.numerator; }
			set { this.numerator = value; }
		}

		/// <summary>
		/// Gets and sets the denominator of the rational number
		/// </summary>
		public T Denominator
		{
			get { return this.denominator; }
			set { this.denominator = value; }
		}

		/// <summary>
		/// Gets the MaxValue for the given <typeparam name="T" />
		/// </summary>
		private static decimal MaxValue
		{
			get
			{
				if (Rational<T>.maxValue == default(decimal))
				{
					FieldInfo maxValue = typeof(T).GetField("MaxValue", BindingFlags.Static|BindingFlags.Public);
					if (maxValue != null)
					{
						try
						{
							Rational<T>.maxValue = Convert.ToDecimal(maxValue.GetValue(null));
						}
						catch (OverflowException)
						{
							Rational<T>.maxValue = Decimal.MaxValue;
						}
					}
					else
					{
						Rational<T>.maxValue = Int32.MaxValue;
					}
				}

				return Rational<T>.maxValue;
			}
		}

		#endregion Properties

		#region Parse Methods

		/// <summary>
		/// Approximate the decimal value accurate to a precision of 0.000001m
		/// </summary>
		/// <param name="value">decimal value to approximate</param>
		/// <returns>an approximation of the value as a rational number</returns>
		/// <remarks>
		/// http://stackoverflow.com/questions/95727
		/// </remarks>
		public static Rational<T> Approximate(decimal value)
		{
			return Rational<T>.Approximate(value, 0.000001m);
		}

		/// <summary>
		/// Approximate the decimal value accurate to a certain precision
		/// </summary>
		/// <param name="value">decimal value to approximate</param>
		/// <param name="epsilon">maximum precision to converge</param>
		/// <returns>an approximation of the value as a rational number</returns>
		/// <remarks>
		/// http://stackoverflow.com/questions/95727
		/// </remarks>
		public static Rational<T> Approximate(decimal value, decimal epsilon)
		{
			decimal numerator = 1m;
			decimal denominator = 1m;
			decimal fraction = 1m;
			decimal maxValue = Rational<T>.MaxValue;

			while (Math.Abs(fraction-value) > epsilon && (denominator < maxValue) && (numerator < maxValue))
			{
				if (fraction < value)
				{
					numerator++;
				}
				else
				{
					denominator++;

					decimal temp = Math.Round(Decimal.Multiply(value, denominator));
					if (temp > maxValue)
					{
						denominator--;
						break;
					}

					numerator = temp;
				}

				fraction = Decimal.Divide(numerator, denominator);
			}

			return new Rational<T>(
				(T)Convert.ChangeType(numerator, typeof(T)),
				(T)Convert.ChangeType(denominator, typeof(T)));
		}

		/// <summary>
		/// Converts the string representation of a number to its <see cref="Rational&lt;T&gt;"/> equivalent.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Rational<T> Parse(string value)
		{
			Rational<T> rational = new Rational<T>();

			if (String.IsNullOrEmpty(value))
			{
				return rational;
			}

			if (Rational<T>.Parser == null)
			{
				Rational<T>.Parser = Rational<T>.BuildParser();
			}

			string[] parts = value.Split(Rational<T>.DelimSet, 2, StringSplitOptions.RemoveEmptyEntries);
			rational.numerator = Rational<T>.Parser(parts[0]);
			if (parts.Length > 1)
			{
				rational.denominator = Rational<T>.Parser(parts[1]);
			}

			return rational;
		}

		/// <summary>
		/// Converts the string representation of a number to its <see cref="Rational&lt;T&gt;"/> equivalent.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="rational"></param>
		/// <returns></returns>
		public static bool TryParse(string value, out Rational<T> rational)
		{
			rational = new Rational<T>();

			if (String.IsNullOrEmpty(value))
			{
				return false;
			}

			if (Rational<T>.TryParser == null)
			{
				Rational<T>.TryParser = Rational<T>.BuildTryParser();
			}

			string[] parts = value.Split(Rational<T>.DelimSet, 2, StringSplitOptions.RemoveEmptyEntries);
			if (!Rational<T>.TryParser(parts[0], out rational.numerator))
			{
				return false;
			}
			if (parts.Length > 1)
			{
				if (!Rational<T>.TryParser(parts[1], out rational.denominator))
				{
					return false;
				}
			}

			return (parts.Length == 2);
		}

		private static Rational<T>.ParseDelegate BuildParser()
		{
			MethodInfo parse = typeof(T).GetMethod(
				"Parse",
				BindingFlags.Public|BindingFlags.Static,
				null,
				new Type[] { typeof(string) },
				null);

			if (parse == null)
			{
				throw new InvalidOperationException("Underlying Rational type T must support Parse in order to parse Rational<T>.");
			}

			return new Rational<T>.ParseDelegate(
				delegate(string value)
				{
					try
					{
						return (T)parse.Invoke(null, new object[] { value });
					}
					catch (TargetInvocationException ex)
					{
						if (ex.InnerException != null)
						{
							throw ex.InnerException;
						}
						throw;
					}
				});
		}

		private static Rational<T>.TryParseDelegate BuildTryParser()
		{
			// http://stackoverflow.com/questions/1933369

			MethodInfo tryParse = typeof(T).GetMethod(
				"TryParse",
				BindingFlags.Public|BindingFlags.Static,
				null,
				new Type[] { typeof(string), typeof(T).MakeByRefType() },
				null);

			if (tryParse == null)
			{
				throw new InvalidOperationException("Underlying Rational type T must support TryParse in order to try-parse Rational<T>.");
			}

			return new Rational<T>.TryParseDelegate(
				delegate(string value, out T output)
				{
					object[] args = new object[] { value, default(T) };
					try
					{
						bool success = (bool)tryParse.Invoke(null, args);
						output = (T)args[1];
						return success;
					}
					catch (TargetInvocationException ex)
					{
						if (ex.InnerException != null)
						{
							throw ex.InnerException;
						}
						throw;
					}
				});
		}

		#endregion Parse Methods

		#region Math Methods

		/// <summary>
		/// Finds the greatest common divisor and reduces the fraction by this amount.
		/// </summary>
		/// <returns>true if <see cref="Rational&lt;T&gt;" /> was reduced</returns>
		public bool Reduce()
		{
			bool reduced = false;
			decimal n = Convert.ToDecimal(numerator);
			decimal d = Convert.ToDecimal(denominator);

			// greatest common divisor
			decimal gcd = Rational<T>.GCD(n, d);
			if (gcd != Decimal.One && gcd != Decimal.Zero)
			{
				reduced = true;
				n /= gcd;
				d /= gcd;
			}

			// cancel out signs
			if (d < Decimal.Zero)
			{
				reduced = true;
				n = -n;
				d = -d;
			}

			if (reduced)
			{
				this.numerator = (T)Convert.ChangeType(n, typeof(T));
				this.denominator = (T)Convert.ChangeType(d, typeof(T));
			}

			return reduced;
		}

		/// <summary>
		/// Lowest Common Denominator
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		private static decimal LCD(decimal a, decimal b)
		{
			if (a == Decimal.Zero && b == Decimal.Zero)
			{
				return Decimal.Zero;
			}

			return (a * b) / Rational<T>.GCD(a, b);
		}

		/// <summary>
		/// Greatest Common Devisor
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		private static decimal GCD(decimal a, decimal b)
		{
			if (a < Decimal.Zero)
			{
				a = -a;
			}
			if (b < Decimal.Zero)
			{
				b = -b;
			}

			while (a != b)
			{
				if (a == Decimal.Zero)
				{
					return b;
				}
				if (b == Decimal.Zero)
				{
					return a;
				}

				if (a > b)
				{
					a %= b;
				}
				else
				{
					b %= a;
				}
			}
			return a;
		}

		#endregion Math Methods

		#region IConvertible Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public string ToString(IFormatProvider provider)
		{
			return String.Concat(
				this.Numerator.ToString(provider),
				Rational<T>.Delim,
				this.Denominator.ToString(provider));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public decimal ToDecimal(IFormatProvider provider)
		{
			try
			{
				return this.Numerator.ToDecimal(provider)/this.Denominator.ToDecimal(provider);
			}
			catch (InvalidCastException)
			{
				return ((IConvertible)this.Numerator.ToInt64(provider)).ToDecimal(provider) /
					((IConvertible)this.Denominator.ToInt64(provider)).ToDecimal(provider);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public double ToDouble(IFormatProvider provider)
		{
			return this.Numerator.ToDouble(provider)/this.Denominator.ToDouble(provider);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public float ToSingle(IFormatProvider provider)
		{
			return this.Numerator.ToSingle(provider)/this.Denominator.ToSingle(provider);
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return ((IConvertible)this.ToDecimal(provider)).ToBoolean(provider);
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return ((IConvertible)this.ToDecimal(provider)).ToByte(provider);
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			return ((IConvertible)this.ToDecimal(provider)).ToChar(provider);
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return ((IConvertible)this.ToDecimal(provider)).ToInt16(provider);
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return ((IConvertible)this.ToDecimal(provider)).ToInt32(provider);
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return ((IConvertible)this.ToDecimal(provider)).ToInt64(provider);
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return ((IConvertible)this.ToDecimal(provider)).ToSByte(provider);
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return ((IConvertible)this.ToDecimal(provider)).ToUInt16(provider);
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return ((IConvertible)this.ToDecimal(provider)).ToUInt32(provider);
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return ((IConvertible)this.ToDecimal(provider)).ToUInt64(provider);
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return new DateTime(((IConvertible)this).ToInt64(provider));
		}

		TypeCode IConvertible.GetTypeCode()
		{
			return this.Numerator.GetTypeCode();
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return Convert.ChangeType(this, conversionType, provider);
		}

		#endregion IConvertible Members

		#region IComparable Members

		/// <summary>
		/// Compares this instance to a specified System.Object.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object that)
		{
			return Convert.ToDecimal(this).CompareTo(Convert.ToDecimal(that));
		}

		#endregion IComparable Members

		#region IComparable<T> Members

		/// <summary>
		/// Compares this instance to another <typeparamref name="T"/> instance.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(T that)
		{
			return Decimal.Compare(Convert.ToDecimal(this), Convert.ToDecimal(that));
		}

		#endregion IComparable<T> Members

		#region Operators

		/// <summary>
		/// Negation
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static Rational<T> operator-(Rational<T> r)
		{
			T numerator = (T)Convert.ChangeType(-Convert.ToDecimal(r.numerator), typeof(T));
			return new Rational<T>(numerator, r.denominator);
		}

		/// <summary>
		/// Addition
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static Rational<T> operator+(Rational<T> r1, Rational<T> r2)
		{
			decimal r1n = Convert.ToDecimal(r1.numerator);
			decimal r1d = Convert.ToDecimal(r1.denominator);
			decimal r2n = Convert.ToDecimal(r2.numerator);
			decimal r2d = Convert.ToDecimal(r2.denominator);

			decimal denominator = Rational<T>.LCD(r1d, r2d);
			if (denominator > r1d)
			{
				r1n *= (denominator/r1d);
			}
			if (denominator > r2d)
			{
				r2n *= (denominator/r2d);
			}

			decimal numerator = r1n + r2n;

			return new Rational<T>((T)Convert.ChangeType(numerator, typeof(T)), (T)Convert.ChangeType(denominator, typeof(T)));
		}

		/// <summary>
		/// Subtraction
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static Rational<T> operator-(Rational<T> r1, Rational<T> r2)
		{
			return r1 + (-r2);
		}

		/// <summary>
		/// Multiplication
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static Rational<T> operator*(Rational<T> r1, Rational<T> r2)
		{
			decimal numerator = Convert.ToDecimal(r1.numerator) * Convert.ToDecimal(r2.numerator);
			decimal denominator = Convert.ToDecimal(r1.denominator) * Convert.ToDecimal(r2.denominator);

			return new Rational<T>((T)Convert.ChangeType(numerator, typeof(T)), (T)Convert.ChangeType(denominator, typeof(T)));
		}

		/// <summary>
		/// Division
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static Rational<T> operator/(Rational<T> r1, Rational<T> r2)
		{
			return r1 * new Rational<T>(r2.denominator, r2.numerator);
		}

		/// <summary>
		/// Less than
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static bool operator<(Rational<T> r1, Rational<T> r2)
		{
			return r1.CompareTo(r2) < 0;
		}

		/// <summary>
		/// Less than or equal to
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static bool operator<=(Rational<T> r1, Rational<T> r2)
		{
			return r1.CompareTo(r2) <= 0;
		}

		/// <summary>
		/// Greater than
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static bool operator>(Rational<T> r1, Rational<T> r2)
		{
			return r1.CompareTo(r2) > 0;
		}

		/// <summary>
		/// Greater than or equal to
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static bool operator>=(Rational<T> r1, Rational<T> r2)
		{
			return r1.CompareTo(r2) >= 0;
		}

		/// <summary>
		/// Equal to
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static bool operator==(Rational<T> r1, Rational<T> r2)
		{
			return r1.CompareTo(r2) == 0;
		}

		/// <summary>
		/// Not equal to
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static bool operator!=(Rational<T> r1, Rational<T> r2)
		{
			return r1.CompareTo(r2) != 0;
		}

		#endregion Operators

		#region Object Overrides

		public override string ToString()
		{
			return Convert.ToString(this);
		}

		public override bool Equals(object obj)
		{
			return (this.CompareTo(obj) == 0);
		}

		public override int GetHashCode()
		{
			// adapted from Anonymous Type: { uint Numerator, uint Denominator }
			int num = 0x1fb8d67d;
			num = (-1521134295 * num) + EqualityComparer<T>.Default.GetHashCode(this.Numerator);
			return ((-1521134295 * num) + EqualityComparer<T>.Default.GetHashCode(this.Denominator));
		}

		#endregion Object Overrides
	}
}
