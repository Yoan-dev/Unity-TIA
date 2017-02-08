using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class CustomTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES

        private TrackableBehaviour mTrackableBehaviour;

        private bool firstFound = false;

        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS

        private void OnTrackingFound()
        {
            if (!firstFound)
            {
                GetComponent<PuzzleManager>().Initialize();
                firstFound = true;
            }

            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            AudioSource[] audioSourceComponents = GetComponentsInChildren<AudioSource>(true);
            ParticleSystem[] particleSystemComponents = GetComponentsInChildren<ParticleSystem>(true);
            Light[] lightComponents = GetComponentsInChildren<Light>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            // Enable audioSources:
            foreach (AudioSource component in audioSourceComponents)
            {
                component.enabled = true;
            }

            // Enable particleSystems:
            foreach (ParticleSystem component in particleSystemComponents)
            {
                component.Play();
            }

            // Enable lights:
            foreach (Light component in lightComponents)
            {
                component.enabled = true;
            }

            // Enable canvas:
            foreach (Canvas component in canvasComponents)
            {
                component.enabled = true;
            }
        }

        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            AudioSource[] audioSourceComponents = GetComponentsInChildren<AudioSource>(true);
            ParticleSystem[] particleSystemComponents = GetComponentsInChildren<ParticleSystem>(true);
            Light[] lightComponents = GetComponentsInChildren<Light>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            // Disable audioSources:
            foreach (AudioSource component in audioSourceComponents)
            {
                component.enabled = false;
            }

            // Disable particleSystems:
            foreach (ParticleSystem component in particleSystemComponents)
            {
                component.Stop();
            }

            // Disable lights:
            foreach (Light component in lightComponents)
            {
                component.enabled = false;
            }

            // Enable canvas:
            foreach (Canvas component in canvasComponents)
            {
                component.enabled = false;
            }
        }

        #endregion // PRIVATE_METHODS
    
    }
}
