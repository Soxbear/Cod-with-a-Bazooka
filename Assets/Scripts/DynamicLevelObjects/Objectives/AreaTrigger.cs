using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Soxbear.Channels;

public class AreaTrigger : MonoBehaviour
{
    public bool repeatTrigger;
    bool hasTriggered;

    public ChannelReadWrite<bool> triggered;

    void OnTriggerEnter2D(Collider2D Col) {
        if ((Col.GetComponent<Player>() || Col.GetComponentInParent<Player>()) && ((repeatTrigger && hasTriggered) || !repeatTrigger)) {
            triggered.channelValue = true;
            hasTriggered = true;
        }
    }
}