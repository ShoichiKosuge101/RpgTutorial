using Controller;
using Cysharp.Threading.Tasks;
using Interface;
using MoveState.Interface;
using UnityEngine;

namespace MoveState
{
    public class DefenseState
        : ActionBase
    {
        private readonly bool _isPlayer;
        
        public DefenseState(
            bool isPlayer
            )
        {
            _isPlayer = isPlayer;
        }
        
        public override async UniTask EnterAsync()
        {
            Debug.Log("<color=cyan>DefenseState Enter</color>");
            if(_isPlayer)
            {
                // nullチェック
                if (PlayerController == null)
                {
                    Debug.Log("Player is Empty");
                    return;
                }
                
                await PlayerController.DefendAsync();
            }
            else
            {
                // nullチェック
                if (EnemyController == null)
                {
                    Debug.Log("Enemy is Empty");
                    return;
                }
                
                await EnemyController.DefendAsync();
            }
        }

        public override async UniTask ExitAsync()
        {
            Debug.Log("<color=cyan>DefenseState Exit</color>");
            await UniTask.CompletedTask;
        }
    }
}