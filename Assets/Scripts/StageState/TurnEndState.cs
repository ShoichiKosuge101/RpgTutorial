using Cysharp.Threading.Tasks;
using Interface;
using Manager;
using PlayerState;
using UnityEngine;

namespace StageState
{
    public class TurnEndState
        : IState
    {
        private readonly IState _previousState;

        public TurnEndState(in IState previousState)
        {
            _previousState = previousState;
        }
        
        public async UniTask EnterAsync()
        {
            Debug.Log("TurnEndState Enter");
            
            // ターンエンド処理
            await UniTask.Delay(500);

            if (_previousState is PlayerTurnState)
            {
                await RpgGameManager.Instance.ChangeState(new EnemyTurnState());
            }
            else if (_previousState is EnemyTurnState)
            {
                await RpgGameManager.Instance.ChangeState(new PlayerTurnState());
            }
        }

        public async UniTask ExecuteAsync()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask ExitAsync()
        {
            Debug.Log("TurnEndState Exit");
            
            await UniTask.CompletedTask;
        }
    }
}