﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime {
    sealed class IntroductionState : MonoBehaviour, IUIState {
        enum State {
            Unknown,
            Start
        }
        [SerializeField]
        Transform buttonContainer;

        [Header("Prefabs")]
        [SerializeField]
        GameObject newGamePrefab;

        State state;

        Button continueButton;

        void Start() {
            continueButton = buttonContainer.InstantiateButton("Continue", () => state = State.Start);
        }

        public IEnumerator WaitForCompletion() {
            yield return new WaitUntil(() => continueButton);

            continueButton.Select();

            yield return new WaitWhile(() => state == State.Unknown);

            switch (state) {
                case State.Start:
                    yield return newGamePrefab.InstantiateAndWaitForCompletion();
                    break;
            }
        }
    }
}
