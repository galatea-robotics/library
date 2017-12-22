/*
 *  LSH 12/22/2017
 */

namespace System
{
    public static class FormattableStringExtension
    {
        public static string CurrentCultureFormat(FormattableString formattable)
        {
            if (formattable == null)
            {
                throw new ArgumentNullException("formattable");
            }
            return formattable.ToString(Globalization.CultureInfo.CurrentCulture);
        }

        public static string CurrentUICultureFormat(FormattableString formattable)
        {
            if (formattable == null)
            {
                throw new ArgumentNullException("formattable");
            }
            return formattable.ToString(Globalization.CultureInfo.CurrentUICulture);
        }

        public static string InvariantCultureFormat(FormattableString formattable)
        {
            if (formattable == null)
            {
                throw new ArgumentNullException("formattable");
            }
            return formattable.ToString(Globalization.CultureInfo.InvariantCulture);
        }
    }
}