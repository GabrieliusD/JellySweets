using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public Button AdButton;
    string PlacementID = "rewardedVideo";
    public int RewardAmount = 1000;
    // Start is called before the first frame update
    void Start()
    {
        AdButton.interactable = Advertisement.IsReady(PlacementID);

        if (AdButton) AdButton.onClick.AddListener(ShowAd);

        Advertisement.AddListener(this);
        Advertisement.Initialize("3930121", true);
    }

    public void ShowAd()
    {  
        Advertisement.Show(PlacementID);
    }

    public void OnUnityAdsReady(string placementId)
    {
        if(placementId == PlacementID)
        AdButton.interactable = true;
        
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(showResult == ShowResult.Finished)
        {
            ValueManager.instance.whiteSugar += RewardAmount;
            ValueManager.instance.updateText();
            ErrorManager.instance.SetMessage("Awarded " + RewardAmount.ToString()+ " Sugar");
            ErrorManager.instance.ShowMessage();
        } else if(showResult == ShowResult.Skipped)
        {
            // do nothing;
        }
    }
}
