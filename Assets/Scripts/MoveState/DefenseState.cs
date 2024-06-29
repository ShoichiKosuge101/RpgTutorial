using Controller;
using Cysharp.Threading.Tasks;
using Interface;
using MoveState.Interface;
using UnityEngine;

namespace MoveState
{
    public class DefenseState
        : IActionState
    {
        private readonly PlayerController _playerController;
        private readonly EnemyController _enemyController;
        private readonly bool _isPlayer;
        
        public DefenseState(
            PlayerController playerController, 
            EnemyController enemyController, 
            bool isPlayer
            )
        {
            _playerController = playerController;
            _enemyController = enemyController;
            _isPlayer = isPlayer;
        }
        
        public async UniTask Enter()
        {
            Debug.Log("<color=cyan>DefenseState Enter</color>");
            if(_isPlayer)
            {
                // nullチェック
                if (_playerController == null)
                {
                    Debug.Log("Player is Empty");
                    return;
                }
                
                await _playerController.DefendAsync(_playerController);
            }
            else
            {
                // nullチェック
                if (_enemyController == null)
                {
                    Debug.Log("Enemy is Empty");
                    return;
                }
                
                await _enemyController.DefendAsync(_playerController);
            }
        }

        public async UniTask Execute()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            Debug.Log("<color=cyan>DefenseState Exit</color>");
            await UniTask.CompletedTask;
        }
    }
}