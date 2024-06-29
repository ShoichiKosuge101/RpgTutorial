using Cysharp.Threading.Tasks;

namespace Interface
{
    public interface IState
    {
        UniTask EnterAsync();
        UniTask ExecuteAsync();
        UniTask ExitAsync();
    }
}

