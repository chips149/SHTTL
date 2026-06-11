using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class UserAreaManager
{
    private bool _isSelecting;

    public Action<UserArea> OnClick;

    public readonly List<UserArea> Areas = new List<UserArea>();

    private readonly List<Vector3> _colliderPlacedPositions = new();

    private const float NeighborDistance = 1f;


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

    public void StartChosenColliderArea(Action<UserArea> chosen)
    {
        SetEnable(area => !IsAdjacentToOccupied(area.transform.position));

        OnClick = area =>
        {
            _colliderPlacedPositions.Add(area.transform.position);
            SetEnable(false);
            OnClick = null;

            chosen.Invoke(area);
        };
    }

    public void RemovePlacedPosition(Vector3 pos)
    {
        // 移除该位置相邻1格范围内的记录，使格子重新可用
        _colliderPlacedPositions.RemoveAll(p => Vector3.Distance(p, pos) < NeighborDistance);
    }

    private bool IsAdjacentToOccupied(Vector3 pos)
    {
        foreach (var occupied in _colliderPlacedPositions)
        {
            if (Vector3.Distance(pos, occupied) < NeighborDistance)
                return true;
        }
        return false;
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