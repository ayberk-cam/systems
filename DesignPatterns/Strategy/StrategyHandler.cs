using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StrategyTypes
{
    First,
    Second
}

public class StrategyHandler : MonoBehaviour
{
    //public void FalseHandler(StrategyTypes type)
    //{
    //    switch(type)
    //    {
    //        case StrategyTypes.First:
    //            break;
    //        case StrategyTypes.Second:
    //            break;
    //    }
    //}

    public void TrueHandler(IStrategy strategy)
    {
        strategy.ExampleFunction();
    }
}
