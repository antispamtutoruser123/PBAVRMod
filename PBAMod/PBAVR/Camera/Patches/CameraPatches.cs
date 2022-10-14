using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;
using UnityEngine.SceneManagement;

namespace PBAVR
{

    [HarmonyPatch]
    public class CameraPatches
    {

        public static GameObject DummyCamera, VRCamera, worldcam;
        public static GameObject newUI;
        public static RenderTexture rt;

        private static readonly string[] canvasesToIgnore =
{
        "com.sinai.unityexplorer_Root", // UnityExplorer.
        "com.sinai.unityexplorer.MouseInspector_Root", // UnityExplorer.
        "com.sinai.universelib.resizeCursor_Root",
        "IntroCanvas"
    };

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIPauseScreen), "ShowPausePanel")]
        private static void pause(UIPauseScreen __instance)
        {
            var canvas =__instance.gameObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            __instance.transform.localPosition = new Vector3(0, 3.5f, 3.789f);
            __instance.transform.localScale = new Vector3(.005f,.005f,1f);
            __instance.transform.localEulerAngles = new Vector3(.935f, 358.3714f, 359.2864f);
            worldcam.GetComponent<Camera>().enabled = true;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UISettings), "Show")]
        private static void options(UISettings __instance)
        {
            var canvas = __instance.gameObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            __instance.transform.localPosition = new Vector3(0, 3.1f, 3f);
            __instance.transform.localScale = new Vector3(.003f, .002f, 1f);
            __instance.transform.localEulerAngles = new Vector3(.935f, 358.3714f, 359.2864f);
            worldcam.GetComponent<Camera>().enabled = true;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UISettings), "Hide")]
        private static void unoptions(UISettings __instance)
        {
            var canvas = __instance.gameObject.GetComponent<Canvas>();
           // worldcam.GetComponent<Camera>().enabled = false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIPauseScreen), "OnContinueButtonClick")]
        private static void unpause(UIPauseScreen __instance)
        {
            worldcam.GetComponent<Camera>().enabled = false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIPauseScreen), "OnQuitButtonClick")]
        private static void quit(UIPauseScreen __instance)
        {
            Logs.WriteInfo($"LLLLLL: Creating Worldcam:  {__instance.name}");
            GameObject.Destroy(worldcam);
            GameObject.Destroy(rt);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerController), "Awake")]
        private static void DeleteCamera(UIMainMenu __instance)
        {
            worldcam.GetComponent<Camera>().enabled = false;
        }

            [HarmonyPostfix]
        [HarmonyPatch(typeof(UIMainMenu), "Awake")]
        private static void MakeCamera(UIMainMenu __instance)
        {
            worldcam.GetComponent<Camera>().enabled = true;

            if (!DummyCamera)
            {
                Logs.WriteInfo($"CREATING DUMMY CAMERA:  {__instance.name} {__instance.tag}");
                VRCamera = new GameObject("VRCamera");
                VRCamera.AddComponent<Camera>();
                VRCamera.tag = "MainCamera";
                VRCamera.GetComponent<Camera>().backgroundColor = Color.black;
                DummyCamera = new GameObject("DummyCamera");
                VRCamera.transform.parent = DummyCamera.transform;
            }

            if (!rt)
            {
                Logs.WriteInfo($"LLLLL:  Creating Render Texture  {__instance.name}");
                rt = new RenderTexture(1280, 720, 24);

                worldcam.GetComponent<Camera>().targetTexture = rt;

                newUI = new GameObject("newUI");
                newUI.AddComponent<Canvas>();
                newUI.AddComponent<RawImage>();
                newUI.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
                newUI.GetComponent<RawImage>().texture = rt;
                newUI.transform.localPosition = new Vector3(0, 0, 62f);
                newUI.transform.localScale = new Vector3(.8f, .45f, 1f);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CanvasScaler), "OnEnable")]
        private static void MoveIntroCanvas(CanvasScaler __instance)
        {
            if (IsCanvasToIgnore(__instance.name)) return;

  
                Logs.WriteInfo($"Hiding Canvas:  {__instance.name}");
            var canvas = __instance.gameObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;

            if (!worldcam)
            {
                Logs.WriteInfo($"LLLLLL: Creating Worldcam:  {__instance.name}");
                worldcam = new GameObject("worldcam");
                worldcam.AddComponent<Camera>();

            }

            canvas.worldCamera = worldcam.GetComponent<Camera>();

            if (SceneManager.GetActiveScene().name == "LoadingScene")
            {
                canvas.renderMode = RenderMode.WorldSpace;
                __instance.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                __instance.scaleFactor = .2f;

                foreach (Camera cam in Camera.allCameras)
                {
                    cam.clearFlags = CameraClearFlags.Color;
                    cam.backgroundColor = Color.black;
                }
               
            }
        }


        private static bool IsCanvasToIgnore(string canvasName)
        {
            foreach (var s in canvasesToIgnore)
                if (Equals(s, canvasName))
                    return true;
            return false;
        }

    }
}

