﻿using System;

namespace NinjaTurtles.AppDomainIsolation
{
    /// <summary>
    /// Isolated class provide a way to easily instantiate object
    /// in a different AppDomain than the calling code
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Isolated<T> : IDisposable where T : MarshalByRefObject
    {
        private AppDomain _domain;
        private T _instance;

        /// <summary>
        /// Instantiate a new isolated T in a AppDomain defined with a guid
        /// </summary>
        public Isolated()
        {
            _domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(), null,
                AppDomain.CurrentDomain.SetupInformation);
            var instanceType = typeof (T);
            _instance = (T) _domain.CreateInstanceAndUnwrap(instanceType.Assembly.FullName, instanceType.FullName);
        }

        /// <summary>
        /// Instantiate a new isolated T in a AppDomain defined with the idx parameter
        /// </summary>
        public Isolated(int idx)
        {
            _domain = AppDomain.CreateDomain("Isolated:" + idx, null,
                AppDomain.CurrentDomain.SetupInformation);
            var instanceType = typeof(T);
            _instance = (T)_domain.CreateInstanceAndUnwrap(instanceType.Assembly.FullName, instanceType.FullName);
        }

        /// <summary>
        /// Access the internal T instance
        /// </summary>
        public T Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Unload the associated AppDomain
        /// </summary>
        public void Dispose()
        {
            if (_domain != null)
            {
                AppDomain.Unload(_domain);
                _domain = null;
            }
        }
    }
}
