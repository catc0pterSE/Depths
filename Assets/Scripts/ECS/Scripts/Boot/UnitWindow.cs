using TMPro;
using UnityEngine;

namespace ECS.Boot
{
    public sealed class UnitWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textStatsLabel;
        [SerializeField] private TMP_Text _textPartsLabel;
        public void SetStats(string description)
        {
            _textStatsLabel.text = description;
        }
		
        public void SetParts(string description)
        {
            _textPartsLabel.text = description;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}