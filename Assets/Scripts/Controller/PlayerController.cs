using Cysharp.Threading.Tasks;
using Manager;
using UnityEngine;
using Utils;

namespace Controller
{
    public class PlayerController
        : ControllerBase
    {
        [SerializeField]
        private PlayerData playerData;

        private void Start()
        {
            BaseParam = playerData.BaseParam;
            CurrentParam = playerData.BaseParam;
            
            // パラメータ情報を表示
            SendStatus(CurrentParam, true);
        }
        
        public override async UniTask AttackAsync(ControllerBase enemyController)
        {
            if (enemyController == null)
            {
                Debug.Log("Enemy is Empty");
                return;
            }
            
            SendLog("<color=yellow>Player Attack</color>");
            var damage = playerData.Attack;
            await enemyController.TakeDamage(damage);
        }
        
        public override async UniTask DefendAsync(ControllerBase enemyController)
        {
            SendLog("<color=yellow>Player Defend</color>");
            // 防御力を上げる処理
            
            // 適当にawait
            await UniTask.CompletedTask;
        }
        
        public override async UniTask TakeDamage(int damage)
        {
            // damageを防御力で減らす
            damage = damage - playerData.Defense > 0 
                ? damage - playerData.Defense 
                : 0;
            CurrentParam.Hp -= damage;

            SendLog($"<color=yellow>Player Take Damage: {damage}</color>");
            SendStatus(CurrentParam, true);
            
            if(CurrentParam.Hp <= 0)
            {
                await UniTask.DelayFrame(1);
                
                // プレイヤーの死亡処理
                SendLog("Player is Dead");
            }
        }
    }
}