using Cysharp.Threading.Tasks;
using DG.Tweening;
using Manager;
using StageState;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Controller
{
    /// <summary>
    /// 敵のコントローラー
    /// </summary>
    public class EnemyController
        : ControllerBase
    {
        [SerializeField]
        private EnemyData enemyData;
        
        [SerializeField]
        private Image enemyIcon;
        
        [SerializeField]
        private float fadeTime = 3f;
        
        /// <summary>
        /// 初期化処理
        /// </summary>
        private void Start()
        {
            enemyIcon.sprite = enemyData.Sprite;
            BaseParam = enemyData.BaseParam;
            CurrentParam = enemyData.BaseParam;
            
            // 購読処理
            CurrentParam.HpRx
                .Merge(CurrentParam.DefenseRx)
                .TakeUntilDestroy(this)
                .Subscribe(_ => 
                {
                    // パラメータが変更されたらステータスを更新
                    SendStatus(CurrentParam, false);
                });
            
            // パラメータ情報を表示
            SendStatus(enemyData.BaseParam, false);
        }
        
        /// <summary>
        /// 敵の攻撃処理
        /// </summary>
        /// <param name="playerController"></param>
        public override async UniTask AttackAsync(ControllerBase playerController)
        {
            if (playerController == null)
            {
                Debug.Log("Player is Empty");
                return;
            }
            
            SendLog("Enemy Attack");
            await playerController.TakeDamage(CurrentParam.Attack);
        }
        
        /// <summary>
        /// 敵のダメージ処理
        /// </summary>
        /// <param name="damage"></param>
        public override async UniTask TakeDamage(int damage)
        {
            // damageを防御力で減らす
            damage = damage - CurrentParam.Defense > 0 
                ? damage - CurrentParam.Defense 
                : 0;
            CurrentParam.SetHp(CurrentParam.Hp - damage);
            
            SendLog($"Enemy Take Damage: {damage}");
            
            // 受けたダメージをフロート表示
            RpgGameManager.Instance.ShowFloatingValue(-1 * damage, BaseParam.StatType.Hp, false);
            
            // HPが0以下になったら死亡処理
            if(CurrentParam.Hp <= 0)
            {
                await DeadAsync();
            }
        }
        
        /// <summary>
        /// 敵の死亡処理
        /// </summary>
        private async UniTask DeadAsync()
        {
            await UniTask.DelayFrame(1);

            // 徐々に透過させる
            await enemyIcon
                .DOFade(0f, fadeTime)
                .SetLink(gameObject)
                .AsyncWaitForCompletion();

            SendLog("Enemy is Dead");
            
            // 終了処理を呼ぶ
            await RpgGameManager.Instance.ChangeState(new GameClearState());
            
            // 敵の死亡処理
            Destroy(gameObject);
        }
    }
}