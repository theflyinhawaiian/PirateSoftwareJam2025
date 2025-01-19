
using TMPro;
using UnityEngine;

namespace Assets.Scripts {
    enum ViewMode {
        HeadOn,
        Isometric
    }

    public class LevelEditorManager : MonoBehaviour {
        public GameObject panel;
        public TMP_Text viewModeText;
        public TMP_Dropdown obstacleSelector;
        public TMP_InputField xInput;
        public TMP_InputField yInput;
        public TMP_InputField zInput;
        public TMP_Dropdown roomsDropdown;
        public TMP_InputField saveRoomInput;

        void Start(){
            // 1. Check Screen.width and set panel width accordingly
            var transform = panel.GetComponent<RectTransform>();

            // 2. load & populate obstacle names from the `Assets/Prefabs/Obstacles`
            //   dir

            // 3. load & populate room names from the `Assets/Resources/Rooms` dir
        }

        public void DeleteObject(){

        }
        
        public void CreateObject(){

        }

        public void ToggleViewMode() {

        }

        public void CoordsChanged(){

        }

        public void ListItemSelected(){

        }

        public void LoadRoom(){

        }

        public void SaveRoom(){

        }
    }
}