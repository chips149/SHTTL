using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserArea : MonoBehaviour, IPointerClickHandler
{
    private UserAreaManager _manager;

    private void Start()
    {
        _manager = ModulesManager.Get<UserAreaManager>();
        _manager.Areas.Add(this);
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _manager.OnAreaClick(this);
    }

    private void OnDestroy()
    {
        _manager.Areas.Remove(this);
    }
}