using UnityEngine;
public class TurnManager { 
    #region Callback System OnTick > Info
    /* A callback system allows any part of the code to give a method,
     * that can be called when an event happens. In this case, when a turn happens,
     * all registered components will get notified and the given methods
     * will be called.*/
    #endregion
    public event System.Action OnTick;
    private int m_TurnCount;
    public TurnManager() { 
        m_TurnCount = 1;
    }
    public void Tick() {
        #region OnTick Invoke > Info
        /* Invoke is method in System.Action calls (“invoke”) all the callback methods,
         * that were registered to the OnTick event.
         * The ? is a special C# syntax used to test if OnTick is null. If null, ? does nothing,
         * If not null, ? will call the method on the right of the ?. (Invoke).
         * Shorthand version that does the same thing as:
         * if(OnTick != null)
           {
               OnTick.Invoke();
           } */
        #endregion
        OnTick?.Invoke();
        m_TurnCount += 1;
        Debug.Log("Current turn count : " + m_TurnCount);
    }
}