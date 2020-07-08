using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Kwyjibo.Fluent;

namespace Kwyjibo.Impl
{
    public class Assertion<TData> : IConditionClause<TData>, IConditionOrSessionClause<TData>,
        IActionClause<TData>
    {
        private readonly bool _hasData;
        private readonly TData _data;
        private readonly IKwyjibo _kwyjibo;
        private Predicate<TData> _predicate;
        private bool _useSessionData;

        internal Assertion(IKwyjibo kwyjibo)
        {
            _kwyjibo = kwyjibo;
            _hasData = false;
            _useSessionData = true;
        }

        internal Assertion(IKwyjibo kwyjibo, TData data)
        {
            _kwyjibo = kwyjibo;
            _data = data;
            _hasData = true;
            _useSessionData = false;
        }

        public IActionClause<TData> Matches(Predicate<TData> predicate)
        {
            _predicate = predicate;
            return this;
        }

        public IConditionClause<TData> OrSession()
        {
            _useSessionData = true;
            return this;
        }

        public void Throw<TException>() where TException : Exception, new()
        {
            if (IsTriggered()) {
                throw new TException();
            }
        }

        public void Throw(Func<Exception> exceptionFactory)
        {
            if (IsTriggered()) {
                throw exceptionFactory();
            }
        }

        public void Wait(TimeSpan delay)
        {
            if (IsTriggered()) {
                Thread.Sleep(delay);
            }
        }

        public async Task WaitAsync(TimeSpan delay)
        {
            if (IsTriggered()) {
                await Task.Delay(delay).ConfigureAwait(false);
            }
        }

        public void Do(Action action)
        {
            if (IsTriggered()) {
                action();
            }
        }

        public async Task DoAsync(Func<Task> task)
        {
            if (IsTriggered()) {
                await task().ConfigureAwait(false);
            }
        }

        public bool IsTriggered()
        {
            var context = _kwyjibo.Context;
            if (context != null && !context.Enabled) {
                return false;
            }

            var session = _kwyjibo.Session;
            if (_hasData && _predicate(_data)) {
                return true;
            }

            if (_useSessionData) {
                foreach (var source in session.GetSources())
                foreach (var item in source.GetData(typeof(TData)).OfType<TData>()) {
                    if (_predicate(item)) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
