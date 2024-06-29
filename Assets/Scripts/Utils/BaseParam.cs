namespace Utils
{
    public class BaseParam
    {
        public int Hp { get; internal set; }
        public int Attack { get; internal set; }
        public int Defense { get; internal set; }
        public float Speed { get; internal set; }
        public int HealPower { get; internal set; }

        public BaseParam(int hp, int attack, int defense, float speed, int healPower)
        {
            Hp = hp;
            Attack = attack;
            Defense = defense;
            Speed = speed;
            HealPower = healPower;
        }
    }
}