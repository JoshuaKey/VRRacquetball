using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnCollisionEnter() {
        Game.Instance.OutOfBounds();
    }
}
