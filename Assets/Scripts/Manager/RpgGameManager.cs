using System;
using Controller;
using Interface;
using StageState;
using TMPro;
using UniRx;
using UnityEngine;
using Utils;

namespace Manager
{
    /// <summary>
    /// ゲームマネージャ
    /// </summary>
    public class RpgGameManager 
        : MonoBehaviour
    {
        public static RpgGameManager Instance { get; private set; }
        
        private IState _currentState;
    
        private readonly Subject<Unit> _onCurrentStateChanged = new Subject<Unit>();
        public IObservable<Unit> OnCurrentStateChanged => _onCurrentStateChanged;
        
        [SerializeField]
        private PlayerController playerController;
        public PlayerController PlayerController => playerController;

        [SerializeField]
        private EnemyController enemyController;
        public EnemyController EnemyController => enemyController;
        
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
        
        private void Start()
        {
            ChangeState(new StartState());
        }

        public void ChangeState(IState newState)
        {
            if(_currentState != null)
            {
                _currentState.ExitAsync();
            }
        
            _currentState = newState;
            _currentState.EnterAsync();
        
            _onCurrentStateChanged.OnNext(Unit.Default);
        }
    
        public void Update()
        {
            if(_currentState != null)
            {
                _currentState.ExecuteAsync();
            }
        }
        
        /// <summary>
        /// メッセージを送る
        /// </summary>
        /// <param name="message"></param>
        public void SendLog(string message)
        {
            UIManager.Instance.SetLog(message);
        }
        
        public void SetStatus(BaseParam playerParam, bool isPlayer)
        {
            UIManager.Instance.SetParam(playerParam, isPlayer);
        }
    }   
}
