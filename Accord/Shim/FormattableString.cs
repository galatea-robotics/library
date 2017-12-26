/*
 *  LSH 12/23/2017
 */

using System.Globalization;

namespace System
{
    [__DynamicallyInvokable]
    public abstract class FormattableString : IFormattable
    {
        [__DynamicallyInvokable]
        public abstract int ArgumentCount
        {
            [__DynamicallyInvokable]
            get;
        }

        [__DynamicallyInvokable]
        public abstract string Format
        {
            [__DynamicallyInvokable]
            get;
        }

        [__DynamicallyInvokable]
        protected FormattableString()
        {
        }

        [__DynamicallyInvokable]
        public abstract object GetArgument(int index);

        [__DynamicallyInvokable]
        public abstract object[] GetArguments();

        [__DynamicallyInvokable]
        public static string Invariant(FormattableString formattable)
        {
            if (formattable == null)
            {
                throw new ArgumentNullException("formattable");
            }
            return formattable.ToString(CultureInfo.InvariantCulture);
        }

        [__DynamicallyInvokable]
        string System.IFormattable.ToString(string ignored, IFormatProvider formatProvider)
        {
            return this.ToString(formatProvider);
        }

        [__DynamicallyInvokable]
        public abstract string ToString(IFormatProvider formatProvider);

        [__DynamicallyInvokable]
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentCulture);
        }
    }
}