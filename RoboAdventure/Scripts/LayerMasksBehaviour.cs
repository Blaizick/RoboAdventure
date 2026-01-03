using UnityEngine;

public class LayerMasksBehaviour : MonoBehaviour
{
    public LayerMask playerMask;
    public LayerMask enemyMask;
    
    public void Init()
    {
        LayerMasks.Construct(playerMask, enemyMask);
    }
}

public class LayerMasks
{
    public static void Construct(LayerMask playerMask, LayerMask enemyMask)
    {
        LayerMasks.playerMask = playerMask;
        LayerMasks.enemyMask = enemyMask;
    }
    
    public static LayerMask playerMask;
    public static LayerMask enemyMask;
}

public class LayerMaskUtils
{
    public static bool Contains(LayerMask layerMask, int layer)
    {
        return (layerMask.value & (1 << layer)) > 0;       
    }
}