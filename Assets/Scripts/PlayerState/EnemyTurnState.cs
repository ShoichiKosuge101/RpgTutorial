using Controller;
using Cysharp.Threading.Tasks;
using Interface;
using Manager;
using MoveState;
using MoveState.Interface;
using StageState;
using UnityEngine;
using Random = System.Random;

namespace PlayerState
{
    public class EnemyTurnState
        : IState
    {
        private readonly Random _random = new Random();

        public async UniTask EnterAsync()
        {
            Debug.Log("<color=green>EnemyTurnState Enter</color>");
            RpgGameManager.Instance.IncrementEnemyTurnCount();
            
            // 適当にwait
            await UniTask.Delay(1000);
            
            await ChooseActionAsync();
        }

        private async UniTask ChooseActionAsync()
        {
            int random = _random.Next(0, 3);
            IActionState actionState = random switch
            {
                0 => new AttackState(false),
                1 => new DefenseState(false),
                2 => new HealState(false),
                _ => new HealState(false)
            };

            await actionState.EnterAsync();
            
            // 敵のターン終了
            await EndEnemyTurnAsync();
        }
        
        /// <summary>
        /// 敵のターン終了
        /// </summary>
        private async UniTask EndEnemyTurnAsync()
        {
            await RpgGameManager.Instance.ChangeState(new TurnEndState(this));
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