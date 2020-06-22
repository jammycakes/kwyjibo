using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kwyjibo.Impl
{
    public class InputSource : IInputSource
    {
        private readonly IList<object> _data;

        public InputSource(IList<object> data)
        {
            _data = data;
        }

        public IEnumerable<object> GetData(Type inputType)
        {
            return _data.Where(d => inputType.IsInstanceOfType(d));
        }
    }
}
