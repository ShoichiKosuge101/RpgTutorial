using System;
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
        private Button playerActionButton;
        public IObservable<Unit> OnClickPlayerActionButton => playerActionButton.OnClickAsObservable().TakeUntilDestroy(this);
        
        [SerializeField]
        private Button defenseButton;
        public IObservable<Unit> OnClickDefenseButton => defenseButton.OnClickAsObservable().TakeUntilDestroy(this);
        
        [SerializeField]
        private Button healButton;
        public IObservable<Unit> OnClickHealButton => healButton.OnClickAsObservable().TakeUntilDestroy(this);
        
        [SerializeField]private TMP_Text messageText;
        [SerializeField]private TMP_Text stateText;
        
        [SerializeField]private ParamInformationView playerParam;
        [SerializeField]private ParamInformationView enemyParam;
        
        /// <summary>
        /// シングルトン化
        /// </summary>
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
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
        
        public void SetLog(in string message)
        {
            messageText.text = message;
        }
        
        public void SetParam(in BaseParam player, in bool isPlayer)
        {
            if(isPlayer)
            {
                playerParam.SetParam(player);
            }
            else
            {
                enemyParam.SetParam(player);
            }
        }
    }
}