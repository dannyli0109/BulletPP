using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class CSVParser
{
    string str;
    CSVTokenizer tokenizer;
    CSVToken lookahead;

    public CSVParser()
    {
        str = "";
        tokenizer = new CSVTokenizer();
    }

    public List<string> Parse(string str)
    {
        this.str = str;
        tokenizer.Init(str);
        lookahead = tokenizer.GetNextToken();

        List<string> list = new List<string>();
        while (lookahead != null)
        {
            list.Add(Item());
        }
        return list;
    }

    public string Item()
    {
        string result = "";

        while (lookahead != null && lookahead.type != ",")
        {
            CSVToken token = Eat(lookahead.type);
            if (token.type == "string")
            {
                result += token.value;
            }
            else
            {
                result += token.value;
            }
        }
        if (lookahead != null)
        {
            Eat(",");
        }

        result = cleanString(result);
        return result;
    }

    public CSVToken Eat(string tokenType)
    {
        CSVToken token = lookahead;
        if (token == null)
        {
            throw new Exception("Unexpected end of input, expected: " + tokenType);
        }

        if (token.type != tokenType)
        {
            throw new Exception("Unexpected token: " + token.value + ", expected: " + tokenType);
        }

        lookahead = tokenizer.GetNextToken();
        return token;
    }
    public string cleanString(string input)
    {
        if (input.Length >= 2 && input[0] == '"' && input[input.Length - 1] == '"')
        {
            input = input.Substring(1, input.Length - 2);
        }
        return input;
    }
}
