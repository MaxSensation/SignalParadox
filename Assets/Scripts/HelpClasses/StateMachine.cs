//Main author: Maximiliam Rosén

using System.Collections.Generic;
using System;

public class StateMachine
{
    private State currentState;
    private State queuedState;
    private Stack<State> automaton = new Stack<State>();
    private readonly Dictionary<Type, State> stateDictionary = new Dictionary<Type, State>();

    public StateMachine(object controller, State[] states)
    {
        foreach (var state in states)
        {
            var instance = UnityEngine.Object.Instantiate(state);
            instance.owner = controller;
            instance.stateMachine = this;
            stateDictionary.Add(instance.GetType(), instance);
            
            if (currentState == null)
                currentState = instance;
        }
        if (currentState != null)
            currentState.Enter();
    }

    public void TransitionTo<T>() where T : State
    {
        queuedState = stateDictionary[typeof(T)];
    }

    public void StackState<T>() where T : State
    {
        automaton.Push(stateDictionary[typeof(T)]);
    }
    
    public void UnStackState()
    {
        queuedState = automaton.Pop();
    }
    
    public void Run()
    {
        UpdateState();
        currentState.Run();
    }

    private void UpdateState()
    {
        if (queuedState == null || queuedState == currentState) return;
        if (currentState != null)
            currentState.Exit();
        currentState = queuedState;
        currentState.Enter();
    }
}