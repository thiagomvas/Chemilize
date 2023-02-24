using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    // Singleton for easy access
    private static SettingsData _current;
    public static SettingsData Current
    {
        get
        {
            if (_current == null)
                _current = new SettingsData();
            return _current;

        }
        set
        {
            if (_current != value)
            {
                _current = value;
            }
        }

    }


    //Data i wish to save
    public int mouseSensitivity;

}
