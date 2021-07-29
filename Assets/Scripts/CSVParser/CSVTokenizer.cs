using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;


public class CSVToken
{
    public string type;
    public string value;
}


public class CSVTokenizer
{
    public List<List<string>> spec = new List<List<string>>()
    {
        new List<string>() { @"^\s", null },
        new List<string>() { @"^,", "," },
        new List<string>() { @"^""[^""]*""", "string" },
        new List<string>() { @"^.", "token" }
    };
    string str;
    int cursor;

    public void Init(string str)
    {
        this.str = str;
        cursor = 0;
    }

    public CSVToken GetNextToken()
    {
        if (!HasMoreTokens()) return null;
        string s = str.Substring(cursor);
        for (int i = 0; i < spec.Count; i++)
        {
            string regexp = spec[i][0];
            string tokenType = spec[i][1];
            Match matched = new Regex(regexp).Match(s);

            if (matched.Success)
            {
                cursor += matched.Value.Length;

                if (tokenType == null)
                {
                    return GetNextToken();
                }

                return new CSVToken()
                {
                    type = tokenType,
                    value = matched.Value
                };
            }
        }
        throw new Exception("Unexpected token: " + s);
    }

    public bool HasMoreTokens()
    {
        return cursor < str.Length;
    }
}
