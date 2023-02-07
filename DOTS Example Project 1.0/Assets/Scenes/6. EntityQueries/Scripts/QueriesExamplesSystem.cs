using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;



public partial struct QueriesExamplesSystem : ISystem
{
    
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<QuerySingleton>();
    }

    public void OnDestroy(ref SystemState state)
    {
       
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityQuery newEntityquery = state.GetEntityQuery(ComponentType.ReadOnly<TagComponent1>());
        float dt = SystemAPI.Time.DeltaTime;
        
        //this query is the same as the one below it
        EntityQuery query = state.GetEntityQuery(ComponentType.ReadWrite<Translation>(), ComponentType.ReadOnly<Rotation>());

        new MovementJob().Schedule();
        
        EntityQuery query2 = state.GetEntityQuery(ComponentType.ReadOnly<TagComponent1>(), ComponentType.ReadOnly<TagComponent2>(), ComponentType.ReadOnly<RotateTag>());
        
        new RotateJob1
        {
            dt = SystemAPI.Time.DeltaTime
        }.Schedule();
        
        
        EntityQuery query3 = state.GetEntityQuery(ComponentType.Exclude<TagComponent1>(), ComponentType.ReadOnly<TagComponent2>());
        
        new RotateJob2
        {
            dt = SystemAPI.Time.DeltaTime
        }.Schedule();
        
        EntityQueryDesc myQueryDesc = new EntityQueryDesc
        {
            Any = new[] {ComponentType.ReadOnly<TagComponent3>(), ComponentType.ReadOnly<TagComponent2>()},
            All = new[] {ComponentType.ReadWrite<Rotation>()},
        
        };
        
        new RotateJob3
        {
            dt = SystemAPI.Time.DeltaTime
        }.Schedule();
    }
}

public partial struct MovementJob : IJobEntity
{
    public void Execute(ref Translation trans, in Rotation rot)
    {
        trans.Value += new float3(0, 0.001f, 0);
    }
}

[WithAny(typeof(TagComponent1))]
public partial struct RotateJob1 : IJobEntity
{
    public float dt; 
    public void Execute( ref Rotation rot)
    {
        var xRot = quaternion.RotateX(80 * Mathf.Deg2Rad * dt);
        rot.Value = math.mul(rot.Value, xRot);
    }
}
[WithAll( typeof(TagComponent2))]
[WithNone( typeof(TagComponent3))]
public partial struct RotateJob2 : IJobEntity
{
    public float dt; 
    public void Execute( ref Rotation rot)
    {
        var xRot = quaternion.RotateY(80 * Mathf.Deg2Rad * dt);
        rot.Value = math.mul(rot.Value, xRot);
    }
}

[WithAll(typeof(TagComponent3), typeof(TagComponent2))]
public partial struct RotateJob3 : IJobEntity
{
    public float dt; 
    public void Execute( [EntityIndexInQuery] int i,  ref Rotation rot)
    {
        var xRot = quaternion.RotateZ(80 * Mathf.Deg2Rad * dt);
        rot.Value = math.mul(rot.Value, xRot);
    }
}