using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class BotController
{
    public abstract class BotAIStateBase : StateBase
    {
        protected BotController bot;

        public BotAIStateBase(BotController bot)
        {
            this.bot = bot;
        }

    }

    public class BotStateMachine
    {
        public BotAIStateBase currState;


        public void Initialize(BotAIStateBase state)
        {
            currState = state;
            state.Enter();
        }

        public void ChangeState(BotAIStateBase newState)
        {
            currState.Exit();
            currState = newState;
            newState.Enter();
        }
    }

    public class BotStopState : BotAIStateBase
    {
        public BotStopState(BotController bot) : base(bot)
        {

        }

    } 

    public class BotIdleState : BotAIStateBase
    {
        Coroutine searchEnemyCoroutine, searchCrushableCoroutine;

        float searchMaxDuration = 4f;

        public BotIdleState(BotController bot) : base(bot)
        {
            
        }

        public override void Start()
        {
            base.Start();
            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            bot.Move(Time.fixedDeltaTime);
        }

        public override void Enter()
        {
            base.Enter();

            bot.moveVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            if (searchCrushableCoroutine != null)
            {
                bot.StopCoroutine(searchCrushableCoroutine);
            }

            searchCrushableCoroutine = bot.StartCoroutine(SearchCrushable());

            if (searchEnemyCoroutine != null)
            {
                bot.StopCoroutine(searchEnemyCoroutine);
            }

            searchEnemyCoroutine = bot.StartCoroutine(SearchEnemy());
        }

        public override void Exit()
        {
            base.Exit();

            if (searchEnemyCoroutine != null)
            {
                bot.StopCoroutine(searchEnemyCoroutine);
            }

            if (searchCrushableCoroutine != null)
            {
                bot.StopCoroutine(searchCrushableCoroutine);
            }
        }

        private IEnumerator SearchCrushable()
        {
            yield return new WaitForSeconds(5.0f);

            float searchDuration = 0;

            while (true)
            {
                var crushable = bot.FindClosestCrushable();
                if (crushable != null)
                {
                    if(Random.Range(0f,1f) > bot.crushProb)
                    {
                        if (Vector2.Distance(crushable.transform.position, bot.transform.position) <= bot.detectionRange)
                        {
                            bot.sm.ChangeState(new BotCrushingState(bot, crushable));
                        }
                        /*
                        else if (searchDuration > searchMaxDuration)
                        {
                            bot.sm.ChangeState(new BotCrushingState(bot, crushable));
                            searchDuration = 0;
                        }
                        */
                    }
                }
                searchDuration += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator SearchEnemy()
        {
            yield return new WaitForSeconds(5.0f);

            float searchDuration = 0;

            while (true)
            {
                var hitColliders = Physics.OverlapSphere(bot.transform.position, bot.detectionRange);
                if (hitColliders.Count() != 0)
                {
                    var newList = hitColliders.ToList();
                    foreach (var collider in newList)
                    {
                        if(collider.gameObject.tag.Contains("Blade") && collider.gameObject.transform.position != bot.gameObject.transform.position)
                        {
                            if (Random.Range(0f, 1f) > bot.followProb)
                            {
                                if (Vector2.Distance(collider.gameObject.transform.position, bot.transform.position) <= bot.detectionRange)
                                {
                                    bot.sm.ChangeState(new BotFollowingState(bot, collider.gameObject));
                                }
                            }
                        }
                    }
                }
                searchDuration += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

    public class BotCrushingState : BotAIStateBase
    {
        Coroutine crushCoroutine;

        GameObject target;

        GameObject cubeTarget;

        Vector3 direction;

        Vector3 newPos;

        public BotCrushingState(BotController bot, GameObject crushable) : base(bot)
        {
            target = crushable;
            if(target.GetComponent<DestructibleController>().Destructible.cubeCount > 0)
            {
                cubeTarget = target.transform.GetChild(0).gameObject;
            }
            else
            {
                cubeTarget = null;
            }
            
        }

        public override void Start()
        {
            base.Start();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(target != null && cubeTarget != null && !bot.force)
            {
                direction = cubeTarget.transform.position - bot.transform.position;
                newPos = bot.transform.position + direction.normalized * bot.movementSpeed * Time.fixedDeltaTime;
                bot.rigidBody.MovePosition(newPos);
            }
        }

        public override void Enter()
        {
            base.Enter();

            if (crushCoroutine != null)
            {
                bot.StopCoroutine(crushCoroutine);
            }

            crushCoroutine = bot.StartCoroutine(ContinueCrush());
        }

        public override void Exit()
        {
            base.Exit();

            if (crushCoroutine != null)
            {
                bot.StopCoroutine(crushCoroutine);
            }
        }

        private IEnumerator ContinueCrush()
        {
            while(true)
            {
                if(target == null)
                {
                    bot.sm.ChangeState(new BotIdleState(bot));
                }
                else
                {
                    if(target.GetComponent<DestructibleController>().Destructible.cubeCount > 0)
                    {
                        cubeTarget = target.transform.GetChild(0).gameObject;
                    }
                    else
                    {
                        bot.sm.ChangeState(new BotIdleState(bot));
                    }
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    public class BotFollowingState : BotAIStateBase
    {
        Coroutine followCoroutine;

        GameObject target;

        Vector3 direction;

        Vector3 newPos;

        public BotFollowingState(BotController bot, GameObject blade) : base(bot)
        {
            target = blade;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (target != null && !bot.force)
            {
                direction = target.transform.position - bot.transform.position;
                newPos = bot.transform.position + direction.normalized * bot.movementSpeed * Time.fixedDeltaTime;
                bot.rigidBody.MovePosition(newPos);
            }
        }

        public override void Enter()
        {
            base.Enter();

            if (followCoroutine != null)
            {
                bot.StopCoroutine(followCoroutine);
            }

            followCoroutine = bot.StartCoroutine(ContinueFollow());
        }

        public override void Exit()
        {
            base.Exit();

            if (followCoroutine != null)
            {
                bot.StopCoroutine(followCoroutine);
            }
        }

        private IEnumerator ContinueFollow()
        {
            float searchDuration = 0;

            while (true)
            {
                if (target == null || searchDuration > bot.maxFollowDuration)
                {
                    bot.sm.ChangeState(new BotIdleState(bot));
                }
                searchDuration += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
