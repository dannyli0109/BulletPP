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
using OfficeOpenXml;


public class AugmentManager : MonoBehaviour
{
    public static AugmentManager current;
    public List<Augment> augments;

    //public List<AugmentData> augmentDatas;
    //public List<SynergyData> synergyDatas;
    public int rarityLevels = 5;
    //public List<List<int>> augmentRarities;
    //public int[] costs = { 1, 2, 3, 4, 5 };
    public Color[] colors = {
        new Color(0.5f, 0.5f, 0.5f),
        new Color(0.12f, 1, 0),
        new Color(0, 0.44f, 0.87f),
        new Color(0.64f, 0.21f, 0.93f),
        new Color(1.0f, 0.5f, 0)
    };

    public string[] rarityTexts =
    {
        "Common",
        "Uncommon",
        "Rare",
        "Epic",
        "Legendary"
    };
    //public Character character;
    //public Transform gunPoint;
    //public LineRenderer laserSightLineRenderer;
    //Parser parser;
    //Expression attachExpression;
    //Expression updateExpression;



    private void Awake()
    {
        current = this;
        for (int i = 0; i < augments.Count; i++)
        {
            augments[i].Init();
            augments[i].id = i;
        }
        //Init();
    }
    public static List<Augment> GetAugments()
    {
        return current.augments;
    }

    //public void Update()
    //{

    //    if (GameManager.current.GameTransitional()) return;
    //    laserSightLineRenderer.gameObject.SetActive(false);
    //    for (int i = 0; i < character.inventory.augments.Count; i++)
    //    {
    //        OnAugmentUpdate(character.inventory.augments[i].id, character.inventory.augments[i].level);
    //    }

    //    for (int i = 0; i < character.synergies.Count; i++)
    //    {
    //        OnSynergyUpdate(character.synergies[i].id, character.synergies[i].breakPoint);
    //    }
    //}

    public List<int> GetAugmentIdList()
    {
        List<int> ids = new List<int>();
        for (int i = 0; i < augments.Count; i++)
        {
            ids.Add(augments[i].id);
        }
        return ids;
    }

    public List<List<int>> GetRarityList(List<int> ids)
    {
        List<List<int>> rarities = new List<List<int>>();
        for (int i = 0; i < rarityLevels; i++)
        {
            rarities.Add(new List<int>());
        }

        for (int i = 0; i < ids.Count; i++)
        {
            int rarity = augments[ids[i]].rarity;
            rarities[rarity].Add(augments[ids[i]].id);
        }

        return rarities;
    }

    //public void Init()
    //{
    //    parser = new Parser();

    //    attachExpression = parser.Parse("OnAttached();");
    //    updateExpression = parser.Parse("OnUpdate();");

    //    augmentRarities = new List<List<int>>();

    //    for (int i = 0; i < rarityLevels; i++)
    //    {
    //        augmentRarities.Add(new List<int>());
    //    }

    //    synergyDatas = InitSynergyDatas();
    //    augmentDatas = InitAugmentDatas(synergyDatas);
    //}

    //public List<AugmentData> InitAugmentDatas(List<SynergyData> allSynergies)
    //{
    //    List<AugmentData> list = new List<AugmentData>();

    //    string fileName = "AugmentList";
    //    string tempFileName = "temp";
    //    string fileExtension = ".xlsx";
    //    string filePath = Application.streamingAssetsPath + "/" + fileName + fileExtension;
    //    string tempFilePath = Application.streamingAssetsPath + "/" + tempFileName + fileExtension;

    //    // create a temp file instead of reading the existing one, so that the user can have the file open while running the game
    //    //File.Copy(filePath, tempFilePath, true);

    //    int columnNum = 0, rowNum = 0;
    //    DataRowCollection collection = ReadExcel(filePath, 0, ref columnNum, ref rowNum);

    //    for (int i = 1; i < rowNum; i++)
    //    {
    //        List<string> descriptions = new List<string>();
    //        List<string> codes = new List<string>();
    //        List<Eva> evaluators = new List<Eva>();
    //        List<string> rawSynergies = collection[i][3].ToString().Split(',').ToList();
    //        List<SynergyData> synergies = new List<SynergyData>();

    //        for (int j = 0; j < rawSynergies.Count; j++)
    //        {
    //            synergies.Add(allSynergies[int.Parse(rawSynergies[j])]);
    //        }


    //        for (int j = 0; j < 3; j++)
    //        {
    //            string code = collection[i][6 + j * 2].ToString();
    //            codes.Add(code);

    //            Eva evaluator = InitEvaluator();
    //            descriptions.Add(collection[i][5 + j * 2].ToString());
    //            evaluator.eval(parser.Parse(code));
    //            evaluators.Add(evaluator);
    //        }

    //        AugmentData data = ScriptableObject.CreateInstance<AugmentData>();
    //        data.id = int.Parse(collection[i][0].ToString());
    //        data.title = collection[i][1].ToString();
    //        data.rarity = int.Parse(collection[i][2].ToString());
    //        data.synergies = synergies;
    //        data.iconPath = collection[i][4].ToString();
    //        data.descriptions = descriptions;
    //        data.codes = codes;
    //        data.evaluators = evaluators;
    //        data.iconSprite = Util.LoadNewSprite(Application.streamingAssetsPath + "/" + data.iconPath);

    //        list.Add(data);
    //        augmentRarities[data.rarity].Add(data.id);
    //    }
    //    return list;
    //}


    //public List<SynergyData> InitSynergyDatas()
    //{
    //    List<SynergyData> list = new List<SynergyData>();

    //    string fileName = "AugmentList";
    //    string tempFileName = "temp";
    //    string fileExtension = ".xlsx";
    //    string filePath = Application.streamingAssetsPath + "/" + fileName + fileExtension;
    //    string tempFilePath = Application.streamingAssetsPath + "/" + tempFileName + fileExtension;

    //    // create a temp file instead of reading the existing one, so that the user can have the file open while running the game
    //    //File.Copy(filePath, tempFilePath, true);

    //    int columnNum = 0, rowNum = 0;
    //    DataRowCollection collection = ReadExcel(filePath, 1, ref columnNum, ref rowNum);

    //    for (int i = 1; i < rowNum; i++)
    //    {

    //        List<string> descriptions = new List<string>();
    //        List<string> codes = new List<string>();
    //        List<Eva> evaluators = new List<Eva>();
    //        List<string> rawBreakpoints = collection[i][2].ToString().Split(',').ToList();
    //        List<int> breakpoints = new List<int>();

    //        for (int j = 0; j < rawBreakpoints.Count; j++)
    //        {
    //            breakpoints.Add(int.Parse(rawBreakpoints[j]));
    //        }

    //        // string when none is activated
    //        descriptions.Add(collection[i][4].ToString());

    //        for (int j = 0; j < breakpoints.Count; j++)
    //        {
    //            string code = collection[i][6 + j * 2].ToString();
    //            codes.Add(code);

    //            Eva evaluator = InitEvaluator();
    //            descriptions.Add(collection[i][5 + j * 2].ToString());
    //            evaluator.eval(parser.Parse(code));
    //            evaluators.Add(evaluator);
    //        }

    //        SynergyData data = ScriptableObject.CreateInstance<SynergyData>();
    //        data.id = int.Parse(collection[i][0].ToString());
    //        data.title = collection[i][1].ToString();
    //        data.breakpoints = breakpoints;
    //        data.iconPath = collection[i][3].ToString();
    //        data.descriptions = descriptions;
    //        data.codes = codes;
    //        data.evaluators = evaluators;

    //        data.iconSprite = Util.LoadNewSprite(Application.streamingAssetsPath + "/" + data.iconPath);

    //        list.Add(data);
    //    }

    //    return list;
    //}

    //public void ImportAugmentData()
    //{

    //}

    //public DataRowCollection ReadExcel(string filePath, int tableIndex, ref int columnnum, ref int rownum)
    //{
    //    System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    //    FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    //    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
    //    DataSet result = excelReader.AsDataSet();


    //    columnnum = result.Tables[tableIndex].Columns.Count;
    //    rownum = result.Tables[tableIndex].Rows.Count;
    //    stream.Close();

    //    return result.Tables[tableIndex].Rows;
    //}

    //public void WriteExcel(string filePath)
    //{
    //    string fileExtension = ".xlsx";
    //    string path = Application.streamingAssetsPath + "/" + filePath + fileExtension;
    //    FileInfo newFile = new FileInfo(path);
    //    if (newFile.Exists)
    //    {
    //        newFile.Delete();
    //        newFile = new FileInfo(path);
    //    }

    //    using (ExcelPackage package = new ExcelPackage(newFile))
    //    {
    //        {
    //            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Augments");

    //            worksheet.Cells[1, 1].Value = "Id";
    //            worksheet.Cells[1, 2].Value = "Name";
    //            worksheet.Cells[1, 3].Value = "Rarity";
    //            worksheet.Cells[1, 4].Value = "Synergies";
    //            worksheet.Cells[1, 5].Value = "IconPath";
    //            worksheet.Cells[1, 6].Value = "Descriptions";
    //            worksheet.Cells[1, 7].Value = "Code";
    //            worksheet.Cells[1, 8].Value = "Descriptions";
    //            worksheet.Cells[1, 9].Value = "Code";
    //            worksheet.Cells[1, 10].Value = "Descriptions";
    //            worksheet.Cells[1, 11].Value = "Code";

    //            UnityEngine.Object[] augments = Resources.LoadAll("Data/Augments", typeof(AugmentData));
    //            for (int i = 0; i < augments.Length; i++)
    //            {
    //                AugmentData augment = (AugmentData)augments[i];
    //                int id = augment.id;
    //                worksheet.Cells[id + 2, 1].Value = augment.id;
    //                worksheet.Cells[id + 2, 2].Value = augment.title;
    //                worksheet.Cells[id + 2, 3].Value = augment.rarity;
    //                var synergies = augment.synergies.Select(synergy => synergy.id).ToArray();
    //                worksheet.Cells[id + 2, 4].Value = String.Join(",", synergies);
    //                worksheet.Cells[id + 2, 5].Value = augment.iconPath;
    //                for (int j = 0; j < 3; j++)
    //                {
    //                    worksheet.Cells[id + 2, 6 + j * 2].Value = augment.descriptions[j];
    //                    worksheet.Cells[id + 2, 6 + j * 2 + 1].Value = augment.codes[j];
    //                }
    //            }
    //        }

    //        {
    //            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Synergies");


    //            worksheet.Cells[1, 1].Value = "Id";
    //            worksheet.Cells[1, 2].Value = "Name";
    //            worksheet.Cells[1, 3].Value = "Break Points";
    //            worksheet.Cells[1, 4].Value = "IconPath";
    //            worksheet.Cells[1, 5].Value = "Descriptions";
    //            worksheet.Cells[1, 6].Value = "Descriptions";
    //            worksheet.Cells[1, 7].Value = "Code";
    //            worksheet.Cells[1, 8].Value = "Descriptions";
    //            worksheet.Cells[1, 9].Value = "Code";
    //            worksheet.Cells[1, 10].Value = "Descriptions";
    //            worksheet.Cells[1, 11].Value = "Code";

    //            UnityEngine.Object[] synergies = Resources.LoadAll("Data/Synergies", typeof(SynergyData));
    //            for (int i = 0; i < synergies.Length; i++)
    //            {
    //                SynergyData synergy = (SynergyData)synergies[i];
    //                int id = synergy.id;
    //                worksheet.Cells[id + 2, 1].Value = synergy.id;
    //                worksheet.Cells[id + 2, 2].Value = synergy.title;
    //                worksheet.Cells[id + 2, 3].Value = String.Join(",", synergy.breakpoints);
    //                worksheet.Cells[id + 2, 4].Value = synergy.iconPath;

    //                worksheet.Cells[id + 2, 5].Value = synergy.descriptions[0];

    //                for (int j = 0; j < synergy.breakpoints.Count; j++)
    //                {
    //                    worksheet.Cells[id + 2, 6 + j * 2].Value = synergy.descriptions[j + 1];
    //                    worksheet.Cells[id + 2, 6 + j * 2 + 1].Value = synergy.codes[j];
    //                }
    //            }
    //        }

    //        package.Save();
    //    }
    //}


    //public Eva InitEvaluator()
    //{
    //    Dictionary<string, Value> records = new Dictionary<string, Value>()
    //            {
    //                { "OnUpdate", new Value(ToImportFunction(()=>{  })) },
    //                { "OnAttached", new Value(ToImportFunction(()=>{  })) },
    //                { "LaserSight", new Value(ToImportFunction(LaserSight))},
    //                { "AddModifier", new Value(ToImportFunction(AddModifier))}
    //            };
    //    Env env = new Env(records);
    //    return new Eva(env);
    //}

    //public Func<List<Value>, Expression> ToImportFunction(Action func)
    //{
    //    return (_) =>
    //    {
    //        func();
    //        return null;
    //    }; 
    //}

    //public Func<List<Value>, Expression> ToImportFunction(Action<string, string, string, double> func)
    //{
    //    return (list) =>
    //    {
    //        func(list[0].stringValue, list[1].stringValue, list[2].stringValue, list[3].doubleValue);
    //        return null;
    //    };
    //}


    //public void OnAugmentAttached(int id, int level)
    //{
    //    augmentDatas[id].evaluators[level].eval(attachExpression);
    //}

    //private void OnAugmentUpdate(int id, int level)
    //{
    //    augmentDatas[id].evaluators[level].eval(updateExpression);
    //}

    //public void OnSynergyAttached(int id, int level)
    //{
    //    if (level == -1) return;
    //    synergyDatas[id].evaluators[level].eval(attachExpression);
    //}

    //private void OnSynergyUpdate(int id, int level)
    //{
    //    if (level == -1) return;
    //    synergyDatas[id].evaluators[level].eval(updateExpression);
    //}


    //#region Augment Functions
    //void AddModifier(string statType, string stat, string modifierType, double amount)
    //{
    //    StatModifier modifier = new StatModifier((float)amount, modifierType);
    //    character.AddModifier(statType, stat, modifier);
    //}
    //public void LaserSight()
    //{
    //    laserSightLineRenderer.gameObject.SetActive(true);
    //    Vector3 lookDir = gunPoint.forward * 100;
    //    laserSightLineRenderer.startWidth = 0.01f;
    //    laserSightLineRenderer.endWidth = 0.01f;
    //    laserSightLineRenderer.useWorldSpace = true;
    //    laserSightLineRenderer.SetPosition(0, gunPoint.position);
    //    laserSightLineRenderer.SetPosition(1, gunPoint.position + lookDir);
    //}
    //#endregion

}
