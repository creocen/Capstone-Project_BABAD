using System;
using System.Collections.Generic;

namespace Core.State_Machine
{
    public class StateMachine
    {
        IState currentState;
        Dictionary<Type, StateNode> nodes = new();
        List<Transition> anyTransitions = new();

        public void Update()
        {
            var transition = GetTransition();

            if (transition != null)
            {
                ChangeState(transition.To);

                currentState?.Update();
            }
        }

        public void FixedUpdate()
        {
            currentState?.FixedUpdate();
        }

        public void SetState(IState state)
        {
            currentState = state;
            currentState?.OnEnter();
        }

        public void ChangeState(IState nextState)
        {
            if (nextState == currentState) return;

            currentState?.OnExit();
            currentState = nextState;
            currentState?.OnEnter();
        }

        Transition GetTransition()
        {
            foreach (var transition in anyTransitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }

            if (nodes.TryGetValue(currentState.GetType(), out var node))
            {
                foreach (var transition in node.Transitions)
                {
                    if (transition.Condition.Evaluate())
                    {
                        return transition;
                    }
                }
            }
            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            var fromNode = GetOrAddNode(from);
            fromNode.Transitions.Add(new Transition(to, condition));
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            anyTransitions.Add(new Transition(to, condition));
        }

        StateNode GetOrAddNode(IState state)
        {
            var type = state.GetType();

            if (!nodes.TryGetValue(type, out var node))
            {
                node = new StateNode(state);
                nodes[type] = node;
            }
            return node;
        }

        class StateNode
        {
            public IState State;
            public List<Transition> Transitions = new();
            public StateNode(IState state) => State = state;
        }

        class Transition
        {
            public IState To;
            public IPredicate Condition;
            public Transition(IState to, IPredicate condition)
            {
                To = to;
                Condition = condition;
            }
        }
    }
}
