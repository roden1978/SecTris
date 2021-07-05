// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputSwipe/Pause/PauseActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PauseActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PauseActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PauseActions"",
    ""maps"": [
        {
            ""name"": ""PauseGame"",
            ""id"": ""e73e3feb-45c4-428f-a1b8-ab3aa9ff5a65"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f7b1ae61-de1c-4283-83f4-7e739e289bdd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b7691a2a-ee79-4ab4-a826-80719c1f7ab5"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PauseGame
        m_PauseGame = asset.FindActionMap("PauseGame", throwIfNotFound: true);
        m_PauseGame_Pause = m_PauseGame.FindAction("Pause", throwIfNotFound: true);
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

    // PauseGame
    private readonly InputActionMap m_PauseGame;
    private IPauseGameActions m_PauseGameActionsCallbackInterface;
    private readonly InputAction m_PauseGame_Pause;
    public struct PauseGameActions
    {
        private @PauseActions m_Wrapper;
        public PauseGameActions(@PauseActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_PauseGame_Pause;
        public InputActionMap Get() { return m_Wrapper.m_PauseGame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PauseGameActions set) { return set.Get(); }
        public void SetCallbacks(IPauseGameActions instance)
        {
            if (m_Wrapper.m_PauseGameActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_PauseGameActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PauseGameActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PauseGameActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_PauseGameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public PauseGameActions @PauseGame => new PauseGameActions(this);
    public interface IPauseGameActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
}
