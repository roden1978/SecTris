using System;

[Serializable]
public class SettingsData
{
    public int _mute;
    public float _volume;

    public SettingsData()
    {
        _mute = 0;
        _volume = 1f;
    }
}
