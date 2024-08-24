﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;



public partial class StructuralChangesExample : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<StructuralChangesSingleton>();
    }

    protected override void OnUpdate()
    {
        
         Entities.WithNone<StructuralChangesComp>().ForEach((Entity entity, ref LocalTransform trans) =>
        {
            EntityManager.AddComponent<StructuralChangesComp>(entity);
            Debug.Log("run once");
            
            EntityManager.RemoveComponent<LocalTransform>(entity);
            var createdEntity = EntityManager.CreateEntity();
            EntityManager.DestroyEntity(createdEntity);
        }).WithStructuralChanges().Run();
        
        Entities.ForEach((Entity entity, ref LocalTransform trans) =>
        {
            var refToMonobehavior = MonoBehaviorExample.Instance; 
        }).WithoutBurst().Run();
        

        
    }
}
