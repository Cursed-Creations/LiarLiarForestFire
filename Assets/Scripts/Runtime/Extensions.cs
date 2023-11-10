﻿using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityObject = UnityEngine.Object;

namespace Runtime {
    static class Extensions {
        internal static Button InstantiateButton(this Transform parent, string label, UnityAction onClick) {
            var button = UnityObject.Instantiate(GameManager.instance.buttonPrefab, parent);

            var localizedLabel = new LocalizedString("Buttons", label);
            localizedLabel.StringChanged += button.BindTo;
            button.BindTo(localizedLabel.GetLocalizedString());

            button.onClick.AddListener(onClick);

            return button;
        }

        internal static void BindTo<T>(this GameObject gameObject, T model) {
            if (gameObject) {
                foreach (var receiver in gameObject.GetComponentsInChildren<IBindingReceiver<T>>()) {
                    receiver.Bind(model);
                }
            }
        }

        internal static void BindTo<T>(this Component component, T model) {
            if (component) {
                component.gameObject.BindTo(model);
            }
        }

        internal static IEnumerator InstantiateAndWaitForCompletion(this GameObject prefab) {
            var instance = UnityObject.Instantiate(prefab);

            foreach (var state in instance.GetComponents<IScreen>()) {
                yield return state.WaitForCompletion();
            }

            UnityObject.Destroy(instance);
        }

        internal static Locale GetOther(this ILocalesProvider provider, Locale locale) {
            return provider
                .Locales
                .DefaultIfEmpty(locale)
                .FirstOrDefault(l => l != locale);
        }
    }
}
