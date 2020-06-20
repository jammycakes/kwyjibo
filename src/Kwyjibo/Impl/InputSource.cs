using System;
using System.Collections;
using System.Linq;

namespace Kwyjibo.Impl
{
    public class InputSource : IInputSource
    {
        private readonly object[] _data;

        public InputSource(object[] data)
        {
            _data = data;
        }

        public IEnumerable GetData(Type serviceType)
        {
            return _data.Where(d => serviceType.IsInstanceOfType(d));
        }
    }
}
