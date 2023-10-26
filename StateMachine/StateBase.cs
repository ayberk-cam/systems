using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    public virtual void Start() { }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}