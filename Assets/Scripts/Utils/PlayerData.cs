using UnityEngine;

namespace Utils
{
    /// <summary>
    /// プレイヤーデータ
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 2)]
    public class PlayerData
        : CharacterSheet
    {
        public BaseParam BaseParam => new BaseParam(Hp, Attack, Defense, Speed, HealPower);

        // プレイヤー固有のデータをここに追加
    }
}