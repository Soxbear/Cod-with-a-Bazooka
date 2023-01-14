using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class ExternalTrigger : MonoBehaviour {
    [SerializeField]
    public MonoBehaviour User;

    int Identifier;

    void OnTriggerEnter2D(Collider2D Collider) {
        if ((User as ExternalTriggerEnter2DUser) != null)
            ((ExternalTriggerEnter2DUser) User).ExternalTriggerEnter2D(Collider, Identifier);
    }
    void OnTriggerStay2D(Collider2D Collider) {
        if ((User as ExternalTriggerStay2DUser) != null)
            ((ExternalTriggerStay2DUser) User).ExternalTriggerStay2D(Collider, Identifier);
    }
    void OnTriggerExit2D(Collider2D Collider) {
        if ((User as ExternalTriggerExit2DUser) != null)
            ((ExternalTriggerExit2DUser) User).ExternalTriggerExit2D(Collider, Identifier);
    }
}

public interface ExternalTriggerEnter2DUser
{
    public virtual void ExternalTriggerEnter2D(Collider2D Collider, int Identifier = 0) {

    }
}
public interface ExternalTriggerStay2DUser
{
    public virtual void ExternalTriggerStay2D(Collider2D Collider, int Identifier = 0) {

    }
}public interface ExternalTriggerExit2DUser
{
    public virtual void ExternalTriggerExit2D(Collider2D Collider, int Identifier = 0) {

    }
}