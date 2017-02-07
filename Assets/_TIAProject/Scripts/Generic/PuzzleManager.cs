using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour, IPuzzleManager
{
    #region Editor;

    public CameraController controller;
    public GameObject infobulle;
    public GameObject[] decorations;
    public GameObject[] prefabs;
    public string[] titles;
    public string[] descriptions;
    public int[] infobulleDistances;
    public bool[] considerRotations;
    public UnityEngine.UI.Text textClock;

    #endregion Editor;

    #region Attributes;

    private IList<GameObject> puzzleObjects = new List<GameObject>();
    private IList<GameObject> additionalObjects = new List<GameObject>();
    private bool initialized = false;
    private bool win = false;
    private IClock clock;

    #endregion Attributes;

    #region MonoBehaviour;

    void Start()
    {
        clock = new Clock();
    }

    void Update()
    {
        if (!win) textClock.text = clock.ToStringTime();
    }

    #endregion MonoBehaviour;

    #region Initialization;

    public void Initialize()
    {
        // puzzle objects instantiation
        int i = 0;
        foreach (GameObject current in prefabs)
        {
            // blueprint
            GameObject blueprint = Instantiate(current, transform);
            blueprint.name = current.name + "(Blueprint)";
            blueprint.AddComponent<Blueprint>();
            blueprint.GetComponent<IBlueprint>().SetConsiderRotation(considerRotations[i]);
            Transparency(blueprint);
            //

            // graspable
            GameObject graspable = Instantiate(current, transform);
            graspable.name = current.name + "(Graspable)";
            blueprint.GetComponent<IBlueprint>().SetGraspable(graspable);
            graspable.AddComponent<GraspableObject>();
            graspable.GetComponent<IGraspableObject>().SetOriginalParent(transform);
            graspable.GetComponent<IPuzzleObject>().SetManager(this);
            graspable.GetComponent<IPuzzleObject>().SetController(controller);
            InitializeGraspableParts(graspable.transform, graspable.GetComponent<IGraspableObject>());
            InitializeHighlightedParts(graspable.transform, graspable.GetComponent<IHighlightedObject>());
            float angle = i * 360 / prefabs.Length;
            graspable.transform.position = new Vector3(transform.position.x + Mathf.Cos(angle * Mathf.PI / 180) * 0.3f, transform.position.y + 0.1f, transform.position.z + Mathf.Sin(angle * Mathf.PI / 180) * 0.3f);
            //

            // infobulle
            GameObject info = Instantiate(infobulle, transform);
            info.name = current.name + "(Infobulle)";
            info.transform.position = blueprint.transform.position;
            info.transform.FindChild("Canvas").localPosition = new Vector3(0, 0, -infobulleDistances[i]);
            foreach (UnityEngine.UI.Text text in info.GetComponentsInChildren<UnityEngine.UI.Text>())
            {
                string temp = "";
                if (text.gameObject.name.Contains("Title")) temp = titles[i];
                if (text.gameObject.name.Contains("Description")) temp = descriptions[i];
                text.text = temp;
            }
            graspable.GetComponent<IPuzzleObject>().SetInfobulle(info);
            //

            ChangeObjectEnabled(graspable, false);
            ChangeObjectEnabled(blueprint, false);
            puzzleObjects.Add(graspable);
            i++;
        }
        //

        // additional objects instantiation
        foreach (GameObject current in decorations)
        {
            GameObject instance = Instantiate(current, transform);
            ChangeObjectEnabled(instance, false);
            additionalObjects.Add(instance);
        }
        //

        initialized = true;
        clock.StartTime();
    }

    private void InitializeHighlightedParts(Transform transform, IHighlightedObject parent)
    {
        for (int j = 0; j < transform.childCount; j++)
        {
            GameObject part = transform.GetChild(j).gameObject;
            part.AddComponent<HighlightedPart>();
            part.GetComponent<HighlightedPart>().SetParent(parent);
            InitializeHighlightedParts(transform.GetChild(j), parent);
        }
    }

    private void InitializeGraspableParts(Transform transform, IGraspableObject parent)
    {
        for (int j = 0; j < transform.childCount; j++)
        {
            GameObject part = transform.GetChild(j).gameObject;
            part.AddComponent<GraspablePart>();
            part.GetComponent<GraspablePart>().SetParent(parent);
            InitializeGraspableParts(transform.GetChild(j), parent);
        }
    }

    #endregion Initialization;

    #region Divers;

    private void ChangeObjectEnabled(GameObject current, bool enable)
    {
        foreach (Component comp in current.GetComponentsInChildren(typeof(Component)))
        {
            SetComponentEnabled(comp, enable);
        }
    }

    public void SetComponentEnabled(Component component, bool value)
    {
        if (component == null) return;
        if (component is Animation) (component as Animation).enabled = value;
        else if (component is Animator) (component as Animator).enabled = value;
        else if (component is AudioSource) (component as AudioSource).enabled = value;
        else if (component is MonoBehaviour &&
            !(component is Blueprint) &&
            !(component is GraspableObject))
            (component as MonoBehaviour).enabled = value;
        else if (value && (component is Blueprint || component is GraspableObject))
            (component as MonoBehaviour).enabled = value;
    }

    public void VerifyPuzzle()
    {
        if (!initialized || win) return;
        foreach (GameObject current in puzzleObjects)
        {
            if (!current.GetComponent<IGraspableObject>().IsCompleted()) return;
        }
        win = true;
        foreach (GameObject current in puzzleObjects)
            ChangeObjectEnabled(current, true);
        foreach (GameObject current in additionalObjects)
            ChangeObjectEnabled(current, true);
        Debug.Log("Win!");
    }

    private void Transparency(GameObject current)
    {
        foreach (Renderer rend in current.GetComponentsInChildren(typeof(Renderer)))
        {
            Material mat = rend.material;
            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
            mat.SetColor("_Color", new Color(0, 0, 0, 0));
        }
    }

    #endregion Divers;
}
