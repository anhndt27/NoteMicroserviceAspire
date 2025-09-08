namespace NoteMicroservice.Note.Domain.Dtos.BaseDtos
{
    public class ResponseMessageDto<T> : ResponseMessage
    {
        public T Dto { get; set; }
        public ResponseMessageDto(MessageType type, string title = null, string message = null) : base(type, title, message)
        {
            
        }
    }
}
