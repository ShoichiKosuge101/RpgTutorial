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
            ChooseAction();
            
            await UniTask.CompletedTask;
        }

        private void ChooseAction()
        {
            int random = _random.Next(0, 3);
            IActionState actionState = default;
            switch (random)
            {
                case 0:
                    actionState = new AttackState(_playerController, _enemyController, false);
                    break;
                case 1:
                    actionState = new DefenseState(_playerController, _enemyController, false);
                    break;
                case 2:
                default:
                    actionState = new HealState(_playerController, _enemyController, false);
                    break;
            }
            actionState.Enter();
            
            // 敵のターン終了
            EndEnemyTurn();
        }
        
        /// <summary>
        /// 敵のターン終了
        /// </summary>
        private static void EndEnemyTurn()
        {
            RpgGameManager.Instance.ChangeState(new PlayerTurnState());
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