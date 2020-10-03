using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Collections;

namespace Debugging.EventReproduce
{
    public class EventPlayer : MonoBehaviour
    {
        [SerializeField]
        EventRecord _currentRecord;

        private void Start()
        {
            StartCoroutine(Sequence());
        }

        public void Play(EventRecord record)
        {
            _currentRecord = record;
            StartCoroutine(Sequence());
        }

        IEnumerator Sequence()
        {
            foreach(var track in _currentRecord.Tracks)
            {
                yield return new WaitForSeconds(track.WaitSec);

                GameObject target = GameObject.Find(track.Path);
                while (target == null)
                {
                    yield return new WaitForSeconds(0.5f);
                    target = GameObject.Find(track.Path);
                }

                var clickHandler = target.GetComponent<IPointerClickHandler>();
                if(clickHandler != null)
                {
                    ExecuteEvents.pointerClickHandler(clickHandler, new PointerEventData(EventSystem.current));
                    continue;
                }

                var downHanlder = target.GetComponent<IPointerDownHandler>();
                if (downHanlder != null)
                {
                    ExecuteEvents.pointerDownHandler(downHanlder, new PointerEventData(EventSystem.current));
                    continue;
                }
            }
        }
    }
}