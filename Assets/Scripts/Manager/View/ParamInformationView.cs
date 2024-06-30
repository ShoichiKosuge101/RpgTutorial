using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utils;

namespace Manager.View
{
    /// <summary>
    /// パラメータ情報を表示するビュー
    /// </summary>
    public class ParamInformationView
        : MonoBehaviour
    {
        /// <summary>
        /// HPテキスト
        /// </summary>
        [SerializeField]
        private TMP_Text hpText;
        
        /// <summary>
        /// 攻撃テキスト
        /// </summary>
        [SerializeField]
        private TMP_Text attackText;
        
        /// <summary>
        /// 防御テキスト
        /// </summary>
        [SerializeField]
        private TMP_Text defenseText;
        
        /// <summary>
        /// フローティングテキストのプレハブ
        /// </summary>
        [SerializeField]
        private GameObject floatingTextPrefab;
        
        /// <summary>
        /// パラメータを設定
        /// </summary>
        /// <param name="param"></param>
        public void SetParam(in BaseParam param)
        {
            hpText.text = $"{param.Hp}";
            attackText.text = $"{param.Attack}";
            defenseText.text = $"{param.Defense}";
        }

        /// <summary>
        /// フローティングテキストを表示
        /// </summary>
        /// <param name="paramValue"></param>
        /// <param name="statType"></param>
        public async UniTaskVoid ShowFloatingText(
            int paramValue, 
            BaseParam.StatType statType)
        {
            TMP_Text targetText = statType switch
            {
                BaseParam.StatType.Hp => hpText,
                BaseParam.StatType.Attack => attackText,
                BaseParam.StatType.Defense => defenseText,
                _ => null
            };
            if (targetText == null)
            {
                return;
            }
            
            Vector3 initialDifference = new Vector3(50f, 0f, 0f);
            Vector3 spawnPos = targetText.transform.position + initialDifference;
            GameObject floatingTextObj = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);
            floatingTextObj.transform.SetParent(targetText.transform, false);
            
            // ローカル座標をリセット
            RectTransform floatingTextRect = floatingTextObj.GetComponent<RectTransform>();
            // 初期位置を設定
            floatingTextRect.anchoredPosition = initialDifference;

            TMP_Text floatingTextText = floatingTextObj.GetComponent<TMP_Text>();
            if(paramValue > 0)
            {
                floatingTextText.text = $"+{paramValue}";
                floatingTextText.color = Color.green;
            }
            else
            {
                floatingTextText.text = $"{paramValue}";
                floatingTextText.color = Color.red;
            }
            
            // 上にフェード
            await floatingTextRect
                .DOLocalMoveY(floatingTextRect.localPosition.y + 10f, 0.5f)
                .SetLink(floatingTextObj)
                .SetEase(Ease.OutCubic)
                .AsyncWaitForCompletion();
            
            // 破棄
            Destroy(floatingTextObj);
        }
    }
}