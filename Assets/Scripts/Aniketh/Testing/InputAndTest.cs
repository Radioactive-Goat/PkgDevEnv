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
        private TypeWriter _typeWriter;

        private void Start()
        {
            _inputState = InputState.Conversation;
            _typeWriter = DialogueSystemRefs.Instance.TypeWriter;
            _typeWriter.UpdateTimeGapBetweenLetters(_defaultTypingSpeed);
            _typeWriter.OnTypingComplete += OnDialogueEnd;

            DialogueSystemRefs.Instance.DialogFlowHandler.OnCollectionEnded += OnDialogueChainEnded;
            DialogueSystemRefs.Instance.ResponseOptionsHandler.OnStartResponse += OnResponseOptionsBegun;
            DialogueSystemRefs.Instance.ResponseOptionsHandler.OnEndResponse += OnResponseOptionsEnd;
        }

        private void OnDestroy()
        {
            _typeWriter.OnTypingComplete -= OnDialogueEnd;

            DialogueSystemRefs.Instance.DialogFlowHandler.OnCollectionEnded -= OnDialogueChainEnded;
            DialogueSystemRefs.Instance.ResponseOptionsHandler.OnStartResponse -= OnResponseOptionsBegun;
            DialogueSystemRefs.Instance.ResponseOptionsHandler.OnEndResponse -= OnResponseOptionsEnd;
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
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                if (!_typeWriter.IsTyping)
                {
                    if (_dialogChainTriggered)
                    {
                        DialogueSystemRefs.Instance.DialogFlowHandler.NextDialogue();
                    }
                    else
                    {
                        DialogueSystemRefs.Instance.DialogFlowHandler.StartNewDialogChain(_dialogueChain);
                        _dialogChainTriggered = true;
                    }
                }
                else
                {
                    if (_eBehavior == BehaviorE.SpeedUpMultiply)
                    {
                        _typeWriter.SpeedUpWithMultiplier(_speedUpMultiplier);
                    }
                    else if (_eBehavior == BehaviorE.SpeedUpValue)
                    {
                        _typeWriter.SpeedUpWithSetValue(_speedUpValue);
                    }
                    else if (_eBehavior == BehaviorE.SkipToEnd)
                    {
                        _typeWriter.SkipToEnd();
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                if (_typeWriter.IsTyping)
                {
                    if (_eBehavior == BehaviorE.SpeedUpMultiply || _eBehavior == BehaviorE.SpeedUpValue)
                    {
                        _typeWriter.ReturnToDefaultSpeed();
                    }
                }
            }
        }

        private void DialogResponcesInputs()
        {
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                DialogueSystemRefs.Instance.ResponseOptionsHandler?.NavigateDown();
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                DialogueSystemRefs.Instance.ResponseOptionsHandler?.NavigateUp();
            }
            else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                DialogueSystemRefs.Instance.ResponseOptionsHandler?.SelectResponse();
            }
        }

        private void OnDialogueEnd()
        {
            _typeWriter.ReturnToDefaultSpeed();
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
