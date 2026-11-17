
using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Identidade")]
    [SerializeField] private EntityOwnership ownership =
        EntityOwnership.Neutral;

    [SerializeField] private Entity owner;

    private EntityState state = EntityState.Alive;
    private HealthComponent healthComponent;

    public EntityOwnership Ownership => ownership;
    public Entity Owner => owner;
    public EntityState State => state;

    public HealthComponent Health => healthComponent;

    public bool IsAlive => state == EntityState.Alive;
    public bool IsDead => state == EntityState.Dead;

    public event Action<Entity> OnDied;

    protected virtual void Awake()
    {
        state = EntityState.Alive;
        healthComponent = GetComponent<HealthComponent>();

        if (owner == this)
        {
            Debug.LogError(
                "Uma entidade não pode ser proprietária de si mesma!",
                this
            );

            owner = null;
        }
    }

    public void SetOwnership(
        EntityOwnership newOwnership,
        Entity newOwner = null)
    {
        if (newOwner == this)
        {
            Debug.LogError(
                "Uma entidade não pode ser proprietária de si mesma!",
                this
            );

            return;
        }

        ownership = newOwnership;
        owner = newOwner;
    }

    public bool Die()
    {
        if (state != EntityState.Alive)
            return false;

        state = EntityState.Dying;

        OnDeath();

        state = EntityState.Dead;

        OnDied?.Invoke(this);

        return true;
    }

    protected virtual void OnDeath()
    {
        // Comportamentos específicos de morte serão adicionados
        // posteriormente por sistemas especializados.
    }
}
