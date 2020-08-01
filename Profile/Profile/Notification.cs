using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile
{
    public enum NotificationType
    {
        NewPictureComment,
        NewPictureReaction,
        NewCommentReaction,
        FriendReceived,
        FriendAccepted,
        MessageReceived,
        CommentTag
    }

    public class Notification
    {
        private Models.Profile senderProfile;
        private NotificationType notification_type;
        private System.DateTime timestamp;
        private bool seen;

        public Notification(Models.Profile senderProfile, NotificationType notification_type, System.DateTime timestamp, bool seen)
        {
            this.senderProfile = senderProfile;
            this.notification_type = notification_type;
            this.timestamp = timestamp;
            this.seen = seen;
        }

        public Models.Profile GetNotificationSender()
        {
            return senderProfile;
        }

        public string GetNotificationText()
        {
            string notificationText;
            switch (this.notification_type)
            {
                case NotificationType.NewPictureComment:
                    notificationText = "commented on your picture!";
                    break;
                case NotificationType.NewPictureReaction:
                    notificationText = "reacted to your picture!";
                    break;
                case NotificationType.NewCommentReaction:
                    notificationText = "reacted to your comment!";
                    break;
                case NotificationType.FriendReceived:
                    notificationText = "wants to be your friend!";
                    break;
                case NotificationType.FriendAccepted:
                    notificationText = "accepted your friend request!";
                    break;
                case NotificationType.MessageReceived:
                    notificationText = "sent you a message!"; // functionality removed in favor of messages tab
                    break;
                case NotificationType.CommentTag:
                    notificationText = "tagged you in a comment!";
                    break;
                default:
                    notificationText = "performed a magic trick!";
                    break;
            }

            return notificationText;
        }

        public System.DateTime GetTimestamp()
        {
            return this.timestamp;
        }

        public bool IsSeen()
        {
            return this.seen;
        }

        public void SetSeen()
        {
            this.seen = false;
        }
    }
}