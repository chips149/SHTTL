using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework;
using UnityEngine;
using UnityEngine.Assertions;

public static class CardHandler
{
    public static CardData[] Data;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var targetAssemblies = assemblies.Where(_assembly =>
            _assembly.FullName.StartsWith("Assembly-CSharp") ||
            _assembly.FullName.StartsWith("Assembly-CSharp-firstpass"));

        var types = targetAssemblies
            .SelectMany(_assembly => _assembly.GetTypes())
            .Where(Predicate);


        List<CardData> data = new();

        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<CardPropertyAttribute>();
            data.Add(GetCardData(attr, type));
        }

        data.Sort((a, b) => a.id - b.id);
        Data = data.ToArray();
        return;

        bool Predicate(Type _type)
        {
            return _type.IsSubclassOf(typeof(CardData)) &&
                   Attribute.IsDefined(_type, typeof(CardPropertyAttribute)) &&
                   !_type.IsAbstract &&
                   _type.IsClass &&
                   _type.FullName != null &&
                   !_type.FullName.Contains("+");
        }
    }

    private static CardData GetCardData(CardPropertyAttribute attr, Type type)
    {
        var cardData = Activator.CreateInstance(type) as CardData;
        Assert.IsNotNull(cardData);
        cardData.id = attr.id;
        cardData.imgPath = attr.imgPath;
        cardData.description = attr.description;
        return cardData;
    }

    public static CardData[] RandomCardData()
    {
        // 算法 
        return Data.OrderBy(value => value.Priority()).ToArray();
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class CardPropertyAttribute : Attribute
{
    public int id;
    public string name;
    public string imgPath;
    public string description;

    public CardPropertyAttribute(int id, string name, string imgPath, string description)
    {
        this.id = id;
        this.name = name;
        this.imgPath = imgPath;
        this.description = description;
    }
}

public abstract class CardData
{
    public int id;
    public string imgPath;
    public string description;

    public virtual int Priority()
    {
        return UnityEngine.Random.Range(0, 100);
    }

    public abstract void OnChosen();
}


public static class GameState
{
    public static BattleManager Bm;
    public static UserAreaManager Um;
    public static Player Player;
    public static MonsterUI MonsterUI;

    public static int currentLevel;
    public static bool spawnOver;
    public static bool isGameOver;
}