using Cysharp.Threading.Tasks;
using Manager;
using StageState;
using UniRx;
using UnityEngine;
using Utils;

namespace Controller
{
    /// <summary>
    /// プレイヤーのコントローラー
    /// </summary>
    public class PlayerController
        : ControllerBase
    {
        [SerializeField]
        private PlayerData playerData;

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void Start()
        {
            BaseParam = playerData.BaseParam;
            CurrentParam = playerData.BaseParam;
            
            // 購読処理
            CurrentParam.HpRx
                .Merge(CurrentParam.DefenseRx)
                .TakeUntilDestroy(this)
                .Subscribe(_ => 
                {
                    // パラメータが変更されたらステータスを更新
                    SendStatus(CurrentParam, true);
                });
            
            // パラメータ情報を表示
            SendStatus(CurrentParam, true);
        }
        
        /// <summary>
        /// プレイヤーの攻撃処理
        /// </summary>
        /// <param name="enemyController"></param>
        public override async UniTask AttackAsync(ControllerBase enemyController)
        {
            if (enemyController == null)
            {
                Debug.Log("Enemy is Empty");
                return;
            }
            
            SendLog("Player Attack");
            var damage = playerData.Attack;
            await enemyController.TakeDamage(damage);
        }
        
        /// <summary>
        /// プレイヤーのダメージ処理
        /// </summary>
        /// <param name="damage"></param>
        public override async UniTask TakeDamage(int damage)
        {
            // damageを防御力で減らす
            damage = Mathf.Max(damage - CurrentParam.Defense, 0);
            CurrentParam.SetHp(CurrentParam.Hp - damage);

            SendLog($"Player Take Damage: {damage}");
            
            // 受けたダメージをフロート表示
            RpgGameManager.Instance.ShowFloatingValue(-1 * damage, BaseParam.StatType.Hp, true);
            
            // HPが0以下になったら死亡処理
            if(CurrentParam.Hp <= 0)
            {
                await UniTask.DelayFrame(1);
                
                // プレイヤーの死亡処理
                SendLog("Player is Dead");
                await DeadAsync();
            }
        }

        /// <summary>
        /// プレイヤーの死亡処理
        /// </summary>
        private static async UniTask DeadAsync()
        {
            // 終了処理を呼ぶ
            await RpgGameManager.Instance.ChangeState(new GameOverState());
        }
    }
}