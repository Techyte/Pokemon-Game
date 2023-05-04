using System.Collections;
using PokemonGame.ScriptableObjects;
using TMPro;
using UnityEngine.UI;

namespace PokemonGame.Game
{
    using UnityEngine;
    using System;

    public class ItemGetNotificationManager : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float speed;
        [SerializeField] private float totalStayTime;
        [Space]
        [SerializeField] private Transform notificationStartPos;
        [SerializeField] private Transform notificationEndPos;
        [Space]
        [SerializeField] private Transform notification;
        [Space]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image textureImage;
        [SerializeField] private TextMeshProUGUI amountText;
        
        private bool moving;

        private void Awake()
        {
            Bag.GotItem += ShowNotification;
        }

        private void Update()
        {
            if (moving)
            {
                notification.position = Vector3.Lerp(notification.position, notificationEndPos.position, speed * Time.deltaTime);
            }
            else
            {
                notification.position = notificationStartPos.position;
            }
        }

        private void ShowNotification(object sender, BagGotItemEventArgs e)
        {
            moving = true;
            notification.position = notificationStartPos.position;
            SetNotificationInfo(e.item, e.amount);
            StartCoroutine(StayTimer());
        }

        private void SetNotificationInfo(Item item, int amount)
        {
            nameText.text = item.name;
            descriptionText.text = item.description;
            textureImage.sprite = item.sprite;
            amountText.text = $"x{amount}";
        }

        private IEnumerator StayTimer()
        {
            yield return new WaitForSeconds(totalStayTime);
            moving = false;
        }
    }   
}