using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace com.MLR.Wesp
{
    public class RankingManager : SingletonBehaviour<RankingManager>
    {

        public class Rank
        {
            public int score;
            public int level;
            public string playerName;

            public Rank(int score, int level, string playerName)
            {
                this.score = score;
                this.level = level;
                this.playerName = playerName;
            }
        }

        Rank[] ranking = new Rank[10];

        // Use this for initialization
        void Awake()
        {
            this.LoadRanking();
        }

        void LoadRanking()
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(Path.Combine(Application.persistentDataPath, "ranking.dat"), System.Text.Encoding.UTF8);
                this.ParseRanking(reader.ReadToEnd());

            }
            catch
            {
                TextAsset rankingAsset = Resources.Load("Ranking/Ranking") as TextAsset;
                this.ParseRanking(rankingAsset.text);
                this.SaveRanking();

            }
            finally
            {
                if (reader != null)
                {
                    try
                    {
                        reader.Close();
                    }
                    catch
                    {
                    }
                }
            }
        }

        void ParseRanking(string textRanking)
        {
            string[] rankingArray = textRanking.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < 10; i++)
            {
                string[] rank = rankingArray[i].Split(new char[] { '\t' }, StringSplitOptions.None);
                int score;
                int level;
                if (rank.Length == 3 && Int32.TryParse(rank[0], out score) && Int32.TryParse(rank[1], out level))
                {
                    this.ranking[i] = new Rank(score, level, rank[2]);
                }
            }
        }

        void SaveRanking()
        {
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(Path.Combine(Application.persistentDataPath, "ranking.dat"), false, System.Text.Encoding.UTF8);

                string separator = "";
                foreach (Rank rank in this.ranking)
                {
                    writer.Write(separator + rank.score + "\t" + rank.level + "\t" + rank.playerName);
                    separator = "\n";
                }
            }
            finally
            {
                if (writer != null)
                {
                    try
                    {
                        writer.Close();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public bool EnterInRank(int score)
        {
            return score > this.ranking[9].score;
        }

        public void AddScore(int score, int level, string playerName)
        {
            Rank[] newRanking = new Rank[10];
            Rank playerRank = new Rank(score, level, playerName);

            bool playerAdded = false;
            int j = 0;
            for (int i = 0; i < 10; i++)
            {
                Rank rank = this.ranking[j];
                if (!playerAdded && playerRank.score > rank.score)
                {
                    newRanking[i] = playerRank;
                    playerAdded = true;
                }
                else
                {
                    newRanking[i] = this.ranking[j];
                    j++;
                }
            }

            this.ranking = newRanking;

            this.SaveRanking();
        }

        public Rank[] GetRanking()
        {
            return this.ranking;
        }
    } 
}
