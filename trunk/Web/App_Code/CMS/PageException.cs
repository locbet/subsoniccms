/*
 * SubSonic - http://subsonicproject.com
 * 
 * The contents of this file are subject to the Mozilla Public
 * License Version 1.1 (the "License"); you may not use this file
 * except in compliance with the License. You may obtain a copy of
 * the License at http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an 
 * "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or
 * implied. See the License for the specific language governing
 * rights and limitations under the License.
*/

using System;
using System.Runtime.Serialization;

namespace CMS
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PageNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageNotFoundException"/> class.
        /// </summary>
        public PageNotFoundException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public PageNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public PageNotFoundException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected PageNotFoundException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class PageNotAuthorizedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageNotAuthorizedException"/> class.
        /// </summary>
        public PageNotAuthorizedException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageNotAuthorizedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public PageNotAuthorizedException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageNotAuthorizedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public PageNotAuthorizedException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageNotAuthorizedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected PageNotAuthorizedException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) { }
    }

}