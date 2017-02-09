using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour, IPuzzleManager
{
    #region Editor;

    [Header("Generic")]
    public CameraController controller; // the arcamera
    public GameObject infobulle; // infobulle pregab
    public GameObject highlightParticles; // highlight prefab
    public UnityEngine.UI.Text textClock; // clock text UI
    public GameObject[] decorations; // prefabs of objects belonging to the puzzle but not puzzle pieces themselves
    [Header("Puzzle objects")]
    public GameObject[] prefabs; // prefabs of objects belonging to the puzzle and that are puzzle pieces
    public string[] titles; // titles corresponding to the puzzle pieces (for infobulle)
    [TextArea(12, 18)]
    public string[] descriptions; // descriptions corresponding to the puzzle pieces (for infobulle)
    public int[] infobulleDistances; // metric corresponding to the puzzle pieces (for infobulle)
    public bool[] considerRotations; // metric corresponding to the puzzle pieces (for blueprint completion)

    #endregion Editor;

    #region Attributes;

    private IList<GameObject> puzzleObjects = new List<GameObject>(); // our puzzle/graspable objects
    private IList<GameObject> additionalObjects = new List<GameObject>(); // the decorations
    private bool initialized = false; // is the puzzle initialized
    private bool win = false; // is the puzzle finished
    private IClock clock; // the clock

    #endregion Attributes;

    #region MonoBehaviour;

    void Start()
    {
        clock = new Clock(); // clock initialization
    }

    void Update()
    {
        // while the puzzle is not finished, the clock will go on
        if (!win) textClock.text = clock.ToStringTime();
    }

    #endregion MonoBehaviour;

    #region Initialization;

    public void Initialize()
    {
        // puzzle objects instantiation
        int i = 0;
        foreach (GameObject current in prefabs) // foreach puzzle piece
        {
            // creation of a blueprint
            GameObject blueprint = Instantiate(current, transform);
            blueprint.name = current.name + "(Blueprint)";
            blueprint.AddComponent<Blueprint>();
            blueprint.GetComponent<IBlueprint>().SetConsiderRotation(considerRotations[i]);
            Transparency(blueprint);
            //

            // creation of a graspable object (the puzzle piece)
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
            graspable.transform.position = new Vector3(transform.position.x + Mathf.Cos(angle * Mathf.PI / 180) * 1.0f, transform.position.y + 0.5f, transform.position.z + Mathf.Sin(angle * Mathf.PI / 180) * 1.0f);
            //

            // initialization of highlightParticles (for the puzzle piece)
            GameObject highlight = Instantiate(highlightParticles, graspable.transform.position, Quaternion.identity, graspable.transform);
            highlight.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            graspable.GetComponent<IPuzzleObject>().SetHighlight(highlight.GetComponent<ParticleSystem>());
            //

            // initialization of infobulle (for the puzzle piece)
            GameObject info = Instantiate(infobulle, graspable.transform);
            info.name = current.name + "(Infobulle)";
            info.transform.position = graspable.transform.position;
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

            // disactivation of objects components
            // (scripts that does not belong to our framework, animations and so on)
            // they will be reactivated once the puzzle if finished
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

    // initialization of highlighted parts (childs of an highlighted object)
    // allowing to highlight an object whatever the part we aim at
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

    // initialization of graspable parts (childs of an graspable object)
    // allowing to grab an object whatever the part we aim at
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

    // enable/disable all fancy components of an object
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

    // verify if the puzzle is finished
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
    }

    // fade an object (for blueprints)
    private void Transparency(GameObject current)
    {
        foreach (Renderer rend in current.GetComponentsInChildren(typeof(Renderer)))
        {
            Material mat = rend.material;
            
            mat.shader = Shader.Find("Standard");

            // Fade
            mat.SetFloat("_Mode", 2);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;

            // Color
            mat.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 0.25f));
        }
    }

    #endregion Divers;
}
