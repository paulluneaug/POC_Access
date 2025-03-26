using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using static RebindingInfosController;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public abstract class RebindableController : MonoBehaviour
{
    public event Action<RebindableController> OnRebindingStart;
    public event Action<RebindableController> OnRebindingEnds;

    [SerializeField] protected TMP_Text m_rebindableNameField;
    [SerializeField] protected TMP_Text m_rebindableBindingField;
    [SerializeField] protected Button m_rebindButton;

    [NonSerialized] protected RebindingManager m_rebindingManager;

    [NonSerialized] protected RebindingOperation m_rebindingOperation;

    [NonSerialized] protected DeviceData m_selectedDeviceDatas;

    protected void InitRebindableController(RebindingManager rebindingManager, DeviceData deviceData)
    {
        m_rebindingManager = rebindingManager;
        SetDeviceData(deviceData);

        m_rebindableNameField.text = GetRebindableName();

        m_rebindButton.onClick.AddListener(OnRebindButtonClicked);
    }

    public virtual void SetDeviceData(DeviceData deviceData)
    {
        m_selectedDeviceDatas = deviceData;
        UpdateBindingText();
    }

    public virtual void Dispose()
    {
        m_rebindButton.onClick.RemoveListener(OnRebindButtonClicked);
    }

    public abstract RebindingDisplayInfos GetDisplayInfos();

    protected abstract void UpdateBindingText();
    protected abstract string GetRebindableName();

    private void OnRebindButtonClicked()
    {
        if (m_rebindingManager.RebindingOperationRunning)
        {
            return;
        }
        StartRebindingOperation();
    }

    public void CancelRebindingOperation()
    {
        m_rebindingOperation?.Cancel();
    }

    protected virtual void StartRebindingOperation()
    {
        OnRebindingStart?.Invoke(this);
    }

    protected void OnRebindingCanceled(RebindingOperation rebindingOperation)
    {
        EndRebindingOperation();
    }

    protected void OnRebindingComplete(RebindingOperation rebindingOperation)
    {
        EndRebindingOperation();
        UpdateBindingText();

        Debug.Log("End rebinding");
    }

    protected void EndRebindingOperation()
    {
        OnRebindingEnds?.Invoke(this);
        m_rebindingOperation = null;
    }
}
