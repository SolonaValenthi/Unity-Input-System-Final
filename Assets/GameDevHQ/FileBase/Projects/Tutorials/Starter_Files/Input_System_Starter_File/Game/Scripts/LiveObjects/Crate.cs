using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.LiveObjects
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] private float _punchDelay;
        [SerializeField] private GameObject _wholeCrate, _brokenCrate;
        [SerializeField] private Rigidbody[] _pieces;
        [SerializeField] private BoxCollider _crateCollider;
        [SerializeField] private InteractableZone _interactableZone;
        private bool _isReadyToBreak = false;
        private float _holdStarted;
        private float _holdDelay = 0.5f; //key must be held for half second to break multiple parts.

        private List<Rigidbody> _breakOff = new List<Rigidbody>();

        private void OnEnable()
        {
            InteractableZone.onHoldStarted += InteractableZone_onHoldStarted;
            InteractableZone.onHoldEnded += InteractableZone_onHoldEnded;
        }

        private void InteractableZone_onHoldStarted(int zoneID)
        {
            if (zoneID == 6)
            {
                if (_isReadyToBreak == false && _breakOff.Count > 0)
                {
                    _wholeCrate.SetActive(false);
                    _brokenCrate.SetActive(true);
                    _isReadyToBreak = true;
                }

                if (_isReadyToBreak)
                {
                    _holdStarted = Time.time;
                }
            }
        }

        private void InteractableZone_onHoldEnded(int zoneID)
        {
            if (_isReadyToBreak)
            {
                if (Time.time > _holdStarted + _holdDelay)
                {
                    int breakCount = Random.Range(3, 6);

                    for (int i = 0; i < breakCount; i++)
                    {
                        BreakPart();
                    }
                }
                else
                    BreakPart();
            }
        }

        private void Start()
        {
            _breakOff.AddRange(_pieces);   
        }

        public void BreakPart()
        {
            if (_breakOff.Count > 0)
            {
                int rng = Random.Range(0, _breakOff.Count);
                _breakOff[rng].constraints = RigidbodyConstraints.None;
                _breakOff[rng].AddForce(new Vector3(1f, 1f, 1f), ForceMode.Force);
                _breakOff.Remove(_breakOff[rng]);
            }
            else if (_isReadyToBreak)
            {
                _isReadyToBreak = false;
                _crateCollider.enabled = false;
                _interactableZone.CompleteTask(6);
                Debug.Log("Completely Busted");
            }
        }

        IEnumerator PunchDelay()
        {
            float delayTimer = 0;
            while (delayTimer < _punchDelay)
            {
                yield return new WaitForEndOfFrame();
                delayTimer += Time.deltaTime;
            }

            _interactableZone.ResetAction(6);
        }

        private void OnDisable()
        {
            InteractableZone.onHoldStarted -= InteractableZone_onHoldStarted;
            InteractableZone.onHoldEnded -= InteractableZone_onHoldEnded;
        }
    }
}
