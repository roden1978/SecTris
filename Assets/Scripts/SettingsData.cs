using System;

[Serializable]
public class SettingsData
{
    public int _mute;
    public float _volume;

    public SettingsData()
    {
        _mute = 1;
        _volume = 0.5f;
    }
}
