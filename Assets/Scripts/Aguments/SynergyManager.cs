using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyData
{
    public int id { get; set; }
    public string name { get; set; }
    public string descriptions { get; set; }
    public string code { get; set; }
    public Expression updateExpression;
    public Expression attachExpression;
}
public class SynergyManager : MonoBehaviour
{
    public SynergyManager current;
    private void Awake()
    {
        current = this;
    }

    public void Init()
    {
        
    }


}
