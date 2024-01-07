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
            TypeWriter.Instance.OnStartTyping += OnDialogueStarted;
            TypeWriter.Instance.OnTextUpdated += OnDialogueUpdated;
            TypeWriter.Instance.OnTypingComplete += OnDialogueCompleted;

            DialogFlowHandler.Instance.OnCollectionEnded += CloseUI;
            DialogFlowHandler.Instance.OnForceEnded += CloseUI;
        }

        private void OnDestroy()
        {
            TypeWriter.Instance.OnStartTyping -= OnDialogueStarted;
            TypeWriter.Instance.OnTextUpdated -= OnDialogueUpdated;
            TypeWriter.Instance.OnTypingComplete -= OnDialogueCompleted;

            DialogFlowHandler.Instance.OnCollectionEnded -= CloseUI;
            DialogFlowHandler.Instance.OnForceEnded -= CloseUI;
        }

        private void OnDialogueStarted()
        {
            _arrowObject.SetActive(false);
        }

        private void OnDialogueUpdated(string newDialogueValue)
        {
            _dialogueText.SetText(newDialogueValue);
        }

        private void OnDialogueCompleted()
        {
            _arrowObject.SetActive(true);
        }

        public void OpenUI()
        {
            if (!_dialogSystemPanel.activeSelf)
            {
                CharactorDisplayHandler.Instance.ResetCharactorDisplays();
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
