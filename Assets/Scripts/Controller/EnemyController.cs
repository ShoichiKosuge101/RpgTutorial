using Cysharp.Threading.Tasks;
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
            
            SendLog("<color=red>Enemy Attack</color>");
            await playerController.TakeDamage(CurrentParam.Attack);
        }
        
        public override async UniTask DefendAsync(ControllerBase playerController)
        {
            SendLog("<color=red>Enemy Defend</color>");
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
            
            SendLog($"<color=red>Enemy Take Damage: {damage}</color>");
            SendStatus(CurrentParam, false);
            
            if(CurrentParam.Hp <= 0)
            {
                await UniTask.DelayFrame(1);
                
                SendLog("Enemy is Dead");
                // 敵の死亡処理
                Destroy(gameObject);
            }
        }
    }
}