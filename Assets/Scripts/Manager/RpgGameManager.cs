using System;
using Controller;
using Cysharp.Threading.Tasks;
using Interface;
using PlayerState;
using StageState;
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
    
        private readonly Subject<IState> _onCurrentStateChanged = new Subject<IState>();
        public IObservable<IState> OnCurrentStateChanged => _onCurrentStateChanged;
        
        [SerializeField]
        private PlayerController playerController;
        public PlayerController PlayerController => playerController;

        [SerializeField]
        private EnemyController enemyController;
        public EnemyController EnemyController => enemyController;

        private bool _isGameOver;

        private int _turnCount = 0;
        public int TurnCount => _turnCount;
        
        private int _playerTurnCount = 0;
        public int PlayerTurnCount => _playerTurnCount;
        
        private int _enemyTurnCount = 0;
        public int EnemyTurnCount => _enemyTurnCount;
        
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
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            await ChangeState(new StartState());
            // 初回更新
            SetCurrentState(_currentState);
            
            // ログの初期化
            UIManager.Instance.SetLog(string.Empty);
            
            // Stateの更新
            _onCurrentStateChanged
                .TakeUntilDestroy(this)
                .Subscribe(SetCurrentState);
        }

        public async UniTask ChangeState(IState newState)
        {
            if (_isGameOver)
            {
                Debug.Log("Game Over");
                return;
            }
            
            if(_currentState != null)
            {
                await _currentState.ExitAsync();
            }
        
            _currentState = newState;
            _onCurrentStateChanged.OnNext(newState);
            
            await _currentState.EnterAsync();
        }
    
        public void Update()
        {
            if(_currentState != null)
            {
                _currentState.ExecuteAsync().Forget();
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
        
        /// <summary>
        /// パラメータを設定する
        /// </summary>
        /// <param name="playerParam"></param>
        /// <param name="isPlayer"></param>
        public void SetStatus(BaseParam playerParam, bool isPlayer)
        {
            UIManager.Instance.SetParam(playerParam, isPlayer);
        }
        
        public void SetCurrentState(IState state)
        {
            string currentState;
            switch (state)
            {
                case StartState _:
                    currentState = "Start";
                    break;
                case PlayerTurnState _:
                    currentState = "Player Turn";
                    break;
                case EnemyTurnState _:
                    currentState = "Enemy Turn";
                    break;
                case GameOverState _:
                    currentState = "Game Over";
                    break;
                case GameClearState _:
                    currentState = "Game Clear";
                    break;
                default:
                    currentState = "Executing";
                    break;
            }
            UIManager.Instance.SetState(currentState);
        }
        
        public void ShowFloatingValue(int paramValue, BaseParam.StatType statType, bool isPlayer)
        {
            UIManager.Instance.ShowFloatingValue(paramValue, statType, isPlayer);
        }
        
        public void IncrementTurnCount()
        {
            _turnCount++;
        }
        
        public void IncrementPlayerTurnCount()
        {
            _playerTurnCount++;
        }
        
        public void IncrementEnemyTurnCount()
        {
            _enemyTurnCount++;
        }
        
        public void SetGameOver()
        {
            _isGameOver = true;
        }
    }   
}
