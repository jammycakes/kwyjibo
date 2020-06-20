using System;
using System.Collections.Generic;

namespace Kwyjibo.Extensions.DependencyInjection
{
    public class KwyjiboDIOptions : KwyjiboOptions
    {
        private IList<Type> _inputs = new List<Type>();

        public KwyjiboOptions AddInput<TService>() where TService : class
        {
            _inputs.Add(typeof(TService));
            return this;
        }

        internal IList<Type> GetInputs() => _inputs;
    }
}
