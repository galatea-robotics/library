// Accord Core Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2015
// cesarsouza at gmail.com
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
// Based on the BiDictionary implementation by Alexander Prokhorov. Available
// under the public domain at GitHub, under the project name Alba.Framework.
// https://github.com/Athari/Alba.Framework/blob/master/Alba.Framework/Collections/Collections/BiDictionary(TFirst%2CTSecond).cs
//

namespace Accord.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Linq;

    /// <summary>
    ///   Two-way dictionary for efficient lookups by both key and value. This
    ///   can be used to represent a one-to-one relation among two object types.
    /// </summary>
    /// 
    /// <typeparam name="TFirst">The type of right keys in the dictionary.</typeparam>
    /// <typeparam name="TSecond">The type of left keys in the dictionary.</typeparam>
    /// 
    public sealed partial class TwoWayDictionary<TFirst, TSecond>
    {
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            secondToFirst.Clear();
            foreach (var item in firstToSecond)
                secondToFirst.Add(item.Value, item.Key);
        }
    }
}
