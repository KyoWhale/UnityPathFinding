using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;

public class PathfinderSelector : MonoBehaviour
{
    private Pathfinder[] m_pathFinders;
    private TMP_Dropdown m_dropdown;
    private int m_dropdownIndex = -1;
    private Button m_startButton;

    private void Start()
    {
        FindPathFinders();
        SetDropdown();
        SetStartButton();
    }

    private void FindPathFinders()
    {
        var pathfinderGameObject = FindObjectOfType<Pathfinder>();
        m_pathFinders = pathfinderGameObject.GetComponents<Pathfinder>();
    }

    private void SetDropdown()
    {
        m_dropdown = GetComponentInChildren<TMP_Dropdown>();

        List<OptionData> options = new List<OptionData>();
        foreach (var pathFinder in m_pathFinders)
        {
            options.Add(new OptionData(pathFinder.GetType().ToString()));
        }
        m_dropdown.AddOptions(options);
        m_dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDropdownChanged(int index)
    {
        m_dropdownIndex = index - 1;
    }

    private void SetStartButton()
    {
        m_startButton = GetComponentInChildren<Button>();
        m_startButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (m_dropdownIndex < 0 || m_pathFinders.Length <= m_dropdownIndex)
        {
            return;
        }

        m_pathFinders[m_dropdownIndex].StartPathFinding();
    }
}
