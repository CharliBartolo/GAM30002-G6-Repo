using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConditions
{
    void AddCondition(string nameOfCondition);
    void RemoveCondition(string nameOfCondition);

    List<string> ActiveConditions {get; set;}
}
