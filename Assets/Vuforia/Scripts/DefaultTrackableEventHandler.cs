/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using System;
using UnityEngine.Events;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{

    public TargetType targetType;
    private GameObject trackedGround;
    public string birdType;
    public MyStrEvent spawnBirdCall;
    public AudioClip groundCreatedClip;

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        if(spawnBirdCall == null)
        {
            spawnBirdCall = new MyStrEvent();
        }
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound(mTrackableBehaviour.TrackableName);
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost(mTrackableBehaviour.TrackableName);
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost(mTrackableBehaviour.TrackableName);
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    /*
    protected virtual void OnTrackingFound(string trackableName)
    {
        if (targetType == TargetType.ground)
        {
            trackedGround = this.transform.GetChild(0).gameObject;
            this.transform.DetachChildren();
            trackedGround.SetActive(true);

        } else
        {

        }
    }

    protected virtual void OnTrackingLost(string trackableName)
    {
        /*
        if (trackableName.Contains("field"))
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            
        }
        
    }
    */
    private void groundFound()
    {
        if (this.transform.GetChild(0).gameObject.activeSelf == false)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.GetComponent<AudioSource>().PlayOneShot(groundCreatedClip);
        }
        
    }

    private void birdFound()
    {
        if (spawnBirdCall != null)
        {
            spawnBirdCall.Invoke(birdType);
            Debug.Log("Invoked Bird Call");
        }
            
    }

    private void groundLost()
    {

    }

    private void birdLost()
    {

    }

    protected virtual void OnTrackingFound(string trackableName = "")
    {
        if (this.targetType == TargetType.ground)
        {
            groundFound();
            return;
        }
        if (this.targetType == TargetType.bird)
        {
            birdFound();
            return;
        }
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);
        var audioComponents = GetComponentsInChildren<AudioSource>(true);


        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;

        foreach (var audioSource in audioComponents)
        {
            audioSource.Play();
        }
    }


    protected virtual void OnTrackingLost(string trackableName = "")
    {

        if (this.targetType == TargetType.ground)
        {
            groundLost();
            return;
        }
        if (this.targetType == TargetType.bird)
        {
            birdLost();
            return;
        }
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        var audioComponents = GetComponentsInChildren<AudioSource>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
        {
            //disabling this will keep the item visible
            //component.enabled = false;
        }


        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;

        foreach (var audioSource in audioComponents)
        {
            audioSource.Stop();
        }
    }
    

    #endregion // PROTECTED_METHODS
}

public enum TargetType
{
    ground,
    bird,
    other
}

[System.Serializable]
public class MyStrEvent : UnityEvent<string>
{
}

