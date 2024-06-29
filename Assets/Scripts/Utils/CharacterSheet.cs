using UnityEngine;

namespace Utils
{
    /// <summary>
    /// キャラクターシート
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterSheet", menuName = "ScriptableObjects/CharacterSheet", order = 1)]
    public class CharacterSheet
        : ScriptableObject
    {
        /// <summary>
        /// HP
        /// </summary>
        [SerializeField] 
        private int _hp;
        public int Hp => _hp;
        
        /// <summary>
        /// 攻撃力
        /// </summary>
        [SerializeField] 
        private int _attack;
        public int Attack => _attack;
        
        /// <summary>
        /// 防御力
        /// 受けるダメージは、攻撃力 - 防御力
        /// </summary>
        [SerializeField]
        private int _defense;
        public int Defense => _defense;
        
        /// <summary>
        /// １ターンに行動できる回数
        /// 通常は１で１回
        /// </summary>
        [SerializeField]
        private float _speed;
        public float Speed => _speed;
        
        /// <summary>
        /// 回復力
        /// 回復行動をしたときに回復するHPの補正値(1-6のランダム値を加算)
        /// </summary>
        [SerializeField]
        private int _healPower;
        public int HealPower => _healPower;
    }
}