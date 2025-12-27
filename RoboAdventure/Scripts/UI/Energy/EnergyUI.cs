using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EnergyUI : MonoBehaviour
{
    [Inject] public EnergySystem energySystem;

    public Image energyFiller;
    public Button absorbButton;

    public RectTransform absorbSlotParentTransform;
    
    [Inject] public IFactory<StorageItemStackReference, RectTransform, InventorySlotContainerPrefab> factory;

    [NonSerialized] public GameObject absorbSlotInstance;
    
    public void Init()
    {
        absorbButton.onClick.AddListener(() => energySystem.Absorb());

        energySystem.absorbStorage.onChange += Rebuild;
        Rebuild();
    }

    public void Rebuild()
    {
        if (absorbSlotInstance != null)
        {
            GameObject.Destroy(absorbSlotInstance);
        }
        
        var itemStackRef = new StorageItemStackReference(energySystem.absorbStorage, AbsorbStorage.AbsorbStackId);
        
        var script = factory.Create(itemStackRef, absorbSlotParentTransform);
        
        var absorbStack = energySystem.absorbStorage.AbsorbStack;
        
        script.itemIcon.gameObject.SetActive(absorbStack != null);
        script.itemCountText.gameObject.SetActive(absorbStack != null);
        if (energySystem.absorbStorage.AbsorbStack != null)
        {
            script.itemIcon.sprite = absorbStack.item.Get<CmsInventoryIconComp>().icon;
            script.itemCountText.gameObject.SetActive(absorbStack.count > 1);
            script.itemCountText.text = absorbStack.count.ToString();
        }
        
        absorbSlotInstance = script.gameObject;
    }

    public void _Update()
    {
        energyFiller.fillAmount = energySystem.energy / energySystem.maxEnergy;
    }
}
