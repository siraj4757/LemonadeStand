using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace LemonadeStand.Presentation.UI
{
    /// <summary>
    /// Level 2 board:
    /// - Player taps the lemon box.
    /// - Each tap spawns one lemon and moves it into the clear container.
    /// - When target count is reached, we notify the caller.
    /// </summary>
    public class CountLemonsBoardView : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject root;

        [Header("Container & Box")]
        [SerializeField] private Image containerImage;       
        [SerializeField] private Button lemonBoxButton;
        [SerializeField] private RectTransform boxSpawnPoint;
        

        [Header("Lemon Prefab")]
        [SerializeField] private RectTransform lemonPrefab;

        [Header("Slots Inside Container")]
        [SerializeField] private RectTransform[] lemonSlots;

        [Header("Animation")]
        [SerializeField] private float moveDuration = 0.4f;
        [SerializeField] private Ease moveEase = Ease.OutQuad;

        private int _targetCount;
        private int _currentCount;
        private Action _onCompleted;
        private readonly List<RectTransform> _spawnedLemons = new List<RectTransform>();


        private void Awake()
        {
            if (root == null)
                root = gameObject;

            lemonBoxButton.onClick.RemoveAllListeners();
        }

 
        public void Show(int targetCount, Action onCompleted)
        {
            _targetCount = Mathf.Clamp(targetCount, 0, lemonSlots.Length);
            _currentCount = 0;
            _onCompleted = onCompleted;

            ClearExistingLemons();

            root.SetActive(true);
            lemonBoxButton.interactable = true;

            lemonBoxButton.onClick.RemoveAllListeners();
            lemonBoxButton.onClick.AddListener(OnBoxClicked);
        }

 
        public void Hide()
        {
            lemonBoxButton.onClick.RemoveListener(OnBoxClicked);
            ClearExistingLemons();
            root.SetActive(false);
        }

        private void OnBoxClicked()
        {
            if (_currentCount >= _targetCount)
                return;

            if (_currentCount >= lemonSlots.Length)
                return; 

            SpawnLemon(_currentCount);

            _currentCount++;

            if (_currentCount >= _targetCount)
            {
                lemonBoxButton.interactable = false;

                DOVirtual.DelayedCall(moveDuration + 0.1f, () =>
                {
                    _onCompleted?.Invoke();
                });
            }
        }

        private void SpawnLemon(int index)
        {
            if (lemonPrefab == null || boxSpawnPoint == null || lemonSlots == null)
                return;

            RectTransform slot = lemonSlots[index];

            RectTransform lemon = Instantiate(lemonPrefab, root.transform);
            _spawnedLemons.Add(lemon);

            lemon.gameObject.SetActive(true);

            lemon.position = boxSpawnPoint.position;

            var numberLabel = lemon.GetComponentInChildren<TextMeshProUGUI>();
            if (numberLabel != null)
                numberLabel.text = (index + 1).ToString();

            lemon.DOMove(slot.position, moveDuration)
                 .SetEase(moveEase);
        }


        private void ClearExistingLemons()
        {
            foreach (var lemon in _spawnedLemons)
            {
                if (lemon != null)
                    Destroy(lemon.gameObject);
            }

            _spawnedLemons.Clear();
        }
    }
}
