using RG.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Testing
{
    public class CharacterIconEffects : MonoBehaviour
    {
        private CharactorIconHandler _iconHandler;
        private Vector3 _initialScale;
        private Vector3 _listnerScale;

        private void Start()
        {
            _iconHandler = GetComponent<CharactorIconHandler>();
            _initialScale = transform.localScale;
            _listnerScale = transform.localScale / 2;

            _iconHandler.OnActiveListner.AddListener(OnBecomeListner);
            _iconHandler.OnActiveSpeaker.AddListener(OnBecomeSpeaker);
        }

        private void OnDestroy()
        {
            _iconHandler.OnActiveListner.RemoveListener(OnBecomeListner);
            _iconHandler.OnActiveSpeaker.RemoveListener(OnBecomeSpeaker);
        }

        private void OnBecomeListner()
        {
            transform.localScale = _listnerScale;
        }

        private void OnBecomeSpeaker()
        {
            transform.localScale = _initialScale;
        }
    }
}
