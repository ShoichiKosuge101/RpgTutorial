using Cysharp.Threading.Tasks;
using Interface;
using Manager;
using UnityEngine;

namespace StageState
{
    public class GameOverState
        : IState
    {
        public async UniTask EnterAsync()
        {
            Debug.Log("<color=red><b>GameOverState Enter</b></color>");
            
            RpgGameManager.Instance.SetGameOver();
            
            // ゲームオーバー処理
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