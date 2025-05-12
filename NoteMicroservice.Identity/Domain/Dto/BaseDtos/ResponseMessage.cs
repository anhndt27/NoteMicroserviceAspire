using Microsoft.Extensions.Localization;
using NoteMicroservice.Identity.Domain.Resources;

namespace NoteMicroservice.Identity.Domain.Dto.BaseDtos
{
    public class ResponseMessage
    {
        public MessageType MessageType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public ResponseMessage(MessageType type, string title = null, string message = null)
        {
            MessageType = type;
            Title = title;
            Message = message;
        }

        public static ResponseMessage SomethingWrong(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Error, commonTitles["SomethingWrong"], commonMessage["SomethingWrong"]);

        public static ResponseMessage AddedSuccess(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Success, commonTitles["AddedSuccess"], commonMessage["AddedSuccess"]);

        public static ResponseMessage DataExisted(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Warning, commonTitles["DataExisted"], commonMessage["DataExisted"]);

        public static ResponseMessage DataNotFound(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Error, commonTitles["DataNotFound"], commonMessage["DataNotFound"]);

        public static ResponseMessage DeletedSuccess(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Success, commonTitles["DeletedSuccess"], commonMessage["DeletedSuccess"]);

        public static ResponseMessage RestoredSuccess(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Success, commonTitles["RestoredSuccess"], commonMessage["RestoredSuccess"]);

        public static ResponseMessage InvalidInput(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Error, commonTitles["InvalidInput"], commonMessage["InvalidInput"]);

        public static ResponseMessage NoRecordAdded(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Warning, commonTitles["NoRecordAdded"], commonMessage["NoRecordAdded"]);

        public static ResponseMessage NoRecordDeleted(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Warning, commonTitles["NoRecordDeleted"], commonMessage["NoRecordDeleted"]);

        public static ResponseMessage NoRecordUpdated(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Warning, commonTitles["NoRecordUpdated"], commonMessage["NoRecordUpdated"]);
        public static ResponseMessage NoDataRestored(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Warning, commonTitles["NoDataRestored"], commonMessage["NoDataRestored"]);

        public static ResponseMessage Unauthorized(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Error, commonTitles["Unauthorized"], commonMessage["Unauthorized"]);

        public static ResponseMessage UpdatedSuccess(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessage) =>
            new ResponseMessage(MessageType.Success, commonTitles["UpdatedSuccess"], commonMessage["UpdatedSuccess"]);
    }

    public enum MessageType
    {
        Success = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }
}
