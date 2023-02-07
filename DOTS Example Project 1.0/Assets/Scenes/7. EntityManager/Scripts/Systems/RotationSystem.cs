using System.Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct RotationSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
       
    }

    public void OnDestroy(ref SystemState state)
    {
    
    }

    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;

        foreach (var (rot, rotData) in SystemAPI.Query<RefRW<Rotation>, RefRO<RotatingData>>().WithNone<StopRotatingTag>().WithAll<RotateTag>())
        {
            var xRot = quaternion.RotateX( rotData.ValueRO.Value * Mathf.Deg2Rad * dt);
            
            rot.ValueRW.Value = math.mul(rot.ValueRO.Value, xRot);
        }
        
    }
}
