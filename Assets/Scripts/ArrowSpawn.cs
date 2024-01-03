using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawn : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;

    private XRGrabInteractable _bow;
    private bool _arrowNotched = false;
    private GameObject _currentArrow = null;

    private void Start()
    {
        _bow = GetComponent<XRGrabInteractable>();

        // subscribe to NotchEmpty
        PullInteraction.PullActionReleased += NotchEmpty;
    }

    // unsubscribe on destroy to avoid bugs
    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= NotchEmpty;
    }

    private void Update()
    {
        if (_bow.isSelected && _arrowNotched == false) {
            _arrowNotched = true;
            StartCoroutine("DelayedSpawn");
        }
        if (!_bow.isSelected && _currentArrow != null) {
            Destroy(_currentArrow);
            NotchEmpty(0f);
        }
    }
    // spawn an arrow after a delayed amount of time
    IEnumerator DelayedSpawn() {
        yield return new WaitForSeconds(1f);
        _currentArrow = Instantiate(arrow, notch.transform);
    }

    private void NotchEmpty(float v) {
        _arrowNotched = false;
        _currentArrow = null;
    }
}
