using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    [HelpURL("https://bennykok.gitbook.io/rhythm-game-starter/hierarchy-overview/input")]
    public class KeyboardInputHandler : MonoBehaviour
    {
        [Title("Track Action Key", 3)]
        [ReorderableDisplay("Track")]
        public StringList keyMapping;

        [Title("Swipe Action Key", 3)]
        public KeyCode up;
        public KeyCode down;
        public KeyCode left;
        public KeyCode right;

        private NoteArea[] noteAreas;

        [System.Serializable]
        public class StringList : ReorderableList<string> { }

        private void Start()
        {
            noteAreas = GetComponentsInChildren<NoteArea>();
            for (int i = 0; i < noteAreas.Length; i++)
            {
                var noteArea = noteAreas[i];
                noteArea.keyboardInputHandler = this;

                var key = keyMapping.values[i];
                noteArea.key = key;
            }
        }
    }
}