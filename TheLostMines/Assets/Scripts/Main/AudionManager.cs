using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudionManager : MonoBehaviour
{
    [SerializeField] private AudioSource _musik;
    [SerializeField] private Slider _sliderVolume;

    private bool _on = true;

    public void ChangeVolume()
    {
        _musik.volume = _sliderVolume.value;
    }

    public void ChengeOnMusik()
    {
        _on = !_on;
        if (_on)
        {
            _musik.Play();
        }
        else
        {
            _musik.Stop();
        }
    }
}
