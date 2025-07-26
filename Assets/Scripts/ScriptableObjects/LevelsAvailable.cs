using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu]
public class LevelsAvailable : ScriptableObject
{
    public List<LevelData> levels;
}

[Serializable]
public class LevelData
{
    public string levelName;
    public AssetReferenceGameObject levelPrefab;
}
