using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tools
{
    public class Prefabs : Singleton<Prefabs>
    {
        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private UnityMapStringGameObject[] _map;

        private Dictionary<string, GameObject> _dictionary = new Dictionary<string, GameObject>();

        public Dictionary<string, GameObject> Dictionary { get { return _dictionary; } }

        public Canvas Canvas { get { return _canvas; } }

        private void Awake()
        {
            foreach (var prefab in _map)
            {
                _dictionary.Add(prefab.Element1.Value, prefab.Element2.Value);
            }
        }
    }
}