using System.Collections.Generic;

namespace Kwyjibo
{
    public interface ISession
    {
        IContext GetContext(string context);

        IEnumerable<IInputSource> GetSources();
    }
}
