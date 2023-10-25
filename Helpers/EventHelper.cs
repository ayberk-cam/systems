using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHelper
{
    public static Action EmptyEvent;
    public static Action<string> StringEvent;
    public static Action<int> IntEvent;
    public static Action<bool> BooleanEvent;
    public static Action<ClassObj> ObjectEvent;
    public static Action<string, int, int> MultipleInputEvent;
}
