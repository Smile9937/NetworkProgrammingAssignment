using TMPro;
using Unity.Collections;
using UnityEngine;

public class PlayerNameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Vector2 upDirection;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private OverheadText overheadText;

    void Awake() => overheadText.playerName.OnValueChanged += UpdateUI;

    private void UpdateUI(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        textField.text = newValue.ToString();
    } 

    void Update()
    {
        transform.eulerAngles = upDirection;
        transform.position = playerTransform.position + positionOffset;
    }
}