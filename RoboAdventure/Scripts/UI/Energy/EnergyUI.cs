using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EnergyUI : MonoBehaviour
{
    public EnergySystem energySystem;

    public Image energyFiller;
    public Button absorbButton;
    public InventorySlotContainerPrefab absorbSlot;
        
    [Inject]
    public void Construct(EnergySystem energySystem)
    {
        this.energySystem = energySystem;
    }
    
    public void Init()
    {
        absorbButton.onClick.AddListener(() => energySystem.Absorb());
    }

    public void _Update()
    {
        energyFiller.fillAmount = energySystem.energy / energySystem.maxEnergy;
        
        absorbSlot.itemIcon.gameObject.SetActive(energySystem.absorbStack != null);
        absorbSlot.itemCountText.gameObject.SetActive(energySystem.absorbStack != null);
        if (energySystem.absorbStack != null)
        {
            absorbSlot.itemIcon.sprite = energySystem.absorbStack.item.Get<CmsInventoryIconComp>().icon;
            absorbSlot.itemCountText.gameObject.SetActive(energySystem.absorbStack.count > 1);
            absorbSlot.itemCountText.text = energySystem.absorbStack.count.ToString();
        }
    }
}
