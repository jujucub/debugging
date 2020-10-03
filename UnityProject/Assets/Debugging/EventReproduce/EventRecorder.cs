using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;

namespace Debugging.EventReproduce
{
    public class EventRecorder : MonoBehaviour
    {
        public bool IsRecording { get; private set; }

        float _time = 0f;
        EventRecord _currentRecord;
        GameObject _beforePointerPressGameObject;
        MethodInfo _getLastPointerEventMethod;

        private void Start()
        {
            _getLastPointerEventMethod = typeof(PointerInputModule).GetMethod("GetLastPointerEventData", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
        }

        public void Recod()
        {
            IsRecording = true;
            _currentRecord = ScriptableObject.CreateInstance<EventRecord>();
        }

        public void Stop()
        {
            IsRecording = false;

#if UNITY_EDITOR
            if(_currentRecord != null)
            {
                var filename = EditorUtility.SaveFilePanel("Record", Application.dataPath, "EventRecord", "asset");
                if(string.IsNullOrEmpty(filename))
                {
                    return;
                }
                AssetDatabase.CreateAsset(_currentRecord, FileUtil.GetProjectRelativePath(filename));
            }
#endif
        }

        void LateUpdate()
        {
            if(!IsRecording)
            {
                return;
            }

            var pointerInputModule = EventSystem.current.currentInputModule as PointerInputModule;
            if(pointerInputModule == null)
            {
                return;
            }

            var pointerEventData = _getLastPointerEventMethod.Invoke(pointerInputModule, new object[] { PointerInputModule.kMouseLeftId }) as PointerEventData;
            //var pointerEventData = new PointerEventData(EventSystem.current);
            if (pointerEventData != null)
            {
                if (pointerEventData.pointerPress != null && _beforePointerPressGameObject != pointerEventData.pointerPress)
                {
                    var record = new EventRecordTrack() { WaitSec = _time, Path = GetHierarchyPath(pointerEventData.pointerPress.transform) };
                    _currentRecord.Tracks.Add(record);
                    Debug.Log($"{record.WaitSec} {record.Path}");
                    _time = 0f;
                }
                _beforePointerPressGameObject = pointerEventData.pointerPress;
            }

            _time += Time.deltaTime;
        }

        public string GetHierarchyPath(Transform self)
        {
            var path = self.name;
            Transform parent = self.parent;
            while (parent != null)
            {
                path = $"{parent.name}/{path}";
                parent = parent.parent;
            }
            return path;
        }
    }
}