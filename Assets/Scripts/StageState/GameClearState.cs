using Cysharp.Threading.Tasks;
using Interface;
using Manager;
using UnityEngine;

namespace StageState
{
    public class GameClearState
        : IState
    {
        public async UniTask EnterAsync()
        {
            Debug.Log("<color=green><b>GameClearState Enter</b></color>");
            
            RpgGameManager.Instance.SetGameOver();
            
            // ゲームクリア処理
            await UniTask.CompletedTask;
        }

        public async UniTask ExecuteAsync()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask ExitAsync()
        {
            await UniTask.CompletedTask;
        }
    }
}