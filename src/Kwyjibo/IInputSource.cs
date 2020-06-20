using System;
using System.Collections;
using System.Collections.Generic;

namespace Kwyjibo
{
    public interface IInputSource
    {
        IEnumerable<object> GetData(Type inputType);
    }
}
