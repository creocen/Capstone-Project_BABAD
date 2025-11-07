using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BouncySurface : MonoBehaviour
{
    public enum ForceType
    {
        Additive,
        Multiplicative
    }

    [SerializeField] private ForceType forceType = ForceType.Additive;
    [SerializeField] private float bounceStrength = 0.5f;

    private PhysicsMaterial2D material;

    void Awake()
    {
        material = new PhysicsMaterial2D("DynamicBounce");
        Collider2D col = GetComponent<Collider2D>();
        col.sharedMaterial = material;
        UpdateBounce();
    }

    void UpdateBounce()
    {
        if (forceType == ForceType.Additive)
        {
            material.bounciness = bounceStrength;
        }
        else
        {
            material.bounciness = material.bounciness * bounceStrength;
        }

        material.friction = 0;
        material.bounceCombine = PhysicsMaterialCombine2D.Maximum;
    }
}
