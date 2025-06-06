namespace Hexed.Extensions
{
    internal static class InputHelper
    {
        public static VRCInput GetVRCInput(string inputName)
        {
            VRCInput result = VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0[inputName];
            return result;
        }

        public static float GetHeldTime(this VRCInput input)
        {
            return input.prop_Single_1;
        }

        public static bool IsPressed(this VRCInput input)
        {
            return input.prop_Boolean_0;
        }

        public static bool IsReleased(this VRCInput input)
        {
            return input.prop_Boolean_1;
        }
        public static bool IsHeld(this VRCInput input)
        {
            return input.prop_Boolean_2;
        }

        public static float GetAxis(this VRCInput input)
        {
            return input.prop_Single_2;
        }
    }
}
