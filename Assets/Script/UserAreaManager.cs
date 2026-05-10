using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class UserAreaManager
{
    private bool _isSelecting;

    public Action<UserArea> OnClick;

    public readonly List<UserArea> Areas = new List<UserArea>();


    public void StartChosenArea(Action<UserArea> chosen, Func<UserArea, bool> filter = null)
    {
        if (filter == null)
            SetEnable(true);
        else
            SetEnable(filter);


        OnClick = area =>
        {
            SetEnable(false);
            OnClick = null;
            
            chosen.Invoke(area);
        };
    }


    public void OnAreaClick(UserArea area)
    {
        OnClick?.Invoke(area);
    }


    public void SetEnable(bool enable)
    {
        foreach (var area in Areas)
        {
            area.gameObject.SetActive(enable);
        }
    }

    public void SetEnable(Func<UserArea, bool> func)
    {
        foreach (var area in Areas)
        {
            area.gameObject.SetActive(func(area));
        }
    }
}