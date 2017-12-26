using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel
{
    /// <summary>Provides data for the <see cref="E:System.ComponentModel.BackgroundWorker.ProgressChanged" /> event.</summary>
    public class ProgressChangedEventArgs : EventArgs
    {
        private readonly int progressPercentage;

        private readonly object userState;

        /// <summary>Gets the asynchronous task progress percentage.</summary>
        /// <returns>A percentage value indicating the asynchronous task progress.</returns>
        [Description("Async_ProgressChangedEventArgs_ProgressPercentage")]
        public int ProgressPercentage
        {
            get
            {
                return this.progressPercentage;
            }
        }

        /// <summary>Gets a unique user state.</summary>
        /// <returns>A unique <see cref="T:System.Object" /> indicating the user state.</returns>
        [Description("Async_ProgressChangedEventArgs_UserState")]
        public object UserState
        {
            get
            {
                return this.userState;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ProgressChangedEventArgs" /> class.</summary>
        /// <param name="progressPercentage">The percentage of an asynchronous task that has been completed.</param>
        /// <param name="userState">A unique user state.</param>
        public ProgressChangedEventArgs(int progressPercentage, object userState)
        {
            this.progressPercentage = progressPercentage;
            this.userState = userState;
        }
    }
}
