using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    private string appID = "ca-app-pub-4317840593715214~6916891201";

    private InterstitialAd fullScreenAd;
    private string fullScreenAdID = "ca-app-pub-3940256099942544/1033173712";

    private RewardBasedVideoAd rewardedAd;
    private string rewardedAdID = "ca-app-pub-3940256099942544/5224354917";
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        MobileAds.Initialize(appID);
        rewardedAd = RewardBasedVideoAd.Instance;
        requestFullScreenAd();
        requestRewardedAd();


        rewardedAd.OnAdLoaded += HandleRewardBasedVideoLoaded;

        rewardedAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;

        rewardedAd.OnAdRewarded += HandleRewardBasedVideoRewarded;

        rewardedAd.OnAdClosed += HandleRewardBasedVideoClosed;

    }
    public void requestFullScreenAd()
    {
        fullScreenAd = new InterstitialAd(fullScreenAdID);

        AdRequest request = new AdRequest.Builder().Build();

        fullScreenAd.LoadAd(request);
    }

    public void showFullScreenAd()
    {
        if (fullScreenAd.IsLoaded())
        {
            fullScreenAd.Show();
        }
        else
        {
            Debug.Log("full screen ad not loaded!!!");
        }
    }

    public void requestRewardedAd()
    {
        AdRequest request = new AdRequest.Builder().Build();

        rewardedAd.LoadAd(request, rewardedAdID);
    }

    public void showRewardedAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("rewarded ad not loaded!!!");
        }
    }


    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("Rewarded Video ad loaded successfully");

    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Failed to load rewarded video ad : " + args.Message);


    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("You have been rewarded with  " + amount.ToString() + " " + type);

        GameManager.Instance.receiveRewardedAd();
    }


    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("Rewarded video has closed");
        requestRewardedAd();

    }
}
