
using System;

[Serializable]
public class DeviceData
{
    // les devices acceptés dans l'écoute
    public string[] ControlPaths;
    // le contrôle scheme dans lequel c'est rangé
    public string BindingGroup;
}
