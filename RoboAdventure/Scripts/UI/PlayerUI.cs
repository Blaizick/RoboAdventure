using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class PlayerUI : MonoBehaviour
{
    [NonSerialized] public Player player;

    public GameObject collectionCircleRoot;
    public RectTransform collectionCircleRootTransform;
    public Image collectionCircleImg;
    
    [Inject]
    public void Construct(Player player)
    {
        this.player = player;
    }
    
    public void Init()
    {
        
    }

    public void _Update()
    {
        var playerUnit = player.unit;
        if (playerUnit.collectionCollectable)
        {
            collectionCircleRoot.SetActive(true);
            collectionCircleImg.fillAmount = playerUnit.curCollectionTime / playerUnit.maxCollectionTime;
            collectionCircleRootTransform.anchoredPosition = Camera.main.WorldToScreenPoint(playerUnit.collectionCollectable.transform.position);
        }
        else
        {
            collectionCircleRoot.SetActive(false);
        }
    }
}
