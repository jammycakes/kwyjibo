using System;
using System.Collections;

namespace Kwyjibo
{
    public interface IInputSource
    {
        IEnumerable GetData(Type serviceType);
    }
}
