﻿/*
 *  Copyright (c) 2013-2016, Cureos AB.
 *  All rights reserved.
 *  http://www.cureos.com
 *
 *	This file is part of Shim.
 *
 *  Shim is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  Shim is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with Shim. If not, see <http://www.gnu.org/licenses/>.
 */

namespace System.Threading.Tasks
{
    /// <include file='../../_Doc/mscorlib.xml' path='doc/members/member[@name="T:System.Threading.Tasks.ParallelLoopResult"]/*' />
    public struct ParallelLoopResult
    {
        #region PROPERTIES

        /// <include file='../../_Doc/mscorlib.xml' path='doc/members/member[@name="P:System.Threading.Tasks.ParallelLoopResult.IsCompleted"]/*' />
        public bool IsCompleted { get; internal set; }

        /// <include file='../../_Doc/mscorlib.xml' path='doc/members/member[@name="P:System.Threading.Tasks.ParallelLoopResult.LowestBreakIteration"]/*' />
        public long? LowestBreakIteration { get; internal set; }

        #endregion
    }
}