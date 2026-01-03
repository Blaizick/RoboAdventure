using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CraftUI : MonoBehaviour
{
    public GameObject craftRecipePrefab;
    public RectTransform recipesViewContentRootTransform;

    public TMP_Text recipeNameText;
    public CButton craftButton;
    public Image craftProgressBar;
    
    public RectTransform recipeIngradientsContentRootTransform;
    public RectTransform recipeProductsContentRootTransform;
    public GameObject recipeIngradientPrefab;
    
    [NonSerialized] public List<CraftRecipeContainerPrefab> recipeViewInstances = new();

    [NonSerialized] public List<CraftRecipeIngradientContainerPrefab> recipeIngradientInstances = new();
    [NonSerialized] public List<CraftRecipeIngradientContainerPrefab> recipeProductInstances = new();

    public List<ContentSizeFitter> allSizeFitters = new();

    [NonSerialized] public CraftSystem craftSystem;

    [Inject]
    public void Construct(CraftSystem craftSystem)
    {
        this.craftSystem = craftSystem;
    }
    
    public void Init()
    {
        RebuildRecipesView();
    }

    public void _Update()
    {
        var method = typeof(ContentSizeFitter).GetMethod("SetDirty", BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var i in allSizeFitters)
        {
            method!.Invoke(i, new object[] { });
        }

        if (craftButton.IsHolding)
        {
            craftSystem.ProgressCraft();
        }
        else
        {
            craftSystem.OnStopCrafting();
        }

        if (craftSystem.Crafting)
        {
            craftProgressBar.fillAmount = craftSystem.curCraftingTime / craftSystem.craftingTime;
        }
        else
        {
            craftProgressBar.fillAmount = 1.0f;
        }
    }
    
    public void RebuildRecipesView()
    {
        recipeViewInstances.ForEach(i => GameObject.Destroy(i.gameObject));
        recipeViewInstances.Clear();

        foreach (var recipe in CraftRecipes.all)
        {
            var go = GameObject.Instantiate(craftRecipePrefab, recipesViewContentRootTransform);
            var script = go.GetComponent<CraftRecipeContainerPrefab>();
            
            script.icon.sprite = recipe.GetComponent<CmsSpriteComp>().sprite;
            script.button.onClick.AddListener(() =>
            {
                craftSystem.SelectRecipe(recipe);
                ShowRecipe(recipe);
            });

            recipeViewInstances.Add(script);
        }
    }


    public void ShowRecipe(CmsEntity cmsEntity)
    {
        recipeNameText.text = cmsEntity.GetComponent<CmsNameComp>().name;
        
        RebuildItemStacks(cmsEntity.GetComponent<CmsInputItemsComp>().inputStacks, recipeIngradientsContentRootTransform, recipeIngradientInstances);
        RebuildItemStacks(cmsEntity.GetComponent<CmsOutputItemsComp>().outputStacks, recipeProductsContentRootTransform, recipeProductInstances);

        void RebuildItemStacks(CmsItemStack[] cmsStacks, RectTransform contentRootTransform, List<CraftRecipeIngradientContainerPrefab> instances)
        {
            instances.ForEach(i => GameObject.Destroy(i.gameObject));
            instances.Clear();
            
            foreach (var cmsStack in cmsStacks)
            {
                var stack = cmsStack.AsItemStack();

                var go = GameObject.Instantiate(recipeIngradientPrefab, contentRootTransform);
                var script = go.GetComponent<CraftRecipeIngradientContainerPrefab>();

                script.itemCountText.text = stack.count.ToString();
                script.itemIcon.sprite = stack.item.GetComponent<CmsInventoryIconComp>().icon;
            
                instances.Add(script);
            }
        }
    }
}