using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelButton : MonoBehaviour
{
    public ControlPanel controlPanel;
    public ControlPanel.BUTTON_TYPE type;

    private void OnCollisionEnter(Collision collision)
    {
        controlPanel.OnButton(type);
    }
}
