using UnityEngine;

namespace com.MLR.Wesp
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        delegate void WaitForPlayeCallback();

        public enum GameMode
        {
            Classic,
            Random
        }

        public enum GameState
        {
            Menu,
            Start,
            Playing,
            Win,
            Lose,
            EnteringRanking
        };

        public GameState state = GameState.Start;
        public LevelManager levelManager;
        public UIManager uiManager;
        public SoundManager soundManager;
        public RankingManager rankingManager;
        public PersistenceManager persistenceManager;

        public GameMode mode = GameMode.Classic;

        public int bonusPoints = 100;
        public int movePoints = 10;

        public int level = 1;
        public int lastLevel = 29;
        int score = 0;
        int levelBonus;

        float bonusTimer = 1f;

        // Use this for initialization
        void Start()
        {
            this.soundManager = GetComponent<SoundManager>();
            this.levelManager = GetComponent<LevelManager>();
            this.uiManager = GetComponent<UIManager>();
            this.rankingManager = GetComponent<RankingManager>();
            this.persistenceManager = GetComponent<PersistenceManager>();

            this.uiManager.SetRanking(this.rankingManager.GetRanking());
            this.GoToMenu();
        }

        void InitLevel()
        {
            this.levelBonus = this.bonusPoints;

            this.uiManager.ShowGameLevel(this.score, this.levelBonus, this.level);

            this.levelManager.StartLevel(this.mode, this.level);
            this.state = GameState.Playing;
        }

        void FixedUpdate()
        {
            if (this.state == GameState.Playing)
            {
                this.bonusTimer -= Time.deltaTime;

                if (this.bonusTimer <= 0)
                {
                    this.levelBonus--;
                    this.uiManager.SetBonusText(this.levelBonus);

                    this.bonusTimer += 1;
                }
            }
        }

        void LateUpdate()
        {
            switch (this.state)
            {
                case GameState.Menu:
                    //Nothing
                    break;

                case GameState.Start:
                    WaitForPlayer(new WaitForPlayeCallback(InitLevel));
                    break;

                case GameState.Playing:
                    /* Disabled in order to prevent Unity UI bug
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (Time.timeScale == 1) {
                            Time.timeScale = 0;
                            this.levelManager.HideBoard();
                            this.uiManager.ShowOptions();
                        } else {
                            this.OnOptionsBack();
                        }
                    }*/
                    Input.ResetInputAxes();
                    break;

                case GameState.Win:
                    WaitForPlayer(new WaitForPlayeCallback(InitLevel));
                    break;

                case GameState.Lose:
                    WaitForPlayer(new WaitForPlayeCallback(GoToMenu));
                    break;

                case GameState.EnteringRanking:
                    //Nothing
                    break;
            }
        }

        void WaitForPlayer(WaitForPlayeCallback callback)
        {
            if (Input.anyKeyDown)
            {
                callback();
                Input.ResetInputAxes();
            }
        }

        public void OnClassicModeEnter()
        {
            this.mode = GameMode.Classic;

            int levelReached = this.persistenceManager.LoadLevelReached();

            if (levelReached > 1) {
                this.uiManager.ShowSelectLevel(levelReached);
            } else {
                this.GoToStart();
            }
            
        }

        public void OnStartClassicGame() {
            int levelReached = this.persistenceManager.LoadLevelReached();
            this.level = Mathf.Min(this.uiManager.GetSelectedLevel(), levelReached);

            this.GoToStart();
        }

        public void OnRandomModeEnter()
        {
            this.mode = GameMode.Random;
            this.GoToStart();
        }

        public void OnOptionsEnter()
        {
            this.uiManager.ShowOptions();
        }

        public void OnOptionsBack()
        {
            /*if (this.state == GameState.Playing) {
                this.levelManager.ShowBoard();
                this.uiManager.ShowGameLevel();
                Time.timeScale = 1;
            }
            else
            {
                this.GoToMenu();
            }*/

            this.GoToMenu();
        }

        public void OnSelectLevelBack()
        {
            this.GoToMenu();
        }

        public void OnExit()
        {
            Application.Quit();
        }

        public void OnPlayerMove(PlayerController player)
        {
            if (this.levelManager.CountTiles() == 0)
            {
                this.soundManager.PlaySuccessSound();
                this.GoToWin();

                Destroy(player.gameObject);
            }
            else if (this.levelManager.getTile(player.x, player.y) == null)
            {
                this.soundManager.PlayDeadSound();
                this.EndGame();

                Destroy(player.gameObject);
            }
            else
            {
                
#if UNITY_IOS || UNITY_ANDROID || UNITY_BLACKBERRY
                int remainingTiles = this.levelManager.CountTiles();
                if (remainingTiles > 1) {
                    int passableTiles = this.levelManager.HightlightOnlyPassableTiles(player.x, player.y);

                    if (passableTiles == 0) {
                        this.soundManager.PlayDeadSound();
                        this.EndGame();

                        //Destroy(player.gameObject);
                    }  else {
                        this.score += this.movePoints;
                        this.uiManager.SetScoreText(this.score);

                        this.soundManager.PlayMoveSound();
                    }
                } else if (remainingTiles <= 1) {
                    this.score += this.movePoints;
                    this.uiManager.SetScoreText(this.score);
                    this.soundManager.PlaySuccessSound();
                    this.GoToWin();
                }
#else
                this.score += this.movePoints;
                this.uiManager.SetScoreText(this.score);
                this.soundManager.PlayMoveSound();
#endif
            }
        }

        public void OnScoreSubmit()
        {
            try
            {
                string playerName = this.uiManager.GetPlayerName();
                if (string.IsNullOrEmpty(playerName))
                {
                    playerName = "No Name";
                }

                this.rankingManager.AddScore(this.score, this.level, playerName);
                this.uiManager.SetRanking(this.rankingManager.GetRanking());
            }
            catch
            {
            }
            this.GoToLose();
        }

        void GoToMenu()
        {
            Input.ResetInputAxes();

            this.state = GameState.Menu;
            this.levelManager.UnloadLevel();
            this.uiManager.ShowMenu();
        }

        void GoToStart()
        {
            Input.ResetInputAxes();

            this.state = GameState.Start;
            this.uiManager.ShowStartLevel(this.level);
        }

        void GoToWin()
        {
            Input.ResetInputAxes();

            this.state = GameState.Win;

            this.score += this.movePoints;
            this.score += this.bonusPoints;

            this.uiManager.SetScoreText(this.score);
            this.uiManager.SetBonusText(this.levelBonus);

            //Si es el último nivel, terminamos la partida
            if (this.mode == GameMode.Classic && this.level == this.lastLevel)
            {
                this.EndGame();
            }
            else
            {
                this.uiManager.ShowLevelCleared(this.level);
                this.level++;

                if (this.level > this.persistenceManager.LoadLevelReached()) {
                    this.persistenceManager.SaveLevelReached(this.level);
                }
            }
        }

        void GoToLose()
        {
            Input.ResetInputAxes();

            this.state = GameState.Lose;
            this.uiManager.ShowGameOver(this.score);

            this.level = 1;
            this.score = 0;
        }

        void GoToEnteringRanking()
        {
            Input.ResetInputAxes();
            this.state = GameState.EnteringRanking;
            this.uiManager.ShowEnterRank();
        }

        void EndGame()
        {
            if (this.rankingManager.EnterInRank(this.score))
            {
                this.GoToEnteringRanking();
            }
            else
            {
                this.GoToLose();
            }
        }
    } 
}
