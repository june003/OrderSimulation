//-----------------------------------------------------------------------------
// File Name   : Subscription
// Author      : junlei
// Date        : 6/13/2020 6:38:24 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using System;
using System.Reflection;

namespace OrderSimulation.Handler
{
    public class Subscription<Tmessage> : IDisposable
    {
        private readonly WeakReference _targetObjet;
        private readonly bool _isStatic;
        private readonly MethodInfo _methodInfo;
        private readonly EventAggregator _eventAggregator;
        private bool _isDisposed;
        public Subscription(Action<Tmessage> action, EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _methodInfo = action.Method;
            if (action.Target == null)
            {
                _isStatic = true;
            }

            _targetObjet = new WeakReference(action.Target);
        }

        public Action<Tmessage> CreatAction()
        {
            if (_targetObjet.Target != null && _targetObjet.IsAlive)
                return (Action<Tmessage>)Delegate.CreateDelegate(typeof(Action<Tmessage>), _targetObjet.Target, _methodInfo);

            if (this._isStatic)
                return (Action<Tmessage>)Delegate.CreateDelegate(typeof(Action<Tmessage>), _methodInfo);

            return null;
        }

        ~Subscription()
        {
            if (!_isDisposed)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            _eventAggregator.UnSbscribe(this);
            _isDisposed = true;
        }
    }
}
