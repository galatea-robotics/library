// Accord Imaging Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © Diego Catalano, 2013
// diego.catalano at live.com
//
// Copyright © César Souza, 2009-2013
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

namespace Accord.Imaging
{
	public partial class HaralickDescriptorDictionary
    {
        /// <summary>
        ///    Initializes a new instance of the <see cref="HaralickDescriptorDictionary"/>
        ///    class with serialized data.
        /// </summary>
        /// 
        /// <param name="info">A <see cref="System.Runtime.Serialization.SerializationInfo"/>
        ///   object containing the information required to serialize this 
        ///   <see cref="HaralickDescriptorDictionary"/>.</param>
        /// <param name="context">A <see cref="System.Runtime.Serialization.StreamingContext"/>
        ///    structure containing the source and destination of the serialized stream
        ///    associated with this <see cref="HaralickDescriptorDictionary"/>.</param>
        /// 
        protected HaralickDescriptorDictionary(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}