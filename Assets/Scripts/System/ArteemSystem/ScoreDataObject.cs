using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName = "Arteem/Score Data", order = 4)]
public class ScoreDataObject : ScriptableObject
{
    public List<ScoreData> _scoreData = new List<ScoreData>();

    public void refreshData()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "/Levels/");
        FileInfo[] info = dir.GetFiles("*.unity");

        int numOfLevels =  info.Length;

        if (_scoreData.Count < numOfLevels)
        {
            int c = 0;
            if(_scoreData.Count > 0)
            {
                c = _scoreData.Count - 1;
            }

            for(int i = c; i < numOfLevels; i++)
            {
                ScoreData sd = new ScoreData();
                sd.starAValue = 2000;
                sd.starBValue = 5000;
                sd.starCValue = 10000;

                _scoreData.Add(sd);
            }
        }
    }
}
