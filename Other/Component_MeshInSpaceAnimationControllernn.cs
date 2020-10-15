// Decompiled with JetBrains decompiler
// Type: NeoAxis.Component_MeshInSpaceAnimationControllernn
// Assembly: NeoAxis.Core, Version=2020.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EDA28C08-D9E1-4128-B189-1D9AD9F5F9DC
// Assembly location: E:\NeoAxis Projects 2020.1.1\New Project 6\Binaries\NeoAxis.Core.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NeoAxis
{
  /// <summary>An animation controller for the mesh in space.</summary>
  /// <remarks>Implemented linear skinning algorithm.</remarks>
  public class Component_MeshInSpaceAnimationControllernn : Component
  {
    private ReferenceField<double> _speed = (ReferenceField<double>) 1.0;
    private ReferenceField<bool> _autoRewind = (ReferenceField<bool>) true;
    private ReferenceField<bool> _displaySkeleton = (ReferenceField<bool>) false;
    private ReferenceField<bool> _calculateOnCPU = (ReferenceField<bool>) false;
    public double currentAnimationTime;
    private double currentEngineTime;
    private bool needResetToOriginalMesh;
    private bool needRecreateModifiableMesh;
    private Component_SkeletonBone[] bones;
    private Component_SkeletonAnimationTrack.CalculateBoneTransformsItem[] boneTransforms;
    private Component_MeshInSpaceAnimationControllernn.BoneGlobalTransformItem[] boneGlobalTransforms;
    private Matrix4F[] transformMatrixRelativeToSkin;
    private Component_SkeletonAnimationTrack.CalculateBoneTransformsItem[] transformRelativeToSkin;
    private ReferenceField<Component_Animation> _playAnimation;
    private ReferenceField<Component_Skeleton> _replaceSkeleton;

    private double calculatedForTime { get; set; } = -1.0;

    private bool hasScale { get; set; }

    [Browsable(false)]
    public Component_MeshInSpace ParentMeshInSpace
    {
      get
      {
        return this.Parent as Component_MeshInSpace;
      }
    }

    /// <summary>The animation used by the controller.</summary>
    public Reference<Component_Animation> PlayAnimation
    {
      get
      {
        if (this._playAnimation.BeginGet())
          this.PlayAnimation = this._playAnimation.Get((object) this);
        return this._playAnimation.value;
      }
      set
      {
        if (!this._playAnimation.BeginSet(ref value))
          return;
        try
        {
          Action<Component_MeshInSpaceAnimationControllernn> animationChanged = this.PlayAnimationChanged;
          if (animationChanged != null)
            animationChanged(this);
          this.ResetTime(this._playAnimation.value.Value == null);
          this.Reset();
        }
        finally
        {
          this._playAnimation.EndSet();
        }
      }
    }

    /// <summary>Occurs when the <see cref="P:NeoAxis.Component_MeshInSpaceAnimationControllernn.PlayAnimation" /> property value changes.</summary>
    public event Action<Component_MeshInSpaceAnimationControllernn> PlayAnimationChanged;

    /// <summary>Animation speed multiplier.</summary>
    [Range(0.0, 10.0, RangeAttribute.ConvenientDistributionEnum.Exponential, 3.0)]
    [DefaultValue(1.0)]
    public Reference<double> Speed
    {
      get
      {
        if (this._speed.BeginGet())
          this.Speed = this._speed.Get((object) this);
        return this._speed.value;
      }
      set
      {
        if (!this._speed.BeginSet(ref value))
          return;
        try
        {
          Action<Component_MeshInSpaceAnimationControllernn> speedChanged = this.SpeedChanged;
          if (speedChanged != null)
            speedChanged(this);
        }
        finally
        {
          this._speed.EndSet();
        }
      }
    }

    /// <summary>Occurs when the <see cref="P:NeoAxis.Component_MeshInSpaceAnimationControllernn.Speed" /> property value changes.</summary>
    public event Action<Component_MeshInSpaceAnimationControllernn> SpeedChanged;

    /// <summary>Whether to rewind to the start when playing ended.</summary>
    [DefaultValue(true)]
    public Reference<bool> AutoRewind
    {
      get
      {
        if (this._autoRewind.BeginGet())
          this.AutoRewind = this._autoRewind.Get((object) this);
        return this._autoRewind.value;
      }
      set
      {
        if (!this._autoRewind.BeginSet(ref value))
          return;
        try
        {
          Action<Component_MeshInSpaceAnimationControllernn> autoRewindChanged = this.AutoRewindChanged;
          if (autoRewindChanged != null)
            autoRewindChanged(this);
        }
        finally
        {
          this._autoRewind.EndSet();
        }
      }
    }

    /// <summary>Occurs when the <see cref="P:NeoAxis.Component_MeshInSpaceAnimationControllernn.AutoRewind" /> property value changes.</summary>
    public event Action<Component_MeshInSpaceAnimationControllernn> AutoRewindChanged;

    /// <summary>The skeleton used by the controller.</summary>
    [DefaultValue(null)]
    public Reference<Component_Skeleton> ReplaceSkeleton
    {
      get
      {
        if (this._replaceSkeleton.BeginGet())
          this.ReplaceSkeleton = this._replaceSkeleton.Get((object) this);
        return this._replaceSkeleton.value;
      }
      set
      {
        if (!this._replaceSkeleton.BeginSet(ref value))
          return;
        try
        {
          Action<Component_MeshInSpaceAnimationControllernn> replaceSkeletonChanged = this.ReplaceSkeletonChanged;
          if (replaceSkeletonChanged != null)
            replaceSkeletonChanged(this);
          this.ResetTime(false);
          this.Reset();
        }
        finally
        {
          this._replaceSkeleton.EndSet();
        }
      }
    }

    /// <summary>Occurs when the <see cref="P:NeoAxis.Component_MeshInSpaceAnimationControllernn.ReplaceSkeleton" /> property value changes.</summary>
    public event Action<Component_MeshInSpaceAnimationControllernn> ReplaceSkeletonChanged;

    /// <summary>Whether to display skeleton.</summary>
    [DefaultValue(false)]
    public Reference<bool> DisplaySkeleton
    {
      get
      {
        if (this._displaySkeleton.BeginGet())
          this.DisplaySkeleton = this._displaySkeleton.Get((object) this);
        return this._displaySkeleton.value;
      }
      set
      {
        if (!this._displaySkeleton.BeginSet(ref value))
          return;
        try
        {
          Action<Component_MeshInSpaceAnimationControllernn> displaySkeletonChanged = this.DisplaySkeletonChanged;
          if (displaySkeletonChanged != null)
            displaySkeletonChanged(this);
        }
        finally
        {
          this._displaySkeleton.EndSet();
        }
      }
    }

    /// <summary>Occurs when the <see cref="P:NeoAxis.Component_MeshInSpaceAnimationControllernn.DisplaySkeleton" /> property value changes.</summary>
    public event Action<Component_MeshInSpaceAnimationControllernn> DisplaySkeletonChanged;

    /// <summary>
    /// Whether to calculate skinning by means CPU instead GPU.
    /// </summary>
    [Category("Debug")]
    [DisplayName("Calculate On CPU")]
    [DefaultValue(false)]
    public Reference<bool> CalculateOnCPU
    {
      get
      {
        if (this._calculateOnCPU.BeginGet())
          this.CalculateOnCPU = this._calculateOnCPU.Get((object) this);
        return this._calculateOnCPU.value;
      }
      set
      {
        if (!this._calculateOnCPU.BeginSet(ref value))
          return;
        try
        {
          Action<Component_MeshInSpaceAnimationControllernn> calculateOnCpuChanged = this.CalculateOnCPUChanged;
          if (calculateOnCpuChanged != null)
            calculateOnCpuChanged(this);
          this.needRecreateModifiableMesh = true;
        }
        finally
        {
          this._calculateOnCPU.EndSet();
        }
      }
    }

    /// <summary>Occurs when the <see cref="P:NeoAxis.Component_MeshInSpaceAnimationControllernn.CalculateOnCPU" /> property value changes.</summary>
    public event Action<Component_MeshInSpaceAnimationControllernn> CalculateOnCPUChanged;

    protected override void OnEnabledInHierarchyChanged()
    {
      base.OnEnabledInHierarchyChanged();
      if (this.ParentMeshInSpace == null)
        return;
      if (this.EnabledInHierarchy)
      {
        this.ParentMeshInSpace.GetRenderSceneDataBefore += new Component_ObjectInSpace.GetRenderSceneDataDelegate(this.ParentMeshInSpace_GetRenderSceneDataBefore);
        this.ParentMeshInSpace.GetRenderSceneDataAddToFrameData += new Component_MeshInSpace.GetRenderSceneDataAddToFrameDataDelegate(this.ParentMeshInSpace_GetRenderSceneDataAddToFrameData);
      }
      else
      {
        this.ParentMeshInSpace.GetRenderSceneDataBefore -= new Component_ObjectInSpace.GetRenderSceneDataDelegate(this.ParentMeshInSpace_GetRenderSceneDataBefore);
        this.ParentMeshInSpace.GetRenderSceneDataAddToFrameData -= new Component_MeshInSpace.GetRenderSceneDataAddToFrameDataDelegate(this.ParentMeshInSpace_GetRenderSceneDataAddToFrameData);
        if (this.ParentMeshInSpace.ModifiableMesh_CreatedByObject == this)
          this.ParentMeshInSpace.ModifiableMesh_Destroy();
        this.needRecreateModifiableMesh = false;
      }
    }

    private void ParentMeshInSpace_GetRenderSceneDataBefore(
      Component_ObjectInSpace sender,
      ViewportRenderingContext context,
      GetRenderSceneDataMode mode)
    {
      bool flag = this.CheckNeedModifiableMesh();
      if (this.needRecreateModifiableMesh)
      {
        if (this.ParentMeshInSpace.ModifiableMesh_CreatedByObject == this)
          this.ParentMeshInSpace.ModifiableMesh_Destroy();
        this.needRecreateModifiableMesh = false;
      }
      if (flag)
      {
        if (this.ParentMeshInSpace.ModifiableMesh == null)
        {
          if ((bool) this.CalculateOnCPU)
            this.ParentMeshInSpace.ModifiableMesh_Create((Component) this, Component_MeshInSpace.ModifiableMeshCreationFlags.VertexBuffersCreateDuplicate | Component_MeshInSpace.ModifiableMeshCreationFlags.VertexBuffersDynamic);
          this.needRecreateModifiableMesh = false;
        }
      }
      else if (this.ParentMeshInSpace.ModifiableMesh_CreatedByObject == this)
      {
        this.ParentMeshInSpace.ModifiableMesh_Destroy();
        this.needRecreateModifiableMesh = false;
      }
      if (this.ParentMeshInSpace.ModifiableMesh_CreatedByObject == this)
        this.UpdateModifiableMesh(context);
      if (!(bool) this.DisplaySkeleton)
        return;
      this.RenderSkeleton(context.Owner);
    }

    private unsafe void ParentMeshInSpace_GetRenderSceneDataAddToFrameData(
      Component_MeshInSpace sender,
      ViewportRenderingContext context,
      GetRenderSceneDataMode mode,
      ref Component_RenderingPipeline.RenderSceneData.MeshItem item)
    {
      if ((bool) this.CalculateOnCPU)
        return;
      Component_Skeleton skeleton1 = (Component_Skeleton) this.ReplaceSkeleton;
      if (skeleton1 == null)
      {
        Reference<Component_Skeleton>? skeleton2 = this.ParentMeshInSpace?.Mesh.Value?.Skeleton;
        skeleton1 = skeleton2.HasValue ? (Component_Skeleton) skeleton2.GetValueOrDefault() : (Component_Skeleton) null;
      }
      if (skeleton1 != null)
      {
        Component_Animation componentAnimation = this.PlayAnimation.Value;
        if (componentAnimation != null)
        {
          this.UpdateAnimationTime();
          Component_SkeletonAnimationTrack animationTrack = componentAnimation is Component_SkeletonAnimation skeletonAnimation ? skeletonAnimation.Track.Value : (Component_SkeletonAnimationTrack) null;
          if (animationTrack != null || this.CalculateBoneTransforms != null)
          {
            this.Update(skeleton1, animationTrack, this.currentAnimationTime);
            if (this.transformMatrixRelativeToSkin != null && (uint) this.transformMatrixRelativeToSkin.Length > 0U)
            {
              item.AnimationData = new Component_RenderingPipeline.RenderSceneData.MeshItem.AnimationDataClass();
              item.AnimationData.Mode = true ? 1 : 2;
              Vector2I size = new Vector2I(4, MathEx.NextPowerOfTwo(this.transformMatrixRelativeToSkin.Length));
              Component_Image componentImage = context.DynamicTexture_Alloc(ViewportRenderingContext.DynamicTextureType.DynamicTexture, Component_Image.TypeEnum._2D, size, PixelFormat.Float32RGBA, 0, false, 1, false, false);
              GpuTexture.SurfaceData[] surfaceDataArray = componentImage.Result.GetData();
              if (surfaceDataArray == null)
                surfaceDataArray = new GpuTexture.SurfaceData[1]
                {
                  new GpuTexture.SurfaceData(0, 0, new byte[size.X * size.Y * 16])
                };
              byte[] data = surfaceDataArray[0].data;
              fixed (byte* numPtr = data)
              {
                for (int index = 0; index < this.transformMatrixRelativeToSkin.Length; ++index)
                  ((Matrix4F*) numPtr)[index] = this.transformMatrixRelativeToSkin[index];
              }
              componentImage.Result.SetData(new GpuTexture.SurfaceData[1]
              {
                new GpuTexture.SurfaceData(0, 0, data)
              });
              item.AnimationData.BonesTexture = componentImage;
            }
          }
        }
      }
    }

    private void RenderSkeleton(Viewport viewport)
    {
      Component_MeshInSpace parentMeshInSpace = this.ParentMeshInSpace;
      Matrix4? nullable;
      if (parentMeshInSpace == null)
      {
        nullable = new Matrix4?();
      }
      else
      {
        Transform transform = parentMeshInSpace.Transform.Value;
        nullable = (object) transform != null ? new Matrix4?(transform.ToMatrix4()) : new Matrix4?();
      }
      Matrix4 matrix4 = nullable ?? Matrix4.Identity;
      IList<Line3F> animatedSkeletonArrows = this.GetCurrentAnimatedSkeletonArrows();
      if (animatedSkeletonArrows == null)
        return;
      ColorValue colorVisible = new ColorValue(0.0, 0.5, 1.0, 0.7);
      viewport.Simple3DRenderer.SetColor(colorVisible, colorVisible * (ColorValue) ProjectSettings.Get.HiddenByOtherObjectsColorMultiplier);
      foreach (Line3F line3F in (IEnumerable<Line3F>) animatedSkeletonArrows)
        viewport.Simple3DRenderer.AddArrow(matrix4 * (Vector3) line3F.Start, matrix4 * (Vector3) line3F.End, 0.0, 0.0, false, 0.0);
    }

    private bool CheckNeedModifiableMesh()
    {
      if ((bool) this.CalculateOnCPU)
      {
        Reference<Component_Skeleton> reference = this.ReplaceSkeleton;
        if (reference.ReferenceSpecified)
          return true;
        Component_Mesh componentMesh = this.ParentMeshInSpace?.Mesh.Value;
        int num;
        if (componentMesh != null)
        {
          reference = componentMesh.Skeleton;
          num = reference.ReferenceSpecified ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0 || this.PlayAnimation.ReferenceSpecified)
          return true;
      }
      return false;
    }

    private void UpdateModifiableMesh(ViewportRenderingContext context)
    {
      Component_Mesh originalMesh = this.ParentMeshInSpace.Mesh.Value;
      Component_Mesh modifiableMesh = this.ParentMeshInSpace.ModifiableMesh;
      Component_Skeleton skeleton = (Component_Skeleton) this.ReplaceSkeleton ?? (Component_Skeleton) originalMesh.Skeleton;
      if (skeleton == null)
        return;
      Component_Animation componentAnimation = this.PlayAnimation.Value;
      if (componentAnimation != null)
      {
        this.UpdateAnimationTime();
        Component_SkeletonAnimationTrack animationTrack = componentAnimation is Component_SkeletonAnimation skeletonAnimation ? skeletonAnimation.Track.Value : (Component_SkeletonAnimationTrack) null;
        if (animationTrack != null || this.CalculateBoneTransforms != null)
        {
          this.Update(skeleton, animationTrack, this.currentAnimationTime);
          this.CalculateCPU(skeleton, originalMesh, modifiableMesh);
        }
      }
      if (this.needResetToOriginalMesh)
      {
        this.needResetToOriginalMesh = false;
        if ((bool) this.CalculateOnCPU)
          Component_MeshInSpaceAnimationControllernn.ResetToOriginalMesh(originalMesh, modifiableMesh);
      }
    }

    private void ResetTime(bool needResetToOriginalMesh)
    {
      if (needResetToOriginalMesh)
        this.needResetToOriginalMesh = true;
      this.currentEngineTime = EngineApp.EngineTime;
      this.currentAnimationTime = (double) (this.PlayAnimation.Value is Component_SkeletonAnimation skeletonAnimation ? skeletonAnimation.TrackStartTime : (Reference<double>) 0.0);
    }

    private void UpdateAnimationTime()
    {
      double engineTime = EngineApp.EngineTime;
      double num = engineTime - this.currentEngineTime;
      this.currentEngineTime = engineTime;
      this.currentAnimationTime += num * (double) this.Speed;
      if (this.PlayAnimation.Value is Component_SkeletonAnimation skeletonAnimation)
      {
        double trackStartTime = (double) skeletonAnimation.TrackStartTime;
        double length = (double) skeletonAnimation.Length;
        if (length > 0.0)
        {
          if ((bool) this.AutoRewind)
          {
            while (this.currentAnimationTime > trackStartTime + length)
              this.currentAnimationTime -= length;
            while (this.currentAnimationTime < trackStartTime)
              this.currentAnimationTime += length;
          }
          else
            MathEx.Clamp(ref this.currentAnimationTime, trackStartTime, trackStartTime + length);
        }
        else
          this.currentAnimationTime = trackStartTime;
      }
      else
        this.currentAnimationTime = 0.0;
    }

    public IList<Line3F> GetCurrentAnimatedSkeletonArrows()
    {
      Component_Skeleton componentSkeleton = this.ReplaceSkeleton.Value;
      if (componentSkeleton == null)
      {
        Reference<Component_Skeleton>? skeleton = this.ParentMeshInSpace?.Mesh.Value?.Skeleton;
        componentSkeleton = skeleton.HasValue ? (Component_Skeleton) skeleton.GetValueOrDefault() : (Component_Skeleton) null;
      }
      Component_Skeleton skeleton1 = componentSkeleton;
      Reference<Component_Animation> playAnimation;
      int num;
      if (skeleton1 != null)
      {
        playAnimation = this.PlayAnimation;
        num = playAnimation.Value == null ? 1 : 0;
      }
      else
        num = 1;
      if (num != 0)
        return (IList<Line3F>) null;
      playAnimation = this.PlayAnimation;
      Reference<Component_SkeletonAnimationTrack>? nullable1 = playAnimation.Value is Component_SkeletonAnimation skeletonAnimation ? new Reference<Component_SkeletonAnimationTrack>?(skeletonAnimation.Track) : new Reference<Component_SkeletonAnimationTrack>?();
      if (!nullable1.HasValue)
        return (IList<Line3F>) null;
      this.UpdateAnimationTime();
      Component_Skeleton skeleton2 = skeleton1;
      Reference<Component_SkeletonAnimationTrack>? nullable2 = nullable1;
      Component_SkeletonAnimationTrack animationTrack = nullable2.HasValue ? (Component_SkeletonAnimationTrack) nullable2.GetValueOrDefault() : (Component_SkeletonAnimationTrack) null;
      double currentAnimationTime = this.currentAnimationTime;
      this.Update(skeleton2, animationTrack, currentAnimationTime);
      return this.GetSkeletonArrows(skeleton1);
    }

    protected virtual void CalculateCPU(
      Component_Skeleton skeleton,
      Component_Mesh originalMesh,
      Component_Mesh modifiableMesh)
    {
      bool dualQuaternion = false;
      for (int index = 0; index < modifiableMesh.Result.MeshData.RenderOperations.Count; ++index)
      {
        Component_RenderingPipeline.RenderSceneData.MeshDataRenderOperation renderOperation1 = originalMesh.Result.MeshData.RenderOperations[index];
        Component_RenderingPipeline.RenderSceneData.MeshDataRenderOperation renderOperation2 = modifiableMesh.Result.MeshData.RenderOperations[index];
        Component_MeshInSpaceAnimationControllernn.ChannelFloat3 channelFloat3_1 = new Component_MeshInSpaceAnimationControllernn.ChannelFloat3(renderOperation1, renderOperation2, VertexElementSemantic.Position);
        if (channelFloat3_1.Exists)
        {
          Component_MeshInSpaceAnimationControllernn.ChannelFloat3 channelFloat3_2 = new Component_MeshInSpaceAnimationControllernn.ChannelFloat3(renderOperation1, renderOperation2, VertexElementSemantic.Normal);
          Component_MeshInSpaceAnimationControllernn.ChannelFloat4 channelFloat4 = new Component_MeshInSpaceAnimationControllernn.ChannelFloat4(renderOperation1, renderOperation2, VertexElementSemantic.Tangent);
          Component_MeshInSpaceAnimationControllernn.SourceChannel<Vector4I> sourceChannel1 = new Component_MeshInSpaceAnimationControllernn.SourceChannel<Vector4I>(renderOperation1, VertexElementSemantic.BlendIndices, VertexElementType.Integer4);
          Component_MeshInSpaceAnimationControllernn.SourceChannel<Vector4F> sourceChannel2 = new Component_MeshInSpaceAnimationControllernn.SourceChannel<Vector4F>(renderOperation1, VertexElementSemantic.BlendWeights, VertexElementType.Float4);
          if (sourceChannel1.Exists && sourceChannel2.Exists)
          {
            if (channelFloat3_2.Exists)
            {
              if (channelFloat4.Exists)
                this.TransformVertices(dualQuaternion, channelFloat3_1.SourceData, channelFloat3_2.SourceData, channelFloat4.SourceData, sourceChannel1.SourceData, sourceChannel2.SourceData, channelFloat3_1.DestData, channelFloat3_2.DestData, channelFloat4.DestData);
              else
                this.TransformVertices(dualQuaternion, channelFloat3_1.SourceData, channelFloat3_2.SourceData, sourceChannel1.SourceData, sourceChannel2.SourceData, channelFloat3_1.DestData, channelFloat3_2.DestData);
            }
            else
              this.TransformVertices(dualQuaternion, channelFloat3_1.SourceData, sourceChannel1.SourceData, sourceChannel2.SourceData, channelFloat3_1.DestData);
            channelFloat3_1.WriteChannel();
            channelFloat3_2.WriteChannel();
            channelFloat4.WriteChannel();
          }
        }
      }
    }

    private static void ResetToOriginalMesh(
      Component_Mesh originalMesh,
      Component_Mesh modifiableMesh)
    {
      for (int index1 = 0; index1 < modifiableMesh.Result.MeshData.RenderOperations.Count; ++index1)
      {
        Component_RenderingPipeline.RenderSceneData.MeshDataRenderOperation renderOperation1 = originalMesh.Result.MeshData.RenderOperations[index1];
        Component_RenderingPipeline.RenderSceneData.MeshDataRenderOperation renderOperation2 = modifiableMesh.Result.MeshData.RenderOperations[index1];
        VertexElementSemantic[] vertexElementSemanticArray = new VertexElementSemantic[4]
        {
          VertexElementSemantic.Position,
          VertexElementSemantic.Normal,
          VertexElementSemantic.Tangent,
          VertexElementSemantic.Bitangent
        };
        foreach (VertexElementSemantic semantic in vertexElementSemanticArray)
        {
          Component_MeshInSpaceAnimationControllernn.ChannelFloat3 channelFloat3 = new Component_MeshInSpaceAnimationControllernn.ChannelFloat3(renderOperation1, renderOperation2, semantic);
          if (channelFloat3.Exists)
          {
            for (int index2 = 0; index2 < channelFloat3.DestData.Length; ++index2)
              channelFloat3.DestData[index2] = channelFloat3.SourceData[index2];
            channelFloat3.WriteChannel();
          }
        }
      }
    }

    private void Reset()
    {
      this.calculatedForTime = -1.0;
      this.boneGlobalTransforms = (Component_MeshInSpaceAnimationControllernn.BoneGlobalTransformItem[]) null;
      this.bones = (Component_SkeletonBone[]) null;
      this.transformMatrixRelativeToSkin = (Matrix4F[]) null;
      this.transformRelativeToSkin = (Component_SkeletonAnimationTrack.CalculateBoneTransformsItem[]) null;
      this.hasScale = false;
    }

    public event Component_MeshInSpaceAnimationControllernn.CalculateBoneTransformsDelegate CalculateBoneTransforms;

    private void Update(
      Component_Skeleton skeleton,
      Component_SkeletonAnimationTrack animationTrack,
      double time)
    {
      if (time == this.calculatedForTime)
        return;
      this.calculatedForTime = time;
      this.bones = skeleton.GetBones(false);
      if (this.boneTransforms == null || this.boneTransforms.Length != this.bones.Length)
        this.boneTransforms = new Component_SkeletonAnimationTrack.CalculateBoneTransformsItem[this.bones.Length];
      animationTrack?.CalculateBoneTransforms(time, this.boneTransforms);
      Component_MeshInSpaceAnimationControllernn.CalculateBoneTransformsDelegate calculateBoneTransforms = this.CalculateBoneTransforms;
      if (calculateBoneTransforms != null)
        calculateBoneTransforms(this, this.boneTransforms);
      if (this.transformMatrixRelativeToSkin == null || this.transformMatrixRelativeToSkin.Length != this.bones.Length)
        this.transformMatrixRelativeToSkin = new Matrix4F[this.bones.Length];
      if (this.transformRelativeToSkin == null || this.transformRelativeToSkin.Length != this.bones.Length)
        this.transformRelativeToSkin = new Component_SkeletonAnimationTrack.CalculateBoneTransformsItem[this.bones.Length];
      if (this.boneGlobalTransforms == null || this.boneGlobalTransforms.Length != this.bones.Length)
        this.boneGlobalTransforms = new Component_MeshInSpaceAnimationControllernn.BoneGlobalTransformItem[this.bones.Length];
      Matrix4F zero = Matrix4F.Zero;
      for (int index = 0; index < this.bones.Length; ++index)
        this.boneGlobalTransforms[index] = new Component_MeshInSpaceAnimationControllernn.BoneGlobalTransformItem(false, ref zero);
      foreach (Component_SkeletonBone bone in this.bones)
        this.GetBoneGlobalTransformRecursive(skeleton, bone);
      this.hasScale = false;
      for (int index = 0; index < this.bones.Length; ++index)
      {
        Component_SkeletonBone bone = this.bones[index];
        Matrix4F result;
        if (bone != null)
          Matrix4F.Multiply(ref this.GetBoneGlobalTransformRecursive(skeleton, bone), ref bone.GetTransformMatrixInverse(), out result);
        else
          result = Matrix4F.Identity;
        this.transformMatrixRelativeToSkin[index] = result;
        Vector3F translation;
        QuaternionF rotation;
        Vector3F scale;
        result.Decompose(out translation, out rotation, out scale);
        this.transformRelativeToSkin[index] = new Component_SkeletonAnimationTrack.CalculateBoneTransformsItem()
        {
          Position = translation,
          Rotation = rotation,
          Scale = scale
        };
        if ((double) Math.Abs(1f - scale.X) > 1.0 / 1000.0 || (double) Math.Abs(1f - scale.Y) > 1.0 / 1000.0 || (double) Math.Abs(1f - scale.Y) > 1.0 / 1000.0)
          this.hasScale = true;
      }
    }

    private ref Matrix4F GetBoneGlobalTransformRecursive(
      Component_Skeleton skeleton,
      Component_SkeletonBone bone)
    {
      int cachedBoneIndex = bone.GetCachedBoneIndex(skeleton);
      if (!this.boneGlobalTransforms[cachedBoneIndex].HasValue)
      {
        Matrix4F result;
        Component_MeshInSpaceAnimationControllernn.ToMatrix4(ref this.boneTransforms[cachedBoneIndex], out result);
        if (bone.Parent is Component_SkeletonBone parent)
          result = this.GetBoneGlobalTransformRecursive(skeleton, parent) * result;
        this.boneGlobalTransforms[cachedBoneIndex] = new Component_MeshInSpaceAnimationControllernn.BoneGlobalTransformItem(true, ref result);
      }
      return ref this.boneGlobalTransforms[cachedBoneIndex].Value;
    }

    private IList<Line3F> GetSkeletonArrows(Component_Skeleton skeleton)
    {
      List<Line3F> line3FList = new List<Line3F>(this.bones.Length);
      for (int index = 0; index < this.bones.Length; ++index)
      {
        if (this.bones[index].Parent is Component_SkeletonBone parent)
        {
          int cachedBoneIndex = parent.GetCachedBoneIndex(skeleton);
          if (cachedBoneIndex != -1)
          {
            ref Component_MeshInSpaceAnimationControllernn.BoneGlobalTransformItem local1 = ref this.boneGlobalTransforms[cachedBoneIndex];
            ref Component_MeshInSpaceAnimationControllernn.BoneGlobalTransformItem local2 = ref this.boneGlobalTransforms[index];
            if (local1.HasValue && local2.HasValue)
            {
              Vector3F result1;
              local1.Value.GetTranslation(out result1);
              Vector3F result2;
              local2.Value.GetTranslation(out result2);
              line3FList.Add(new Line3F(result1, result2));
            }
          }
        }
      }
      return (IList<Line3F>) line3FList;
    }

    private void TransformVertices(
      bool dualQuaternion,
      Vector3F[] position,
      Vector3F[] normal,
      Vector4F[] tangent,
      Vector4I[] blendIndex,
      Vector4F[] blendWeight,
      Vector3F[] newPosition,
      Vector3F[] newNormal,
      Vector4F[] newTangent)
    {
      if (dualQuaternion)
      {
        for (int index = 0; index < newPosition.Length; ++index)
          newPosition[index] = this.TransformByDualQuaternionSkinning(position[index], normal[index], tangent[index], blendIndex[index], blendWeight[index], out newNormal[index], out newTangent[index]);
      }
      else
      {
        for (int index = 0; index < newPosition.Length; ++index)
          newPosition[index] = this.TransformVertexByLinearBlendingSkinning(position[index], normal[index], tangent[index], blendIndex[index], blendWeight[index], out newNormal[index], out newTangent[index]);
      }
    }

    private void TransformVertices(
      bool dualQuaternion,
      Vector3F[] position,
      Vector3F[] normal,
      Vector4I[] blendIndex,
      Vector4F[] blendWeight,
      Vector3F[] newPosition,
      Vector3F[] newNormal)
    {
      Vector4F newTangent;
      if (dualQuaternion)
      {
        for (int index = 0; index < newPosition.Length; ++index)
          newPosition[index] = this.TransformByDualQuaternionSkinning(position[index], normal[index], Vector4F.One, blendIndex[index], blendWeight[index], out newNormal[index], out newTangent);
      }
      else
      {
        for (int index = 0; index < newPosition.Length; ++index)
          newPosition[index] = this.TransformVertexByLinearBlendingSkinning(position[index], normal[index], Vector4F.One, blendIndex[index], blendWeight[index], out newNormal[index], out newTangent);
      }
    }

    private void TransformVertices(
      bool dualQuaternion,
      Vector3F[] position,
      Vector4I[] blendIndex,
      Vector4F[] blendWeight,
      Vector3F[] newPosition)
    {
      if (dualQuaternion)
      {
        for (int index = 0; index < newPosition.Length; ++index)
          newPosition[index] = this.TransformByDualQuaternionSkinning(position[index], blendIndex[index], blendWeight[index]);
      }
      else
      {
        for (int index = 0; index < newPosition.Length; ++index)
          newPosition[index] = this.TransformVertexByLinearBlendingSkinning(position[index], blendIndex[index], blendWeight[index]);
      }
    }

    private Vector3F TransformVertexByLinearBlendingSkinning(
      Vector3F position,
      Vector4I blendIndex,
      Vector4F blendWeight)
    {
      Vector4F vector4F = new Vector4F(position, 1f);
      Matrix4F matrix;
      return !this.GetVertexTransformByLinearBlendingSkinning(blendIndex, blendWeight, out matrix) ? position : (matrix * vector4F).ToVector3F();
    }

    private Vector3F TransformVertexByLinearBlendingSkinning(
      Vector3F position,
      Vector3F normal,
      Vector4F tangent,
      Vector4I blendIndex,
      Vector4F blendWeight,
      out Vector3F newNormal,
      out Vector4F newTangent)
    {
      Vector4F vector4F = new Vector4F(position, 1f);
      Matrix4F matrix;
      if (!this.GetVertexTransformByLinearBlendingSkinning(blendIndex, blendWeight, out matrix))
      {
        newNormal = normal;
        newTangent = tangent;
        return position;
      }
      QuaternionF rotation;
      matrix.Decompose(out Vector3F _, out rotation, out Vector3F _);
      newNormal = this.CalculateBlendNormal(normal, rotation);
      newTangent = new Vector4F(this.CalculateBlendNormal(tangent.ToVector3F(), rotation), tangent.W);
      return (matrix * vector4F).ToVector3F();
    }

    private Vector3F TransformByDualQuaternionSkinning(
      Vector3F position,
      Vector4I blendIndex,
      Vector4F blendWeight)
    {
      Component_MeshInSpaceAnimationControllernn.DualQuaternionF sumDq;
      Vector3F scale;
      if (!this.GetVertexTransformByDualQuaternionSkinning(blendIndex, blendWeight, out sumDq, out scale))
        return position;
      sumDq.Normalize();
      return this.CalculateBlendPosition(scale * position, sumDq);
    }

    private Vector3F TransformByDualQuaternionSkinning(
      Vector3F position,
      Vector3F normal,
      Vector4F tangent,
      Vector4I blendIndex,
      Vector4F blendWeight,
      out Vector3F newNormal,
      out Vector4F newTangent)
    {
      Component_MeshInSpaceAnimationControllernn.DualQuaternionF sumDq;
      Vector3F scale;
      if (!this.GetVertexTransformByDualQuaternionSkinning(blendIndex, blendWeight, out sumDq, out scale))
      {
        newNormal = normal;
        newTangent = tangent;
        return position;
      }
      sumDq.Normalize();
      newNormal = this.CalculateBlendNormal(normal, sumDq.Q0);
      newTangent = new Vector4F(this.CalculateBlendNormal(tangent.ToVector3F(), sumDq.Q0), tangent.W);
      return this.CalculateBlendPosition(scale * position, sumDq);
    }

    private Vector3F CalculateBlendNormal(Vector3F vIn, QuaternionF blendQ)
    {
      Vector3F v1 = new Vector3F(blendQ.X, blendQ.Y, blendQ.Z);
      float w = blendQ.W;
      return vIn + 2f * Vector3F.Cross(v1, Vector3F.Cross(v1, vIn) + w * vIn);
    }

    private Vector3F CalculateBlendPosition(
      Vector3F position,
      Component_MeshInSpaceAnimationControllernn.DualQuaternionF blendDQ)
    {
      Vector3F v1 = new Vector3F(blendDQ.Q0.X, blendDQ.Q0.Y, blendDQ.Q0.Z);
      Vector3F v2 = new Vector3F(blendDQ.Qe.X, blendDQ.Qe.Y, blendDQ.Qe.Z);
      float w1 = blendDQ.Q0.W;
      float w2 = blendDQ.Qe.W;
      return position + 2f * Vector3F.Cross(v1, Vector3F.Cross(v1, position) + w1 * position) + 2f * (w1 * v2 - w2 * v1 + Vector3F.Cross(v1, v2));
    }

    private static void ToMatrix4(
      ref Component_SkeletonAnimationTrack.CalculateBoneTransformsItem keyframe,
      out Matrix4F result)
    {
      Matrix3F result1;
      keyframe.Rotation.ToMatrix3(out result1);
      Matrix3F result2;
      Matrix3F.FromScale(ref keyframe.Scale, out result2);
      Matrix3F result3;
      Matrix3F.Multiply(ref result1, ref result2, out result3);
      result = new Matrix4F(result3, keyframe.Position);
    }

    private static float Dot(ref QuaternionF q1, ref QuaternionF q2)
    {
      return (float) ((double) q1.W * (double) q2.W + (double) q1.X * (double) q2.X + (double) q1.Y * (double) q2.Y + (double) q1.Z * (double) q2.Z);
    }

    private bool GetVertexTransformByLinearBlendingSkinning(
      Vector4I blendIndex,
      Vector4F blendWeight,
      out Matrix4F matrix)
    {
      int x = blendIndex.X;
      if (x == -1)
      {
        matrix = Matrix4F.Identity;
        return false;
      }
      matrix = blendWeight.X * this.transformMatrixRelativeToSkin[x];
      int y = blendIndex.Y;
      if (y == -1)
        return true;
      matrix += blendWeight.Y * this.transformMatrixRelativeToSkin[y];
      int z = blendIndex.Z;
      if (z == -1)
        return true;
      matrix += blendWeight.Z * this.transformMatrixRelativeToSkin[z];
      int w = blendIndex.W;
      if (w == -1)
        return true;
      matrix += blendWeight.W * this.transformMatrixRelativeToSkin[w];
      return true;
    }

    private bool GetVertexTransformByDualQuaternionSkinning(
      Vector4I blendIndex,
      Vector4F blendWeight,
      out Component_MeshInSpaceAnimationControllernn.DualQuaternionF sumDq,
      out Vector3F scale)
    {
      float x1 = blendWeight.X;
      int x2 = blendIndex.X;
      if (x2 == -1)
      {
        sumDq = new Component_MeshInSpaceAnimationControllernn.DualQuaternionF(QuaternionF.Identity, Vector3F.Zero);
        scale = Vector3F.One;
        return false;
      }
      Component_SkeletonAnimationTrack.CalculateBoneTransformsItem boneTransformsItem1 = this.transformRelativeToSkin[x2];
      sumDq = new Component_MeshInSpaceAnimationControllernn.DualQuaternionF(boneTransformsItem1.Rotation, boneTransformsItem1.Position) * x1;
      scale = boneTransformsItem1.Scale * x1;
      QuaternionF rotation = boneTransformsItem1.Rotation;
      float y1 = blendWeight.Y;
      int y2 = blendIndex.Y;
      if (y2 == -1)
        return true;
      Component_SkeletonAnimationTrack.CalculateBoneTransformsItem boneTransformsItem2 = this.transformRelativeToSkin[y2];
      Component_MeshInSpaceAnimationControllernn.DualQuaternionF dualQuaternionF = new Component_MeshInSpaceAnimationControllernn.DualQuaternionF(boneTransformsItem2.Rotation, boneTransformsItem2.Position);
      sumDq += (double) Component_MeshInSpaceAnimationControllernn.Dot(ref rotation, ref boneTransformsItem2.Rotation) < 0.0 ? dualQuaternionF * -y1 : dualQuaternionF * y1;
      scale += boneTransformsItem2.Scale * y1;
      float z1 = blendWeight.Z;
      int z2 = blendIndex.Z;
      if (z2 == -1)
        return true;
      Component_SkeletonAnimationTrack.CalculateBoneTransformsItem boneTransformsItem3 = this.transformRelativeToSkin[z2];
      dualQuaternionF = new Component_MeshInSpaceAnimationControllernn.DualQuaternionF(boneTransformsItem3.Rotation, boneTransformsItem3.Position);
      sumDq += (double) Component_MeshInSpaceAnimationControllernn.Dot(ref rotation, ref boneTransformsItem3.Rotation) < 0.0 ? dualQuaternionF * -z1 : dualQuaternionF * z1;
      scale += boneTransformsItem3.Scale * z1;
      float w1 = blendWeight.W;
      int w2 = blendIndex.W;
      if (w2 == -1)
        return true;
      Component_SkeletonAnimationTrack.CalculateBoneTransformsItem boneTransformsItem4 = this.transformRelativeToSkin[w2];
      dualQuaternionF = new Component_MeshInSpaceAnimationControllernn.DualQuaternionF(boneTransformsItem4.Rotation, boneTransformsItem4.Position);
      sumDq += (double) Component_MeshInSpaceAnimationControllernn.Dot(ref rotation, ref boneTransformsItem4.Rotation) < 0.0 ? dualQuaternionF * -w1 : dualQuaternionF * w1;
      scale += boneTransformsItem4.Scale * w1;
      return true;
    }

    private struct BoneGlobalTransformItem
    {
      public bool HasValue;
      public Matrix4F Value;

      public BoneGlobalTransformItem(bool hasValue, ref Matrix4F value)
      {
        this.HasValue = hasValue;
        this.Value = value;
      }
    }

    private struct SourceChannel<TBuffer> where TBuffer : unmanaged
    {
      public bool Exists;
      private VertexElement sourceElement;
      private GpuVertexBuffer sourceBuffer;
      public TBuffer[] SourceData;

      public SourceChannel(
        Component_RenderingPipeline.RenderSceneData.MeshDataRenderOperation sourceOper,
        VertexElementSemantic semantic,
        VertexElementType type)
        : this()
      {
        if (!sourceOper.VertexStructure.GetElementBySemantic(semantic, out this.sourceElement) || this.sourceElement.Type != type)
          return;
        this.sourceBuffer = sourceOper.VertexBuffers[this.sourceElement.Source];
        this.SourceData = this.sourceBuffer.ExtractChannel<TBuffer>(this.sourceElement.Offset);
        this.Exists = true;
      }
    }

    private struct ChannelFloat3
    {
      public bool Exists;
      private VertexElement destElement;
      private GpuVertexBuffer destBuffer;
      public Vector3F[] SourceData;
      public Vector3F[] DestData;

      public ChannelFloat3(
        Component_RenderingPipeline.RenderSceneData.MeshDataRenderOperation sourceOper,
        Component_RenderingPipeline.RenderSceneData.MeshDataRenderOperation destOper,
        VertexElementSemantic semantic)
        : this()
      {
        VertexElement element;
        if (!sourceOper.VertexStructure.GetElementBySemantic(semantic, out element) || element.Type != VertexElementType.Float3)
          return;
        this.SourceData = sourceOper.VertexBuffers[element.Source].ExtractChannel<Vector3F>(element.Offset);
        if (destOper.VertexStructure.GetElementBySemantic(semantic, out this.destElement) && this.destElement.Type == VertexElementType.Float3)
        {
          this.destBuffer = destOper.VertexBuffers[this.destElement.Source];
          this.DestData = new Vector3F[this.destBuffer.VertexCount];
          this.Exists = true;
        }
      }

      public void WriteChannel()
      {
        if (!this.Exists)
          return;
        this.destBuffer.WriteChannel<Vector3F>(this.destElement.Offset, this.DestData);
      }
    }

    private struct ChannelFloat4
    {
      public bool Exists;
      private VertexElement destElement;
      private GpuVertexBuffer destBuffer;
      public Vector4F[] SourceData;
      public Vector4F[] DestData;

      public ChannelFloat4(
        Component_RenderingPipeline.RenderSceneData.MeshDataRenderOperation sourceOper,
        Component_RenderingPipeline.RenderSceneData.MeshDataRenderOperation destOper,
        VertexElementSemantic semantic)
        : this()
      {
        VertexElement element;
        if (!sourceOper.VertexStructure.GetElementBySemantic(semantic, out element) || element.Type != VertexElementType.Float4)
          return;
        this.SourceData = sourceOper.VertexBuffers[element.Source].ExtractChannel<Vector4F>(element.Offset);
        if (destOper.VertexStructure.GetElementBySemantic(semantic, out this.destElement) && this.destElement.Type == VertexElementType.Float4)
        {
          this.destBuffer = destOper.VertexBuffers[this.destElement.Source];
          this.DestData = new Vector4F[this.destBuffer.VertexCount];
          this.Exists = true;
        }
      }

      public void WriteChannel()
      {
        if (!this.Exists)
          return;
        this.destBuffer.WriteChannel<Vector4F>(this.destElement.Offset, this.DestData);
      }
    }

    public delegate void CalculateBoneTransformsDelegate(
      Component_MeshInSpaceAnimationControllernn sender,
      Component_SkeletonAnimationTrack.CalculateBoneTransformsItem[] result);

    private struct DualQuaternionF
    {
      public QuaternionF Q0;
      public QuaternionF Qe;

      public DualQuaternionF(QuaternionF q, Vector3F t)
      {
        float w = (float) (-0.5 * ((double) t.X * (double) q.X + (double) t.Y * (double) q.Y + (double) t.Z * (double) q.Z));
        float x = (float) (0.5 * ((double) t.X * (double) q.W + (double) t.Y * (double) q.Z - (double) t.Z * (double) q.Y));
        float y = (float) (0.5 * (-(double) t.X * (double) q.Z + (double) t.Y * (double) q.W + (double) t.Z * (double) q.X));
        float z = (float) (0.5 * ((double) t.X * (double) q.Y - (double) t.Y * (double) q.X + (double) t.Z * (double) q.W));
        this.Q0 = q;
        this.Qe = new QuaternionF(x, y, z, w);
      }

      public static Component_MeshInSpaceAnimationControllernn.DualQuaternionF operator +(
        Component_MeshInSpaceAnimationControllernn.DualQuaternionF dq1,
        Component_MeshInSpaceAnimationControllernn.DualQuaternionF dq2)
      {
        return new Component_MeshInSpaceAnimationControllernn.DualQuaternionF()
        {
          Q0 = dq1.Q0 + dq2.Q0,
          Qe = dq1.Qe + dq2.Qe
        };
      }

      public static Component_MeshInSpaceAnimationControllernn.DualQuaternionF operator *(
        Component_MeshInSpaceAnimationControllernn.DualQuaternionF dq,
        float scalar)
      {
        return new Component_MeshInSpaceAnimationControllernn.DualQuaternionF()
        {
          Q0 = dq.Q0 * scalar,
          Qe = dq.Qe * scalar
        };
      }

      public void Normalize()
      {
        float num = 1f / this.Q0.Length();
        this.Q0 *= num;
        this.Qe *= num;
      }
    }
  }
}
