using Cysharp.Threading.Tasks;
using DG.Tweening;
using Manager;
using StageState;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Controller
{
    public class EnemyController
        : ControllerBase
    {
        [SerializeField]
        private EnemyData enemyData;
        
        [SerializeField]
        private Image enemyIcon;
        
        [SerializeField]
        private float fadeTime = 3f;
        
        private void Start()
        {
            enemyIcon.sprite = enemyData.Sprite;
            BaseParam = enemyData.BaseParam;
            CurrentParam = enemyData.BaseParam;
            
            // パラメータ情報を表示
            SendStatus(enemyData.BaseParam, false);
        }
        
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
        
        public override async UniTask DefendAsync(ControllerBase playerController)
        {
            SendLog("Enemy Defend");
            // 防御力を上げる処理
            
            // 適当にawait
            await UniTask.CompletedTask;
        }
        
        public override async UniTask TakeDamage(int damage)
        {
            // damageを防御力で減らす
            damage = damage - CurrentParam.Defense > 0 
                ? damage - CurrentParam.Defense 
                : 0;
            CurrentParam.Hp -= damage;
            
            SendLog($"Enemy Take Damage: {damage}");
            SendStatus(CurrentParam, false);
            
            if(CurrentParam.Hp <= 0)
            {
                await DeadAsync();
            }
        }
        
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