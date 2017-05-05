using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tools
{
    [Serializable]
    public class UnityMapStringGameObject
    {
        public MapString Element1;
        public MapGameObject Element2;
    }

    [Serializable]
    public class MapString : MapElement<String>
    {
    }

    [Serializable]
    public class MapGameObject : MapElement<GameObject>
    {
    }

    [Serializable]
    public class MapElement<T>
    {
        public T Value;
    }
}

