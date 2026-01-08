using System;
using UnityEngine;

public class LayerMasksBehaviour : MonoBehaviour
{
    public LayerMasks layerMasks;
    
    public void Init()
    {
        LayerMasks.all = layerMasks;
    }
}

[Serializable]
public class LayerMasks
{
    public static LayerMasks all;
    
    public LayerMask playerMask;
    public LayerMask enemyMask;
    public LayerMask solidMask;
}

public class LayerMaskUtils
{
    public static bool ContainsLayer(LayerMask layerMask, int layer)
    {
        return (layerMask.value & (1 << layer)) > 0;       
    }
}