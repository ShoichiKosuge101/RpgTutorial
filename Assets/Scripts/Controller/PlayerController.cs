using Cysharp.Threading.Tasks;
using Manager;
using StageState;
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
            
            SendLog("Player Attack");
            var damage = playerData.Attack;
            await enemyController.TakeDamage(damage);
        }
        
        public override async UniTask DefendAsync(ControllerBase enemyController)
        {
            SendLog("Player Defend");
            // 防御力を上げる処理
            
            // 適当にawait
            await UniTask.CompletedTask;
        }
        
        public override async UniTask TakeDamage(int damage)
        {
            // damageを防御力で減らす
            damage = Mathf.Max(damage - CurrentParam.Defense, 0);
            CurrentParam.Hp -= damage;

            SendLog($"Player Take Damage: {damage}");
            SendStatus(CurrentParam, true);
            
            if(CurrentParam.Hp <= 0)
            {
                await UniTask.DelayFrame(1);
                
                // プレイヤーの死亡処理
                SendLog("Player is Dead");
                await DeadAsync();
            }
        }

        private static async UniTask DeadAsync()
        {
            // 終了処理を呼ぶ
            await RpgGameManager.Instance.ChangeState(new GameOverState());
        }
    }
}