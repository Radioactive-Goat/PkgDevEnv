using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RG.DialogueSystem;

namespace RG.Testing
{
    public class InputAndTest : MonoBehaviour
    {
        private enum BehaviorE { SpeedUpMultiply, SpeedUpValue, SkipToEnd }
        [SerializeField] private float _defaultTypingSpeed = 0.1f;
        [SerializeField] private DialogCollection _dialogueChain;
        [SerializeField] private BehaviorE _eBehavior;
        [SerializeField] private float _speedUpMultiplier, _speedUpValue;

        [SerializeField] private UiHandler _uiHandler;

        private bool _dialogChainTriggered;

        private void Start()
        {
            TypeWriter.Instance.UpdateTimeGapBetweenLetters(_defaultTypingSpeed);
            TypeWriter.Instance.OnTypingComplete += OnDialogueEnd;

            DialogFlowHandler.Instance.OnCollectionEnded += OnDialogueChainEnded;
        }

        private void OnDestroy()
        {
            TypeWriter.Instance.OnTypingComplete -= OnDialogueEnd;

            DialogFlowHandler.Instance.OnCollectionEnded -= OnDialogueChainEnded;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                _uiHandler.OpenUI();
            if (!_uiHandler.IsDialoguePanelOpen)
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(!TypeWriter.Instance.IsTyping)
                {
                    if(_dialogChainTriggered)
                    {
                        DialogFlowHandler.Instance.NextDialogue();
                    }
                    else
                    {
                        DialogFlowHandler.Instance.StartNewDialogChain(_dialogueChain);
                        _dialogChainTriggered = true;
                    }
                }
                else if(_eBehavior == BehaviorE.SpeedUpMultiply)
                {
                    TypeWriter.Instance.SpeedUpWithMultiplier(_speedUpMultiplier);
                }
                else if (_eBehavior == BehaviorE.SpeedUpValue)
                {
                    TypeWriter.Instance.SpeedUpWithSetValue(_speedUpValue);
                }
                else if (_eBehavior == BehaviorE.SkipToEnd)
                {
                    TypeWriter.Instance.SkipToEnd();
                }
            }

            if(Input.GetKeyUp(KeyCode.E))
            {
                if (TypeWriter.Instance.IsTyping)
                {
                    if (_eBehavior == BehaviorE.SpeedUpMultiply || _eBehavior == BehaviorE.SpeedUpValue)
                    {
                        TypeWriter.Instance.ReturnToDefaultSpeed();
                    }
                }
            }
        }

        private void OnDialogueEnd()
        {
            TypeWriter.Instance.ReturnToDefaultSpeed();
        }

        private void OnDialogueChainEnded()
        {
            _dialogChainTriggered = false;
        }
    }
}
