using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Linq;
using System;
using ExcelDataReader;
using System.Data;

using Yinyue200.Corefx.CodePages;

public class AugmentData
{
    public int id { get; set; }
    public string name { get; set; }

    public string descriptions { get; set; }

    public int rarity { get; set; }

    public string code { get; set; }

    public Eva evaluator;

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
    Expression attachExpression;
    Expression updateExpression;

    private void Awake()
    {
        current = this;
        Init();
    }

    public void Update()
    {
        if (GameManager.current.gameState == GameState.Shop) return;
        for (int i = 0; i < character.augments.Count; i++)
        {
            OnUpdate(character.augments[i].id);
        }
    }

    public void Init()
    {
        parser = new Parser();

        augmentDatas = new List<AugmentData>();
        string filePath = Application.streamingAssetsPath + "/AugmentList.xlsx";
        int columnNum = 0, rowNum = 0;
        DataRowCollection collection = ReadExcel(filePath, ref columnNum, ref rowNum);

        attachExpression = parser.Parse("OnAttached();");
        updateExpression = parser.Parse("OnUpdate();");

        for (int i = 1; i < rowNum; i++)
        {
            Dictionary<string, Value> records = new Dictionary<string, Value>()
            {
                { "OnUpdate", new Value(ToImportFunction(()=>{  })) },
                { "OnAttached", new Value(ToImportFunction(()=>{  })) },
                { "LaserSight", new Value(ToImportFunction(LaserSight))},
                { "AddModifier", new Value(ToImportFunction(AddModifier))}
            };

            Env env = new Env(records);
            AugmentData data = new AugmentData()
            {
                id = int.Parse(collection[i][0].ToString()),
                name = collection[i][1].ToString(),
                descriptions = collection[i][2].ToString(),
                rarity = int.Parse(collection[i][3].ToString()),
                code = collection[i][4].ToString(),
                evaluator = new Eva(env)
            };

            data.evaluator.eval(parser.Parse(data.code));
            augmentDatas.Add(data);
        }
    }

    public DataRowCollection ReadExcel(string filePath, ref int columnnum, ref int rownum)
    {
        System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();

        
        columnnum = result.Tables[0].Columns.Count;
        rownum = result.Tables[0].Rows.Count;
        stream.Close();

        return result.Tables[0].Rows;
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
        augmentDatas[id].evaluator.eval(attachExpression);
    }

    private void OnUpdate(int id)
    {
        augmentDatas[id].evaluator.eval(updateExpression);
    }


    #region Augment Functions
    void AddModifier(string statType, string stat, string modifierType, double amount)
    {
        StatModifier modifier = new StatModifier((float)amount, modifierType);
        character.AddModifier(statType, stat, modifier);
    }
    public void LaserSight()
    {
        Vector3 lookDir = gunPoint.forward * 100;
        laserSightLineRenderer.SetPosition(0, gunPoint.position);
        laserSightLineRenderer.SetPosition(1, gunPoint.position + lookDir);
    }

   

    #endregion

}
