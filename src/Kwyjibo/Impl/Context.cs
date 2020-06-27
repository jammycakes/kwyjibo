using System.Collections.Generic;

namespace Kwyjibo.Impl
{
    public class Context : IContext
    {
        public string FullName { get; }

        public string Name { get; }

        public IContext Parent { get; }

        public Status Status { get; internal set; }
            = Status.Inherit;

        public bool Enabled =>
            Status == Status.Enabled ||
            Status == Status.Inherit && (Parent?.Enabled ?? false);

        public void Handle(string handler, IList<IInputSource> sources)
        {
            throw new System.NotImplementedException();
        }

        public Context(IContext parent, string fullName, string name)
        {
            Parent = parent;
            FullName = fullName;
            Name = name;
        }
    }
}
