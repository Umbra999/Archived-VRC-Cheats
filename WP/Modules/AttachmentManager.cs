using System;
using UnityEngine;

namespace WengaPort.Modules
{
    class AttachmentManager : MonoBehaviour 
    {
        public void Update()
        {
            if (RoomManager.field_Internal_Static_ApiWorld_0 == null || TransformParent == null) return;
            Utils.CurrentUser.transform.position = TransformParent.position;
        }
        internal static void SetAttachment(Transform Instance)
        {
            TransformParent = Instance;
        }
        internal static void SetAttachment(VRCPlayer Instance)
        {
            TransformParent = Instance.gameObject.transform;
        }
        internal static void SetAttachment(VRCPlayer Instance, HumanBodyBones bone)
        {
            TransformParent = Instance.gameObject.transform.GetComponentInChildren<Animator>().GetBoneTransform(bone);
        }
        internal static void Reset()
        {
            TransformParent = null;
        }
        private static Transform TransformParent;
        public AttachmentManager(IntPtr ptr) : base(ptr) { }
    }
}
