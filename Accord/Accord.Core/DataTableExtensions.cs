// Accord.Core Library
// The Portable Accord.NET Framework
// https://github.com/cureos/accord
//
// Copyright © Anders Gustafsson, 2013-2014
// anders at cureos dot com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord
{
    using System;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// DataTable support methods designed specifically to avoid the use of various DataTable.Select
    /// constructs in the Portable Accord implementation.
    /// </summary>
    internal static class DataTableExtensions
    {
        /// <summary>
        /// Extract rows from a specific column where the column value is equal to the specified <paramref name="columnValue"/>.
        /// </summary>
        /// <typeparam name="T">Type of <paramref name="columnValue"/>.</typeparam>
        /// <param name="table">Data table subject to extraction.</param>
        /// <param name="columnName">Name of column in which selection should be made.</param>
        /// <param name="columnValue">Value that should be used to filter out data rows of interest.</param>
        /// <returns>Data rows where the value in the named column is equal to the specified <paramref name="columnValue"/>.</returns>
        internal static DataRow[] GetRows<T>(this DataTable table, string columnName, T columnValue)
        {
            return
                table.Rows.Cast<DataRow>()
                    .Where(row => columnValue.Equals((T)Convert.ChangeType(row[columnName], typeof(T))))
                    .ToArray();
        }

        /// <summary>
        /// Get the minimum value from a named column.
        /// </summary>
        /// <param name="table">Data table subject to analysis.</param>
        /// <param name="columnName">Name of column where minimum should be identified.</param>
        /// <returns>Minimum value from a named column in the specified data table.</returns>
        internal static object GetMin(this DataTable table, string columnName)
        {
            return table.Rows.Cast<DataRow>().Select(row => row[columnName]).Min();
        }

        /// <summary>
        /// Get the maximum value from a named column.
        /// </summary>
        /// <param name="table">Data table subject to analysis.</param>
        /// <param name="columnName">Name of column where maximum should be identified.</param>
        /// <returns>Maximum value from a named column in the specified data table.</returns>
        internal static object GetMax(this DataTable table, string columnName)
        {
            return table.Rows.Cast<DataRow>().Select(row => row[columnName]).Max();
        }

        /// <summary>
        /// Get the average value for a named column.
        /// </summary>
        /// <param name="table">Data table subject to analysis.</param>
        /// <param name="columnName">Name of column where average should be computed.</param>
        /// <returns>Average value for a named column in the specified data table.</returns>
        internal static double GetAverage(this DataTable table, string columnName)
        {
            return table.Rows.Cast<DataRow>().Select(row => Convert.ToDouble(row[columnName])).Average();
        }

        /// <summary>
        /// Get the standard deviation value for a named column.
        /// </summary>
        /// <param name="table">Data table subject to analysis.</param>
        /// <param name="columnName">Name of column where standard deviation should be computed.</param>
        /// <returns>Standard deviation value for a named column in the specified data table.</returns>
        internal static double GetStdev(this DataTable table, string columnName)
        {
            var n = table.Rows.Count;
            if (n <= 1) return 0.0;

            var k = table.Rows.Cast<DataRow>().Select(row => System.Math.Pow(Convert.ToDouble(row[columnName]), 2.0)).Sum();
            var s = table.Rows.Cast<DataRow>().Select(row => Convert.ToDouble(row[columnName])).Sum();
            return System.Math.Sqrt((k - s * s / n) / (n - 1.0));
        }
    }
}