using Cysharp.Threading.Tasks;

namespace MoveState.Interface
{
    public interface IActionState
    {
        UniTask Enter();
        UniTask Execute();
        UniTask Exit();
    }
}