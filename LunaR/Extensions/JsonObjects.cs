using System;
using System.Collections.Generic;
using VRC.Core;

namespace LunaR.Objects
{
    public class AvatarObject
    {
        public string name { get; set; }
        public string id { get; set; }
        public string authorId { get; set; }
        public string authorName { get; set; }
        public string assetUrl { get; set; }
        public string thumbnailImageUrl { get; set; }
        public string supportedPlatforms { get; set; }
        public string releaseStatus { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }
        public int version { get; set; }

        public static ApiAvatar ToApiAvatar(AvatarObject avtr)
        {
            return new ApiAvatar
            {
                name = avtr.name,
                id = avtr.id,
                authorId = avtr.authorId,
                authorName = avtr.authorName,
                assetUrl = avtr.assetUrl,
                thumbnailImageUrl = avtr.thumbnailImageUrl,
                supportedPlatforms = avtr.supportedPlatforms == "All" ? ApiModel.SupportedPlatforms.All : ApiModel.SupportedPlatforms.StandaloneWindows,
                description = avtr.description,
                releaseStatus = avtr.releaseStatus,
                imageUrl = avtr.imageUrl,
                version = avtr.version
            };
        }

        public static AvatarObject ToAvatarObject(ApiAvatar avtr)
        {
            return new AvatarObject
            {
                name = avtr.name,
                id = avtr.id,
                authorId = avtr.authorId,
                authorName = avtr.authorName,
                assetUrl = avtr.assetUrl,
                thumbnailImageUrl = avtr.thumbnailImageUrl,
                supportedPlatforms = avtr.supportedPlatforms.ToString(),
                description = avtr.description,
                releaseStatus = avtr.releaseStatus,
                imageUrl = avtr.imageUrl,
                version = avtr.version
            };
        }
    }

    public class AssetBundleObject
    {
        public string name;
        public string id;
        public string ownerId;
        public string mimeType;
        public string extension;
        public string[] tags;
    }

    public class PlayerLog
    {
        public string DisplayName { get; set; }
        public string UserID { get; set; }
        public Modules.PlayerExtensions.TrustRanks Rank { get; set; }
        public int Actor { get; set; }
        public bool IsVR { get; set; }
    }


    // Websocket
    public class User
    {
        public string id { get; set; }
        public string username { get; set; }
        public string displayName { get; set; }
        public string bio { get; set; }
        public string currentAvatarImageUrl { get; set; }
        public string currentAvatarThumbnailImageUrl { get; set; }
        public string userIcon { get; set; }
        public string last_platform { get; set; }
        public string status { get; set; }
        public string state { get; set; }
        public List<string> tags { get; set; }
        public string developerType { get; set; }
        public bool isFriend { get; set; }
    }

    public class NotificationDetails
    {
        public string worldName { get; set; }
        public string worldId { get; set; }
        public string responseMessage { get; set; }
    }

    public class World
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool featured { get; set; }
        public string authorId { get; set; }
        public string authorName { get; set; }
        public int capacity { get; set; }
        public List<string> tags { get; set; }
        public string releaseStatus { get; set; }
        public string imageUrl { get; set; }
        public string thumbnailImageUrl { get; set; }
        public string assetUrl { get; set; }
        public int version { get; set; }
        public string organization { get; set; }
        public object previewYoutubeId { get; set; }
        public int favorites { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime publicationDate { get; set; }
        public string labsPublicationDate { get; set; }
        public int visits { get; set; }
        public int popularity { get; set; }
        public int heat { get; set; }
        public int publicOccupants { get; set; }
        public object[][] instances { get; set; }
    }
}