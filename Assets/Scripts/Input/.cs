//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Input System/Input.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace SnakeApple.Space
{
    public partial class @Input: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Input()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""4b898d7d-147a-4c35-ac78-eda43810e511"",
            ""actions"": [
                {
                    ""name"": ""TouchAction"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5a092bd3-c40a-4e22-9459-65855ee6768f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TouchDelta"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bab42e61-c4ed-42a7-8b43-375c4a3ef3f9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""abe43e23-0afd-4ee6-bdf5-59c455437dd8"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""181f9951-e721-49f9-a9ea-ce082758d55e"",
                    ""path"": ""<Touchscreen>/primaryTouch/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_TouchAction = m_Player.FindAction("TouchAction", throwIfNotFound: true);
            m_Player_TouchDelta = m_Player.FindAction("TouchDelta", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Player
        private readonly InputActionMap m_Player;
        private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
        private readonly InputAction m_Player_TouchAction;
        private readonly InputAction m_Player_TouchDelta;
        public struct PlayerActions
        {
            private @Input m_Wrapper;
            public PlayerActions(@Input wrapper) { m_Wrapper = wrapper; }
            public InputAction @TouchAction => m_Wrapper.m_Player_TouchAction;
            public InputAction @TouchDelta => m_Wrapper.m_Player_TouchDelta;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void AddCallbacks(IPlayerActions instance)
            {
                if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
                @TouchAction.started += instance.OnTouchAction;
                @TouchAction.performed += instance.OnTouchAction;
                @TouchAction.canceled += instance.OnTouchAction;
                @TouchDelta.started += instance.OnTouchDelta;
                @TouchDelta.performed += instance.OnTouchDelta;
                @TouchDelta.canceled += instance.OnTouchDelta;
            }

            private void UnregisterCallbacks(IPlayerActions instance)
            {
                @TouchAction.started -= instance.OnTouchAction;
                @TouchAction.performed -= instance.OnTouchAction;
                @TouchAction.canceled -= instance.OnTouchAction;
                @TouchDelta.started -= instance.OnTouchDelta;
                @TouchDelta.performed -= instance.OnTouchDelta;
                @TouchDelta.canceled -= instance.OnTouchDelta;
            }

            public void RemoveCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IPlayerActions instance)
            {
                foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        public interface IPlayerActions
        {
            void OnTouchAction(InputAction.CallbackContext context);
            void OnTouchDelta(InputAction.CallbackContext context);
        }
    }
}
