using System;
using System.Collections.Generic;

namespace Kwyjibo
{
    public class KwyjiboDIOptions : KwyjiboOptions
    {
        private IList<Type> _inputs = new List<Type>();

        public KwyjiboDIOptions AddInput<TService>() where TService : class
        {
            _inputs.Add(typeof(TService));
            return this;
        }

        internal IList<Type> GetInputs() => _inputs;
    }
}
