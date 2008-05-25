#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2005-2006 Stephen M. McKamey

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

namespace ExifUtils
{
	/// <summary>
	/// Represents a rational number.
	/// </summary>
	[Serializable]
	public struct Rational<T> : IConvertible
		where T : IConvertible
	{
		#region Fields

		private T numerator;
		private T denominator;

		#endregion Fields

		#region Init

		/// <summary>
		/// Initializes a new instance of the <see cref="ExifUtils.Rational{T}"/> class.
		/// </summary>
		/// <param name="numerator">The numerator of the rational number.</param>
		/// <param name="denominator">The denominator of the rational number.</param>
		public Rational(T numerator, T denominator)
		{
			bool reduced = false;
			decimal n = Convert.ToDecimal(numerator);
			decimal d = Convert.ToDecimal(denominator);

			decimal gcd = GCD(n, d);
			if (gcd != 1m && gcd != 0m)
			{
				reduced = true;
				n /= gcd;
				d /= gcd;
			}

			if (d < 0m)
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
			else
			{
				this.numerator = numerator;
				this.denominator = denominator;
			}
		}

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets the numerator of the rational number.
		/// </summary>
		public T Numerator
		{
			get { return this.numerator; }
			set { this.numerator = value; }
		}

		/// <summary>
		/// Gets the denominator of the rational number.
		/// </summary>
		public T Denominator
		{
			get { return this.denominator; }
			set { this.denominator = value; }
		}

		#endregion Properties

		#region Operators

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

			decimal denominator = LCD(r1d, r2d);
			if (denominator > r1d)
				r1n *= (denominator/r1d);
			if (denominator > r2d)
				r2n *= (denominator/r2d);

			decimal numerator = r1n + r2n;

			return new Rational<T>((T)Convert.ChangeType(numerator, typeof(T)), (T)Convert.ChangeType(denominator, typeof(T)));
		}

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

		#endregion Operators

		#region Object Overrides

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Convert.ToString(this);
		}

		#endregion Object Overrides

		#region IConvertible Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public string ToString(IFormatProvider provider)
		{
			return String.Format("{0}/{1}", this.Numerator.ToString(provider), this.Denominator.ToString(provider));
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

		#region Math Methods

		private static decimal LCD(decimal a, decimal b)
		{
			if (a == 0m && b == 0m)
				return 0m;

			return (a*b)/GCD(a, b);
		}

		private static decimal GCD(decimal a, decimal b)
		{
			if (a < 0m)
				a = -a;
			if (b < 0m)
				b = -b;

			while (a != b)
			{
				if (a == 0m)
					return b;
				if (b == 0m)
					return a;

				if (a > b)
					a %= b;
				else
					b %= a;
			}
			return a;
		}

		#endregion Math Methods
	}
}
