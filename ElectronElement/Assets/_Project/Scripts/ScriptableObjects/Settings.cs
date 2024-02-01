using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = "Settings")]
public class Settings : ScriptableObject
{
    public enum Keybind
    {
        Forward, Backward, Left, Right,
        Jump, Sprint, Crouch,
        Shoot
    }

    public class Keybinds
    {
        public Dictionary<Keybind, KeyCode[]> bindings = new()
        {
            {Keybind.Forward, new KeyCode[] {KeyCode.W, KeyCode.UpArrow} },
            {Keybind.Backward, new KeyCode[] {KeyCode.S, KeyCode.DownArrow} },
            {Keybind.Left, new KeyCode[] {KeyCode.A, KeyCode.LeftArrow} },
            {Keybind.Right, new KeyCode[] {KeyCode.D, KeyCode.RightArrow} },

            {Keybind.Jump, new KeyCode[] {KeyCode.Space} },
            {Keybind.Sprint, new KeyCode[] {KeyCode.LeftShift} },
            {Keybind.Crouch, new KeyCode[] {KeyCode.LeftControl} },

            {Keybind.Shoot, new KeyCode[] {KeyCode.Mouse0} },
        };
        public bool GetKeybind(Keybind key)
        {
            foreach (KeyCode k in bindings[key])
            {
                if (Input.GetKey(k)) return true;
            }
            return false;
        }
        public bool GetKeybindDown(Keybind key)
        {
            foreach (KeyCode k in bindings[key])
            {
                if (Input.GetKeyDown(k)) return true;
            }
            return false;
        }
        public bool GetKeybindUp(Keybind key)
        {
            foreach (KeyCode k in bindings[key])
            {
                if (Input.GetKeyUp(k)) return true;
            }
            return false;
        }
    }
    [Header("General")]
    public Keybinds keybinds;
    public float sensitivity_normal;
    public float sensitivity_cams;

    [Header("Video")]
    public Resolution resolution;
    public bool isFullscreen;
    public int antiAliasingQuality;

    [Header("Audio")]
    public float masterVolume;

    [Space]

    public float sfxVolume;
    public float sfx_AlarmsVolume;
    public float sfx_FootstepsVolume;
    public float sfx_GunshotVolume;

    [Space]

    public float musicVolume;
    public float ambienceVolume;

    [Header("PostProcessing")]
    public bool useBloom;
    public float bloomIntensity;
    public bool useMotionBlur;
    public float motionBlurIntensity;
    public bool useSSR;
    public float SSRIntensitiy;
}