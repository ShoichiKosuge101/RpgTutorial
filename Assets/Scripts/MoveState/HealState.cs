using Controller;
using Cysharp.Threading.Tasks;
using Interface;
using MoveState.Interface;
using UnityEngine;

namespace MoveState
{
    public class HealState
        : ActionBase
    {
        private readonly bool _isPlayer;

        public HealState(
            bool isPlayer
        )
        {
            _isPlayer = isPlayer;
        }

        public override async UniTask EnterAsync()
        {
            Debug.Log("<color=cyan>AttackState Enter</color>");

            if(_isPlayer)
            {
                // nullチェック
                if (PlayerController == null)
                {
                    Debug.Log("Player is Empty");
                    return;
                }
                
                await PlayerController.HealAsync();
            }
            else
            {
                // nullチェック
                if (EnemyController == null)
                {
                    Debug.Log("Enemy is Empty");
                    return;
                }
                
                await EnemyController.HealAsync();
            }
        }

        public override async UniTask ExecuteAsync()
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask ExitAsync()
        {
            Debug.Log("<color=cyan>AttackState Exit</color>");
            
            await UniTask.CompletedTask;
        }
    }
}