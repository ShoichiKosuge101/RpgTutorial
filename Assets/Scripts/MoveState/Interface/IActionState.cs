using Cysharp.Threading.Tasks;

namespace MoveState.Interface
{
    public interface IActionState
    {
        UniTask EnterAsync();
        UniTask ExecuteAsync();
        UniTask ExitAsync();
    }
}