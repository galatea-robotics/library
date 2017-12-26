using System;

namespace System.Runtime.CompilerServices
{
    [__DynamicallyInvokable]
    public static class FormattableStringFactory
    {
        [__DynamicallyInvokable]
        public static FormattableString Create(string format, params object[] arguments)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }
            return new FormattableStringFactory.ConcreteFormattableString(format, arguments);
        }

        private sealed class ConcreteFormattableString : FormattableString
        {
            private readonly string _format;

            private readonly object[] _arguments;

            public override int ArgumentCount
            {
                get
                {
                    return (int)this._arguments.Length;
                }
            }

            public override string Format
            {
                get
                {
                    return this._format;
                }
            }

            internal ConcreteFormattableString(string format, object[] arguments)
            {
                this._format = format;
                this._arguments = arguments;
            }

            public override object GetArgument(int index)
            {
                return this._arguments[index];
            }

            public override object[] GetArguments()
            {
                return this._arguments;
            }

            public override string ToString(IFormatProvider formatProvider)
            {
                return string.Format(formatProvider, this._format, this._arguments);
            }
        }
    }
}