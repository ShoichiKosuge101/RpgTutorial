using System;
using UniRx;

namespace Utils
{
    [Serializable]
    public class BaseParam
    {
        public enum StatType
        {
            Hp,
            Attack,
            Defense,
            Speed,
            HealPower
        }

        public int Hp { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public float Speed { get; private set; }
        public int HealPower { get; private set; }
        
        [NonSerialized]
        public readonly Subject<int> HpRx = new Subject<int>();
        [NonSerialized]
        public readonly Subject<int> AttackRx = new Subject<int>();
        [NonSerialized]
        public readonly Subject<int> DefenseRx = new Subject<int>();
        [NonSerialized]
        public readonly Subject<float> SpeedRx = new Subject<float>();
        [NonSerialized]
        public readonly Subject<int> HealPowerRx = new Subject<int>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="attack"></param>
        /// <param name="defense"></param>
        /// <param name="speedRx"></param>
        /// <param name="healPower"></param>
        public BaseParam(int hp, int attack, int defense, float speedRx, int healPower)
        {
            Hp = hp;
            Attack = attack;
            Defense = defense;
            Speed = speedRx;
            HealPower = healPower;
        }
        
        public void SetHp(int hp)
        {
            Hp = hp;
            HpRx.OnNext(hp);
        }
        
        public void SetAttack(int attack)
        {
            Attack = attack;
            AttackRx.OnNext(attack);
        }
        
        public void SetDefense(int defense)
        {
            Defense = defense;
            DefenseRx.OnNext(defense);
        }
        
        public void SetSpeed(float speed)
        {
            Speed = speed;
            SpeedRx.OnNext(speed);
        }
        
        public void SetHealPower(int healPower)
        {
            HealPower = healPower;
            HealPowerRx.OnNext(healPower);
        }
    }
}