using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace com.MLR.Wesp
{
    public class SoundManager : SingletonBehaviour<SoundManager>
    {

        public AudioClip moveSound;
        public AudioClip deadSound;
        public AudioClip succesSound;

        public AudioClip uiMoveSound;
        public AudioClip uiSelectSound;

        void OnEnable()
        {
            Button[] buttons = FindObjectsOfType<Button>();
            foreach (Button button in buttons)
            {
                EventTrigger eventTrigger = null;
                if (button.gameObject.GetComponent<EventTrigger>() == null)
                {
                    button.gameObject.AddComponent<EventTrigger>();
                }
                eventTrigger = button.gameObject.GetComponent<EventTrigger>();
                EventTrigger.Entry over = new EventTrigger.Entry();
                over.eventID = EventTriggerType.PointerEnter;
                over.callback.AddListener(delegate
                {
                    this.PlayUIMoveSound();
                });
                if (eventTrigger.triggers == null)
                {
                    eventTrigger.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();
                }
                eventTrigger.triggers.Add(over);
                button.onClick.AddListener(delegate
                {
                    this.PlayUISelectSound();
                });
            }
        }

        public void PlayMoveSound()
        {
            this.GetComponent<AudioSource>().clip = moveSound;
            this.GetComponent<AudioSource>().Play();
        }

        public void PlayDeadSound()
        {
            this.GetComponent<AudioSource>().clip = deadSound;
            this.GetComponent<AudioSource>().Play();
        }

        public void PlaySuccessSound()
        {
            this.GetComponent<AudioSource>().clip = succesSound;
            this.GetComponent<AudioSource>().Play();
        }

        public void PlayUIMoveSound()
        {
            this.GetComponent<AudioSource>().clip = uiMoveSound;
            this.GetComponent<AudioSource>().Play();
        }

        public void PlayUISelectSound()
        {
            this.GetComponent<AudioSource>().clip = uiSelectSound;
            this.GetComponent<AudioSource>().Play();
        }
    } 
}
