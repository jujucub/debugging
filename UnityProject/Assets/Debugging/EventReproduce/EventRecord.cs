using System.Collections.Generic;
using UnityEngine;

namespace Debugging.EventReproduce
{
    [System.Serializable]
    public class EventRecordTrack
    {
        public float WaitSec;
        public string Path;
    }

    public class EventRecord : ScriptableObject
    {
        [SerializeField]
        List<EventRecordTrack> _tracks = new List<EventRecordTrack>();
        public List<EventRecordTrack> Tracks { get { return _tracks; } }
    }
}
