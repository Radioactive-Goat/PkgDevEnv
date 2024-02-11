using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RG.DialogueSystem;

namespace RG.Testing
{
    public class UiHandler : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _dialogueText;
        [SerializeField] GameObject _dialogSystemPanel, _arrowObject;
        public bool IsDialoguePanelOpen => _dialogSystemPanel.activeSelf;

        void Start()
        {
            DialogueSystemRefs.Instance.TypeWriter.OnStartTyping += OnDialogueStarted;
            DialogueSystemRefs.Instance.TypeWriter.OnTextUpdated += OnDialogueUpdated;
            DialogueSystemRefs.Instance.TypeWriter.OnTypingComplete += OnDialogueCompleted;

            DialogueSystemRefs.Instance.DialogFlowHandler.OnCollectionEnded += CloseUI;
            DialogueSystemRefs.Instance.DialogFlowHandler.OnForceEnded += CloseUI;

            DialogueSystemRefs.Instance.ResponseOptionsHandler.OnStartResponse += OnResponcesShown;
        }

        private void OnDestroy()
        {
            DialogueSystemRefs.Instance.TypeWriter.OnStartTyping -= OnDialogueStarted;
            DialogueSystemRefs.Instance.TypeWriter.OnTextUpdated -= OnDialogueUpdated;
            DialogueSystemRefs.Instance.TypeWriter.OnTypingComplete -= OnDialogueCompleted;

            DialogueSystemRefs.Instance.DialogFlowHandler.OnCollectionEnded -= CloseUI;
            DialogueSystemRefs.Instance.DialogFlowHandler.OnForceEnded -= CloseUI;

            DialogueSystemRefs.Instance.ResponseOptionsHandler.OnStartResponse -= OnResponcesShown;
        }

        private void OnDialogueStarted()
        {
            _arrowObject.SetActive(false);
        }

        private void OnDialogueUpdated(string newDialogueValue)
        {
            _dialogueText.SetText(newDialogueValue);
        }

        private void OnResponcesShown()
        {
            _dialogueText.SetText("");
        }

        private void OnDialogueCompleted()
        {
            _arrowObject.SetActive(true);
        }

        public void OpenUI()
        {
            if (!_dialogSystemPanel.activeSelf)
            {
                DialogueSystemRefs.Instance.CharactorDisplayHandler.ResetCharactorDisplays();
                _dialogueText?.SetText("");
                _dialogSystemPanel.SetActive(true);
            }
        }

        public void CloseUI()
        {
            if (_dialogSystemPanel.activeSelf)
            {
                _dialogSystemPanel.SetActive(false);
            }
        }
    }
}
