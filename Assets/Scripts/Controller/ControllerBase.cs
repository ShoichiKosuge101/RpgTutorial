using Controller.Interface;
using Cysharp.Threading.Tasks;
using Manager;
using UnityEngine;
using Utils;

namespace Controller
{
    /// <summary>
    /// コントローラーベース
    /// </summary>
    public class ControllerBase
        : MonoBehaviour, IController
    {
        public BaseParam BaseParam { get; internal set; }
        public BaseParam CurrentParam { get; internal set; }

        public virtual async UniTask AttackAsync(ControllerBase target)
        {
            await UniTask.DelayFrame(1);
        }
        
        public virtual async UniTask DefendAsync(ControllerBase target)
        {
            await UniTask.DelayFrame(1);
        }
        
        public virtual async UniTask HealAsync(ControllerBase target)
        {
            bool isPlayer = this is PlayerController;
            
            // 回復処理
            // 回復力 + 1~3のランダム値
            int healVal = CurrentParam.HealPower + Random.Range(1, 4);
            CurrentParam.Hp = Mathf.Min(CurrentParam.Hp + healVal, BaseParam.Hp);
            // パラメータ情報を表示
            SendStatus(CurrentParam, isPlayer);

            string textPattern = isPlayer 
                ? $"<color=yellow> Player Heal: {healVal}</color>" 
                : $"<color=red>Enemy Heal: {healVal}</color>";
            SendLog($"{textPattern}");

            // 適当にawait
            if (isPlayer)
            {
                await UniTask.WaitForSeconds(2);
            }
        }
        
        public virtual async UniTask TakeDamage(int damage)
        {
            await UniTask.DelayFrame(1);
        }

        public virtual void SendStatus(BaseParam baseParam, bool isPlayer)
        {
            RpgGameManager.Instance.SetStatus(baseParam, isPlayer);
        }

        /// <summary>
        /// ログウィンドウに出力
        /// </summary>
        /// <param name="message"></param>
        public virtual void SendLog(in string message)
        {
            RpgGameManager.Instance.SendLog(message);
        }
    }
}