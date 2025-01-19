
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

        float screenWidth;
        RectTransform panelTransform;

        void Start(){
            panelTransform = panel.GetComponent<RectTransform>();

            // 1. load & populate obstacle names from the `Assets/Prefabs/Obstacles`
            //   dir

            // 2. load & populate room names from the `Assets/Resources/Rooms` dir
        }

        void Update(){
            if(screenWidth == Screen.width)
                return;

            screenWidth = Screen.width;
            var offsetMin = panelTransform.offsetMin;
            offsetMin.x = Screen.width * .8f;
            panelTransform.offsetMin = offsetMin;
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