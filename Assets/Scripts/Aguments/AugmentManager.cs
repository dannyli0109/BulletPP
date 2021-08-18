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

    public int rarity { get; set; }

    public List<int> synergies { get; set; }

    public List<string> descriptions { get; set; }

    public List<Eva> evaluators;

}

public class SynergyData
{
    public int id { get; set; }
    public string name { get; set; }

    public List<int> breakpoints { get; set; }
    public List<string> descriptions { get; set; }


    public List<Eva> evaluators;

}

public class AugmentManager : MonoBehaviour
{
    public static AugmentManager current;
    public List<AugmentData> augmentDatas;
    public List<SynergyData> synergyDatas;
    public int rarityLevels = 5;
    public List<List<int>> augmentRarities;
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
            OnAugmentUpdate(character.augments[i].id, character.augments[i].level);
        }

        for (int i = 0; i < character.synergies.Count; i++)
        {
            OnSynergyUpdate(character.synergies[i].id, character.synergies[i].breakPoint);
        }
    }

    public void Init()
    {
        parser = new Parser();

        attachExpression = parser.Parse("OnAttached();");
        updateExpression = parser.Parse("OnUpdate();");

        augmentRarities = new List<List<int>>();

        for (int i = 0; i < rarityLevels; i++)
        {
            augmentRarities.Add(new List<int>());
        }

        InitAugmentDatas();
        InitSynergyDatas();
    }

    public void InitAugmentDatas()
    {
        augmentDatas = new List<AugmentData>();

        string fileName = "AugmentList";
        string tempFileName = "temp";
        string fileExtension = ".xlsx";
        string filePath = Application.streamingAssetsPath + "/" + fileName + fileExtension;
        string tempFilePath = Application.streamingAssetsPath + "/" + tempFileName + fileExtension;

        // create a temp file instead of reading the existing one, so that the user can have the file open while running the game
        //File.Copy(filePath, tempFilePath, true);

        int columnNum = 0, rowNum = 0;
        DataRowCollection collection = ReadExcel(filePath, 0, ref columnNum, ref rowNum);

        for (int i = 1; i < rowNum; i++)
        {

            List<string> descriptions = new List<string>();
            List<Eva> evaluators = new List<Eva>();
            List<string> rawSynergies = collection[i][3].ToString().Split(',').ToList();
            List<int> synergies = new List<int>();

            for (int j = 0; j < rawSynergies.Count; j++)
            {
                synergies.Add(int.Parse(rawSynergies[j]));
            }


            for (int j = 0; j < 3; j++)
            {
                Eva evaluator = InitEvaluator();

                descriptions.Add(collection[i][4 + j * 2].ToString());

                evaluator.eval(parser.Parse(collection[i][5 + j * 2].ToString()));
                evaluators.Add(evaluator);
            }

            AugmentData data = new AugmentData()
            {
                id = int.Parse(collection[i][0].ToString()),
                name = collection[i][1].ToString(),
                rarity = int.Parse(collection[i][2].ToString()),
                synergies = synergies,
                descriptions = descriptions,
                evaluators = evaluators
            };
            augmentDatas.Add(data);
            augmentRarities[data.rarity].Add(data.id);
        }

        //File.Delete(filePath);
    }


    public void InitSynergyDatas()
    {
        synergyDatas = new List<SynergyData>();

        string fileName = "AugmentList";
        string tempFileName = "temp";
        string fileExtension = ".xlsx";
        string filePath = Application.streamingAssetsPath + "/" + fileName + fileExtension;
        string tempFilePath = Application.streamingAssetsPath + "/" + tempFileName + fileExtension;

        // create a temp file instead of reading the existing one, so that the user can have the file open while running the game
        //File.Copy(filePath, tempFilePath, true);

        int columnNum = 0, rowNum = 0;
        DataRowCollection collection = ReadExcel(filePath, 1, ref columnNum, ref rowNum);

        for (int i = 1; i < rowNum; i++)
        {

            List<string> descriptions = new List<string>();
            List<Eva> evaluators = new List<Eva>();
            List<string> rawBreakpoints = collection[i][2].ToString().Split(',').ToList();
            List<int> breakpoints = new List<int>();

            for (int j = 0; j < rawBreakpoints.Count; j++)
            {
                breakpoints.Add(int.Parse(rawBreakpoints[j]));
            }


            for (int j = 0; j < breakpoints.Count; j++)
            {
                Eva evaluator = InitEvaluator();

                descriptions.Add(collection[i][3 + j * 2].ToString());

                evaluator.eval(parser.Parse(collection[i][4 + j * 2].ToString()));
                evaluators.Add(evaluator);
            }

            SynergyData data = new SynergyData()
            {
                id = int.Parse(collection[i][0].ToString()),
                name = collection[i][1].ToString(),
                breakpoints = breakpoints,
                descriptions = descriptions,
                evaluators = evaluators
            };
            synergyDatas.Add(data);
        }

        //File.Delete(filePath);
    }

    public DataRowCollection ReadExcel(string filePath, int tableIndex, ref int columnnum, ref int rownum)
    {
        System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();


        columnnum = result.Tables[tableIndex].Columns.Count;
        rownum = result.Tables[tableIndex].Rows.Count;
        stream.Close();

        return result.Tables[tableIndex].Rows;
    }

    public Eva InitEvaluator()
    {
        Dictionary<string, Value> records = new Dictionary<string, Value>()
                {
                    { "OnUpdate", new Value(ToImportFunction(()=>{  })) },
                    { "OnAttached", new Value(ToImportFunction(()=>{  })) },
                    { "LaserSight", new Value(ToImportFunction(LaserSight))},
                    { "AddModifier", new Value(ToImportFunction(AddModifier))}
                };
        Env env = new Env(records);
        return new Eva(env);
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

    
    public void OnAugmentAttached(int id, int level)
    {
        augmentDatas[id].evaluators[level].eval(attachExpression);
    }

    private void OnAugmentUpdate(int id, int level)
    {
        augmentDatas[id].evaluators[level].eval(updateExpression);
    }

    public void OnSynergyAttached(int id, int level)
    {
        if (level == -1) return;
        synergyDatas[id].evaluators[level].eval(attachExpression);
    }

    private void OnSynergyUpdate(int id, int level)
    {
        if (level == -1) return;
        synergyDatas[id].evaluators[level].eval(updateExpression);
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
