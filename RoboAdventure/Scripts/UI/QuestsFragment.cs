using TMPro;
using UnityEngine;
using Zenject;

public class QuestsFragment : MonoBehaviour
{
    public TMP_Text text;
    
    [Inject] public QuestsSystem questsSystem;
    
    public void Init()
    {
        
    }

    public void _Update()
    {
        if (questsSystem.activeQuest != null)
        {
            text.text = questsSystem.GetActiveQuestText();
            // text.text = $"Collect <sprite name=BlueCrystal0> {questsSystem.CurItems}/{questsSystem.TargetItems}";    
        }
    }
}