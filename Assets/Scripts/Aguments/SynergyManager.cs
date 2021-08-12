using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SynergyData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Descriptions { get; set; }
    public string Code { get; set; }
    public Expression UpdateExpression;
    public Expression AttachExpression;
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
