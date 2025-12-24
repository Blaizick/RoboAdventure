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
        if (player.collectionCollectable)
        {
            collectionCircleRoot.SetActive(true);
            collectionCircleImg.fillAmount = player.curCollectionTime / player.maxCollectionTime;
            collectionCircleRootTransform.anchoredPosition = Camera.main.WorldToScreenPoint(player.collectionCollectable.transform.position);
        }
        else
        {
            collectionCircleRoot.SetActive(false);
        }
    }
}
