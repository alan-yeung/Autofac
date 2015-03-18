﻿// This software is part of the Autofac IoC container
// Copyright © 2011 Autofac Contributors
// http://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using Autofac.Util;

namespace Autofac.Core
{
    /// <summary>
    /// Fired after the construction of an instance but before that instance
    /// is shared with any other or any members are invoked on it.
    /// </summary>
    public class ActivatingEventArgs<T> : EventArgs, IActivatingEventArgs<T>
    {
        T _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivatedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="component">The component.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="instance">The instance.</param>
        public ActivatingEventArgs(IComponentContext context, IComponentRegistration component, IEnumerable<Parameter> parameters, T instance)
        {
            Context = Enforce.ArgumentNotNull(context, "context");
            Component = Enforce.ArgumentNotNull(component, "component");
            Parameters = Enforce.ArgumentNotNull(parameters, "parameters");
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            _instance = instance;
        }

        /// <summary>
        /// The context in which the activation occurred.
        /// </summary>
        public IComponentContext Context { get; }

        /// <summary>
        /// The component providing the instance.
        /// </summary>
        public IComponentRegistration Component { get; }

        /// <summary>
        /// The instance that will be used to satisfy the request.
        /// </summary>
        /// <remarks>
        /// The instance can be replaced if needed, e.g. by an interface proxy.
        /// </remarks>
        public T Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _instance = value;
            }
        }

        /// <summary>
        /// The instance can be replaced if needed, e.g. by an interface proxy.
        /// </summary>
        /// <param name="instance">The object to use instead of the activated instance.</param>
        public void ReplaceInstance(object instance)
        {
            Instance = (T)instance;
        }

        /// <summary>
        /// The parameters supplied to the activator.
        /// </summary>
        public IEnumerable<Parameter> Parameters { get; }
    }
}
