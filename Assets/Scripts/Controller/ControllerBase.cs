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
        /// <summary>
        /// ベースパラメータ
        /// </summary>
        public BaseParam BaseParam { get; internal set; }
        
        /// <summary>
        /// 現在のパラメータ
        /// </summary>
        public BaseParam CurrentParam { get; internal set; }
        
        /// <summary>
        /// 防御状態かどうか
        /// </summary>
        internal bool IsDefend { get; set; }
        
        /// <summary>
        /// 防御力バフ
        /// </summary>
        internal int DefenseBuff { get; set; }
        
        /// <summary>
        /// 防御状態の残りターン数
        /// </summary>
        internal int RemainDefendTurn { get; set; } = 0;

        /// <summary>
        /// 攻撃処理
        /// </summary>
        /// <param name="target"></param>
        public virtual async UniTask AttackAsync(ControllerBase target)
        {
            await UniTask.DelayFrame(1);
        }
        
        /// <summary>
        /// 防御処理
        /// </summary>
        public virtual async UniTask DefendAsync()
        {
            if (IsDefend)
            {
                // 既にバフがかかっていたらターン数とバフ値を更新
                CurrentParam.Defense -= DefenseBuff;
            }
            
            string textPattern = this is PlayerController 
                ? "Player Defend" 
                : "Enemy Defend";
            SendLog(textPattern);
            
            // 2ターンの間、防御力を1-3引き上げる
            IsDefend = true;
            RemainDefendTurn = 2;
            DefenseBuff = Random.Range(1, 4);
            
            CurrentParam.Defense += DefenseBuff;
            
            // パラメータ情報を表示
            SendStatus(CurrentParam, this is PlayerController);
            
            await UniTask.CompletedTask;
        }
        
        /// <summary>
        /// 回復処理
        /// </summary>
        public virtual async UniTask HealAsync()
        {
            bool isPlayer = this is PlayerController;
            
            // 回復処理
            // 回復力 + 1~3のランダム値
            int healVal = CurrentParam.HealPower + Random.Range(1, 4);
            CurrentParam.Hp = Mathf.Min(CurrentParam.Hp + healVal, BaseParam.Hp);
            // パラメータ情報を表示
            SendStatus(CurrentParam, isPlayer);

            string textPattern = isPlayer 
                ? $"Player Heal: {healVal}" 
                : $"Enemy Heal: {healVal}";
            SendLog($"{textPattern}");

            // 適当にawait
            if (isPlayer)
            {
                await UniTask.WaitForSeconds(1);
            }
        }
        
        /// <summary>
        /// ダメージ処理
        /// </summary>
        /// <param name="damage"></param>
        public virtual async UniTask TakeDamage(int damage)
        {
            await UniTask.DelayFrame(1);
        }

        /// <summary>
        /// パラメータを送る
        /// </summary>
        /// <param name="baseParam"></param>
        /// <param name="isPlayer"></param>
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

        /// <summary>
        /// 防御状態の更新
        /// </summary>
        public void UpdateDefenseState()
        {
            if (IsDefend)
            {
                RemainDefendTurn--;
                if(RemainDefendTurn <= 0)
                {
                    IsDefend = false;
                    CurrentParam.Defense -= DefenseBuff;
                    DefenseBuff = 0;
                    
                    SendLog("Defense Boost End");
                }
            }
        }
    }
}