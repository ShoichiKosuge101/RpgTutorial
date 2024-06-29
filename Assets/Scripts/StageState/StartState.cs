using Cysharp.Threading.Tasks;
using Interface;
using Manager;
using PlayerState;
using UnityEngine;

namespace StageState
{
    public class StartState
        : IState
    {
        public StartState()
        {
        }
        
        public async UniTask EnterAsync()
        {
            Debug.Log("StartState Enter");
            RpgGameManager.Instance.ChangeState(new PlayerTurnState());
            
            await UniTask.CompletedTask;
        }

        public async UniTask ExecuteAsync()
        {
            Debug.Log("StartState Execute");
            
            await UniTask.CompletedTask;
        }

        public async UniTask ExitAsync()
        {
            Debug.Log("StartState Exit");
            
            await UniTask.CompletedTask;
        }
    }
}