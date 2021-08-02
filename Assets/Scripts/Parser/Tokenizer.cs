using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class Token
{
    public string type;
    public string value;
}
public class Tokenizer
{
    public List<List<string>> spec = new List<List<string>>()
        {
            new List<string>() { @"^\s+", null },
            new List<string>() { @"\/\/.*", null },
            new List<string>() { @"^/\*[\s\S]*?\*/", null },
            new List<string>() { @"^;", ";"},
            new List<string>() { @"^\{", "{"},
            new List<string>() { @"^\}", "}"},
            //new List<string>() { @"^\(.*\)\s*\=\>", "lambda" },
            new List<string>() { @"^\(", "("},
            new List<string>() { @"^\)", ")"},
            new List<string>() { @"^,", ","},
            new List<string>() { @"^\.", "."},
            new List<string>() { @"^\[", "["},
            new List<string>() { @"^\]", "]"},
            //new List<string>() { @"^=>", "=>"},
            new List<string>() { @"^\blet\b", "let" },
            new List<string>() { @"^\blambda\b", "lambda" },
            new List<string>() { @"^\bif\b", "if" },
            new List<string>() { @"^\belse\b", "else" },
            new List<string>() { @"^\btrue\b", "true" },
            new List<string>() { @"^\bfalse\b", "false" },
            new List<string>() { @"^\bnull\b", "null" },
            new List<string>() { @"^\bwhile\b", "while" },
            new List<string>() { @"^\bdo\b", "do" },
            new List<string>() { @"^\bfor\b", "for" },
            new List<string>() { @"^\bdef\b", "def"},
            new List<string>() { @"^\breturn\b", "return"},
            new List<string>(){ @"^\d+\.?\d*", "NUMBER" },
            new List<string>() { @"^[A-Za-z]+\w*", "IDENTIFIER" },
            new List<string>() { @"^[=!]=", "EQUALITY_OPERATOR"},
            new List<string>() { @"^=", "SIMPLE_ASSIGN"},
            new List<string>() { @"^[\*\/\+\-]=", "COMPLEX_ASSIGN"},
            new List<string>() { @"^\+\+", "INCREMENT_ASSIGN"},
            new List<string>() { @"^\-\-", "DECREMENT_ASSIGN"},
            new List<string>() { @"^[+\-]", "ADDITIVE_OPERATOR"},
            new List<string>() { @"^[*\/]", "MULTIPLICATIVE_OPERATOR" },
            new List<string>() { @"^[><]=?", "RELATIONAL_OPERATOR" },
            new List<string>() { @"^&&", "LOGICAL_AND" },
            new List<string>() { @"^\|\|", "LOGICAL_OR" },
            new List<string>() { @"^!", "LOGICAL_NOT" },

            new List<string>(){ @"""[^""]*""", "STRING" },
            new List<string>(){ @"'[^']*'", "STRING" }
        };

    string str;
    int cursor;
    public void Init(string str)
    {
        this.str = str;
        this.cursor = 0;
    }

    public bool IsEOF()
    {
        return cursor >= str.Length;
    }

    public bool HasMoreTokens()
    {
        return this.cursor < this.str.Length;
    }

    public Token GetNextToken()
    {
        if (!HasMoreTokens()) return null;

        string s = str.Substring(cursor);

        for (int i = 0; i < spec.Count; i++)
        {
            string regexp = spec[i][0];
            string tokenType = spec[i][1];
            Regex regex = new Regex(regexp);
            Match matched = regex.Match(s);

            if (matched.Success)
            {
                cursor += matched.Value.Length;

                if (tokenType == null)
                {
                    return GetNextToken();
                }

                return new Token()
                {
                    type = tokenType,
                    value = matched.Value
                };
            }
        }
        throw new Exception("Unexpected token: " + s);
    }
}
