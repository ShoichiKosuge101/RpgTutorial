using Controller;
using Cysharp.Threading.Tasks;
using Interface;
using Manager;
using MoveState;
using MoveState.Interface;
using UnityEngine;
using Random = System.Random;

namespace PlayerState
{
    public class EnemyTurnState
        : IState
    {
        private readonly Random _random = new Random();
        private readonly PlayerController _playerController = RpgGameManager.Instance.PlayerController;
        private readonly EnemyController _enemyController = RpgGameManager.Instance.EnemyController;

        public async UniTask EnterAsync()
        {
            Debug.Log("<color=green>EnemyTurnState Enter</color>");
            
            // 適当にwait
            await UniTask.Delay(1000);
            
            await ChooseActionAsync();
        }

        private async UniTask ChooseActionAsync()
        {
            int random = _random.Next(0, 3);
            IActionState actionState = random switch
            {
                0 => new AttackState(_playerController, _enemyController, false),
                1 => new DefenseState(_playerController, _enemyController, false),
                2 => new HealState(_playerController, _enemyController, false),
                _ => new HealState(_playerController, _enemyController, false)
            };

            await actionState.EnterAsync();
            
            // 敵のターン終了
            await EndEnemyTurnAsync();
        }
        
        /// <summary>
        /// 敵のターン終了
        /// </summary>
        private static async UniTask EndEnemyTurnAsync()
        {
            await RpgGameManager.Instance.ChangeState(new PlayerTurnState());
        }

        public async UniTask ExecuteAsync()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask ExitAsync()
        {
            Debug.Log("<color=green>EnemyTurnState Exit</color>");
            await UniTask.CompletedTask;
        }
    }
}