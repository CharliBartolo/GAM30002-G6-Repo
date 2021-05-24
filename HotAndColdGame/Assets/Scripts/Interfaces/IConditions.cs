using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConditions
{
    enum ConditionTypes 
    { 
        ConditionHot,
        ConditionCold
    };

    void AddCondition(ConditionTypes nameOfCondition);
    void RemoveCondition(ConditionTypes nameOfCondition);

    List<ConditionTypes> ActiveConditions {get; set;}
}
