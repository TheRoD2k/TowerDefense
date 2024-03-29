﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/Asset Root", fileName = "Asset Room")]
    public class AssetRoot : ScriptableObject
    {
        public List<LevelAsset> Levels;
    }
}