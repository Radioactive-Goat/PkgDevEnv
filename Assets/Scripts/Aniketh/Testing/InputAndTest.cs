using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RG.DialogueSystem;

namespace RG.Testing
{
    public class InputAndTest : MonoBehaviour
    {
        private enum BehaviorE { SpeedUpMultiply, SpeedUpValue, SkipToEnd }
        private enum InputState { Conversation, Responding }

        [SerializeField] private float _defaultTypingSpeed = 0.1f;
        [SerializeField] private DialogCollection _dialogueChain;
        [SerializeField] private BehaviorE _eBehavior;
        [SerializeField] private float _speedUpMultiplier, _speedUpValue;

        [SerializeField] private UiHandler _uiHandler;

        private bool _dialogChainTriggered;
        private InputState _inputState;

        private void Start()
        {
            _inputState = InputState.Conversation;
            TypeWriter.Instance.UpdateTimeGapBetweenLetters(_defaultTypingSpeed);
            TypeWriter.Instance.OnTypingComplete += OnDialogueEnd;

            DialogFlowHandler.Instance.OnCollectionEnded += OnDialogueChainEnded;
            ResponseOptionsHandler.Instance.OnStartResponse += OnResponseOptionsBegun;
            ResponseOptionsHandler.Instance.OnEndResponse += OnResponseOptionsEnd;
        }

        private void OnDestroy()
        {
            TypeWriter.Instance.OnTypingComplete -= OnDialogueEnd;

            DialogFlowHandler.Instance.OnCollectionEnded -= OnDialogueChainEnded;
            ResponseOptionsHandler.Instance.OnStartResponse -= OnResponseOptionsBegun;
            ResponseOptionsHandler.Instance.OnEndResponse -= OnResponseOptionsEnd;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                _uiHandler.OpenUI();
            if (!_uiHandler.IsDialoguePanelOpen)
                return;

            if(_inputState == InputState.Conversation)
            {
                DialogConversationInputs();
            }
            else if(_inputState == InputState.Responding)
            {
                DialogResponcesInputs();
            }
        }

        private void DialogConversationInputs()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!TypeWriter.Instance.IsTyping)
                {
                    if (_dialogChainTriggered)
                    {
                        DialogFlowHandler.Instance.NextDialogue();
                    }
                    else
                    {
                        DialogFlowHandler.Instance.StartNewDialogChain(_dialogueChain);
                        _dialogChainTriggered = true;
                    }
                }
                else
                {
                    if (_eBehavior == BehaviorE.SpeedUpMultiply)
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
            }

            if (Input.GetKeyUp(KeyCode.E))
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

        private void DialogResponcesInputs()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                ResponseOptionsHandler.Instance?.NavigateDown();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                ResponseOptionsHandler.Instance?.NavigateUp();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                ResponseOptionsHandler.Instance?.SelectResponse();
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

        private void OnResponseOptionsBegun()
        {
            _inputState = InputState.Responding;
        }

        private void OnResponseOptionsEnd()
        {
            _inputState = InputState.Conversation;
        }
    }
}
