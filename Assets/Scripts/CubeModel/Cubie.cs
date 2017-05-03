using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CubeModel
{
    public struct Cubie
    {
        public byte Position { get; private set; }
        public byte Orient { get; private set; }

        public Cubie(byte pos, byte orient)
        {
            Position = pos;
            Orient = orient;
        }
    }
}
