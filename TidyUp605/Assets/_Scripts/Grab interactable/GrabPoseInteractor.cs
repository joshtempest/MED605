using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEditor;

//this tech. doesn't animatie instead it stops us from having to animate everything
public class GrabPoseInteractor : MonoBehaviour
{
    NearFarInteractor nearFarInteractor; 
    [SerializeField] Transform transformToFollow;
    [Header("Hand Data")]
    [SerializeField] HandData handData;
    [SerializeField] GrabPoseScriptableObject defaultPose;
    [SerializeField] bool leftHand = false;
    GrabPose currentGrabPose;

#if UNITY_EDITOR
    [Header("Set Pose")]
    [SerializeField] GrabPoseScriptableObject newGrabPoseData;
#endif
    void Awake()
    {
        if (handData == null)
        {
            Debug.LogError("Hand data is not assigned or empty.");
            return;
        }
        BendPhalanges(defaultPose.handData);
        if(TryGetComponent<NearFarInteractor>(out var nfInteractor))
        {
            nearFarInteractor = nfInteractor;
        }
    }

    void OnEnable()
    {
        nearFarInteractor.selectEntered.AddListener(Grab);    
        nearFarInteractor.selectExited.AddListener(Release);    
    }

    void OnDisable()
    {
        nearFarInteractor.selectEntered.RemoveListener(Grab);  
        nearFarInteractor.selectExited.RemoveListener(Release);    
    }
    void Grab(SelectEnterEventArgs args)
    {
        if (args != null)
        {
            // Does the selected object have a GrabPose component? If so, apply the hand pose data. If not, reset to default pose.
            if (args.interactableObject.transform.TryGetComponent(out GrabPose grabPose))
            {
                currentGrabPose = grabPose;
                if (grabPose.data == null)
                {
                    Debug.LogWarning("GrabPose component found, but data is not assigned. Hand pose will reset.");
                    BendPhalanges(defaultPose.handData);
                    return;
                }

                BendPhalanges(currentGrabPose.data.handData);   
                // Apply position and rotation offsets from the grab pose data to the attach transform
                if (leftHand)
                {
                    var newRot = new Vector3(currentGrabPose.data.rotationOffset.x, -currentGrabPose.data.rotationOffset.y, -currentGrabPose.data.rotationOffset.z);
                    var newPos = new Vector3(-currentGrabPose.data.positionOffset.x, currentGrabPose.data.positionOffset.y, currentGrabPose.data.positionOffset.z);
                    transformToFollow.localRotation = Quaternion.Euler(newRot);
                    transformToFollow.localPosition = newPos;
                }
                else
                {
                    transformToFollow.localPosition = currentGrabPose.data.positionOffset;
                    transformToFollow.localRotation = Quaternion.Euler(currentGrabPose.data.rotationOffset);
                }
            }
        }
        else
        {
            Debug.LogWarning("Selected object does not have a GrabPose component. Hand pose will reset.");
            BendPhalanges(defaultPose.handData);
        }
    }

    void Release(SelectExitEventArgs args)
    {
        // Reset hand pose to initial values when releasing the object
        BendPhalanges(defaultPose.handData);
    }

    /// <summary>
    /// Bend the phalanges of the hand to match the specified pose.
    /// </summary>
    public void BendPhalanges(HandData newPose)
    {
        for (int i = 0; i < handData.thumb.phalanges.Length; i++)
            BendPhalanx(handData.thumb.phalanges[i], newPose.thumb.phalanxValues[i]);
        for (int i = 0; i < handData.index.phalanges.Length; i++)
        {
            BendPhalanx(handData.index.phalanges[i], newPose.index.phalanxValues[i]);
            BendPhalanx(handData.middle.phalanges[i], newPose.middle.phalanxValues[i]);
            BendPhalanx(handData.ring.phalanges[i], newPose.ring.phalanxValues[i]);
            BendPhalanx(handData.pinky.phalanges[i], newPose.pinky.phalanxValues[i]);
        }
    }

    /// <summary>
    /// Bends the individual phalanx to the specified rotation values. Negates y and z values for left hand to mirror the pose depending on the hand model.
    /// </summary>
    void BendPhalanx(Transform phalanx, Vector3 value)
    {
        if(leftHand)
            phalanx.localRotation = Quaternion.Euler(value.x, -value.y, -value.z);
        else
            phalanx.localRotation = Quaternion.Euler(value.x, value.y, value.z);
    }

    Vector3 GetPhalanxRotation(Transform phalanx)
    {
        return phalanx.localRotation.eulerAngles;
    }


#if UNITY_EDITOR
    /// <summary>
    /// Call this method to set the current rotations of the hand phalanges as the values for the current grab pose. 
    /// This will create a new GrabPoseScriptableObject and assign it to the current grab pose's data field. 
    /// Make sure to have the desired object selected in the scene with its GrabPose component before calling this method, and ensure the hand is in the desired pose in-game.
    /// </summary>
    public void SetGrabPose()
    {
        print("Setting grab pose values...");
        SetPhalanxValues();
        StartCoroutine(SetGrabPoseVariables());
    }
    
    void SetPhalanxValues()
    {
        for (int i = 0; i < handData.thumb.phalanges.Length; i++)
            handData.thumb.phalanxValues[i] = GetPhalanxRotation(handData.thumb.phalanges[i]);
        for (int i = 0; i < handData.index.phalanges.Length; i++)
        {
            handData.index.phalanxValues[i] = GetPhalanxRotation(handData.index.phalanges[i]);
            handData.middle.phalanxValues[i] = GetPhalanxRotation(handData.middle.phalanges[i]);
            handData.ring.phalanxValues[i] = GetPhalanxRotation(handData.ring.phalanges[i]);
            handData.pinky.phalanxValues[i] = GetPhalanxRotation(handData.pinky.phalanges[i]);
        }
    }

    IEnumerator SetGrabPoseVariables()
    {
        if (newGrabPoseData == null)
        {
            Debug.LogWarning("GrabPoseScriptableObject is not assigned.");
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(0.5f); // ensure the for loop in SetPhalanxValues has completed and values are updated

            newGrabPoseData.positionOffset = transformToFollow.localPosition;
            newGrabPoseData.rotationOffset = transformToFollow.localRotation.eulerAngles;
            newGrabPoseData.handData = handData;
            print($"Pose values set for {newGrabPoseData.name}");
        }
    }
#endif
}

#if UNITY_EDITOR
// Create a button for the inspector to call the SetGrabPose method.
[CustomEditor(typeof(GrabPoseInteractor))]
public class GrabPoseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GrabPoseInteractor interactor = (GrabPoseInteractor)target;
        if (GUILayout.Button("Set Grab Pose"))
        {
            interactor.SetGrabPose();
        }
    }
}
#endif