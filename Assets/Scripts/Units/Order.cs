using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Order : MonoBehaviour
{
    private Action<object[]> Action { set; get; }
    
    public enum OrderState { EXECUTED, PAUSED, PROCESSING, ABORTED, QUEUEING}
    public OrderState State { set; get; } = OrderState.QUEUEING;

    private List<Order> Queue { set; get; }

    public Order(Action<object[]> method)
    {
        Action = method;
    }

    public void Execute()
    {
        State = OrderState.PROCESSING;
        Action.Invoke(Action.Method.GetParameters());
    }
    public void Complete()
    {
        State = OrderState.EXECUTED;
    }
    public void Pause()
    {
        State = OrderState.PAUSED;
    }
    public void Abort()
    {
        State = OrderState.ABORTED;
    }
}