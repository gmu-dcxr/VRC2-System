//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Scripts/Animations/RobotDog/RobotDogInputActions.inputactions
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

public partial class @RobotDogInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @RobotDogInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""RobotDogInputActions"",
    ""maps"": [
        {
            ""name"": ""Body"",
            ""id"": ""652dcad8-398c-4d19-9ea1-a6bf06e80768"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a6bfe5e4-68fd-4ac4-a0cc-6201c5a41675"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Turn"",
                    ""type"": ""Value"",
                    ""id"": ""6a43279b-43da-438b-9e86-e20c62f0559c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Stop"",
                    ""type"": ""Button"",
                    ""id"": ""105957da-c66a-46b6-92e5-d8edcbb42a2c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""eaa01479-772d-4ed0-8225-dcb9b5f3c6e5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3760465b-4af5-482d-8f6b-3ea55a89a05d"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""da7ae4a3-8f1b-4a00-80cc-0ef21b1a6a3e"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2e2febbf-64f4-440d-b40b-e96261203f1c"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""62c55d66-67fe-4b92-8cf8-b46bba4e29d7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""42159464-3b62-42fe-900b-827cc8d8a8d0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8ce65dc6-c327-4e35-9f28-99c5c933dae0"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f913f4df-36b4-419a-b565-1ce5d8e7e0c5"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f5c02fc7-d26b-46c9-a4c1-5ac7676ce9e9"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Stop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Arm"",
            ""id"": ""65574e43-257c-498a-bedb-dd74db494570"",
            ""actions"": [
                {
                    ""name"": ""DOF0"",
                    ""type"": ""Value"",
                    ""id"": ""6c40be48-abad-4cef-89ed-cb4f755db9de"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""DOF1"",
                    ""type"": ""Value"",
                    ""id"": ""adeb9457-b646-44c1-a2ec-74c2037af585"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""DOF2"",
                    ""type"": ""Value"",
                    ""id"": ""87f8fbd8-8cca-43a0-b35e-d34bde1c6085"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""DOF3"",
                    ""type"": ""Value"",
                    ""id"": ""dea4c1da-9c9b-4f32-be3e-6c58c460480b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Grip"",
                    ""type"": ""Value"",
                    ""id"": ""5b8cb527-f70e-4e6d-8c74-c73874618a2f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""dd8c9271-fd66-4aec-acbc-582253daf987"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF0"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""12f6f006-07c1-4f84-9e1e-1a53b293ccd9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF0"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""da73b6aa-232a-4d20-a1d8-92b721151e24"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF0"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""9559753b-131d-459f-add0-0f4b2dc21469"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF1"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7653bdcd-a431-460f-b3a3-7be06c158d52"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""60f6ab7e-57f4-4b4e-9ab4-88222a563ac1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""467eb427-3add-49f8-be6c-834899782580"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c26945ed-46ae-42e3-9305-e181988c4953"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""72f00e2d-4a70-420d-a273-1d2e1c480e89"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""05df782e-7da2-476e-a216-bef6ca381f31"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF3"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8eb64527-e3dd-4bc2-9b70-b9e23566be48"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ae242d28-8713-40e1-990e-d04cd5954a9e"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DOF3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""e530b21b-d4ed-4211-9b4a-622be55ae7fc"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grip"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d801c967-465c-4ac5-b5bc-1997f7fd91be"",
                    ""path"": ""<Keyboard>/comma"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""10d68c72-dcd4-497e-9fe9-ebab8740543e"",
                    ""path"": ""<Keyboard>/period"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Body
        m_Body = asset.FindActionMap("Body", throwIfNotFound: true);
        m_Body_Move = m_Body.FindAction("Move", throwIfNotFound: true);
        m_Body_Turn = m_Body.FindAction("Turn", throwIfNotFound: true);
        m_Body_Stop = m_Body.FindAction("Stop", throwIfNotFound: true);
        // Arm
        m_Arm = asset.FindActionMap("Arm", throwIfNotFound: true);
        m_Arm_DOF0 = m_Arm.FindAction("DOF0", throwIfNotFound: true);
        m_Arm_DOF1 = m_Arm.FindAction("DOF1", throwIfNotFound: true);
        m_Arm_DOF2 = m_Arm.FindAction("DOF2", throwIfNotFound: true);
        m_Arm_DOF3 = m_Arm.FindAction("DOF3", throwIfNotFound: true);
        m_Arm_Grip = m_Arm.FindAction("Grip", throwIfNotFound: true);
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

    // Body
    private readonly InputActionMap m_Body;
    private List<IBodyActions> m_BodyActionsCallbackInterfaces = new List<IBodyActions>();
    private readonly InputAction m_Body_Move;
    private readonly InputAction m_Body_Turn;
    private readonly InputAction m_Body_Stop;
    public struct BodyActions
    {
        private @RobotDogInputActions m_Wrapper;
        public BodyActions(@RobotDogInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Body_Move;
        public InputAction @Turn => m_Wrapper.m_Body_Turn;
        public InputAction @Stop => m_Wrapper.m_Body_Stop;
        public InputActionMap Get() { return m_Wrapper.m_Body; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BodyActions set) { return set.Get(); }
        public void AddCallbacks(IBodyActions instance)
        {
            if (instance == null || m_Wrapper.m_BodyActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BodyActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Turn.started += instance.OnTurn;
            @Turn.performed += instance.OnTurn;
            @Turn.canceled += instance.OnTurn;
            @Stop.started += instance.OnStop;
            @Stop.performed += instance.OnStop;
            @Stop.canceled += instance.OnStop;
        }

        private void UnregisterCallbacks(IBodyActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Turn.started -= instance.OnTurn;
            @Turn.performed -= instance.OnTurn;
            @Turn.canceled -= instance.OnTurn;
            @Stop.started -= instance.OnStop;
            @Stop.performed -= instance.OnStop;
            @Stop.canceled -= instance.OnStop;
        }

        public void RemoveCallbacks(IBodyActions instance)
        {
            if (m_Wrapper.m_BodyActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IBodyActions instance)
        {
            foreach (var item in m_Wrapper.m_BodyActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BodyActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public BodyActions @Body => new BodyActions(this);

    // Arm
    private readonly InputActionMap m_Arm;
    private List<IArmActions> m_ArmActionsCallbackInterfaces = new List<IArmActions>();
    private readonly InputAction m_Arm_DOF0;
    private readonly InputAction m_Arm_DOF1;
    private readonly InputAction m_Arm_DOF2;
    private readonly InputAction m_Arm_DOF3;
    private readonly InputAction m_Arm_Grip;
    public struct ArmActions
    {
        private @RobotDogInputActions m_Wrapper;
        public ArmActions(@RobotDogInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @DOF0 => m_Wrapper.m_Arm_DOF0;
        public InputAction @DOF1 => m_Wrapper.m_Arm_DOF1;
        public InputAction @DOF2 => m_Wrapper.m_Arm_DOF2;
        public InputAction @DOF3 => m_Wrapper.m_Arm_DOF3;
        public InputAction @Grip => m_Wrapper.m_Arm_Grip;
        public InputActionMap Get() { return m_Wrapper.m_Arm; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ArmActions set) { return set.Get(); }
        public void AddCallbacks(IArmActions instance)
        {
            if (instance == null || m_Wrapper.m_ArmActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ArmActionsCallbackInterfaces.Add(instance);
            @DOF0.started += instance.OnDOF0;
            @DOF0.performed += instance.OnDOF0;
            @DOF0.canceled += instance.OnDOF0;
            @DOF1.started += instance.OnDOF1;
            @DOF1.performed += instance.OnDOF1;
            @DOF1.canceled += instance.OnDOF1;
            @DOF2.started += instance.OnDOF2;
            @DOF2.performed += instance.OnDOF2;
            @DOF2.canceled += instance.OnDOF2;
            @DOF3.started += instance.OnDOF3;
            @DOF3.performed += instance.OnDOF3;
            @DOF3.canceled += instance.OnDOF3;
            @Grip.started += instance.OnGrip;
            @Grip.performed += instance.OnGrip;
            @Grip.canceled += instance.OnGrip;
        }

        private void UnregisterCallbacks(IArmActions instance)
        {
            @DOF0.started -= instance.OnDOF0;
            @DOF0.performed -= instance.OnDOF0;
            @DOF0.canceled -= instance.OnDOF0;
            @DOF1.started -= instance.OnDOF1;
            @DOF1.performed -= instance.OnDOF1;
            @DOF1.canceled -= instance.OnDOF1;
            @DOF2.started -= instance.OnDOF2;
            @DOF2.performed -= instance.OnDOF2;
            @DOF2.canceled -= instance.OnDOF2;
            @DOF3.started -= instance.OnDOF3;
            @DOF3.performed -= instance.OnDOF3;
            @DOF3.canceled -= instance.OnDOF3;
            @Grip.started -= instance.OnGrip;
            @Grip.performed -= instance.OnGrip;
            @Grip.canceled -= instance.OnGrip;
        }

        public void RemoveCallbacks(IArmActions instance)
        {
            if (m_Wrapper.m_ArmActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IArmActions instance)
        {
            foreach (var item in m_Wrapper.m_ArmActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ArmActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ArmActions @Arm => new ArmActions(this);
    public interface IBodyActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnTurn(InputAction.CallbackContext context);
        void OnStop(InputAction.CallbackContext context);
    }
    public interface IArmActions
    {
        void OnDOF0(InputAction.CallbackContext context);
        void OnDOF1(InputAction.CallbackContext context);
        void OnDOF2(InputAction.CallbackContext context);
        void OnDOF3(InputAction.CallbackContext context);
        void OnGrip(InputAction.CallbackContext context);
    }
}