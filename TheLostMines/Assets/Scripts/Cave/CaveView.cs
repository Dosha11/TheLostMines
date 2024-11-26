using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaveView : MonoBehaviour
{
    public int Key;
    [SerializeField] private Text _textStatus;
    [SerializeField] private GameObject _stone;

    private List<string> _statuses = new List<string>() 
    {
        "��������� ���������",
        "�� �������� ��� ������",
        "�����",
        "�������� ����������",
        "���������� ���������"
    };

    private void Start()
    {
        CaveManager.Instance.CheckView();
    }

    public void ChangeStatus(Status status)
    {
        _textStatus.text = _statuses[(int)status];
    }


    public void Touch() 
    {
        Debug.Log("�������� �����");
        CaveManager.Instance.lol(Key);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�������");
        CaveManager.Instance.CollisionCave(Key);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�������");
        CaveManager.Instance.CollisionCave(Key);
    }

    private void OnCollisionExit(Collision collision)
    {
        Manager.Instance.CloseIteractionButton();
    }

    public void ChangeObval(bool obval)
    {
        _stone.SetActive(obval);
    }
}
