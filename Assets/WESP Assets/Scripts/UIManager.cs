using UnityEngine;
using UnityEngine.UI;
using System;

namespace com.MLR.Wesp
{
    public class UIManager : SingletonBehaviour<UIManager>
    {

        [Serializable]
        class UIRank
        {
            public Text positionText;
            public Text playerNameText;
            public Text scoreText;

            public UIRank(Text positionText, Text playerNameText, Text scoreText)
            {
                this.positionText = positionText;
                this.playerNameText = playerNameText;
                this.scoreText = scoreText;
            }
        }

        public Texture2D cursor;

        Canvas menuCanvas;
        Canvas optionsCanvas;
        Canvas selectLevelCanvas;
        Canvas gameCanvas;
        Canvas levelCanvas;
        Canvas enterRankCanvas;

        Text uiSelectLevelText;
        InputField uiSelectLevelInputField;

        Text uiScoreText;
        Text uiBonusText;
        Text uiLevelText;

        Text uiResultText;
        Text uiActionText;

        InputField uiPlayerNameInputField;
        UIRank[] uiRanking;

        // Use this for initialization
        void Awake()
        {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID || UNITY_BLACKBERRY)
            Cursor.visible = false;        
#endif
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            this.menuCanvas = GameObject.Find("MenuCanvas").GetComponent<Canvas>();
            this.optionsCanvas = GameObject.Find("OptionsCanvas").GetComponent<Canvas>();
            this.selectLevelCanvas = GameObject.Find("SelectLevelCanvas").GetComponent<Canvas>();
            this.gameCanvas = GameObject.Find("GameCanvas").GetComponent<Canvas>();
            this.levelCanvas = GameObject.Find("LevelCanvas").GetComponent<Canvas>();
            this.enterRankCanvas = GameObject.Find("EnterRankCanvas").GetComponent<Canvas>();

            this.uiSelectLevelText = GameObject.Find("SelectLevelText").GetComponent<Text>();
            this.uiSelectLevelInputField = GameObject.Find("SelectLevelInputField").GetComponent<InputField>();

            this.uiScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            this.uiBonusText = GameObject.Find("BonusText").GetComponent<Text>();
            this.uiLevelText = GameObject.Find("LevelText").GetComponent<Text>();

            this.uiResultText = GameObject.Find("ResultText").GetComponent<Text>();
            this.uiActionText = GameObject.Find("ActionText").GetComponent<Text>();

            this.uiPlayerNameInputField = GameObject.Find("PlayerNameInputField").GetComponent<InputField>();

            this.uiRanking = new UIRank[10];
            for (int i = 1; i <= 10; i++)
            {
                uiRanking[i - 1] = new UIRank(
                    GameObject.Find(String.Format("Rank{0}PosText", i)).GetComponent<Text>(),
                    GameObject.Find(String.Format("Rank{0}PlayerText", i)).GetComponent<Text>(),
                    GameObject.Find(String.Format("Rank{0}ScoreText", i)).GetComponent<Text>()
                );
            }

#if UNITY_WEBPLAYER
        GameObject.Destroy(GameObject.Find("ExitButton"));

        RectTransform classicModeButton = GameObject.Find("ClassicModeButton").GetComponent<RectTransform>();
        RectTransform randomModeButton = GameObject.Find("RandomModeButton").GetComponent<RectTransform>();
        RectTransform optionsButton = GameObject.Find("OptionsButton").GetComponent<RectTransform>();

        classicModeButton.position.Set(classicModeButton.position.x, classicModeButton.position.y - 100, classicModeButton.position.z);
        randomModeButton.position.Set(randomModeButton.position.x, randomModeButton.position.y - 100, randomModeButton.position.z);
        optionsButton.position.Set(optionsButton.position.x, optionsButton.position.y - 100, optionsButton.position.z);
#endif
        }

        public void SetScoreText(int score)
        {
            this.uiScoreText.text = String.Format("Score: {0}", score);
        }

        public void SetBonusText(int bonus)
        {
            this.uiBonusText.text = String.Format("Bonus: {0}", bonus);
        }

        public void SetLevelText(int level)
        {
            this.uiLevelText.text = String.Format("Level: {0}", level);
        }

        public void SetRanking(RankingManager.Rank[] ranking)
        {
            int lastScore = -1;
            int pos = 0;
            for (int i = 0; i < 10; i++)
            {
                RankingManager.Rank rank = ranking[i];

                if (rank.score != lastScore)
                {
                    pos = i + 1;
                    lastScore = rank.score;
                }

                this.uiRanking[i].positionText.text = String.Format("{0}º", pos);
                this.uiRanking[i].playerNameText.text = rank.playerName;
                this.uiRanking[i].scoreText.text = rank.score.ToString();
            }
        }

        public void ShowMenu()
        {
            this.menuCanvas.enabled = true;
            this.optionsCanvas.enabled = false;
            this.selectLevelCanvas.enabled = false;
            this.gameCanvas.enabled = false;
            this.levelCanvas.enabled = false;
            this.enterRankCanvas.enabled = false;

#if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID && !UNITY_BLACKBERRY)
            Cursor.visible = true;
#endif
        }

        public void ShowOptions()
        {
            this.menuCanvas.enabled = false;
            this.optionsCanvas.enabled = true;
            this.selectLevelCanvas.enabled = false;
            this.gameCanvas.enabled = false;
            this.levelCanvas.enabled = false;
            this.enterRankCanvas.enabled = false;

#if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID && !UNITY_BLACKBERRY)
            Cursor.visible = true;
#endif
        }

        public void ShowSelectLevel(int levelReached)
        {
            this.menuCanvas.enabled = false;
            this.optionsCanvas.enabled = false;
            this.selectLevelCanvas.enabled = true;
            this.gameCanvas.enabled = false;
            this.levelCanvas.enabled = false;
            this.enterRankCanvas.enabled = false;

            this.uiSelectLevelText.text = String.Format("SELECT A LEVEL (FROM 1 TO {0})", levelReached);
            this.uiSelectLevelInputField.text = String.Empty;

#if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID && !UNITY_BLACKBERRY)
            Cursor.visible = true;
#endif
        }

        public void ShowStartLevel(int level)
        {
            this.menuCanvas.enabled = false;
            this.optionsCanvas.enabled = false;
            this.selectLevelCanvas.enabled = false;
            this.gameCanvas.enabled = false;
            this.levelCanvas.enabled = true;
            this.enterRankCanvas.enabled = false;

            this.uiResultText.text = String.Format("Level {0}", level);
            this.uiActionText.text = "START!";

#if !UNITY_EDITOR && !UNITY_IOS && !UNITY_ANDROID && !UNITY_BLACKBERRY
            Cursor.visible = false;
#endif
        }

        public void ShowLevelCleared(int level)
        {
            this.menuCanvas.enabled = false;
            this.optionsCanvas.enabled = false;
            this.selectLevelCanvas.enabled = false;
            this.gameCanvas.enabled = false;
            this.levelCanvas.enabled = true;
            this.enterRankCanvas.enabled = false;

            this.uiResultText.text = String.Format("Level {0} clear!", level);
            this.uiActionText.text = String.Format("GO TO LEVEL {0}", level + 1);

#if !UNITY_EDITOR && !UNITY_IOS && !UNITY_ANDROID && !UNITY_BLACKBERRY
            Cursor.visible = false;
#endif
        }

        public void ShowGameOver(int score)
        {
            this.menuCanvas.enabled = false;
            this.optionsCanvas.enabled = false;
            this.selectLevelCanvas.enabled = false;
            this.gameCanvas.enabled = false;
            this.levelCanvas.enabled = true;
            this.enterRankCanvas.enabled = false;

            this.uiResultText.text = "Game Over";
            this.uiActionText.text = String.Format("SCORE: {0}", score);

#if !UNITY_EDITOR && !UNITY_IOS && !UNITY_ANDROID && !UNITY_BLACKBERRY
            Cursor.visible = false;
#endif
        }

        public void ShowGameLevel()
        {
            this.menuCanvas.enabled = false;
            this.optionsCanvas.enabled = false;
            this.selectLevelCanvas.enabled = false;
            this.gameCanvas.enabled = true;
            this.levelCanvas.enabled = false;
            this.enterRankCanvas.enabled = false;

#if !UNITY_EDITOR && !UNITY_IOS && !UNITY_ANDROID && !UNITY_BLACKBERRY
            Cursor.visible = false;
#endif
        }

        public void ShowGameLevel(int score, int bonus, int level)
        {
            this.ShowGameLevel();

            this.SetScoreText(score);
            this.SetBonusText(bonus);
            this.SetLevelText(level);
        }

        public void ShowEnterRank()
        {
            this.menuCanvas.enabled = false;
            this.optionsCanvas.enabled = false;
            this.selectLevelCanvas.enabled = false;
            this.gameCanvas.enabled = false;
            this.levelCanvas.enabled = false;
            this.enterRankCanvas.enabled = true;

            this.uiPlayerNameInputField.text = String.Empty;

#if !UNITY_EDITOR && !UNITY_IOS && !UNITY_ANDROID && !UNITY_BLACKBERRY
            Cursor.visible = true;
#endif
        }

        public string GetPlayerName()
        {
            return this.uiPlayerNameInputField.text;
        }

        public int GetSelectedLevel() {
            int level;
            if (Int32.TryParse(this.uiSelectLevelInputField.text, out level)) {
                return level;
            } else {
                return 1;
            }
        }
    }
    
}