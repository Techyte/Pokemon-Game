using UnityEngine;

namespace PokemonGame.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Visual Cue")]
        [SerializeField] private GameObject visualCue;

        [SerializeField] private bool playerInRange;

        [Header("Ink JSON")]
        [SerializeField] private TextAsset inkJson;

        private void Update()
        {
            if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
            {
                visualCue.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("doing the thing");
                    DialogueManager.GetInstance().EnterDialogueMode(inkJson);
                }
            }
            else
            {
                visualCue.SetActive(false);
            }
        }

        private void Awake()
        {
            playerInRange = false;
            visualCue.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
                playerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
                playerInRange = false;
        }
    }
}
