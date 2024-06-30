using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interface;
using Manager.View;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Manager
{
    public class UIManager
        : MonoBehaviour, IUIManager
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] 
        private RectTransform buttonsRoot;
        
        [SerializeField] 
        private Button playerActionButton;
        public IObservable<Unit> OnClickPlayerActionButton => playerActionButton.OnClickAsObservable().TakeUntilDestroy(this);
        
        [SerializeField]
        private Button defenseButton;
        public IObservable<Unit> OnClickDefenseButton => defenseButton.OnClickAsObservable().TakeUntilDestroy(this);
        
        [SerializeField]
        private Button healButton;
        public IObservable<Unit> OnClickHealButton => healButton.OnClickAsObservable().TakeUntilDestroy(this);
        
        [SerializeField]
        private TMP_Text messageText;
        
        [SerializeField]
        private TMP_Text stateText;
        
        [SerializeField]
        private ParamInformationView playerParam;
        
        [SerializeField]
        private ParamInformationView enemyParam;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        /// <summary>
        /// シングルトン化
        /// </summary>
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                // トークンの初期化
                _cancellationTokenSource = new CancellationTokenSource();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void SetActivePlayerActionButtons(bool isActive)
        {
            playerActionButton.gameObject.SetActive(isActive);
            defenseButton.gameObject.SetActive(isActive);
            healButton.gameObject.SetActive(isActive);
        }

        public void ShowButtonWithTween()
        {
            // // 移動
            // buttonsRoot.DOAnchorPos(new Vector2(0, 0), 0.5f).SetLink(this.gameObject).SetEase(Ease.OutBack);
            
            // 回転
            buttonsRoot
                .DOLocalRotate(new Vector3(360f,0,0),0.6f, RotateMode.FastBeyond360)
                .SetLink(this.gameObject)
                .SetEase(Ease.OutCubic);
            
            // // 拡大
            // buttonsRoot.localScale = Vector3.one * 0.2f;
            // buttonsRoot
            //     .DOScale(1f, 0.5f)
            //     .SetLink(this.gameObject)
            //     .SetEase(Ease.OutBack, 5f);
        }
        
        public void SetLog(in string message)
        {
            // 文字送りをキャンセル
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            
            messageText
                .ShowTypeWriteEffectAsync(
                    message, 
                    _cancellationTokenSource.Token
                )
                .Forget();
        }
        
        public void SetParam(in BaseParam currentParam, in bool isPlayer)
        {
            if(isPlayer)
            {
                playerParam.SetParam(currentParam);
            }
            else
            {
                enemyParam.SetParam(currentParam);
            }
        }
        
        public void SetState(in string state)
        {
            stateText.text = state;
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        
        /// <summary>
        /// フローティングテキストを表示
        /// </summary>
        /// <param name="paramValue"></param>
        /// <param name="statType"></param>
        /// <param name="isPlayer"></param>
        public void ShowFloatingValue(int paramValue, BaseParam.StatType statType, bool isPlayer)
        {
            if(isPlayer)
            {
                playerParam.ShowFloatingText(paramValue, statType).Forget();
            }
            else
            {
                enemyParam.ShowFloatingText(paramValue, statType).Forget();
            }
        }
    }
}