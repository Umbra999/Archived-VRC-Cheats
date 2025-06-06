using CoreRuntime.Manager;
using Hexed.Interfaces;
using VRC.Core;

namespace Hexed.Hooking
{
    internal class SubscriptionManager_GetActiveSubscription : IHook
    {
        private delegate nint _GetActiveSubscriptionDelegate();
        private static _GetActiveSubscriptionDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_GetActiveSubscriptionDelegate>(typeof(ObjectPublicStBoStDaBo1StILBo1Unique).GetMethod(nameof(ObjectPublicStBoStDaBo1StILBo1Unique.Method_Public_Static_ApiVRChatSubscription_0)), Patch);
        }

        private static nint Patch()
        {
            nint retVal = originalMethod();
            if (retVal != nint.Zero) return retVal;

            string startDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string expireDate = DateTime.UtcNow.AddDays(-1).AddYears(1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string transactionId = $"txn_{Guid.NewGuid()}";

            Il2CppSystem.Collections.Generic.List<string> licenseGroup = new();
            licenseGroup.Add($"lgrp_{Guid.NewGuid()}");
            //licenseGroup.Add("lgrp_608513da-b213-4e15-80af-bd88c27f097");

            ApiVRChatSubscription sub = new()
            {
                transactionId = transactionId,
                amount = 9999,
                description = "VRChat Plus (Yearly)",
                store = "Admin",
                period = "year",
                active = true,
                status = "active",
                tier = 5,
                starts = "",
                expires = expireDate,
                created_at = startDate,
                updated_at = startDate,
                isGift = false,
                giftedBy = null, // userid
                giftedByDisplayName = null, //displayname
                licenseGroups = licenseGroup,
                id = "vrchatplus-yearly",
                supportedPlatforms = ApiModel.SupportedPlatforms.All,
                Populated = true,
                Endpoint = null,
                steamItemId = "5000",

                _transactionId_k__BackingField = transactionId,
                _amount_k__BackingField = 9999,
                _description_k__BackingField = "VRChat Plus (Yearly)",
                _store_k__BackingField = "Admin",
                _period_k__BackingField = "year",
                _active_k__BackingField = true,
                _status_k__BackingField = "active",
                _tier_k__BackingField = 5,
                _starts_k__BackingField = "",
                _expires_k__BackingField = expireDate,
                _created_at_k__BackingField = startDate,
                _updated_at_k__BackingField = startDate,
                _isGift_k__BackingField = true,
                _giftedBy_k__BackingField = null,
                _giftedByDisplayName_k__BackingField = null,
                _licenseGroups_k__BackingField = licenseGroup,
                _id_k__BackingField = "vrchatplus-yearly",
                _Populated_k__BackingField = true,
                _Endpoint_k__BackingField = null,
                _steamItemId_k__BackingField = "5000",
                _cacheId = null,
            };

            return sub.Pointer;
        }
    }
}
