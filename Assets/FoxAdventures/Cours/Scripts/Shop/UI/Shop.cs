using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class Shop : MonoBehaviour
{
    public string shopName = "catalog";
    public GameObject shopEntryPrefab = null;
    private List<ShopEntry> shopEntries = new List<ShopEntry>();

    void OnEnable()
    {
        if (this.shopEntryPrefab != null)
            this.shopEntryPrefab.gameObject.SetActive(false);

        RefreshShopItems();
    }

    public void RefreshShopItems()
    {
        if (this.shopEntryPrefab == null)
            return;

        if (PlayfabAuth.IsLoggedIn == true)
        {
            // Get the catalog items from PlayFab
            GetCatalogItemsRequest request = new GetCatalogItemsRequest();
            request.CatalogVersion = shopName;

            PlayFabClientAPI.GetCatalogItems(request, OnGetCatalogItemsSuccess, OnGetCatalogItemsError);
        }
    }

    private void OnGetCatalogItemsSuccess(GetCatalogItemsResult result)
    {
        // Clear existing entries
        ClearExistingEntries();

        List<ShopItem> items = new List<ShopItem>();

        

        // Instantiate entries for each item
        for (int i = 0; i < items.Count; i++)
        {
            ShopItem shopItem = items[i];
            if (shopItem != null)
            {
                GameObject shopEntryGameobjectCopy = GameObject.Instantiate(this.shopEntryPrefab, this.shopEntryPrefab.transform.parent);
                if (shopEntryGameobjectCopy != null)
                {
                    shopEntryGameobjectCopy.gameObject.SetActive(true);
                    shopEntryGameobjectCopy.name = ("ShopItemEntry (" + shopItem.itemDisplayName + ")");
                    ShopEntry shopEntry = shopEntryGameobjectCopy.GetComponent<ShopEntry>();
                    if (shopEntry != null)
                    {
                        shopEntry.SetValue(shopItem);
                        if (this.shopEntries == null)
                            this.shopEntries = new List<ShopEntry>();
                        this.shopEntries.Add(shopEntry);
                    }
                    else
                    {
                        GameObject.Destroy(shopEntryGameobjectCopy);
                    }
                }
            }
        }
    }

    private void OnGetCatalogItemsError(PlayFabError error)
    {
        Debug.LogError("Shop.OnGetCatalogItemsError() - Error: " + error.ErrorMessage);
    }

    public void ClearExistingEntries()
    {
        if (this.shopEntries != null)
        {
            while (this.shopEntries.Count > 0)
            {
                if (this.shopEntries[0] != null)
                {
                    GameObject.Destroy(this.shopEntries[0].gameObject);
                }

                this.shopEntries.RemoveAt(0);
            }
        }
    }

}
