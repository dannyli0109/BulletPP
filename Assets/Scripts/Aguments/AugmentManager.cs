using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Linq;
using System;


public class AugmentData
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Descriptions { get; set; }

    public int Rarity { get; set; }

    public string Code { get; set; }

    public Expression UpdateExpression;
    public Expression AttachExpression;

}

public class AugmentManager : MonoBehaviour
{
    public static AugmentManager current;
    public List<AugmentData> augmentDatas;
    public int[] costs = { 1, 2, 3, 4, 5 };
    public Color[] colors = {
        new Color(1, 1, 1),
        new Color(0.12f, 1, 0),
        new Color(0, 0.44f, 0.87f),
        new Color(0.64f, 0.21f, 0.93f),
        new Color(1.0f, 0.5f, 0)
    };
    public Character character;
    public Transform gunPoint;
    public LineRenderer laserSightLineRenderer;
    Parser parser;
    Eva eva;

    private void Awake()
    {
        current = this;

        parser = new Parser();
        Dictionary<string, Value> records = new Dictionary<string, Value>()
        {
            { "OnUpdate", new Value(ToImportFunction(()=>{  })) },
            { "OnAttached", new Value(ToImportFunction(()=>{  })) },
            { "LaserSight", new Value(ToImportFunction(LaserSight))},
            { "AddModifier", new Value(ToImportFunction(AddModifier))}
        };
        Env env = new Env(records);
        eva = new Eva(env);

        augmentDatas = new List<AugmentData>();

        using (var reader = new StreamReader("./Assets/Resources/AugmentList.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            augmentDatas = Enumerable.ToList(csv.GetRecords<AugmentData>());

            for (int i = 0; i < augmentDatas.Count; i++)
            {
                string code = augmentDatas[i].Code;

                augmentDatas[i].AttachExpression = parser.Parse(code + @"OnAttached();");
                augmentDatas[i].UpdateExpression = parser.Parse(code + @"OnUpdate();");
            }
        }
    }

    public Func<List<Value>, Expression> ToImportFunction(Action func)
    {
        return (_) =>
        {
            func();
            return null;
        }; 
    }

    public Func<List<Value>, Expression> ToImportFunction(Action<string, string, string, double> func)
    {
        return (list) =>
        {
            func(list[0].stringValue, list[1].stringValue, list[2].stringValue, list[3].doubleValue);
            return null;
        };
    }
    public void OnAttached(int id)
    {
        eva.eval(augmentDatas[id].AttachExpression);
    }

    private void OnUpdate(int id)
    {
        eva.eval(augmentDatas[id].UpdateExpression);
    }


    #region Augment Functions
    void AddModifier(string statType, string stat, string modifierType, double amount)
    {
        StatModifier modifier = new StatModifier((float)amount, modifierType);
        character.AddModifier(statType, stat, modifier);
    }
    public void LaserSight()
    {
        Physics.SyncTransforms();
        Vector3 lookDir = gunPoint.forward * 100;
        laserSightLineRenderer.SetPosition(0, gunPoint.position);
        laserSightLineRenderer.SetPosition(1, gunPoint.position + lookDir);
    }

   

    #endregion

    public void Update()
    {
        if (GameManager.current.gameState != GameState.Game) return;
        //OnUpdate(augmentDatas[0].Code);
        for (int i = 0; i < character.augs.Count; i++)
        {
            OnUpdate(character.augs[i].id);
        }
    }
}
