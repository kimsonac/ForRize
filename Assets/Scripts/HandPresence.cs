using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;

    public bool showController = false;
    public bool isHapticOnChange;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    private void OnEnable()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnLifeChange += HapticOnChange;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnLifeChange -= HapticOnChange;
    }

    private void Start()
    {
        TryInitalize();
    }

    private void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitalize();
        }
        else
        {
            if (showController)
            {
                spawnedController.SetActive(true);
                spawnedHandModel.SetActive(false);
            }
            else
            {
                spawnedController.SetActive(false);
                spawnedHandModel.SetActive(true);
                UpdateAnimation();
            }
        }
    }

    private void TryInitalize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        //foreach (var item in devices)
        //{
        //    Debug.Log(item.name + item.characteristics);
        //}

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(x => x.name == targetDevice.name);

            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("Did not found corresponding model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    private void UpdateAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    private void HapticOnChange(int _)
    {
        HapticCapabilities capabilities;
        if (isHapticOnChange && targetDevice.TryGetHapticCapabilities(out capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                uint channel = 0;
                float amplitude = 0.5f;
                float duration = 1.0f;
                targetDevice.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }
}
