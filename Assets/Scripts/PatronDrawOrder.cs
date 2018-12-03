using UnityEngine;

public class PatronDrawOrder : MonoBehaviour 
{

    [SerializeField]
    private int NumberOfGroups;

    [SerializeField]
    private SpriteRenderer _head;

    [SerializeField]
    private SpriteRenderer[] _handsAndFeet;

    [SerializeField]
    private SpriteRenderer _torso;

    private const string STAGE_DIVE_LAYER = "StageDiver";
    private const string CROWD_LAYER = "Characters";

    public void SetStageDiverLayer()
    {
        SetLayer(STAGE_DIVE_LAYER);
    }

    public void SetCrowdLayer()
    {
        SetLayer(CROWD_LAYER);
    }

    public void SetIndex(int index)
    {
        int startIndex = index * NumberOfGroups;

        _torso.sortingOrder = startIndex++;
        foreach (SpriteRenderer rend in _handsAndFeet)
        {
            rend.sortingOrder = startIndex;
        }
        _head.sortingOrder = ++startIndex;
    }

    private void SetLayer(string layerName)
    { 
        _torso.sortingLayerName = layerName;
        foreach (SpriteRenderer rend in _handsAndFeet)
        {
            rend.sortingLayerName = layerName;
        }
        _head.sortingLayerName = layerName;
    }
}
