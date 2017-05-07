using Assets.Scripts.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets
{
    public class ConfigManager : Singleton<ConfigManager>
    {
        private Config _config;
        public Config Config
        {
            get { return _config; }
        }

        private void Awake()
        {
            StreamReader configJson = new StreamReader("config.json");
            _config = JsonUtility.FromJson<Config>(configJson.ReadToEnd());
        }
    }
}