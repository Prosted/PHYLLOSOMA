﻿using System;
using Newtonsoft.Json;
using Runtime.Data;
using Runtime.Patterns;
using UnityEngine;

namespace Runtime.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        public const string PLAYER_SAVE_PATH = "save.data";
        public PlayerData PlayerData => _playerData;
        [SerializeField] private PlayerData _playerData = new PlayerData();
        private long _playStartTime = 0;
        private long _playEndTime = 0;
        public override void Awake()
        {
            base.Awake();
            Load();
        }
        private void OnDisable()
        {
            Save();
        }
        public void SaveDataToPath<T>(string path,T data)
        {
            string savePath = Application.persistentDataPath + "\\" + path;
            Debug.Log(savePath);
            System.IO.File.WriteAllText(savePath,JsonConvert.SerializeObject(data));
        }
        public T LoadDataFromPath<T>(string path)
        {
            string loadPath = Application.persistentDataPath + "\\" + path;
            if (!System.IO.File.Exists(loadPath)) {
                Debug.Log("Save file is not exists.");
                return default;
            }
            
            string jsonData = System.IO.File.ReadAllText(loadPath);
            return JsonConvert.DeserializeObject<T>(jsonData) ?? default;
        }
        public void Load() {
            _playerData = LoadDataFromPath<PlayerData>(PLAYER_SAVE_PATH);
            if (_playerData == null) {
                _playerData = new PlayerData();
            }
            _playStartTime = DateTime.Now.Ticks;
            Debug.Log(new DateTime(_playerData.LastSaveTime));
            Debug.Log(new DateTime(_playerData.TotalPlayTime));
        }
        public void Save()
        {
            _playEndTime = DateTime.Now.Ticks;
            _playerData.LastSaveTime = DateTime.Now.Ticks;
            _playerData.TotalPlayTime += _playEndTime - _playStartTime;
            SaveDataToPath(PLAYER_SAVE_PATH,_playerData);
        }
    }
}