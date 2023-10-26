using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public partial class BotController : MonoBehaviour
{
    [SerializeField] protected string nickname;
    [SerializeField] protected TextMeshProUGUI nicknameTMP;

    [SerializeField] protected float movementSpeed;
    [SerializeField] protected Rigidbody rigidBody;

    [SerializeField] protected Vector3 newPos;
    [SerializeField] protected Vector3 moveVector;
    [SerializeField] protected float detectionRange;
    [SerializeField] protected float maxFollowDuration;
    [SerializeField] protected float power;

    [SerializeField] protected float crushProb;
    [SerializeField] protected float followProb;

    BotStateMachine sm;

    public int cubeCount;

    private bool force;
    public bool isDead;

    public Blade blade;

    void Start()
    {
        nickname = leaderboardManager.RandomName();
        blade = new Blade
        {
            name = nickname,
            cubeCount = Random.Range(100, 1000)
        };

        LeaderboardManager.instance.blades.Add(blade);
        cubeCount = 0;
        force = false;
        isDead = false;
        sm = new();
        sm.Initialize(new BotStopState(this));
        StartBot();
        sm.currState.Start();
        SetNickname(nickname);
        destructibleManager = GameObject.Find("DestructibleSpawnManager").GetComponent<DestructibleManager>();
        this.movementSpeed = movementSpeed / 2.0f;
    }

    private void FixedUpdate()
    {
        sm.currState.FixedUpdate();
    }

    public void StopBot()
    {
        sm.ChangeState(new BotStopState(this));
    }

    public void StartBot()
    {
        sm.ChangeState(new BotIdleState(this));
    }

    public void SetNickname(string name)
    {
        nickname = name;
        nicknameTMP.text = name;
    }

    public string GetNickname()
    {
        return nickname;
    }

    public void IncrementCubeCount(int score)
    {
        cubeCount += score;
        blade.cubeCount += score;
    }

    public void DestroyBlade()
    {
        leaderboardManager.blades.Remove(blade);
        Destroy(gameObject);
    }

    public int GetCubeCount()
    {
        return this.cubeCount;
    }

    public void Move(float deltaTime)
    {
        CheckRayCast();
        if(!force && !isDead)
        {
            newPos = transform.position + moveVector.normalized * movementSpeed * deltaTime;
            rigidBody.MovePosition(newPos);
        }
    }

    public void CheckRayCast()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, new Vector3(0, 0, 1f), out hit, 5, layerMask))
        {
            if (moveVector.x > 0)
            {
                moveVector = new Vector3(moveVector.normalized.x + Random.Range(0, 3f), 0, -moveVector.normalized.z - Random.Range(0, 3f));
            }
            else if(moveVector.x == 0)
            {
                moveVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 0f));
            }
            else
            {
                moveVector = new Vector3(moveVector.normalized.x - Random.Range(0, 3f), 0, -moveVector.normalized.z - Random.Range(0, 3f));
            }
        }
        else if (Physics.Raycast(transform.position, new Vector3(0, 0, -1f), out hit, 5, layerMask))
        {
            if (moveVector.x > 0)
            {
                moveVector = new Vector3(moveVector.normalized.x + Random.Range(0, 3f), 0, -moveVector.normalized.z + Random.Range(0, 3f));
            }
            else if (moveVector.x == 0)
            {
                moveVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(0f, 1f));
            }
            else
            {
                moveVector = new Vector3(moveVector.normalized.x - Random.Range(0, 3f), 0, -moveVector.normalized.z + Random.Range(0, 3f));
            }
        }
        else if (Physics.Raycast(transform.position, new Vector3(1f, 0, 0), out hit, 5, layerMask))
        {
            if (moveVector.z < 0)
            {
                moveVector = new Vector3(-moveVector.normalized.x - Random.Range(0, 3f), 0, moveVector.normalized.z - Random.Range(0, 3f));
            }
            else if (moveVector.z == 0)
            {
                moveVector = new Vector3(Random.Range(-1f, 0f), 0, Random.Range(-1f, 1f));
            }
            else
            {
                moveVector = new Vector3(-moveVector.normalized.x - Random.Range(0, 3f), 0, moveVector.normalized.z + Random.Range(0, 3f));
            }
        }
        else if (Physics.Raycast(transform.position, new Vector3(-1f, 0, 0), out hit, 5, layerMask))
        {
            if (moveVector.z < 0)
            {
                moveVector = new Vector3(-moveVector.normalized.x + Random.Range(0, 3f), 0, moveVector.normalized.z - Random.Range(0, 3f));
            }
            else if (moveVector.z == 0)
            {
                moveVector = new Vector3(Random.Range(0f, 1f), 0, Random.Range(-1f, 1f));
            }
            else
            {
                moveVector = new Vector3(-moveVector.normalized.x + Random.Range(0, 3f), 0, moveVector.normalized.z + Random.Range(0, 3f));
            }
        }
    }

    public GameObject FindClosestCrushable()
    {
        var crushables = DestructibleManager.instance.existedCrushables.ToList();
        crushables.Sort((x, y) => Vector3.Distance(x.transform.position, gameObject.transform.position).CompareTo(Vector3.Distance(y.transform.position, gameObject.transform.position)));
        return crushables[0];
    }

    public void SetForce(bool newValue)
    {
        this.force = newValue;
    }

    public bool GetForce()
    {
        return this.force;
    }

    public void SetPower(float newValue)
    {
        this.power = newValue;
    }

    public float GetPower()
    {
        return this.power;
    }
}