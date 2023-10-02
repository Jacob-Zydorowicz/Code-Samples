/*
 * (Jacob Welch, Jacob Zydorowicz)
 * (PlayerBuildController)
 * (CitySim)
 * (Description: )
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerBuildController : MonoBehaviour
{
    #region Fields
    private int currentBuildCost=0;

    [Tooltip("The names of the buildings in the same order as they are in the building factory")]
    [SerializeField] private string[] buildingNames = new string[0];

    [Tooltip("The object that is the shop")]
    [SerializeField] private GameObject shop;

    [HideInInspector] public Building buildingToSell;
    [HideInInspector] public Building currentBuilding;
    [HideInInspector] public PlacingTile currentPlacingTile;
    [HideInInspector] public string currentBuildingName = "";

    public enum ValidLocationState { VALID, INVALID, HASBUILDING }

    private ValidLocationState hasValidLocation = ValidLocationState.INVALID;

    public AudioSource playerBuildClips;
    public AudioClip shopBuyClip;
    public AudioClip shopDenyClip;
    public AudioClip placeBuildingClip;
    public AudioClip undoClip;

    private List<Building> recentlyPlacedBuildings = new List<Building>();

    PlaceBuildingCommand placeBuildingCommand;
    DestroyBuildingCommand destroyBuildingCommand;

    private Stack<Command> previousCommands = new Stack<Command>();

    private static PlayerBuildController Instance;

    public static bool RecieveMoneyFromDestroying
    {
        get
        {
            var recieveMoneyAbilities = FindObjectsOfType<ReturnMoneyAbility>();

            foreach(var returnMoney in recieveMoneyAbilities)
            {
                if (returnMoney.enabled)
                {
                    return true;
                }
            }

            return Instance.placeBuildingCommand.HasBuilding(Instance.buildingToSell);

            //return recieveMoneyAbilities.Length != 0 || Instance.placeBuildingCommand.HasBuilding(Instance.buildingToSell);
        }
    }
    #endregion

    #region Functions
    private void Awake()
    {
        Instance = this;

        placeBuildingCommand = new PlaceBuildingCommand(this);
        destroyBuildingCommand = new DestroyBuildingCommand(this);
    }

    /// <summary>
    /// Handles initilization of components and other fields before anything else.
    /// </summary>
    private void Start()
    {
        ResetBuilding(buildingNames[0]);
    }

    public void ResetBuilding(string name = "")
    {
        if (name == "") return;
        var newBuilding = BuildingFactory.SpawnBuilding(name);

        if(newBuilding != null)
        {
            if (currentBuilding != null) Destroy(currentBuilding.gameObject);

            currentBuildingName = name;
            currentBuilding = newBuilding;

            PlaceBuildingText.SetText(newBuilding.GetData.BuildingName, newBuilding.GetData.Money, false);
        }
    }

    public static void ResetCommands()
    {
        Instance.previousCommands.Clear();

        Instance.placeBuildingCommand.Reset();
    }

    public void UndoLastCommand()
    {
        if(previousCommands.Count > 0)
        {
            previousCommands.Pop().Undo();
            playerBuildClips.PlayOneShot(undoClip);
        }
    }

    private void PlaceBuilding()
    {
        placeBuildingCommand.Execute();
        previousCommands.Push(placeBuildingCommand);
        playerBuildClips.PlayOneShot(placeBuildingClip);
    }

    private void DestroyBuilding()
    {
        destroyBuildingCommand.Execute();
        previousCommands.Push(destroyBuildingCommand);
    }

    #region Player Input
    /// <summary>
    /// Calls for an event to take place once per frame.
    /// </summary>
    private void Update()
    {
        if (currentBuilding == null) return;

        CheckForPlacingTile();
        PlayerInput();
    }

    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UndoLastCommand();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(hasValidLocation == ValidLocationState.VALID)
            {
                playerBuildClips.PlayOneShot(shopBuyClip);
                PlaceBuilding();
            }
            else if (hasValidLocation == ValidLocationState.HASBUILDING)
            {
                DestroyBuilding();
            }
            else
            {
                playerBuildClips.PlayOneShot(shopDenyClip);
            }
        }

        ChangeActiveBuilding();
    }

    /// <summary>
    /// Method <c>getPriceFromShop</c> Uses shop buttons to set the active building price
    /// <paramref name="price"/> cost to place the building. Need assign in scence shop buttons
    /// </summary>
    public void getPriceFromShop(int price)
    {
        currentBuildCost = price;
    }

    private void ChangeActiveBuilding()
    {
        ChangeActiveBuildingKeys();
    }

    private void ChangeActiveBuildingMouse(int shopIndex)
    {
        switch (shopIndex)
        {
            case 0:
                if (currentBuildingName != buildingNames[0])
                    ResetBuilding(buildingNames[0]);
                break;
            case 1:
                if (currentBuildingName != buildingNames[1])
                    ResetBuilding(buildingNames[1]);
                break;
            case 2:
                if (currentBuildingName != buildingNames[2])
                    ResetBuilding(buildingNames[2]);
                break;
            case 3:
                if (currentBuildingName != buildingNames[3])
                    ResetBuilding(buildingNames[3]);
                break;
            case 4:
                if (currentBuildingName != buildingNames[4])
                    ResetBuilding(buildingNames[4]);
                break;
            case 5:
                if (currentBuildingName != buildingNames[5])
                    ResetBuilding(buildingNames[5]);
                break;
            case 6:
                if (currentBuildingName != buildingNames[6])
                    ResetBuilding(buildingNames[6]);
                break;
            case 7:
                if (currentBuildingName != buildingNames[7])
                    ResetBuilding(buildingNames[7]);
                break;
            case 8:
                if (currentBuildingName != buildingNames[8])
                    ResetBuilding(buildingNames[8]);
                break;
            case 9:
                if (currentBuildingName != buildingNames[9])
                    ResetBuilding(buildingNames[9]);
                break;
            case 10:
                if (currentBuildingName != buildingNames[10])
                    ResetBuilding(buildingNames[10]);
                break;
            case 11:
                if (currentBuildingName != buildingNames[11])
                    ResetBuilding(buildingNames[11]);
                break;
            case 12:
                if (currentBuildingName != buildingNames[12])
                    ResetBuilding(buildingNames[12]);
                break;
            case 13:
                if (currentBuildingName != buildingNames[13])
                    ResetBuilding(buildingNames[13]);
                break;
            case 14:
                if (currentBuildingName != buildingNames[14])
                    ResetBuilding(buildingNames[14]);
                break;
        }

    }

    private void ChangeActiveBuildingKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentBuildingName != buildingNames[0])
                ResetBuilding(buildingNames[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentBuildingName != buildingNames[1])
                ResetBuilding(buildingNames[1]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentBuildingName != buildingNames[2])
                ResetBuilding(buildingNames[2]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (currentBuildingName != buildingNames[3])
                ResetBuilding(buildingNames[3]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (currentBuildingName != buildingNames[4])
                ResetBuilding(buildingNames[4]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (currentBuildingName != buildingNames[5])
                ResetBuilding(buildingNames[5]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (currentBuildingName != buildingNames[6])
                ResetBuilding(buildingNames[6]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (currentBuildingName != buildingNames[7])
                ResetBuilding(buildingNames[7]);
        }
    }
    #endregion

    #region Checking tile locations
    private void CheckForPlacingTile()
    {
        currentPlacingTile = GetTileAtLocation(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if(currentPlacingTile)
        {
            CheckExtraBuildingConditions();
        }
        else
        {
            hasValidLocation = ValidLocationState.INVALID;
        }

        SetBuildingText();

        currentBuilding.gameObject.SetActive(hasValidLocation != ValidLocationState.HASBUILDING);
    }

    private void SetBuildingText()
    {
        switch (hasValidLocation)
        {
            case ValidLocationState.VALID:
                PlaceBuildingText.SetText(currentBuilding.GetData.BuildingName, currentBuilding.GetData.Money, true);
                break;
            case ValidLocationState.INVALID:
                PlaceBuildingText.SetText(currentBuilding.GetData.BuildingName, currentBuilding.GetData.Money, false);
                break;
            case ValidLocationState.HASBUILDING:
                PlaceBuildingText.SetDestroyText(buildingToSell.GetData.BuildingName, buildingToSell.GetData.Money);
                break;
        }
    }

    private void CheckExtraBuildingConditions()
    {
        hasValidLocation = CheckValidLocations(currentPlacingTile);

        currentBuilding.transform.position = currentPlacingTile.transform.position;

        if (CheckUI())
        {
            hasValidLocation = ValidLocationState.INVALID;
        }

        if (hasValidLocation == ValidLocationState.VALID)
        {
            var notEnoughMoney = !EconManager.CanBuy(currentBuilding.GetData.Money);
            var reqMet = currentBuilding.gameObject.TryGetComponent(out BuildingPlacementRequirement buildingReq) && !buildingReq.CheckRequirement(currentPlacingTile);

            //print("Not enough Money: " + notEnoughMoney);
            //print("Req Met: " + reqMet);

            if (notEnoughMoney || reqMet)
            {
                hasValidLocation = ValidLocationState.INVALID;
            }
        }
        else if (hasValidLocation == ValidLocationState.HASBUILDING)
        {
            buildingToSell = currentPlacingTile.BuildingOnLocation;
        }

        Building.BuildingStateEnum buildingState = hasValidLocation == ValidLocationState.VALID ? Building.BuildingStateEnum.VALIDPREVIEW : Building.BuildingStateEnum.INVALIDPREVIEW;

        currentBuilding.SetBuildingState(buildingState);
    }

    private bool CheckUI()
    {
        var pointerData = new PointerEventData(FindObjectOfType<EventSystem>());
        //Set the Pointer Event Position to that of the game object
        pointerData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerData, results);

        foreach(var ray in results)
        {
            //print("Result: " + ray.gameObject.name);

            return true;
        }

        return false;
    }

    public PlacingTile GetTileAtLocation(Vector3 position)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform && hit.transform.gameObject.TryGetComponent(out PlacingTile placingTile))
            {
                return placingTile;
            }
        }

        return null;
    }

    private ValidLocationState CheckValidLocations(PlacingTile placingTile)
    {
        return placingTile.IsValidLocation;

        /*
        switch (currentBuilding.locationToCheck.Length)
        {
            case 1:
            case 2:
                return placingTile.IsValidLocation;
            case 0:
            default:
                return false;
        }*/
    }

    public static List<PlacingTile> CheckSurroundingPlacementTiles(PlacingTile placingTile)
    {
        List<PlacingTile> surroundingTiles = new List<PlacingTile>();

        surroundingTiles.Add(Instance.GetTileAtLocation(placingTile.transform.position + new Vector3(-1, 1, 0))); // Top Left
        surroundingTiles.Add(Instance.GetTileAtLocation(placingTile.transform.position + new Vector3(0, 1, 0))); // Top Mid
        surroundingTiles.Add(Instance.GetTileAtLocation(placingTile.transform.position + new Vector3(1, 1, 0))); // Top Right

        surroundingTiles.Add(Instance.GetTileAtLocation(placingTile.transform.position + new Vector3(-1, 0, 0))); // Left
        surroundingTiles.Add(Instance.GetTileAtLocation(placingTile.transform.position + new Vector3(1, 0, 0))); // Right

        surroundingTiles.Add(Instance.GetTileAtLocation(placingTile.transform.position + new Vector3(-1, -1, 0))); // Bot Left
        surroundingTiles.Add(Instance.GetTileAtLocation(placingTile.transform.position + new Vector3(0, -1, 0))); // Bot Mid
        surroundingTiles.Add(Instance.GetTileAtLocation(placingTile.transform.position + new Vector3(1, -1, 0))); // Bot Right


        return surroundingTiles;
    }
    #endregion
    #endregion
}
