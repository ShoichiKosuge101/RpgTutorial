using Controller;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Interface;
using Manager;
using MoveState;
using MoveState.Interface;
using UniRx;
using UnityEngine;

namespace PlayerState
{
    public class PlayerTurnState
        : IState
    {
        private readonly CompositeDisposable _disposable = new ();
        
        private readonly PlayerController _playerController = RpgGameManager.Instance.PlayerController;
        private readonly EnemyController _enemyController = RpgGameManager.Instance.EnemyController;
        
        public async UniTask EnterAsync()
        {
            Debug.Log("<color=green>PlayerTurnState Enter</color>");
            
            // ボタンのタップを購読
            // 毎回の購読解除はExitAsyncで行う
            UIManager.Instance.OnClickPlayerActionButton
                .ToUniTaskAsyncEnumerable()
                .SubscribeAwait(async _ =>
                {
                    await ChooseActionAsync(new AttackState(_playerController, _enemyController, true));
                })
                .AddTo(_disposable);
            
            UIManager.Instance.OnClickDefenseButton
                .ToUniTaskAsyncEnumerable()
                .SubscribeAwait(async _ =>
                {
                    await ChooseActionAsync(new DefenseState(_playerController, _enemyController, true));
                })
                .AddTo(_disposable);
            
            UIManager.Instance.OnClickHealButton
                .ToUniTaskAsyncEnumerable()
                .SubscribeAwait(async _ =>
                {
                    await ChooseActionAsync(new HealState(_playerController, _enemyController, true));
                })
                .AddTo(_disposable);
            
            UIManager.Instance.SetActivePlayerActionButtons(true);

            UIManager.Instance.ShowButtonWithTween();
            
            await UniTask.CompletedTask;
        }
        
        private static async UniTask ChooseActionAsync(IActionState actionState)
        {
            // 選択したので押せないようにする
            UIManager.Instance.SetActivePlayerActionButtons(false);
            
            await actionState.EnterAsync();
            
            await EndPlayerTurnAsync();
        }

        private static async UniTask EndPlayerTurnAsync()
        {
            await RpgGameManager.Instance.ChangeState(new EnemyTurnState());
        }

        public async UniTask ExecuteAsync()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask ExitAsync()
        {
            Debug.Log("<color=green>PlayerTurnState Exit</color>");
            
            _disposable.Clear();
            
            await UniTask.CompletedTask;
        }
    }
}