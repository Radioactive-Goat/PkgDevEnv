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
        [SerializeField] private List<string> _dialogues;
        [SerializeField] private BehaviorE _eBehavior;
        [SerializeField] private float _speedUpMultiplier, _speedUpValue;

        [SerializeField] private UiHandler _uiHandler;

        private int _dialogueIndex;
        private bool _dialogCompleted;

        private void Start()
        {
            TypeWriter.Instance.UpdateTimeGapBetweenLetters(_defaultTypingSpeed);
            TypeWriter.Instance.OnTypingComplete += OnDialogueEnd;
            _dialogCompleted = true;
            _dialogueIndex = 0;
        }

        private void OnDestroy()
        {
            TypeWriter.Instance.OnTypingComplete -= OnDialogueEnd;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                _uiHandler.OpenUI();
            if (!_uiHandler.IsDialoguePanelOpen)
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(_dialogCompleted)
                {
                    if (_dialogueIndex != _dialogues.Count)
                    {
                        TypeWriter.Instance.StartTypeWriter(_dialogues[_dialogueIndex]);
                        _dialogCompleted = false;
                    }
                    if(_dialogueIndex < _dialogues.Count)
                        _dialogueIndex++;
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
                if (!_dialogCompleted)
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
            _dialogCompleted = true;
            TypeWriter.Instance.ReturnToDefaultSpeed();
        }
    }
}
