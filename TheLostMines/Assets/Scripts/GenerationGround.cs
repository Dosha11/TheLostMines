using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.TextCore;

public class GenerationGround : MonoBehaviour
{
    public TerrainData terrainData;
    public List<GroundLout> groundList;
    [SerializeField] private List<Height> _plainHeights;

    public void Registration(TerrainData terrainData, List<Height> _plainHeights) 
    {
        this.terrainData = terrainData;
        this._plainHeights= _plainHeights;
        GenerateTrees();
    }

    public void GenerateTrees()
    {
        var data = terrainData;

        for (int w = 0; w < _plainHeights.Count; w++)
        {
            var elevation = data.GetHeight(_plainHeights[w].X, _plainHeights[w].Y);

            for (int i = 0; i < groundList.Count; i++)
            {
                if (w > groundList[i].minRange && w < groundList[i].maxRange)
                {
                    if (Utility.Chance(groundList[i].chance))
                    {
                        Vector3 pos = new Vector3(_plainHeights[i].X, elevation, _plainHeights[i].Y);
                        GameObject clone = Instantiate(groundList[i].createObject, pos, Quaternion.identity, gameObject.transform);
                    }
                }
            }
        }
    }
}

[Serializable]
public struct GroundLout
{
    public bool index;
    public GameObject createObject;
    public int minRange;
    public int maxRange;
    public int chance;
    public bool randomRotateX;
    public bool randomRotateY;
    public bool randomRotateZ;
    public float randomScaleX;
    public float randomScaleY;
    public float randomScaleZ;
}