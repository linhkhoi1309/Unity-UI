using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "MuseumPlaceholderData", menuName = "Scriptable Objects/MuseumPlaceholderData")]
public class MuseumPlaceholderData : ScriptableObject
{
    [SerializeField]
    public List<TileData> tilesTop;
    
    [SerializeField]
    public List<TileData> tilesMiddle;
    
    [SerializeField]
    public string header;
    
    [SerializeField]
    public string tagLine;
    
    [SerializeReference]
    public List<ArticleData> articles;

    [NonSerialized] public int version;
    
    public void OnValidate()
    {
        version++;
        // When expanding these lists in the Inspector, Unity does not create objects for them by default
        for (int i = 0; i < tilesTop.Count; i++)
        {
            if (tilesTop[i] == null)
                tilesTop[i] = new TileData();
        }
        
        for (int i = 0; i < tilesMiddle.Count; i++)
        {
            if (tilesMiddle[i] == null)
                tilesMiddle[i] = new TileData();
        }

        for (int i = 0; i < articles.Count; i++)
        {
            if (articles[i] == null)
                articles[i] = new ArticleData();
        }
    }
}

[Serializable]
public class ArticleData
{
    public string title;
    public string description;
    public string tag;
}

[Serializable]
public class TileData
{
    public Sprite icon;
    public string title;
}
