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

        public PlayerTurnState()
        {
            _disposable.Clear();
            
            // ボタンのタップを購読
            UIManager.Instance.OnClickPlayerActionButton
                .ToUniTaskAsyncEnumerable()
                .SubscribeAwait(async _ =>
                {
                    await ChooseAction(new AttackState(_playerController, _enemyController, true));
                })
                .AddTo(_disposable);
            
            UIManager.Instance.OnClickDefenseButton
                .ToUniTaskAsyncEnumerable()
                .SubscribeAwait(async _ =>
                {
                    await ChooseAction(new DefenseState(_playerController, _enemyController, true));
                })
                .AddTo(_disposable);
            
            UIManager.Instance.OnClickHealButton
                .ToUniTaskAsyncEnumerable()
                .SubscribeAwait(async _ =>
                {
                    await ChooseAction(new HealState(_playerController, _enemyController, true));
                })
                .AddTo(_disposable);
        }
        
        public async UniTask EnterAsync()
        {
            Debug.Log("<color=green>PlayerTurnState Enter</color>");
            
            UIManager.Instance.SetActivePlayerActionButtons(true);
            
            await UniTask.CompletedTask;
        }
        
        private async UniTask ChooseAction(IActionState actionState)
        {
            await actionState.Enter();
            
            EndPlayerTurn();
        }

        private static void EndPlayerTurn()
        {
            RpgGameManager.Instance.ChangeState(new EnemyTurnState());
        }

        public async UniTask ExecuteAsync()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask ExitAsync()
        {
            Debug.Log("<color=green>PlayerTurnState Exit</color>");
            
            UIManager.Instance.SetActivePlayerActionButtons(false);
            _disposable.Clear();
            
            await UniTask.CompletedTask;
        }
    }
}