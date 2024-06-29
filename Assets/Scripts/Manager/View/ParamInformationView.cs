using TMPro;
using UnityEngine;
using Utils;

namespace Manager.View
{
    public class ParamInformationView
        : MonoBehaviour
    {
        [SerializeField]private TMP_Text hpText;
        [SerializeField]private TMP_Text attackText;
        [SerializeField]private TMP_Text defenseText;
        
        public void SetParam(in BaseParam param)
        {
            hpText.text = $"{param.Hp}";
            attackText.text = $"{param.Attack}";
            defenseText.text = $"{param.Defense}";
        }
    }
}