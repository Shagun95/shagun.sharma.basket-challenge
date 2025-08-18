using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UltimateClean
{
    /// <summary>
    /// Custom switch component used in the kit. You can think of it as an animated toggle.
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Animator))]
    public class Switch : MonoBehaviour
    {
        private Button button;
        private Animator animator;

        private Image bgEnabledImage;
        private Image bgDisabledImage;

        private Image handleEnabledImage;
        private Image handleDisabledImage;

        private bool switchEnabled; 

        public UnityEvent<bool> OnValueChanged;

        private void Awake()
        {
            button = GetComponent<Button>();
            animator = GetComponent<Animator>();

            bgEnabledImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            bgDisabledImage = transform.GetChild(0).GetChild(1).GetComponent<Image>();
            handleEnabledImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
            handleDisabledImage = transform.GetChild(1).GetChild(1).GetComponent<Image>();

            switchEnabled = true;
        }

        private void OnEnable()
        {
            button.onClick.AddListener(Toggle);
        }
        
        private void OnDisable()
        {
            button.onClick.RemoveListener(Toggle);
        }

        public void Toggle()
        {
            switchEnabled = !switchEnabled;
            bgEnabledImage.gameObject.SetActive(switchEnabled);
            bgDisabledImage.gameObject.SetActive(!switchEnabled);
            handleEnabledImage.gameObject.SetActive(switchEnabled);
            handleDisabledImage.gameObject.SetActive(!switchEnabled);
            animator.SetTrigger(switchEnabled ? "Enable" : "Disable");
            OnValueChanged?.Invoke(switchEnabled);
        }

        public bool IsToggled()
        {
            return switchEnabled;
        }
        public void SetState(bool state, bool invokeEvent = true)
        {
            if (switchEnabled == state) return;

            switchEnabled = state;

            bgEnabledImage.gameObject.SetActive(switchEnabled);
            bgDisabledImage.gameObject.SetActive(!switchEnabled);
            handleEnabledImage.gameObject.SetActive(switchEnabled);
            handleDisabledImage.gameObject.SetActive(!switchEnabled);

            animator.SetTrigger(switchEnabled ? "Enable" : "Disable");

            if (invokeEvent)
                OnValueChanged?.Invoke(switchEnabled);
        }
    }
}
