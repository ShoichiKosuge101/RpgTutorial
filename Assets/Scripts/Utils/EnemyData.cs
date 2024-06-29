using UnityEngine;

namespace Utils
{
    /// <summary>
    /// 敵データ
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 3)]
    public class EnemyData
        : CharacterSheet
    {
        public BaseParam BaseParam => new BaseParam(Hp, Attack, Defense, Speed, HealPower);
        
        /// <summary>
        /// 敵のSprite
        /// </summary>
        [SerializeField]
        private Sprite _sprite;
        public Sprite Sprite => _sprite;
    }
}