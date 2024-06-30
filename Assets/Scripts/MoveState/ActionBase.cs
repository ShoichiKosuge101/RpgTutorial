using Controller;
using Cysharp.Threading.Tasks;
using Manager;
using MoveState.Interface;

namespace MoveState
{
    public class ActionBase
        : IActionState
    {
        protected PlayerController PlayerController => RpgGameManager.Instance.PlayerController;
        protected EnemyController EnemyController => RpgGameManager.Instance.EnemyController;
        
        public virtual async UniTask EnterAsync()
        {
            await UniTask.CompletedTask;
        }

        public virtual async UniTask ExecuteAsync()
        {
            await UniTask.CompletedTask;
        }

        public virtual async UniTask ExitAsync()
        {
            await UniTask.CompletedTask;
        }
    }
}