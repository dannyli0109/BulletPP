using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Linq;

public class AugmentData
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Descriptions { get; set; }

    public int Rarity { get; set; }
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
    private void Awake()
    {
        current = this;
        augmentDatas = new List<AugmentData>();

        using (var reader = new StreamReader("./Assets/Resources/AugmentList.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            augmentDatas = Enumerable.ToList(csv.GetRecords<AugmentData>());
        }
    }
}
