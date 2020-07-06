using System.Threading.Tasks;

namespace Kwyjibo
{
    public interface IAssertion
    {
        void Handle(params object[] data);

        Task HandleAsync(params object[] data);
    }
}
