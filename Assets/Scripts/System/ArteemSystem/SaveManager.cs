using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    GameData _gameData = new GameData();
    List<LevelData> levelDatas = new List<LevelData>();
    SettingsData _settingsData = new SettingsData();

    private void Awake()
    {
        _gameData = loadGameData();
        levelDatas = loadLevelsData();
        _settingsData = loadSettingsData();
    }

    public GameData loadGameData()
    {
        GameData data = new GameData();
        data.money = 200;
        data.cash = 10;
        data.diamond = 5;

        if (File.Exists(Application.persistentDataPath + "/GameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);

            data = (GameData)bf.Deserialize(file);
            file.Close();
        }

        _gameData = data;
        return data;
    }

    public void saveGameData(GameData data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/GameData.dat");
        bf.Serialize(file, data);
        file.Close();
        _gameData = data;
    }

    public void saveLevelData(int id, LevelData data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        List<LevelData> newLis = levelDatas;
        if (id > newLis.Count - 1)
        {
            newLis.Add(data);
        }
        else
        {
            newLis[id] = data;
        }
        FileStream file = File.Create(Application.persistentDataPath + "/LevelsData.dat");
        bf.Serialize(file, newLis);
        file.Close();

        levelDatas = newLis;
    }

    public List<LevelData> loadLevelsData()
    {
        List<LevelData> data = new List<LevelData>();
        if (File.Exists(Application.persistentDataPath + "/LevelsData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/LevelsData.dat", FileMode.Open);

            data = (List<LevelData>)bf.Deserialize(file);
            file.Close();
        }
        levelDatas = data;
        return data;

    }

    public void saveSettingsData(SettingsData data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/SettingsData.dat");
        bf.Serialize(file, data);
        file.Close();
        _settingsData = data;
    }

    public SettingsData loadSettingsData()
    {
        SettingsData data = new SettingsData();
        data.musicValue = 20;
        data.sfxValue = 20;

        if (File.Exists(Application.persistentDataPath + "/SettingsData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SettingsData.dat", FileMode.Open);

            data = (SettingsData)bf.Deserialize(file);
            file.Close();
        }

        _settingsData = data;
        return data;
    }

    /*public LevelData getLevelData(int id)
    {
        LevelData data = new LevelData();

        if (id > levelDatas.Count - 1)
        {
            data = levelDatas[id];
        }
        else
        {
            data.star = 0;
            data.score = 0;
            data.minutes = 0;
            data.seconds = 0;
        }

        return data;
    }*/

    public int getNumOfLevels()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "/Levels/");
        FileInfo[] info = dir.GetFiles("*.unity");

        return info.Length;
    }
}