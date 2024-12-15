using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmployeeData", menuName = "Scriptable/EmployeeData")]
public class EmployeeData : ScriptableObject
{
    //Must be unique in same dept
    public int employeeId;

    public EmployeeType employeeType;
    public List<HiringCost> hiringCosts;
    public int unlockLevel;
    public string employeeTag;
    public Sprite tagSprite;
    public Sprite displayImg;
    public float servingTime;
    public float managerCommunicationDelay;
    public string empDescription;
    public GameObject emplyeePrefab;
}