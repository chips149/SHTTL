using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardPanel : MonoBehaviour
{
    private static AudioClip _startChooseCardSfx;
    private static AudioClip _chooseCardSfx;
    private static AudioClip StartChooseCardSfx => _startChooseCardSfx ??= Resources.Load<AudioClip>("Sound/SFX/StartChooseCard");
    private static AudioClip ChooseCardSfx => _chooseCardSfx ??= Resources.Load<AudioClip>("Sound/SFX/ChooseCard");
    private CardViewer[] _viewers;

    void RandomCard()
    {
        _viewers ??= transform.GetComponentsInChildren<CardViewer>();
        var data = CardHandler.RandomCardData();
        for (var i = 0; i < _viewers.Length; i++)
        {
            _viewers[i].Initialize(this, i, data[i]);
        }
    }


    public void OpenDrawCardPanel()
    {
        RandomCard();
        gameObject.SetActive(true);
        AudioManager.PlaySFX(StartChooseCardSfx);
    }


    public void CloseDrawCardPanel()
    {
        gameObject.SetActive(false);
        Portal.ResetAllPortals();
    }
    
    public void OnRefresh()
    {
        RandomCard();
    }
    
}