using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public string currentName;

    public int currentScore;

    public List<PlayerScore> highScores;

    private int maxScores = 10;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScores();
        if (highScores == null)
        {
            highScores = new List<PlayerScore>();
        }
    }

    [System.Serializable]
    public class PlayerScore
    {
        public string Name;

        public int Score;
    }

    [System.Serializable]
    class SaveData
    {
        public PlayerScore[] HighScores;
    }

    public void SaveScores()
    {
        SaveData data = new SaveData();
        data.HighScores = highScores.ToArray();

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScores = new List<PlayerScore>(data.HighScores);
        }
    }

    public void AddCurrentScore()
    {
        PlayerScore score = new PlayerScore();
        score.Name = currentName;
        score.Score = currentScore;
        highScores.Add(score);
        highScores.Sort((PlayerScore a, PlayerScore b) => { return b.Score - a.Score; });
        if (highScores.Count > maxScores)
        {
            highScores.RemoveRange(maxScores, highScores.Count - maxScores);
        }
    }
}
